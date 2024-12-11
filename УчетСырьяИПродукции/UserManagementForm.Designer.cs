using System.Windows.Forms;

namespace SQLiteViewer
{
    partial class UserManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private TabPage tabAddUser;
        private TabPage tabDeleteUser;
        private Button buttonAdd;
        private Button buttonDelete;
        private TextBox textBoxFullName;
        private TextBox textBoxUsername;
        private TextBox textBoxPassword;
        private ComboBox comboBoxDepartment;
        private ComboBox comboBoxPosition;
        private ComboBox comboBoxDeleteUser;
        private Label labelFullName;
        private Label labelUsername;
        private Label labelPassword;
        private Label labelDepartment;
        private Label labelPosition;
        private Label labelDeleteUser;

        // Clean up any resources being used.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAddUser = new System.Windows.Forms.TabPage();
            this.labelFullName = new System.Windows.Forms.Label();
            this.textBoxFullName = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.comboBoxDepartment = new System.Windows.Forms.ComboBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.comboBoxPosition = new System.Windows.Forms.ComboBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabDeleteUser = new System.Windows.Forms.TabPage();
            this.labelDeleteUser = new System.Windows.Forms.Label();
            this.comboBoxDeleteUser = new System.Windows.Forms.ComboBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.comboBoxRole = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabAddUser.SuspendLayout();
            this.tabDeleteUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAddUser);
            this.tabControl.Controls.Add(this.tabDeleteUser);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 323);
            this.tabControl.TabIndex = 0;
            // 
            // tabAddUser
            // 
            this.tabAddUser.Controls.Add(this.label1);
            this.tabAddUser.Controls.Add(this.comboBoxRole);
            this.tabAddUser.Controls.Add(this.labelFullName);
            this.tabAddUser.Controls.Add(this.textBoxFullName);
            this.tabAddUser.Controls.Add(this.labelUsername);
            this.tabAddUser.Controls.Add(this.textBoxUsername);
            this.tabAddUser.Controls.Add(this.labelPassword);
            this.tabAddUser.Controls.Add(this.textBoxPassword);
            this.tabAddUser.Controls.Add(this.labelDepartment);
            this.tabAddUser.Controls.Add(this.comboBoxDepartment);
            this.tabAddUser.Controls.Add(this.labelPosition);
            this.tabAddUser.Controls.Add(this.comboBoxPosition);
            this.tabAddUser.Controls.Add(this.buttonAdd);
            this.tabAddUser.Location = new System.Drawing.Point(4, 25);
            this.tabAddUser.Name = "tabAddUser";
            this.tabAddUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabAddUser.Size = new System.Drawing.Size(376, 294);
            this.tabAddUser.TabIndex = 0;
            this.tabAddUser.Text = "Добавить пользователя";
            this.tabAddUser.UseVisualStyleBackColor = true;
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(20, 23);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(41, 16);
            this.labelFullName.TabIndex = 5;
            this.labelFullName.Text = "ФИО:";
            // 
            // textBoxFullName
            // 
            this.textBoxFullName.Location = new System.Drawing.Point(140, 20);
            this.textBoxFullName.Name = "textBoxFullName";
            this.textBoxFullName.Size = new System.Drawing.Size(200, 22);
            this.textBoxFullName.TabIndex = 0;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(20, 63);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(49, 16);
            this.labelUsername.TabIndex = 6;
            this.labelUsername.Text = "Логин:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(140, 60);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(200, 22);
            this.textBoxUsername.TabIndex = 1;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(20, 103);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(59, 16);
            this.labelPassword.TabIndex = 7;
            this.labelPassword.Text = "Пароль:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(140, 100);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(200, 22);
            this.textBoxPassword.TabIndex = 2;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Location = new System.Drawing.Point(20, 143);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(116, 16);
            this.labelDepartment.TabIndex = 8;
            this.labelDepartment.Text = "Подразделение:";
            // 
            // comboBoxDepartment
            // 
            this.comboBoxDepartment.FormattingEnabled = true;
            this.comboBoxDepartment.Location = new System.Drawing.Point(140, 140);
            this.comboBoxDepartment.Name = "comboBoxDepartment";
            this.comboBoxDepartment.Size = new System.Drawing.Size(200, 24);
            this.comboBoxDepartment.TabIndex = 3;
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(20, 186);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(81, 16);
            this.labelPosition.TabIndex = 9;
            this.labelPosition.Text = "Должность:";
            // 
            // comboBoxPosition
            // 
            this.comboBoxPosition.FormattingEnabled = true;
            this.comboBoxPosition.Location = new System.Drawing.Point(140, 183);
            this.comboBoxPosition.Name = "comboBoxPosition";
            this.comboBoxPosition.Size = new System.Drawing.Size(200, 24);
            this.comboBoxPosition.TabIndex = 4;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(140, 262);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(82, 28);
            this.buttonAdd.TabIndex = 10;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // tabDeleteUser
            // 
            this.tabDeleteUser.Controls.Add(this.labelDeleteUser);
            this.tabDeleteUser.Controls.Add(this.comboBoxDeleteUser);
            this.tabDeleteUser.Controls.Add(this.buttonDelete);
            this.tabDeleteUser.Location = new System.Drawing.Point(4, 25);
            this.tabDeleteUser.Name = "tabDeleteUser";
            this.tabDeleteUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeleteUser.Size = new System.Drawing.Size(376, 251);
            this.tabDeleteUser.TabIndex = 1;
            this.tabDeleteUser.Text = "Удалить пользователя";
            this.tabDeleteUser.UseVisualStyleBackColor = true;
            // 
            // labelDeleteUser
            // 
            this.labelDeleteUser.AutoSize = true;
            this.labelDeleteUser.Location = new System.Drawing.Point(88, 12);
            this.labelDeleteUser.Name = "labelDeleteUser";
            this.labelDeleteUser.Size = new System.Drawing.Size(162, 16);
            this.labelDeleteUser.TabIndex = 9;
            this.labelDeleteUser.Text = "Выбрать пользователя:";
            // 
            // comboBoxDeleteUser
            // 
            this.comboBoxDeleteUser.FormattingEnabled = true;
            this.comboBoxDeleteUser.Location = new System.Drawing.Point(73, 43);
            this.comboBoxDeleteUser.Name = "comboBoxDeleteUser";
            this.comboBoxDeleteUser.Size = new System.Drawing.Size(200, 24);
            this.comboBoxDeleteUser.TabIndex = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(129, 83);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 31);
            this.buttonDelete.TabIndex = 10;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // comboBoxRole
            // 
            this.comboBoxRole.FormattingEnabled = true;
            this.comboBoxRole.Location = new System.Drawing.Point(140, 225);
            this.comboBoxRole.Name = "comboBoxRole";
            this.comboBoxRole.Size = new System.Drawing.Size(200, 24);
            this.comboBoxRole.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Роль:";
            // 
            // UserManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 323);
            this.Controls.Add(this.tabControl);
            this.Name = "UserManagementForm";
            this.Text = "Управление пользоватеями";
            this.tabControl.ResumeLayout(false);
            this.tabAddUser.ResumeLayout(false);
            this.tabAddUser.PerformLayout();
            this.tabDeleteUser.ResumeLayout(false);
            this.tabDeleteUser.PerformLayout();
            this.ResumeLayout(false);

        }

        private Label label1;
        private ComboBox comboBoxRole;
    }
}
