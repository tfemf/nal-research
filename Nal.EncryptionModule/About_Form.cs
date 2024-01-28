// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.About_Form
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Nal.EncryptionModule
{
  internal class About_Form : Form
  {
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Button button1;

    public About_Form() => this.InitializeComponent();

    private void button1_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.button1 = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(58, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(177, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "NAL Encryption Module";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(40, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(212, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "This encrypts and decrypts messages using";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(34, 72);
      this.label3.Name = "label3";
      this.label3.Size = new Size(224, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "AES 256 encryption. This program is designed";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(53, 85);
      this.label4.Name = "label4";
      this.label4.Size = new Size(186, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "for easy use with the modem that NAL";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(73, 98);
      this.label5.Name = "label5";
      this.label5.Size = new Size(147, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Research Corporation makes.";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(34, 125);
      this.label6.Name = "label6";
      this.label6.Size = new Size(224, 13);
      this.label6.TabIndex = 5;
      this.label6.Text = "Copyright (C) 2009 NAL Research Corporation";
      this.button1.Location = new Point(109, 159);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 6;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(292, 201);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (About_Form);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "About";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
