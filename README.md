<p align="center">
<img width="300" src="https://user-images.githubusercontent.com/42527467/72662317-dcf4f300-39f6-11ea-8fd5-3ab6ee4b907c.png">
</p>

# UDP Sound Stream Server
Wi-Fi üzerinden bilgisayardaki sesi UDP protokolü üzerinden stream eden Windows uygulaması. 

## Kullanım
UDP üzerinden mesaj içeriğini `StartStream: true\n` olarak yollandığında ses kaydını stream etmeye başlıyacaktır.

.NET için client örneği, gelen buffer'ı [NAudio](https://github.com/naudio/NAudio), [CSCore](https://github.com/filoe/cscore) gibi kütüphaneler yardımıyla sese çevirebilirsiniz.

```csharp
private UdpClient _udpClient = new UdpClient();
private IPEndPoint IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
private IPEndPoint _clientEndPoint;

public Form1()
{
    InitializeComponent();
}

private void Form1_Load(object sender, EventArgs e)
{
    _udpClient.BeginReceive(AudioStreamRecevied, null);
}

private void button1_Click(object sender, EventArgs e)
{
    var body = Encoding.UTF8.GetBytes("StartStream: true\n");
    _udpClient.Send(body, body.Length, IP);
}

private void AudioStreamRecevied(IAsyncResult ar)
{
    var bufferReceive = _udpClient.EndReceive(ar, ref _clientEndPoint);
    _udpClient.BeginReceive(AudioStreamRecevied, null);
}
```

## Request Tipleri

`StartStream`: `boolen` Ses kaydıyla birlikte ses akışını başlatır. 
`StopStream`: `boolen` Ses kaydını ve ses akışını durdurur.
`ChangeAudioQuality`: `boolen` Bununla birlikte `BitPerSecond` ve `SampleRate` kullanılmak zorundadır, stream edilen ses kalitesini değiştirir.
`BitPerSecond`: `number` Saniyedeki bit sayısını belirtir.
`SampleRate`: `number` Örnekleme oranını belirtir.

#### Örnekler

`StartStream: true\n`
`StopStream: true\n`
`ChangeAudioQuality: true\nBitPerSecond: 32\nSampleRate: 32000\n`

**Not**: `StartStream` için varsayılan ses değerleri `BitPerSecond` 16, `SampleRate` 44100 olarak belirlenmiştir.

## Plugin Desteği
Kendi plugin'lerinizi oluşturabilirsiniz, örnek plugin'i [RemotePlugin](https://github.com/oktaysenkan/udp-sound-stream-server/tree/master/RemotePlugin) adresinden inceleyebilirsiniz. 

```csharp
public class RemoteOnMessageRecievedEventListener : IOnMessageReiceved  
{
    public void OnMessageRecieved(IDictionary<string, string> parameters)
    {
        Debug.WriteLine("Test");
    }
}
```

`parameters` argümanından kullanıcının request'te yolladığı parametreleri key, value şekilde alabilirsiniz.

## Ekran Görüntüleri
![image](https://user-images.githubusercontent.com/42527467/72662312-c353ab80-39f6-11ea-8f98-a4fb6d0a85b0.png)

## Katkıda bulunanlar
@oktaysenkan 