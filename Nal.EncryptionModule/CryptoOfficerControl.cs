// Decompiled with JetBrains decompiler
// Type: Nal.EncryptionModule.CryptoOfficerControl
// Assembly: Nal.EncryptionModule, Version=1.0.2.1, Culture=neutral, PublicKeyToken=null
// MVID: B34C070D-0D34-47B1-A672-01BEC94528EC
// Assembly location: C:\Program Files (x86)\NAL\Server for Trackers Service\Nal.EncryptionModule.DLL

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Nal.EncryptionModule
{
  public class CryptoOfficerControl : UserControl
  {
    private const int UserNameCol = 0;
    private const int ModemInfoCol = 1;
    private const int ModemInfoTypeCol = 2;
    private int currSortCol;
    private int failedLoginCount;
    private CryptoOfficerRole officer = new CryptoOfficerRole();
    private List<CryptoOfficerRole.Record> previousRecords = new List<CryptoOfficerRole.Record>();
    private IContainer components;
    private Panel account_Panel;
    private Panel password_Panel;
    private Panel newPassword_Panel;
    private Panel firstTime_Panel;
    private Button logout_Button;
    private Button cancel_Button;
    private Button save_Button;
    private Button selfTest_Button;
    private Button password_Button;
    private Button clearAll_Button;
    private Button delete_Button;
    private Button modify_Button;
    private Button add_Button;
    private DataGridView accounts_DataGridView;
    private Label account_Label;
    private TextBox password_TextBox;
    private Button login_Button;
    private Label enterPassword_Label;
    private TextBox newPassword1_TextBox;
    private TextBox newPassword2_TextBox;
    private Button cancelNewPassword_Button;
    private Button okNewPassword_Button;
    private Label setPassword_Label;
    private Label firstTime_Label;
    private Timer security_Timer;
    private Button about_Button;
    private Panel selfTestFailed_Panel;
    private Label label1;
    private NotifyIcon notifyIcon1;
    private DataGridViewTextBoxColumn userName;
    private DataGridViewTextBoxColumn imeiOrPhoneNumber;
    private DataGridViewTextBoxColumn Column1;

    public CryptoOfficerControl() => this.InitializeComponent();

    public event EventHandler LoggedOut;

    public string KeysDirectory
    {
      get => this.officer.KeysDirectory;
      set
      {
        this.officer.KeysDirectory = value;
        this.ShowLoginScreen();
      }
    }

    public bool IsLoggedIn() => this.officer.IsLoggedIn();

    private void CryptoOfficerControl_Load(object sender, EventArgs e)
    {
      this.ParentForm.FormClosing += new FormClosingEventHandler(this.parentForm_FormClosing);
      this.ShowLoginScreen();
    }

    private void parentForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.officer.IsLoggedIn())
        return;
      this.Logout();
    }

    private void password_TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          this.login_Button.Focus();
          break;
        case '"':
          e.KeyChar = char.MinValue;
          break;
      }
    }

    private void newPassword1_TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          this.login_Button.Focus();
          break;
        case '"':
          e.KeyChar = char.MinValue;
          break;
      }
    }

    private void newPassword2_TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          this.login_Button.Focus();
          break;
        case '"':
          e.KeyChar = char.MinValue;
          break;
      }
    }

    private void login_Button_Click(object sender, EventArgs e)
    {
      if (this.officer.RunSelfTests() != Erc.Success)
      {
        this.password_Panel.Hide();
        this.password_Panel.Enabled = false;
        this.selfTestFailed_Panel.Show();
      }
      else
      {
        Erc code1 = this.officer.Login(this.password_TextBox.Text);
        switch (code1)
        {
          case Erc.Success:
            this.updateAccountDataGrid();
            this.failedLoginCount = 0;
            this.account_Panel.Show();
            this.account_Panel.Enabled = true;
            this.password_TextBox.Text = string.Empty;
            this.password_Panel.Hide();
            this.password_Panel.Enabled = false;
            this.firstTime_Panel.Hide();
            this.accounts_DataGridView.Focus();
            break;
          case Erc.CryptoOfficerFileLocked:
            if (MessageBox.Show("This application is currentlly locked.\nForcing an unlock may cause unexpected changes.\nDo you want to force an unlock?", "Unlock Crypto Officer Functionality", MessageBoxButtons.YesNo) != DialogResult.Yes)
              break;
            Erc code2 = this.officer.UnlockCoFile();
            if (code2 == Erc.Success)
              break;
            int num1 = (int) MessageBox.Show(ErcStr.GetStringForCode(code2), "Error");
            break;
          default:
            ++this.failedLoginCount;
            string stringForCode = ErcStr.GetStringForCode(code1);
            if (this.failedLoginCount > 3)
            {
              int num2 = (int) MessageBox.Show(stringForCode + "\nFor security reasons, you will not be able to try again for 10 seconds.", "Error", MessageBoxButtons.OK);
              this.security_Timer.Enabled = true;
              this.password_Panel.Hide();
              this.newPassword_Panel.Hide();
              this.firstTime_Panel.Hide();
              this.account_Panel.Hide();
              break;
            }
            int num3 = (int) MessageBox.Show(stringForCode + "\nPlease try your password again.", "Error", MessageBoxButtons.OK);
            break;
        }
      }
    }

    private void okNewPassword_Button_Click(object sender, EventArgs e)
    {
      if (this.newPassword1_TextBox.Text != this.newPassword2_TextBox.Text)
      {
        int num1 = (int) MessageBox.Show("Password do not match.", "Error", MessageBoxButtons.OK);
      }
      else
      {
        if (this.firstTime_Panel.Visible)
        {
          if (this.officer.RunSelfTests() != Erc.Success)
          {
            this.newPassword_Panel.Hide();
            this.newPassword_Panel.Enabled = false;
            this.firstTime_Panel.Hide();
            this.selfTestFailed_Panel.Show();
            return;
          }
          Erc code = this.officer.Login(this.newPassword1_TextBox.Text);
          if (code != Erc.Success)
          {
            int num2 = (int) MessageBox.Show(ErcStr.GetStringForCode(code), "Error");
            return;
          }
          this.firstTime_Panel.Hide();
        }
        else
        {
          Erc code = this.officer.UpdatePassword(this.newPassword1_TextBox.Text);
          if (code != Erc.Success)
          {
            int num3 = (int) MessageBox.Show(ErcStr.GetStringForCode(code), "Error");
            return;
          }
          int num4 = (int) MessageBox.Show("The Crypto Officer password was updated.", "Success");
        }
        this.account_Panel.Show();
        this.account_Panel.Enabled = true;
        this.newPassword1_TextBox.Text = string.Empty;
        this.newPassword2_TextBox.Text = string.Empty;
        this.newPassword_Panel.Hide();
        this.newPassword_Panel.Enabled = false;
        this.accounts_DataGridView.Focus();
      }
    }

    private void cancelNewPassword_Button_Click(object sender, EventArgs e)
    {
      this.newPassword_Panel.Hide();
      this.account_Panel.Show();
      this.account_Panel.Enabled = true;
      this.newPassword1_TextBox.Text = string.Empty;
      this.newPassword2_TextBox.Text = string.Empty;
    }

    private void accounts_DataGridView_SortCompare(
      object sender,
      DataGridViewSortCompareEventArgs e)
    {
      this.currSortCol = e.Column.Index;
      e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
      if (e.SortResult == 0)
      {
        int index = this.currSortCol != 0 ? 0 : 1;
        e.SortResult = string.Compare(this.accounts_DataGridView.Rows[e.RowIndex1].Cells[index].Value.ToString(), this.accounts_DataGridView.Rows[e.RowIndex2].Cells[index].Value.ToString());
        if (this.accounts_DataGridView.SortOrder == SortOrder.Descending)
          e.SortResult *= -1;
      }
      e.Handled = true;
    }

    private void add_Button_Click(object sender, EventArgs e)
    {
      AccountProperties_Form accountPropertiesForm = new AccountProperties_Form(this.officer);
      accountPropertiesForm.FormClosed += new FormClosedEventHandler(this.accountProperties_FormClosed);
      int num = (int) accountPropertiesForm.ShowDialog();
    }

    private void modify_Button_Click(object sender, EventArgs e)
    {
      if (this.accounts_DataGridView.CurrentRow == null)
      {
        int num1 = (int) MessageBox.Show("No Record was selected.", "Error", MessageBoxButtons.OK);
      }
      else
      {
        AccountProperties_Form accountPropertiesForm = new AccountProperties_Form(this.officer, this.getSelectedElement(0), this.getSelectedElement(1), this.getSelectedElement(2));
        accountPropertiesForm.FormClosed += new FormClosedEventHandler(this.accountProperties_FormClosed);
        int num2 = (int) accountPropertiesForm.ShowDialog();
      }
    }

    private void delete_Button_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Are you sure? This entry will be deleted.", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (this.accounts_DataGridView.CurrentRow == null)
      {
        int num1 = (int) MessageBox.Show("There are no accounts to delete.", "Error", MessageBoxButtons.OK);
      }
      else
      {
        string user = this.accounts_DataGridView.CurrentRow.Cells[0].Value.ToString();
        string modemInfo = this.accounts_DataGridView.CurrentRow.Cells[1].Value.ToString();
        string modemInfoType = this.accounts_DataGridView.CurrentRow.Cells[2].Value.ToString();
        Erc code = this.officer.DeleteRecord(user, modemInfo, modemInfoType);
        if (code != Erc.Success)
        {
          int num2 = (int) MessageBox.Show(ErcStr.GetStringForCode(code), "Error", MessageBoxButtons.OK);
        }
        else
        {
          this.officer.GetPasswordForUser(user);
          this.updateAccountDataGrid();
        }
      }
    }

    private void clearAll_Button_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Are you sure? All accounts will be deleted.", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      this.officer.DeleteAllRecords();
      this.updateAccountDataGrid();
    }

    private void password_Button_Click(object sender, EventArgs e)
    {
      this.newPassword_Panel.Show();
      this.newPassword_Panel.Enabled = true;
      this.account_Panel.Hide();
      this.account_Panel.Enabled = false;
      this.cancelNewPassword_Button.Enabled = true;
    }

    private void selfTest_Button_Click(object sender, EventArgs e)
    {
      if (this.officer.RunSelfTests() != Erc.Success)
      {
        this.Logout();
        this.password_Panel.Hide();
        this.password_Panel.Enabled = false;
        this.selfTestFailed_Panel.Show();
      }
      else
      {
        int num = (int) MessageBox.Show("Software Integrity Test Passed\nAES and Triple DES Self Tests Passed", "Success", MessageBoxButtons.OK);
      }
    }

    private void OnAboutButtonClick(object sender, EventArgs e)
    {
      int num = (int) new About_Form().ShowDialog((IWin32Window) this.ParentForm);
    }

    private void OnCancelButtonClick(object sender, EventArgs e)
    {
      this.officer.CancelChanges();
      this.updateAccountDataGrid();
    }

    private void OnSaveButtonClick(object sender, EventArgs e)
    {
      string errors;
      Erc code = this.officer.ApplyChanges(out errors);
      if (code == Erc.Success)
        return;
      int num = (int) MessageBox.Show(ErcStr.GetStringForCode(code) + "\n" + errors, "Error");
    }

    private void OnLogoutButtonClick(object sender, EventArgs e)
    {
      this.Logout();
      if (this.LoggedOut == null)
        return;
      this.LoggedOut((object) this, EventArgs.Empty);
    }

    private void security_Timer_Tick(object sender, EventArgs e)
    {
      this.security_Timer.Enabled = false;
      this.password_Panel.Show();
      if (File.Exists(this.officer.KeysDirectory + "\\" + Utils.CryptoOfficerFileName))
        return;
      this.firstTime_Panel.Show();
    }

    private void accountProperties_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.updateAccountDataGrid();
    }

    private string getSelectedElement(int column)
    {
      return this.accounts_DataGridView.CurrentRow.Cells[column].Value.ToString();
    }

    private void updateAccountDataGrid()
    {
      this.accounts_DataGridView.Rows.Clear();
      foreach (string user in this.officer.GetUsers())
      {
        foreach (string str in this.officer.GetModemsForUser(user, "I"))
        {
          int rowIndex = this.accounts_DataGridView.Rows.Add();
          this.accounts_DataGridView[0, rowIndex].Value = (object) user;
          this.accounts_DataGridView[1, rowIndex].Value = (object) str;
          this.accounts_DataGridView[2, rowIndex].Value = (object) "I";
        }
        foreach (string str in this.officer.GetModemsForUser(user, "P"))
        {
          int rowIndex = this.accounts_DataGridView.Rows.Add();
          this.accounts_DataGridView[0, rowIndex].Value = (object) user;
          this.accounts_DataGridView[1, rowIndex].Value = (object) str;
          this.accounts_DataGridView[2, rowIndex].Value = (object) "P";
        }
      }
      this.accounts_DataGridView.Sort(this.accounts_DataGridView.Columns[this.currSortCol], ListSortDirection.Ascending);
    }

    private void Logout()
    {
      int num = (int) this.officer.Logout();
      this.account_Panel.Hide();
      this.account_Panel.Enabled = false;
      this.password_Panel.Show();
      this.password_Panel.Enabled = true;
    }

    private void ShowLoginScreen()
    {
      this.account_Panel.Hide();
      this.account_Panel.Enabled = false;
      this.password_TextBox.Text = string.Empty;
      this.newPassword1_TextBox.Text = string.Empty;
      this.newPassword2_TextBox.Text = string.Empty;
      if (!File.Exists(this.officer.KeysDirectory + "\\" + Utils.CryptoOfficerFileName))
      {
        this.firstTime_Panel.Show();
        this.newPassword_Panel.Show();
        this.newPassword_Panel.Enabled = true;
        this.cancelNewPassword_Button.Enabled = false;
        this.password_Panel.Hide();
        this.password_Panel.Enabled = false;
      }
      else
      {
        this.password_Panel.Show();
        this.password_Panel.Enabled = true;
        this.firstTime_Panel.Hide();
        this.newPassword_Panel.Hide();
        this.newPassword_Panel.Enabled = false;
        this.cancelNewPassword_Button.Enabled = false;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.account_Panel = new Panel();
      this.about_Button = new Button();
      this.account_Label = new Label();
      this.logout_Button = new Button();
      this.cancel_Button = new Button();
      this.save_Button = new Button();
      this.selfTest_Button = new Button();
      this.password_Button = new Button();
      this.clearAll_Button = new Button();
      this.delete_Button = new Button();
      this.modify_Button = new Button();
      this.add_Button = new Button();
      this.accounts_DataGridView = new DataGridView();
      this.userName = new DataGridViewTextBoxColumn();
      this.imeiOrPhoneNumber = new DataGridViewTextBoxColumn();
      this.Column1 = new DataGridViewTextBoxColumn();
      this.firstTime_Panel = new Panel();
      this.firstTime_Label = new Label();
      this.password_Panel = new Panel();
      this.password_TextBox = new TextBox();
      this.login_Button = new Button();
      this.enterPassword_Label = new Label();
      this.newPassword_Panel = new Panel();
      this.newPassword1_TextBox = new TextBox();
      this.newPassword2_TextBox = new TextBox();
      this.cancelNewPassword_Button = new Button();
      this.okNewPassword_Button = new Button();
      this.setPassword_Label = new Label();
      this.security_Timer = new Timer(this.components);
      this.selfTestFailed_Panel = new Panel();
      this.label1 = new Label();
      this.notifyIcon1 = new NotifyIcon(this.components);
      this.account_Panel.SuspendLayout();
      ((ISupportInitialize) this.accounts_DataGridView).BeginInit();
      this.firstTime_Panel.SuspendLayout();
      this.password_Panel.SuspendLayout();
      this.newPassword_Panel.SuspendLayout();
      this.selfTestFailed_Panel.SuspendLayout();
      this.SuspendLayout();
      this.account_Panel.BackColor = SystemColors.Control;
      this.account_Panel.Controls.Add((Control) this.about_Button);
      this.account_Panel.Controls.Add((Control) this.account_Label);
      this.account_Panel.Controls.Add((Control) this.logout_Button);
      this.account_Panel.Controls.Add((Control) this.cancel_Button);
      this.account_Panel.Controls.Add((Control) this.save_Button);
      this.account_Panel.Controls.Add((Control) this.selfTest_Button);
      this.account_Panel.Controls.Add((Control) this.password_Button);
      this.account_Panel.Controls.Add((Control) this.clearAll_Button);
      this.account_Panel.Controls.Add((Control) this.delete_Button);
      this.account_Panel.Controls.Add((Control) this.modify_Button);
      this.account_Panel.Controls.Add((Control) this.add_Button);
      this.account_Panel.Controls.Add((Control) this.accounts_DataGridView);
      this.account_Panel.Enabled = false;
      this.account_Panel.Location = new Point(0, 0);
      this.account_Panel.Name = "account_Panel";
      this.account_Panel.Size = new Size(414, 444);
      this.account_Panel.TabIndex = 0;
      this.account_Panel.Visible = false;
      this.about_Button.Location = new Point(318, 268);
      this.about_Button.Name = "about_Button";
      this.about_Button.Size = new Size(75, 23);
      this.about_Button.TabIndex = 8;
      this.about_Button.Text = "About...";
      this.about_Button.UseVisualStyleBackColor = true;
      this.about_Button.Click += new EventHandler(this.OnAboutButtonClick);
      this.account_Label.AutoSize = true;
      this.account_Label.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.account_Label.Location = new Point(3, 10);
      this.account_Label.Name = "account_Label";
      this.account_Label.Size = new Size(102, 25);
      this.account_Label.TabIndex = 0;
      this.account_Label.Text = "Accounts";
      this.logout_Button.Location = new Point(318, 389);
      this.logout_Button.Name = "logout_Button";
      this.logout_Button.Size = new Size(75, 23);
      this.logout_Button.TabIndex = 11;
      this.logout_Button.Text = "Logout";
      this.logout_Button.UseVisualStyleBackColor = true;
      this.logout_Button.Click += new EventHandler(this.OnLogoutButtonClick);
      this.cancel_Button.Location = new Point(318, 360);
      this.cancel_Button.Name = "cancel_Button";
      this.cancel_Button.Size = new Size(75, 23);
      this.cancel_Button.TabIndex = 10;
      this.cancel_Button.Text = "Cancel";
      this.cancel_Button.UseVisualStyleBackColor = true;
      this.cancel_Button.Click += new EventHandler(this.OnCancelButtonClick);
      this.save_Button.Location = new Point(318, 331);
      this.save_Button.Name = "save_Button";
      this.save_Button.Size = new Size(75, 23);
      this.save_Button.TabIndex = 9;
      this.save_Button.Text = "Save";
      this.save_Button.UseVisualStyleBackColor = true;
      this.save_Button.Click += new EventHandler(this.OnSaveButtonClick);
      this.selfTest_Button.Location = new Point(318, 239);
      this.selfTest_Button.Name = "selfTest_Button";
      this.selfTest_Button.Size = new Size(75, 23);
      this.selfTest_Button.TabIndex = 7;
      this.selfTest_Button.Text = "Self Test";
      this.selfTest_Button.UseVisualStyleBackColor = true;
      this.selfTest_Button.Click += new EventHandler(this.selfTest_Button_Click);
      this.password_Button.Location = new Point(318, 210);
      this.password_Button.Name = "password_Button";
      this.password_Button.Size = new Size(75, 23);
      this.password_Button.TabIndex = 6;
      this.password_Button.Text = "Password";
      this.password_Button.UseVisualStyleBackColor = true;
      this.password_Button.Click += new EventHandler(this.password_Button_Click);
      this.clearAll_Button.Location = new Point(318, 130);
      this.clearAll_Button.Name = "clearAll_Button";
      this.clearAll_Button.Size = new Size(75, 23);
      this.clearAll_Button.TabIndex = 5;
      this.clearAll_Button.Text = "Clear All";
      this.clearAll_Button.UseVisualStyleBackColor = true;
      this.clearAll_Button.Click += new EventHandler(this.clearAll_Button_Click);
      this.delete_Button.Location = new Point(318, 101);
      this.delete_Button.Name = "delete_Button";
      this.delete_Button.Size = new Size(75, 23);
      this.delete_Button.TabIndex = 4;
      this.delete_Button.Text = "Delete";
      this.delete_Button.UseVisualStyleBackColor = true;
      this.delete_Button.Click += new EventHandler(this.delete_Button_Click);
      this.modify_Button.Location = new Point(318, 72);
      this.modify_Button.Name = "modify_Button";
      this.modify_Button.Size = new Size(75, 23);
      this.modify_Button.TabIndex = 3;
      this.modify_Button.Text = "Modify";
      this.modify_Button.UseVisualStyleBackColor = true;
      this.modify_Button.Click += new EventHandler(this.modify_Button_Click);
      this.add_Button.Location = new Point(318, 43);
      this.add_Button.Name = "add_Button";
      this.add_Button.Size = new Size(75, 23);
      this.add_Button.TabIndex = 2;
      this.add_Button.Text = "Add";
      this.add_Button.UseVisualStyleBackColor = true;
      this.add_Button.Click += new EventHandler(this.add_Button_Click);
      this.accounts_DataGridView.AllowUserToAddRows = false;
      this.accounts_DataGridView.AllowUserToDeleteRows = false;
      this.accounts_DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.accounts_DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.accounts_DataGridView.Columns.AddRange((DataGridViewColumn) this.userName, (DataGridViewColumn) this.imeiOrPhoneNumber, (DataGridViewColumn) this.Column1);
      this.accounts_DataGridView.Location = new Point(0, 43);
      this.accounts_DataGridView.MultiSelect = false;
      this.accounts_DataGridView.Name = "accounts_DataGridView";
      this.accounts_DataGridView.ReadOnly = true;
      this.accounts_DataGridView.Size = new Size(312, 401);
      this.accounts_DataGridView.TabIndex = 1;
      this.accounts_DataGridView.SortCompare += new DataGridViewSortCompareEventHandler(this.accounts_DataGridView_SortCompare);
      this.userName.FillWeight = 114.2132f;
      this.userName.HeaderText = "User Name";
      this.userName.Name = "userName";
      this.userName.ReadOnly = true;
      this.imeiOrPhoneNumber.FillWeight = 160.6226f;
      this.imeiOrPhoneNumber.HeaderText = "Modem Info";
      this.imeiOrPhoneNumber.Name = "imeiOrPhoneNumber";
      this.imeiOrPhoneNumber.ReadOnly = true;
      this.Column1.FillWeight = 25.16421f;
      this.Column1.HeaderText = "T";
      this.Column1.Name = "Column1";
      this.Column1.ReadOnly = true;
      this.firstTime_Panel.BackColor = SystemColors.Control;
      this.firstTime_Panel.Controls.Add((Control) this.firstTime_Label);
      this.firstTime_Panel.Location = new Point(60, 130);
      this.firstTime_Panel.Name = "firstTime_Panel";
      this.firstTime_Panel.Size = new Size(283, 26);
      this.firstTime_Panel.TabIndex = 1;
      this.firstTime_Panel.Visible = false;
      this.firstTime_Label.AutoSize = true;
      this.firstTime_Label.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.firstTime_Label.Location = new Point(27, 3);
      this.firstTime_Label.Name = "firstTime_Label";
      this.firstTime_Label.Size = new Size(225, 20);
      this.firstTime_Label.TabIndex = 0;
      this.firstTime_Label.Text = "First time setting the password";
      this.password_Panel.BackColor = SystemColors.Control;
      this.password_Panel.Controls.Add((Control) this.password_TextBox);
      this.password_Panel.Controls.Add((Control) this.login_Button);
      this.password_Panel.Controls.Add((Control) this.enterPassword_Label);
      this.password_Panel.Enabled = false;
      this.password_Panel.Location = new Point(60, 159);
      this.password_Panel.Name = "password_Panel";
      this.password_Panel.Size = new Size(283, (int) sbyte.MaxValue);
      this.password_Panel.TabIndex = 1;
      this.password_Panel.Visible = false;
      this.password_TextBox.Location = new Point(24, 50);
      this.password_TextBox.Name = "password_TextBox";
      this.password_TextBox.PasswordChar = '*';
      this.password_TextBox.Size = new Size(247, 20);
      this.password_TextBox.TabIndex = 1;
      this.password_TextBox.KeyPress += new KeyPressEventHandler(this.password_TextBox_KeyPress);
      this.login_Button.Location = new Point(108, 76);
      this.login_Button.Name = "login_Button";
      this.login_Button.Size = new Size(75, 23);
      this.login_Button.TabIndex = 2;
      this.login_Button.Text = "Login";
      this.login_Button.UseVisualStyleBackColor = true;
      this.login_Button.Click += new EventHandler(this.login_Button_Click);
      this.enterPassword_Label.AutoSize = true;
      this.enterPassword_Label.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.enterPassword_Label.Location = new Point(20, 27);
      this.enterPassword_Label.Name = "enterPassword_Label";
      this.enterPassword_Label.Size = new Size(251, 20);
      this.enterPassword_Label.TabIndex = 0;
      this.enterPassword_Label.Text = "Enter Crypto Officer Password";
      this.newPassword_Panel.BackColor = SystemColors.Control;
      this.newPassword_Panel.Controls.Add((Control) this.newPassword1_TextBox);
      this.newPassword_Panel.Controls.Add((Control) this.newPassword2_TextBox);
      this.newPassword_Panel.Controls.Add((Control) this.cancelNewPassword_Button);
      this.newPassword_Panel.Controls.Add((Control) this.okNewPassword_Button);
      this.newPassword_Panel.Controls.Add((Control) this.setPassword_Label);
      this.newPassword_Panel.Location = new Point(60, 159);
      this.newPassword_Panel.Name = "newPassword_Panel";
      this.newPassword_Panel.Size = new Size(283, (int) sbyte.MaxValue);
      this.newPassword_Panel.TabIndex = 2;
      this.newPassword_Panel.Visible = false;
      this.newPassword1_TextBox.Location = new Point(28, 41);
      this.newPassword1_TextBox.Name = "newPassword1_TextBox";
      this.newPassword1_TextBox.PasswordChar = '*';
      this.newPassword1_TextBox.Size = new Size(231, 20);
      this.newPassword1_TextBox.TabIndex = 1;
      this.newPassword1_TextBox.KeyPress += new KeyPressEventHandler(this.newPassword1_TextBox_KeyPress);
      this.newPassword2_TextBox.Location = new Point(28, 67);
      this.newPassword2_TextBox.Name = "newPassword2_TextBox";
      this.newPassword2_TextBox.PasswordChar = '*';
      this.newPassword2_TextBox.Size = new Size(231, 20);
      this.newPassword2_TextBox.TabIndex = 2;
      this.newPassword2_TextBox.KeyPress += new KeyPressEventHandler(this.newPassword2_TextBox_KeyPress);
      this.cancelNewPassword_Button.Location = new Point(143, 92);
      this.cancelNewPassword_Button.Name = "cancelNewPassword_Button";
      this.cancelNewPassword_Button.Size = new Size(75, 23);
      this.cancelNewPassword_Button.TabIndex = 4;
      this.cancelNewPassword_Button.Text = "Cancel";
      this.cancelNewPassword_Button.UseVisualStyleBackColor = true;
      this.cancelNewPassword_Button.Click += new EventHandler(this.cancelNewPassword_Button_Click);
      this.okNewPassword_Button.Location = new Point(62, 92);
      this.okNewPassword_Button.Name = "okNewPassword_Button";
      this.okNewPassword_Button.Size = new Size(75, 23);
      this.okNewPassword_Button.TabIndex = 3;
      this.okNewPassword_Button.Text = "OK";
      this.okNewPassword_Button.UseVisualStyleBackColor = true;
      this.okNewPassword_Button.Click += new EventHandler(this.okNewPassword_Button_Click);
      this.setPassword_Label.AutoSize = true;
      this.setPassword_Label.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.setPassword_Label.Location = new Point(24, 18);
      this.setPassword_Label.Name = "setPassword_Label";
      this.setPassword_Label.Size = new Size(235, 20);
      this.setPassword_Label.TabIndex = 0;
      this.setPassword_Label.Text = "Set Crypto Officer Password";
      this.security_Timer.Interval = 10000;
      this.security_Timer.Tick += new EventHandler(this.security_Timer_Tick);
      this.selfTestFailed_Panel.BackColor = SystemColors.Control;
      this.selfTestFailed_Panel.Controls.Add((Control) this.label1);
      this.selfTestFailed_Panel.Location = new Point(60, 159);
      this.selfTestFailed_Panel.Name = "selfTestFailed_Panel";
      this.selfTestFailed_Panel.Size = new Size(283, (int) sbyte.MaxValue);
      this.selfTestFailed_Panel.TabIndex = 2;
      this.selfTestFailed_Panel.Visible = false;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(69, 38);
      this.label1.Name = "label1";
      this.label1.Size = new Size(149, 40);
      this.label1.TabIndex = 0;
      this.label1.Text = "Crypto Officer\r\nSelf Tests Failed.";
      this.label1.TextAlign = ContentAlignment.TopCenter;
      this.notifyIcon1.Text = "notifyIcon1";
      this.notifyIcon1.Visible = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.firstTime_Panel);
      this.Controls.Add((Control) this.newPassword_Panel);
      this.Controls.Add((Control) this.selfTestFailed_Panel);
      this.Controls.Add((Control) this.password_Panel);
      this.Controls.Add((Control) this.account_Panel);
      this.Name = nameof (CryptoOfficerControl);
      this.Size = new Size(414, 444);
      this.Load += new EventHandler(this.CryptoOfficerControl_Load);
      this.account_Panel.ResumeLayout(false);
      this.account_Panel.PerformLayout();
      ((ISupportInitialize) this.accounts_DataGridView).EndInit();
      this.firstTime_Panel.ResumeLayout(false);
      this.firstTime_Panel.PerformLayout();
      this.password_Panel.ResumeLayout(false);
      this.password_Panel.PerformLayout();
      this.newPassword_Panel.ResumeLayout(false);
      this.newPassword_Panel.PerformLayout();
      this.selfTestFailed_Panel.ResumeLayout(false);
      this.selfTestFailed_Panel.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
