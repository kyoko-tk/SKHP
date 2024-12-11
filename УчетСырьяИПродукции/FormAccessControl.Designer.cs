using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace SQLiteViewer
{
    partial class FormAccessControl
    {
        private System.Windows.Forms.ComboBox comboBoxRole;
        private ComboBox comboBoxUsers;
        private System.Windows.Forms.ListBox listBoxTables;
        private System.Windows.Forms.CheckBox checkBoxCanAdd;
        private System.Windows.Forms.CheckBox checkBoxCanDelete;
        private System.Windows.Forms.CheckBox checkBoxCanSave;
        private System.Windows.Forms.Button buttonSave;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccessControl));
            this.comboBoxUsers = new System.Windows.Forms.ComboBox();
            this.listBoxTables = new System.Windows.Forms.ListBox();
            this.checkBoxCanAdd = new System.Windows.Forms.CheckBox();
            this.checkBoxCanDelete = new System.Windows.Forms.CheckBox();
            this.checkBoxCanSave = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxUsers
            // 
            this.comboBoxUsers.FormattingEnabled = true;
            this.comboBoxUsers.Location = new System.Drawing.Point(12, 12);
            this.comboBoxUsers.Name = "comboBoxUsers";
            this.comboBoxUsers.Size = new System.Drawing.Size(322, 24);
            this.comboBoxUsers.TabIndex = 0;
            this.comboBoxUsers.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsers_SelectedIndexChanged);
            // 
            // listBoxTables
            // 
            this.listBoxTables.FormattingEnabled = true;
            this.listBoxTables.ItemHeight = 16;
            this.listBoxTables.Location = new System.Drawing.Point(12, 39);
            this.listBoxTables.Name = "listBoxTables";
            this.listBoxTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxTables.Size = new System.Drawing.Size(321, 228);
            this.listBoxTables.TabIndex = 1;
            // 
            // checkBoxCanAdd
            // 
            this.checkBoxCanAdd.AutoSize = true;
            this.checkBoxCanAdd.Location = new System.Drawing.Point(12, 296);
            this.checkBoxCanAdd.Name = "checkBoxCanAdd";
            this.checkBoxCanAdd.Size = new System.Drawing.Size(81, 20);
            this.checkBoxCanAdd.TabIndex = 2;
            this.checkBoxCanAdd.Text = "Can Add";
            this.checkBoxCanAdd.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanDelete
            // 
            this.checkBoxCanDelete.AutoSize = true;
            this.checkBoxCanDelete.Location = new System.Drawing.Point(12, 319);
            this.checkBoxCanDelete.Name = "checkBoxCanDelete";
            this.checkBoxCanDelete.Size = new System.Drawing.Size(96, 20);
            this.checkBoxCanDelete.TabIndex = 3;
            this.checkBoxCanDelete.Text = "Can Delete";
            this.checkBoxCanDelete.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanSave
            // 
            this.checkBoxCanSave.AutoSize = true;
            this.checkBoxCanSave.Location = new System.Drawing.Point(12, 342);
            this.checkBoxCanSave.Name = "checkBoxCanSave";
            this.checkBoxCanSave.Size = new System.Drawing.Size(88, 20);
            this.checkBoxCanSave.TabIndex = 4;
            this.checkBoxCanSave.Text = "Can Save";
            this.checkBoxCanSave.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 365);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(120, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save Changes";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormAccessControl
            // 
            this.ClientSize = new System.Drawing.Size(390, 391);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkBoxCanSave);
            this.Controls.Add(this.checkBoxCanDelete);
            this.Controls.Add(this.checkBoxCanAdd);
            this.Controls.Add(this.listBoxTables);
            this.Controls.Add(this.comboBoxUsers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAccessControl";
            this.Text = "Access Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
