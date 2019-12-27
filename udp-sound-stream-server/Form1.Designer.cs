namespace udp_sound_stream_server
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblIPAdress = new System.Windows.Forms.Label();
            this.lblIPAdressValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status:";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Location = new System.Drawing.Point(74, 20);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(71, 13);
            this.lblStatusValue.TabIndex = 1;
            this.lblStatusValue.Text = "Waiting client";
            // 
            // lblIPAdress
            // 
            this.lblIPAdress.AutoSize = true;
            this.lblIPAdress.Location = new System.Drawing.Point(12, 42);
            this.lblIPAdress.Name = "lblIPAdress";
            this.lblIPAdress.Size = new System.Drawing.Size(20, 13);
            this.lblIPAdress.TabIndex = 2;
            this.lblIPAdress.Text = "IP:";
            // 
            // lblIPAdressValue
            // 
            this.lblIPAdressValue.AutoSize = true;
            this.lblIPAdressValue.Location = new System.Drawing.Point(74, 42);
            this.lblIPAdressValue.Name = "lblIPAdressValue";
            this.lblIPAdressValue.Size = new System.Drawing.Size(0, 13);
            this.lblIPAdressValue.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 115);
            this.Controls.Add(this.lblIPAdressValue);
            this.Controls.Add(this.lblIPAdress);
            this.Controls.Add(this.lblStatusValue);
            this.Controls.Add(this.lblStatus);
            this.Name = "Form1";
            this.Text = "UDP Sound Stream Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblIPAdress;
        private System.Windows.Forms.Label lblIPAdressValue;
    }
}

