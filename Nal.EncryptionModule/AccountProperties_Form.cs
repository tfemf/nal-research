// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.AccountProperties_Form
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
  internal class AccountProperties_Form : Form
  {
    private CryptoOfficerRole officer;
    private string userBeingModified;
    private string modemInfoBeingModified;
    private string modemInfoTypeBeingModified;
    private IContainer components;
    private Button submit_Button;
    private Button close_Button;
    private CheckBox encryption_CheckBox;
    private CheckBox decryption_CheckBox;
    private MaskedTextBox dKey2_MaskedTextBox;
    private MaskedTextBox dKey1_MaskedTextBox;
    private MaskedTextBox eKey2_MaskedTextBox;
    private MaskedTextBox eKey1_MaskedTextBox;
    private TextBox password_TextBox;
    private TextBox user_TextBox;
    private Label userName_Label;
    private Label password_Label;
    private Label modemInfo_Label;
    private Label encryptionReenter_Label;
    private TextBox modem_TextBox;
    private Label decryptionReenter_Label;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private RadioButton phoneNumber_RadioButton;
    private RadioButton imei_RadioButton;

    public AccountProperties_Form(CryptoOfficerRole officer)
    {
      this.InitializeComponent();
      this.userBeingModified = string.Empty;
      this.modemInfoBeingModified = string.Empty;
      this.modemInfoTypeBeingModified = string.Empty;
      this.Text = "Add Account";
      this.officer = officer;
    }

    public AccountProperties_Form(
      CryptoOfficerRole officer,
      string user,
      string modem,
      string modemType)
    {
      this.InitializeComponent();
      this.userBeingModified = user;
      this.modemInfoBeingModified = modem;
      this.modemInfoTypeBeingModified = modemType;
      this.Text = "Edit Account";
      this.officer = officer;
      CryptoOfficerRole.Record record = officer.GetRecord(user, modem, modemType);
      if (record == null)
      {
        int num = (int) MessageBox.Show("The account does not exist.", "Error");
      }
      else
      {
        this.user_TextBox.Text = record.user;
        this.password_TextBox.Text = record.password;
        this.modem_TextBox.Text = record.modemInfo;
        this.eKey1_MaskedTextBox.Text = record.encryptionKey;
        this.eKey2_MaskedTextBox.Text = record.encryptionKey;
        this.dKey1_MaskedTextBox.Text = record.decryptionKey;
        this.dKey2_MaskedTextBox.Text = record.decryptionKey;
        if (record.modemInfoType == "I")
          this.imei_RadioButton.Checked = true;
        else if (record.modemInfoType == "P")
          this.phoneNumber_RadioButton.Checked = true;
        if (record.keyType == "E")
        {
          this.encryption_CheckBox.Checked = true;
          this.decryption_CheckBox.Checked = false;
        }
        else if (record.keyType == "D")
        {
          this.encryption_CheckBox.Checked = false;
          this.decryption_CheckBox.Checked = true;
        }
        else
        {
          if (!(record.keyType == "B"))
            return;
          this.encryption_CheckBox.Checked = true;
          this.decryption_CheckBox.Checked = true;
        }
      }
    }

    private void user_TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          if (this.password_TextBox.Enabled)
          {
            this.password_TextBox.Focus();
            break;
          }
          this.modem_TextBox.Focus();
          break;
        case '\u001B':
          this.password_TextBox.Text = string.Empty;
          break;
        case '"':
        case '*':
        case '/':
        case ':':
        case '<':
        case '>':
        case '?':
        case '\\':
        case '|':
          e.KeyChar = char.MinValue;
          break;
      }
    }

    private void OnUserTextBoxLeave(object sender, EventArgs e)
    {
      string passwordForUser = this.officer.GetPasswordForUser(this.user_TextBox.Text);
      if (passwordForUser.Length == 0)
        return;
      this.password_TextBox.Text = passwordForUser;
    }

    private void OnPasswordTextBoxKeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          this.modem_TextBox.Focus();
          break;
        case '\u001B':
          this.password_TextBox.Text = string.Empty;
          this.modem_TextBox.Focus();
          break;
        case '"':
          e.KeyChar = char.MinValue;
          break;
      }
    }

    private void OnPasswordTextBoxLeave(object sender, EventArgs e)
    {
      if (this.password_TextBox.Text.Length == 0)
        return;
      if (this.password_TextBox.Text.Length < 8 || this.password_TextBox.Text.Length > 16)
      {
        int num = (int) MessageBox.Show("The Password must be between 8 and 16 characters long.", "Warning", MessageBoxButtons.OK);
        this.password_TextBox.Focus();
      }
      if (!(this.officer.GetPasswordForUser(this.user_TextBox.Text) == string.Empty) || this.officer.PasswordIsUnique(this.password_TextBox.Text))
        return;
      int num1 = (int) MessageBox.Show("Password is not unique.", "Warning", MessageBoxButtons.OK);
    }

    private void modem_TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.encryption_CheckBox.Focus();
    }

    private void modem_TextBox_Leave(object sender, EventArgs e) => this.CheckModemInfo();

    private void OnPhoneNumberRadioButtonClick(object sender, EventArgs e) => this.CheckModemInfo();

    private void OnImeiRadioButtonClick(object sender, EventArgs e) => this.CheckModemInfo();

    private void CheckModemInfo()
    {
      if (!this.imei_RadioButton.Checked && this.modem_TextBox.Text.StartsWith("00"))
      {
        int num = (int) MessageBox.Show("The phone number that was entered begins with \"00\".\r\n\r\nIf this is the international prefix it should probably\r\nbe removed since SMS messages do not contain this\r\nprefix in the sender's phone number.", "Warning");
      }
      string encryptionKey;
      string decryptionKey;
      string type;
      if (this.officer.GetKeysForModem(this.modem_TextBox.Text, this.imei_RadioButton.Checked ? "I" : "P", out encryptionKey, out decryptionKey, out type) != Erc.Success || encryptionKey == this.eKey1_MaskedTextBox.Text && encryptionKey == this.eKey2_MaskedTextBox.Text && decryptionKey == this.dKey1_MaskedTextBox.Text && decryptionKey == this.dKey2_MaskedTextBox.Text || MessageBox.Show("The IMEI / phone number already exists.\nShould its key settings be loaded?", "Automatic Fill In", MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      switch (type)
      {
        case "B":
          this.encryption_CheckBox.Checked = true;
          this.decryption_CheckBox.Checked = true;
          break;
        case "E":
          this.encryption_CheckBox.Checked = true;
          this.decryption_CheckBox.Checked = false;
          break;
        case "D":
          this.encryption_CheckBox.Checked = false;
          this.decryption_CheckBox.Checked = true;
          break;
      }
      this.eKey1_MaskedTextBox.Text = encryptionKey;
      this.eKey2_MaskedTextBox.Text = encryptionKey;
      this.dKey1_MaskedTextBox.Text = decryptionKey;
      this.dKey2_MaskedTextBox.Text = decryptionKey;
    }

    private void encryptionKey1_MaskedTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.KeyChar = this.KeyFormatter(e.KeyChar);
      if (e.KeyChar != '\r')
        return;
      this.eKey2_MaskedTextBox.Focus();
    }

    private void encryptionKey2_MaskedTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.KeyChar = this.KeyFormatter(e.KeyChar);
      if (e.KeyChar != '\r')
        return;
      this.dKey1_MaskedTextBox.Focus();
    }

    private void OnDecryptionKey1MaskedTextBoxKeyPress(object sender, KeyPressEventArgs e)
    {
      e.KeyChar = this.KeyFormatter(e.KeyChar);
      if (e.KeyChar != '\r')
        return;
      this.dKey2_MaskedTextBox.Focus();
    }

    private void OnDecryptionKey2MaskedTextBoxKeyPress(object sender, KeyPressEventArgs e)
    {
      e.KeyChar = this.KeyFormatter(e.KeyChar);
      if (e.KeyChar != '\r')
        return;
      this.submit_Button.Focus();
    }

    private void OnSubmitButtonClick(object sender, EventArgs e)
    {
      if (this.user_TextBox.Text == this.password_TextBox.Text)
      {
        int num1 = (int) MessageBox.Show("For security reasons the user name and password may not be identical.", "Error");
      }
      else
      {
        string str = new string('0', 64);
        string encryptionKey = str;
        if (this.encryption_CheckBox.Checked)
        {
          if (this.eKey1_MaskedTextBox.Text != this.eKey2_MaskedTextBox.Text)
          {
            int num2 = (int) MessageBox.Show("The encryption keys do not match.", "Error");
            return;
          }
          if (this.eKey1_MaskedTextBox.Text == str)
          {
            int num3 = (int) MessageBox.Show("Encryption key may not be all 0s.", "Error");
            return;
          }
          encryptionKey = this.eKey1_MaskedTextBox.Text;
        }
        else if (this.eKey1_MaskedTextBox.Text == this.eKey2_MaskedTextBox.Text && this.eKey1_MaskedTextBox.Text.Length == 64)
          encryptionKey = this.eKey1_MaskedTextBox.Text;
        string decryptionKey = str;
        if (this.decryption_CheckBox.Checked)
        {
          if (this.dKey1_MaskedTextBox.Text != this.dKey2_MaskedTextBox.Text)
          {
            int num4 = (int) MessageBox.Show("The decryption keys do not match.", "Error");
            return;
          }
          if (this.dKey1_MaskedTextBox.Text == str)
          {
            int num5 = (int) MessageBox.Show("Decryption key may not be all 0s.", "Error");
            return;
          }
          decryptionKey = this.dKey1_MaskedTextBox.Text;
        }
        else if (this.dKey1_MaskedTextBox.Text == this.dKey2_MaskedTextBox.Text && this.dKey1_MaskedTextBox.Text.Length == 64)
          decryptionKey = this.dKey1_MaskedTextBox.Text;
        string modemInfoType = this.imei_RadioButton.Checked ? "I" : "P";
        string keyType;
        if (this.encryption_CheckBox.Checked && this.decryption_CheckBox.Checked)
          keyType = "B";
        else if (this.encryption_CheckBox.Checked && !this.decryption_CheckBox.Checked)
          keyType = "E";
        else if (!this.encryption_CheckBox.Checked && this.decryption_CheckBox.Checked)
        {
          keyType = "D";
        }
        else
        {
          int num6 = (int) MessageBox.Show("Either an encryption or decryption key must be entered.", "Error");
          return;
        }
        bool flag = this.Text.StartsWith("Edit");
        if (keyType == "E" || keyType == "B")
        {
          CryptoOfficerRole.Record recordByEncryptionKey = this.officer.GetRecordByEncryptionKey(modemInfoType, encryptionKey);
          if (recordByEncryptionKey != null && (!flag || recordByEncryptionKey.user != this.userBeingModified || recordByEncryptionKey.modemInfo != this.modemInfoBeingModified || recordByEncryptionKey.modemInfoType != this.modemInfoTypeBeingModified))
          {
            int num7 = (int) MessageBox.Show("The encryption key must be unique across " + (modemInfoType == "I" ? "IMEIs" : "phone numbers") + ".", "Error");
            return;
          }
        }
        if (keyType == "D" || keyType == "B")
        {
          CryptoOfficerRole.Record recordByDecryptionKey = this.officer.GetRecordByDecryptionKey(modemInfoType, decryptionKey);
          if (recordByDecryptionKey != null && (!flag || recordByDecryptionKey.user != this.userBeingModified || recordByDecryptionKey.modemInfo != this.modemInfoBeingModified || recordByDecryptionKey.modemInfoType != this.modemInfoTypeBeingModified))
          {
            int num8 = (int) MessageBox.Show("The decryption key must be unique across " + (modemInfoType == "I" ? "IMEIs" : "phone numbers") + ".", "Error");
            return;
          }
        }
        if (this.officer.ModemExistsForOtherUsers(this.userBeingModified, this.modem_TextBox.Text, modemInfoType) && MessageBox.Show("The modem already exists for at least one other user. If you continue all occurances of this modem will be updated.\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
        {
          this.modem_TextBox.Focus();
        }
        else
        {
          Erc code;
          if (this.Text.StartsWith("Edit"))
          {
            string passwordForUser = this.officer.GetPasswordForUser(this.user_TextBox.Text);
            if (passwordForUser != string.Empty && passwordForUser != this.password_TextBox.Text && MessageBox.Show("The user already has a password.\nDo you want to update the existing password.", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
            {
              this.password_TextBox.Text = passwordForUser;
              return;
            }
            code = this.officer.UpdateRecord(this.userBeingModified, this.modemInfoBeingModified, this.modemInfoTypeBeingModified, this.user_TextBox.Text, this.password_TextBox.Text, this.modem_TextBox.Text, modemInfoType, encryptionKey, decryptionKey, keyType);
          }
          else
            code = this.officer.AddRecord(this.user_TextBox.Text, this.password_TextBox.Text, this.modem_TextBox.Text, modemInfoType, encryptionKey, decryptionKey, keyType);
          if (code != Erc.Success)
          {
            int num9 = (int) MessageBox.Show(ErcStr.GetStringForCode(code), "Error", MessageBoxButtons.OK);
          }
          else
            this.Close();
        }
      }
    }

    private void OnCloseButtonClick(object sender, EventArgs e) => this.Close();

    private char KeyFormatter(char key)
    {
      if (key >= 'a' && key <= 'f')
        key = char.ToUpper(key);
      if ((key < '0' || key > '9') && (key < 'A' || key > 'F') && key != '\t' && key != '\r' && key != '\n' && key != '\u001B' && key != '\u0016' && key != '\u0003' && key != '\u0018')
        key = char.MinValue;
      return key;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AccountProperties_Form));
      this.submit_Button = new Button();
      this.close_Button = new Button();
      this.encryption_CheckBox = new CheckBox();
      this.decryption_CheckBox = new CheckBox();
      this.dKey2_MaskedTextBox = new MaskedTextBox();
      this.dKey1_MaskedTextBox = new MaskedTextBox();
      this.eKey2_MaskedTextBox = new MaskedTextBox();
      this.eKey1_MaskedTextBox = new MaskedTextBox();
      this.password_TextBox = new TextBox();
      this.user_TextBox = new TextBox();
      this.userName_Label = new Label();
      this.password_Label = new Label();
      this.modemInfo_Label = new Label();
      this.encryptionReenter_Label = new Label();
      this.modem_TextBox = new TextBox();
      this.decryptionReenter_Label = new Label();
      this.groupBox1 = new GroupBox();
      this.phoneNumber_RadioButton = new RadioButton();
      this.imei_RadioButton = new RadioButton();
      this.groupBox2 = new GroupBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.submit_Button.Location = new Point(471, 316);
      this.submit_Button.Name = "submit_Button";
      this.submit_Button.Size = new Size(75, 23);
      this.submit_Button.TabIndex = 2;
      this.submit_Button.Text = "Submit";
      this.submit_Button.UseVisualStyleBackColor = true;
      this.submit_Button.Click += new EventHandler(this.OnSubmitButtonClick);
      this.close_Button.Location = new Point(552, 316);
      this.close_Button.Name = "close_Button";
      this.close_Button.Size = new Size(75, 23);
      this.close_Button.TabIndex = 3;
      this.close_Button.Text = "Close";
      this.close_Button.UseVisualStyleBackColor = true;
      this.close_Button.Click += new EventHandler(this.OnCloseButtonClick);
      this.encryption_CheckBox.AutoSize = true;
      this.encryption_CheckBox.ForeColor = SystemColors.ControlText;
      this.encryption_CheckBox.Location = new Point(22, 55);
      this.encryption_CheckBox.Name = "encryption_CheckBox";
      this.encryption_CheckBox.Size = new Size(76, 17);
      this.encryption_CheckBox.TabIndex = 4;
      this.encryption_CheckBox.Text = "Encryption";
      this.encryption_CheckBox.UseVisualStyleBackColor = true;
      this.decryption_CheckBox.AutoSize = true;
      this.decryption_CheckBox.ForeColor = SystemColors.ControlText;
      this.decryption_CheckBox.Location = new Point(21, 107);
      this.decryption_CheckBox.Name = "decryption_CheckBox";
      this.decryption_CheckBox.Size = new Size(77, 17);
      this.decryption_CheckBox.TabIndex = 8;
      this.decryption_CheckBox.Text = "Decryption";
      this.decryption_CheckBox.UseVisualStyleBackColor = true;
      this.dKey2_MaskedTextBox.ForeColor = SystemColors.WindowText;
      this.dKey2_MaskedTextBox.Location = new Point(104, 131);
      this.dKey2_MaskedTextBox.Mask = "CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC";
      this.dKey2_MaskedTextBox.Name = "dKey2_MaskedTextBox";
      this.dKey2_MaskedTextBox.Size = new Size(492, 20);
      this.dKey2_MaskedTextBox.TabIndex = 11;
      this.dKey2_MaskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
      this.dKey2_MaskedTextBox.KeyPress += new KeyPressEventHandler(this.OnDecryptionKey2MaskedTextBoxKeyPress);
      this.dKey1_MaskedTextBox.ForeColor = SystemColors.WindowText;
      this.dKey1_MaskedTextBox.Location = new Point(104, 105);
      this.dKey1_MaskedTextBox.Mask = "CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC";
      this.dKey1_MaskedTextBox.Name = "dKey1_MaskedTextBox";
      this.dKey1_MaskedTextBox.Size = new Size(492, 20);
      this.dKey1_MaskedTextBox.TabIndex = 9;
      this.dKey1_MaskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
      this.dKey1_MaskedTextBox.KeyPress += new KeyPressEventHandler(this.OnDecryptionKey1MaskedTextBoxKeyPress);
      this.eKey2_MaskedTextBox.ForeColor = SystemColors.WindowText;
      this.eKey2_MaskedTextBox.Location = new Point(104, 79);
      this.eKey2_MaskedTextBox.Mask = "CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC";
      this.eKey2_MaskedTextBox.Name = "eKey2_MaskedTextBox";
      this.eKey2_MaskedTextBox.Size = new Size(492, 20);
      this.eKey2_MaskedTextBox.TabIndex = 7;
      this.eKey2_MaskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
      this.eKey2_MaskedTextBox.KeyPress += new KeyPressEventHandler(this.encryptionKey2_MaskedTextBox_KeyPress);
      this.eKey1_MaskedTextBox.ForeColor = SystemColors.WindowText;
      this.eKey1_MaskedTextBox.Location = new Point(104, 53);
      this.eKey1_MaskedTextBox.Mask = "CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC CC";
      this.eKey1_MaskedTextBox.Name = "eKey1_MaskedTextBox";
      this.eKey1_MaskedTextBox.Size = new Size(492, 20);
      this.eKey1_MaskedTextBox.TabIndex = 5;
      this.eKey1_MaskedTextBox.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
      this.eKey1_MaskedTextBox.KeyPress += new KeyPressEventHandler(this.encryptionKey1_MaskedTextBox_KeyPress);
      this.password_TextBox.Location = new Point(104, 54);
      this.password_TextBox.Name = "password_TextBox";
      this.password_TextBox.Size = new Size(147, 20);
      this.password_TextBox.TabIndex = 3;
      this.password_TextBox.Leave += new EventHandler(this.OnPasswordTextBoxLeave);
      this.password_TextBox.KeyPress += new KeyPressEventHandler(this.OnPasswordTextBoxKeyPress);
      this.user_TextBox.Location = new Point(104, 28);
      this.user_TextBox.Name = "user_TextBox";
      this.user_TextBox.Size = new Size(147, 20);
      this.user_TextBox.TabIndex = 1;
      this.user_TextBox.Leave += new EventHandler(this.OnUserTextBoxLeave);
      this.user_TextBox.KeyPress += new KeyPressEventHandler(this.user_TextBox_KeyPress);
      this.userName_Label.AutoSize = true;
      this.userName_Label.ForeColor = SystemColors.ControlText;
      this.userName_Label.Location = new Point(63, 31);
      this.userName_Label.Name = "userName_Label";
      this.userName_Label.Size = new Size(35, 13);
      this.userName_Label.TabIndex = 0;
      this.userName_Label.Text = "Name";
      this.password_Label.AutoSize = true;
      this.password_Label.ForeColor = SystemColors.ControlText;
      this.password_Label.Location = new Point(45, 57);
      this.password_Label.Name = "password_Label";
      this.password_Label.Size = new Size(53, 13);
      this.password_Label.TabIndex = 2;
      this.password_Label.Text = "Password";
      this.modemInfo_Label.AutoSize = true;
      this.modemInfo_Label.ForeColor = SystemColors.ControlText;
      this.modemInfo_Label.Location = new Point(35, 30);
      this.modemInfo_Label.Name = "modemInfo_Label";
      this.modemInfo_Label.Size = new Size(63, 13);
      this.modemInfo_Label.TabIndex = 0;
      this.modemInfo_Label.Text = "Modem Info";
      this.encryptionReenter_Label.AutoSize = true;
      this.encryptionReenter_Label.ForeColor = SystemColors.ControlText;
      this.encryptionReenter_Label.Location = new Point(53, 82);
      this.encryptionReenter_Label.Name = "encryptionReenter_Label";
      this.encryptionReenter_Label.Size = new Size(45, 13);
      this.encryptionReenter_Label.TabIndex = 6;
      this.encryptionReenter_Label.Text = "Reenter";
      this.modem_TextBox.ForeColor = SystemColors.WindowText;
      this.modem_TextBox.Location = new Point(104, 27);
      this.modem_TextBox.Name = "modem_TextBox";
      this.modem_TextBox.Size = new Size(147, 20);
      this.modem_TextBox.TabIndex = 1;
      this.modem_TextBox.Leave += new EventHandler(this.modem_TextBox_Leave);
      this.modem_TextBox.KeyPress += new KeyPressEventHandler(this.modem_TextBox_KeyPress);
      this.decryptionReenter_Label.AutoSize = true;
      this.decryptionReenter_Label.ForeColor = SystemColors.ControlText;
      this.decryptionReenter_Label.Location = new Point(53, 134);
      this.decryptionReenter_Label.Name = "decryptionReenter_Label";
      this.decryptionReenter_Label.Size = new Size(45, 13);
      this.decryptionReenter_Label.TabIndex = 10;
      this.decryptionReenter_Label.Text = "Reenter";
      this.groupBox1.Controls.Add((Control) this.phoneNumber_RadioButton);
      this.groupBox1.Controls.Add((Control) this.imei_RadioButton);
      this.groupBox1.Controls.Add((Control) this.decryptionReenter_Label);
      this.groupBox1.Controls.Add((Control) this.modem_TextBox);
      this.groupBox1.Controls.Add((Control) this.encryption_CheckBox);
      this.groupBox1.Controls.Add((Control) this.decryption_CheckBox);
      this.groupBox1.Controls.Add((Control) this.encryptionReenter_Label);
      this.groupBox1.Controls.Add((Control) this.dKey2_MaskedTextBox);
      this.groupBox1.Controls.Add((Control) this.modemInfo_Label);
      this.groupBox1.Controls.Add((Control) this.dKey1_MaskedTextBox);
      this.groupBox1.Controls.Add((Control) this.eKey2_MaskedTextBox);
      this.groupBox1.Controls.Add((Control) this.eKey1_MaskedTextBox);
      this.groupBox1.ForeColor = SystemColors.HotTrack;
      this.groupBox1.Location = new Point(14, (int) sbyte.MaxValue);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(613, 171);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Modem Properties";
      this.phoneNumber_RadioButton.AutoSize = true;
      this.phoneNumber_RadioButton.ForeColor = SystemColors.ControlText;
      this.phoneNumber_RadioButton.Location = new Point(322, 28);
      this.phoneNumber_RadioButton.Name = "phoneNumber_RadioButton";
      this.phoneNumber_RadioButton.Size = new Size(94, 17);
      this.phoneNumber_RadioButton.TabIndex = 3;
      this.phoneNumber_RadioButton.Text = "Phone number";
      this.phoneNumber_RadioButton.UseVisualStyleBackColor = true;
      this.phoneNumber_RadioButton.Click += new EventHandler(this.OnPhoneNumberRadioButtonClick);
      this.imei_RadioButton.AutoSize = true;
      this.imei_RadioButton.Checked = true;
      this.imei_RadioButton.ForeColor = SystemColors.ControlText;
      this.imei_RadioButton.Location = new Point(269, 28);
      this.imei_RadioButton.Name = "imei_RadioButton";
      this.imei_RadioButton.Size = new Size(47, 17);
      this.imei_RadioButton.TabIndex = 2;
      this.imei_RadioButton.TabStop = true;
      this.imei_RadioButton.Text = "IMEI";
      this.imei_RadioButton.UseVisualStyleBackColor = true;
      this.imei_RadioButton.Click += new EventHandler(this.OnImeiRadioButtonClick);
      this.groupBox2.Controls.Add((Control) this.userName_Label);
      this.groupBox2.Controls.Add((Control) this.password_TextBox);
      this.groupBox2.Controls.Add((Control) this.password_Label);
      this.groupBox2.Controls.Add((Control) this.user_TextBox);
      this.groupBox2.ForeColor = SystemColors.HotTrack;
      this.groupBox2.Location = new Point(14, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(613, 95);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "User Properties";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(641, 350);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.close_Button);
      this.Controls.Add((Control) this.submit_Button);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (AccountProperties_Form);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Account Properties";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
