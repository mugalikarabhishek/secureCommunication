namespace ClientAppplication
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.sndMsgBtn = new System.Windows.Forms.Button();
            this.sndKyBtn = new System.Windows.Forms.Button();
            this.useAESrdioBtn = new System.Windows.Forms.RadioButton();
            this.sndIVBtn = new System.Windows.Forms.Button();
            this.useRSArdiobtn = new System.Windows.Forms.RadioButton();
            this.sndRsabtn = new System.Windows.Forms.Button();
            this.algorithmgroupBox = new System.Windows.Forms.GroupBox();
            this.hashRdioBtn = new System.Windows.Forms.RadioButton();
            this.rsaimplementationrRdioBtn = new System.Windows.Forms.RadioButton();
            this.cnclBtn = new System.Windows.Forms.Button();
            this.RsaInitializebtn = new System.Windows.Forms.Button();
            this.sndmsgrsaIBtn = new System.Windows.Forms.Button();
            this.hashBtn = new System.Windows.Forms.Button();
            this.algorithmgroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 52);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MessageLbl";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(16, 153);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(132, 20);
            this.textBox2.TabIndex = 3;
            // 
            // sndMsgBtn
            // 
            this.sndMsgBtn.Location = new System.Drawing.Point(15, 193);
            this.sndMsgBtn.Name = "sndMsgBtn";
            this.sndMsgBtn.Size = new System.Drawing.Size(75, 23);
            this.sndMsgBtn.TabIndex = 4;
            this.sndMsgBtn.Text = "Send";
            this.sndMsgBtn.UseVisualStyleBackColor = true;
            this.sndMsgBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // sndKyBtn
            // 
            this.sndKyBtn.Location = new System.Drawing.Point(145, 179);
            this.sndKyBtn.Name = "sndKyBtn";
            this.sndKyBtn.Size = new System.Drawing.Size(75, 23);
            this.sndKyBtn.TabIndex = 5;
            this.sndKyBtn.Text = "SendKey";
            this.sndKyBtn.UseVisualStyleBackColor = true;
            this.sndKyBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // useAESrdioBtn
            // 
            this.useAESrdioBtn.AutoSize = true;
            this.useAESrdioBtn.Location = new System.Drawing.Point(6, 19);
            this.useAESrdioBtn.Name = "useAESrdioBtn";
            this.useAESrdioBtn.Size = new System.Drawing.Size(65, 17);
            this.useAESrdioBtn.TabIndex = 6;
            this.useAESrdioBtn.TabStop = true;
            this.useAESrdioBtn.Text = "UseAES";
            this.useAESrdioBtn.UseVisualStyleBackColor = true;
            this.useAESrdioBtn.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // sndIVBtn
            // 
            this.sndIVBtn.Location = new System.Drawing.Point(235, 179);
            this.sndIVBtn.Name = "sndIVBtn";
            this.sndIVBtn.Size = new System.Drawing.Size(75, 23);
            this.sndIVBtn.TabIndex = 7;
            this.sndIVBtn.Text = "SendIV";
            this.sndIVBtn.UseVisualStyleBackColor = true;
            this.sndIVBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // useRSArdiobtn
            // 
            this.useRSArdiobtn.AutoSize = true;
            this.useRSArdiobtn.Location = new System.Drawing.Point(6, 42);
            this.useRSArdiobtn.Name = "useRSArdiobtn";
            this.useRSArdiobtn.Size = new System.Drawing.Size(66, 17);
            this.useRSArdiobtn.TabIndex = 8;
            this.useRSArdiobtn.TabStop = true;
            this.useRSArdiobtn.Text = "UseRSA";
            this.useRSArdiobtn.UseVisualStyleBackColor = true;
            this.useRSArdiobtn.CheckedChanged += new System.EventHandler(this.useRSArdiobtn_CheckedChanged);
            // 
            // sndRsabtn
            // 
            this.sndRsabtn.Location = new System.Drawing.Point(16, 223);
            this.sndRsabtn.Name = "sndRsabtn";
            this.sndRsabtn.Size = new System.Drawing.Size(75, 23);
            this.sndRsabtn.TabIndex = 10;
            this.sndRsabtn.Text = "SendRSA";
            this.sndRsabtn.UseVisualStyleBackColor = true;
            this.sndRsabtn.Click += new System.EventHandler(this.sndRsabtn_Click);
            // 
            // algorithmgroupBox
            // 
            this.algorithmgroupBox.Controls.Add(this.hashRdioBtn);
            this.algorithmgroupBox.Controls.Add(this.rsaimplementationrRdioBtn);
            this.algorithmgroupBox.Controls.Add(this.useAESrdioBtn);
            this.algorithmgroupBox.Controls.Add(this.useRSArdiobtn);
            this.algorithmgroupBox.Location = new System.Drawing.Point(182, 52);
            this.algorithmgroupBox.Name = "algorithmgroupBox";
            this.algorithmgroupBox.Size = new System.Drawing.Size(132, 112);
            this.algorithmgroupBox.TabIndex = 11;
            this.algorithmgroupBox.TabStop = false;
            this.algorithmgroupBox.Text = "ALgorithm";
            // 
            // hashRdioBtn
            // 
            this.hashRdioBtn.AutoSize = true;
            this.hashRdioBtn.Location = new System.Drawing.Point(7, 86);
            this.hashRdioBtn.Name = "hashRdioBtn";
            this.hashRdioBtn.Size = new System.Drawing.Size(69, 17);
            this.hashRdioBtn.TabIndex = 10;
            this.hashRdioBtn.TabStop = true;
            this.hashRdioBtn.Text = "UseHash";
            this.hashRdioBtn.UseVisualStyleBackColor = true;
            this.hashRdioBtn.CheckedChanged += new System.EventHandler(this.hashRdioBtn_CheckedChanged);
            // 
            // rsaimplementationrRdioBtn
            // 
            this.rsaimplementationrRdioBtn.AutoSize = true;
            this.rsaimplementationrRdioBtn.Location = new System.Drawing.Point(7, 62);
            this.rsaimplementationrRdioBtn.Name = "rsaimplementationrRdioBtn";
            this.rsaimplementationrRdioBtn.Size = new System.Drawing.Size(121, 17);
            this.rsaimplementationrRdioBtn.TabIndex = 9;
            this.rsaimplementationrRdioBtn.TabStop = true;
            this.rsaimplementationrRdioBtn.Text = "RSA Implementation";
            this.rsaimplementationrRdioBtn.UseVisualStyleBackColor = true;
            this.rsaimplementationrRdioBtn.CheckedChanged += new System.EventHandler(this.rsaimplementationrRdioBtn_CheckedChanged);
            // 
            // cnclBtn
            // 
            this.cnclBtn.Location = new System.Drawing.Point(145, 208);
            this.cnclBtn.Name = "cnclBtn";
            this.cnclBtn.Size = new System.Drawing.Size(75, 23);
            this.cnclBtn.TabIndex = 12;
            this.cnclBtn.Text = "Cancel";
            this.cnclBtn.UseVisualStyleBackColor = true;
            this.cnclBtn.Click += new System.EventHandler(this.cnclBtn_Click);
            // 
            // RsaInitializebtn
            // 
            this.RsaInitializebtn.Location = new System.Drawing.Point(235, 208);
            this.RsaInitializebtn.Name = "RsaInitializebtn";
            this.RsaInitializebtn.Size = new System.Drawing.Size(82, 23);
            this.RsaInitializebtn.TabIndex = 13;
            this.RsaInitializebtn.Text = "RSAInititalize";
            this.RsaInitializebtn.UseVisualStyleBackColor = true;
            this.RsaInitializebtn.Click += new System.EventHandler(this.RsaInitializebtn_Click);
            // 
            // sndmsgrsaIBtn
            // 
            this.sndmsgrsaIBtn.Location = new System.Drawing.Point(40, 193);
            this.sndmsgrsaIBtn.Name = "sndmsgrsaIBtn";
            this.sndmsgrsaIBtn.Size = new System.Drawing.Size(75, 23);
            this.sndmsgrsaIBtn.TabIndex = 14;
            this.sndmsgrsaIBtn.Text = "sndmsg";
            this.sndmsgrsaIBtn.UseVisualStyleBackColor = true;
            this.sndmsgrsaIBtn.Click += new System.EventHandler(this.sndmsgrsaIBtn_Click);
            // 
            // hashBtn
            // 
            this.hashBtn.Location = new System.Drawing.Point(16, 253);
            this.hashBtn.Name = "hashBtn";
            this.hashBtn.Size = new System.Drawing.Size(75, 23);
            this.hashBtn.TabIndex = 15;
            this.hashBtn.Text = "ViewHash";
            this.hashBtn.UseVisualStyleBackColor = true;
            this.hashBtn.Click += new System.EventHandler(this.hashBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 329);
            this.Controls.Add(this.hashBtn);
            this.Controls.Add(this.sndmsgrsaIBtn);
            this.Controls.Add(this.RsaInitializebtn);
            this.Controls.Add(this.cnclBtn);
            this.Controls.Add(this.algorithmgroupBox);
            this.Controls.Add(this.sndRsabtn);
            this.Controls.Add(this.sndIVBtn);
            this.Controls.Add(this.sndKyBtn);
            this.Controls.Add(this.sndMsgBtn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "SendMessage";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.algorithmgroupBox.ResumeLayout(false);
            this.algorithmgroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button sndMsgBtn;
        private System.Windows.Forms.Button sndKyBtn;
        private System.Windows.Forms.RadioButton useAESrdioBtn;
        private System.Windows.Forms.Button sndIVBtn;
        private System.Windows.Forms.RadioButton useRSArdiobtn;
        private System.Windows.Forms.Button sndRsabtn;
        private System.Windows.Forms.GroupBox algorithmgroupBox;
        private System.Windows.Forms.Button cnclBtn;
        private System.Windows.Forms.RadioButton rsaimplementationrRdioBtn;
        private System.Windows.Forms.Button RsaInitializebtn;
        private System.Windows.Forms.Button sndmsgrsaIBtn;
        private System.Windows.Forms.RadioButton hashRdioBtn;
        private System.Windows.Forms.Button hashBtn;
    }
}

