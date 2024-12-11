namespace SQLiteViewer
{
    partial class MovementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MovementForm));
            this.comboBoxFromWarehouse = new System.Windows.Forms.ComboBox();
            this.comboBoxToWarehouse = new System.Windows.Forms.ComboBox();
            this.comboBoxRawMaterial = new System.Windows.Forms.ComboBox();
            this.textBoxQuantity = new System.Windows.Forms.TextBox();
            this.buttonConfirmMovement = new System.Windows.Forms.Button();
            this.labelFromWarehouse = new System.Windows.Forms.Label();
            this.labelToWarehouse = new System.Windows.Forms.Label();
            this.labelRawMaterial = new System.Windows.Forms.Label();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxFromWarehouse
            // 
            this.comboBoxFromWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFromWarehouse.FormattingEnabled = true;
            this.comboBoxFromWarehouse.Location = new System.Drawing.Point(145, 32);
            this.comboBoxFromWarehouse.Name = "comboBoxFromWarehouse";
            this.comboBoxFromWarehouse.Size = new System.Drawing.Size(200, 24);
            this.comboBoxFromWarehouse.TabIndex = 0;
            // 
            // comboBoxToWarehouse
            // 
            this.comboBoxToWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxToWarehouse.FormattingEnabled = true;
            this.comboBoxToWarehouse.Location = new System.Drawing.Point(145, 59);
            this.comboBoxToWarehouse.Name = "comboBoxToWarehouse";
            this.comboBoxToWarehouse.Size = new System.Drawing.Size(200, 24);
            this.comboBoxToWarehouse.TabIndex = 1;
            // 
            // comboBoxRawMaterial
            // 
            this.comboBoxRawMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRawMaterial.FormattingEnabled = true;
            this.comboBoxRawMaterial.Location = new System.Drawing.Point(145, 86);
            this.comboBoxRawMaterial.Name = "comboBoxRawMaterial";
            this.comboBoxRawMaterial.Size = new System.Drawing.Size(200, 24);
            this.comboBoxRawMaterial.TabIndex = 2;
            // 
            // textBoxQuantity
            // 
            this.textBoxQuantity.Location = new System.Drawing.Point(145, 113);
            this.textBoxQuantity.Name = "textBoxQuantity";
            this.textBoxQuantity.Size = new System.Drawing.Size(100, 22);
            this.textBoxQuantity.TabIndex = 3;
            // 
            // buttonConfirmMovement
            // 
            this.buttonConfirmMovement.Location = new System.Drawing.Point(145, 139);
            this.buttonConfirmMovement.Name = "buttonConfirmMovement";
            this.buttonConfirmMovement.Size = new System.Drawing.Size(137, 23);
            this.buttonConfirmMovement.TabIndex = 4;
            this.buttonConfirmMovement.Text = "Подтвердить";
            this.buttonConfirmMovement.UseVisualStyleBackColor = true;
            this.buttonConfirmMovement.Click += new System.EventHandler(this.buttonConfirmMovement_Click);
            // 
            // labelFromWarehouse
            // 
            this.labelFromWarehouse.AutoSize = true;
            this.labelFromWarehouse.Location = new System.Drawing.Point(6, 35);
            this.labelFromWarehouse.Name = "labelFromWarehouse";
            this.labelFromWarehouse.Size = new System.Drawing.Size(133, 16);
            this.labelFromWarehouse.TabIndex = 5;
            this.labelFromWarehouse.Text = "Со склада (откуда):";
            // 
            // labelToWarehouse
            // 
            this.labelToWarehouse.AutoSize = true;
            this.labelToWarehouse.Location = new System.Drawing.Point(6, 62);
            this.labelToWarehouse.Name = "labelToWarehouse";
            this.labelToWarehouse.Size = new System.Drawing.Size(111, 16);
            this.labelToWarehouse.TabIndex = 6;
            this.labelToWarehouse.Text = "На склад (куда):";
            // 
            // labelRawMaterial
            // 
            this.labelRawMaterial.AutoSize = true;
            this.labelRawMaterial.Location = new System.Drawing.Point(6, 89);
            this.labelRawMaterial.Name = "labelRawMaterial";
            this.labelRawMaterial.Size = new System.Drawing.Size(51, 16);
            this.labelRawMaterial.TabIndex = 7;
            this.labelRawMaterial.Text = "Сырьё:";
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Location = new System.Drawing.Point(6, 116);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(88, 16);
            this.labelQuantity.TabIndex = 8;
            this.labelQuantity.Text = "Количество:";
            // 
            // MovementForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 181);
            this.Controls.Add(this.labelQuantity);
            this.Controls.Add(this.labelRawMaterial);
            this.Controls.Add(this.labelToWarehouse);
            this.Controls.Add(this.labelFromWarehouse);
            this.Controls.Add(this.buttonConfirmMovement);
            this.Controls.Add(this.textBoxQuantity);
            this.Controls.Add(this.comboBoxRawMaterial);
            this.Controls.Add(this.comboBoxToWarehouse);
            this.Controls.Add(this.comboBoxFromWarehouse);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MovementForm";
            this.Text = "Перемещение сырья";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFromWarehouse;
        private System.Windows.Forms.ComboBox comboBoxToWarehouse;
        private System.Windows.Forms.ComboBox comboBoxRawMaterial;
        private System.Windows.Forms.TextBox textBoxQuantity;
        private System.Windows.Forms.Button buttonConfirmMovement;
        private System.Windows.Forms.Label labelFromWarehouse;
        private System.Windows.Forms.Label labelToWarehouse;
        private System.Windows.Forms.Label labelRawMaterial;
        private System.Windows.Forms.Label labelQuantity;
    }
}