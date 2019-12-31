using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginBase;
using System.Text;
using System.Threading.Tasks;

namespace udp_sound_stream_server
{
    class PluginLoader
    {
        private readonly List<Type> _onMessageRecievedListeners = new List<Type>();

        public PluginLoader()
        {
            Initialize();
        }

        private static string GetPluginsDirectory()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "plugins");
        }

        private void Initialize()
        {
            var pluginDirectory = GetPluginsDirectory();

            if (!Directory.Exists(pluginDirectory))
            {
                Debug.WriteLine("Cannot find plugins folder.");
                return;
            }

            var assemblyFiles = Directory.GetFiles(pluginDirectory, "*.dll");

            foreach (var assemblyName in assemblyFiles)
            {
                var pluginAssembly = Assembly.LoadFile(assemblyName);

                Debug.WriteLine($"Loaded {assemblyName}");

                var existingTypes = pluginAssembly.GetTypes();

                bool TypePredicate(Type child, Type parent) =>
                    child.IsPublic && !child.IsAbstract && child.IsClass && parent.IsAssignableFrom(child);

                var onMessageReicevedListenerTypes =
                    existingTypes.Where(a => TypePredicate(a, typeof(IOnMessageReiceved))).ToList();
                _onMessageRecievedListeners.AddRange(onMessageReicevedListenerTypes);

                Debug.WriteLine($"Found the following PreCopy types from plugin {assemblyName}:");
                Debug.WriteLine(string.Join("\n", onMessageReicevedListenerTypes.Select(a => a.Name).ToArray()));
            }
        }

        public void Subscribe(IMessageRecievedEventBroadcaster handler)
        {
            _onMessageRecievedListeners.ForEach(listener =>
            {
                var listenerObject = (IOnMessageReiceved) Activator.CreateInstance(listener);
                handler.MessageRecievedEvent += listenerObject.OnMessageRecieved;
            });
        }
    }
}
