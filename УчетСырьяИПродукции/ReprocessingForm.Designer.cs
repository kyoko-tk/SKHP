namespace SQLiteViewer
{
    partial class ReprocessingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxRawMaterial;
        private System.Windows.Forms.ComboBox comboBoxFromWarehouse;
        private System.Windows.Forms.ComboBox comboBoxToProduct;
        private System.Windows.Forms.TextBox textBoxRawMaterialQuantity;
        private System.Windows.Forms.TextBox textBoxProductQuantity;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelRawMaterial;
        private System.Windows.Forms.Label labelFromWarehouse;
        private System.Windows.Forms.Label labelToProduct;
        private System.Windows.Forms.Label labelRawMaterialQuantity;
        private System.Windows.Forms.Label labelProductQuantity;

        // Закрытие формы
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Инициализация компонентов
        private void InitializeComponent()
        {
            this.comboBoxRawMaterial = new System.Windows.Forms.ComboBox();
            this.comboBoxFromWarehouse = new System.Windows.Forms.ComboBox();
            this.comboBoxToProduct = new System.Windows.Forms.ComboBox();
            this.textBoxRawMaterialQuantity = new System.Windows.Forms.TextBox();
            this.textBoxProductQuantity = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelRawMaterial = new System.Windows.Forms.Label();
            this.labelFromWarehouse = new System.Windows.Forms.Label();
            this.labelToProduct = new System.Windows.Forms.Label();
            this.labelRawMaterialQuantity = new System.Windows.Forms.Label();
            this.labelProductQuantity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxRawMaterial
            // 
            this.comboBoxRawMaterial.FormattingEnabled = true;
            this.comboBoxRawMaterial.Location = new System.Drawing.Point(180, 50);
            this.comboBoxRawMaterial.Name = "comboBoxRawMaterial";
            this.comboBoxRawMaterial.Size = new System.Drawing.Size(200, 24);
            this.comboBoxRawMaterial.TabIndex = 0;
            // 
            // comboBoxFromWarehouse
            // 
            this.comboBoxFromWarehouse.FormattingEnabled = true;
            this.comboBoxFromWarehouse.Location = new System.Drawing.Point(180, 100);
            this.comboBoxFromWarehouse.Name = "comboBoxFromWarehouse";
            this.comboBoxFromWarehouse.Size = new System.Drawing.Size(200, 24);
            this.comboBoxFromWarehouse.TabIndex = 1;
            // 
            // comboBoxToProduct
            // 
            this.comboBoxToProduct.FormattingEnabled = true;
            this.comboBoxToProduct.Location = new System.Drawing.Point(180, 150);
            this.comboBoxToProduct.Name = "comboBoxToProduct";
            this.comboBoxToProduct.Size = new System.Drawing.Size(200, 24);
            this.comboBoxToProduct.TabIndex = 2;
            // 
            // textBoxRawMaterialQuantity
            // 
            this.textBoxRawMaterialQuantity.Location = new System.Drawing.Point(180, 198);
            this.textBoxRawMaterialQuantity.Name = "textBoxRawMaterialQuantity";
            this.textBoxRawMaterialQuantity.Size = new System.Drawing.Size(100, 22);
            this.textBoxRawMaterialQuantity.TabIndex = 4;
            // 
            // textBoxProductQuantity
            // 
            this.textBoxProductQuantity.Location = new System.Drawing.Point(180, 248);
            this.textBoxProductQuantity.Name = "textBoxProductQuantity";
            this.textBoxProductQuantity.ReadOnly = true;
            this.textBoxProductQuantity.Size = new System.Drawing.Size(100, 22);
            this.textBoxProductQuantity.TabIndex = 5;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(150, 298);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(117, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelRawMaterial
            // 
            this.labelRawMaterial.AutoSize = true;
            this.labelRawMaterial.Location = new System.Drawing.Point(12, 50);
            this.labelRawMaterial.Name = "labelRawMaterial";
            this.labelRawMaterial.Size = new System.Drawing.Size(51, 16);
            this.labelRawMaterial.TabIndex = 7;
            this.labelRawMaterial.Text = "Сырье:";
            // 
            // labelFromWarehouse
            // 
            this.labelFromWarehouse.AutoSize = true;
            this.labelFromWarehouse.Location = new System.Drawing.Point(12, 100);
            this.labelFromWarehouse.Name = "labelFromWarehouse";
            this.labelFromWarehouse.Size = new System.Drawing.Size(76, 16);
            this.labelFromWarehouse.TabIndex = 8;
            this.labelFromWarehouse.Text = "Со склада:";
            // 
            // labelToProduct
            // 
            this.labelToProduct.AutoSize = true;
            this.labelToProduct.Location = new System.Drawing.Point(12, 150);
            this.labelToProduct.Name = "labelToProduct";
            this.labelToProduct.Size = new System.Drawing.Size(82, 16);
            this.labelToProduct.TabIndex = 9;
            this.labelToProduct.Text = "Продукция:";
            // 
            // labelRawMaterialQuantity
            // 
            this.labelRawMaterialQuantity.AutoSize = true;
            this.labelRawMaterialQuantity.Location = new System.Drawing.Point(12, 198);
            this.labelRawMaterialQuantity.Name = "labelRawMaterialQuantity";
            this.labelRawMaterialQuantity.Size = new System.Drawing.Size(129, 16);
            this.labelRawMaterialQuantity.TabIndex = 11;
            this.labelRawMaterialQuantity.Text = "Количество сырья:";
            // 
            // labelProductQuantity
            // 
            this.labelProductQuantity.AutoSize = true;
            this.labelProductQuantity.Location = new System.Drawing.Point(12, 248);
            this.labelProductQuantity.Name = "labelProductQuantity";
            this.labelProductQuantity.Size = new System.Drawing.Size(162, 16);
            this.labelProductQuantity.TabIndex = 12;
            this.labelProductQuantity.Text = "Количество продукции:";
            // 
            // ReprocessingForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 347);
            this.Controls.Add(this.labelProductQuantity);
            this.Controls.Add(this.labelRawMaterialQuantity);
            this.Controls.Add(this.labelToProduct);
            this.Controls.Add(this.labelFromWarehouse);
            this.Controls.Add(this.labelRawMaterial);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxProductQuantity);
            this.Controls.Add(this.textBoxRawMaterialQuantity);
            this.Controls.Add(this.comboBoxToProduct);
            this.Controls.Add(this.comboBoxFromWarehouse);
            this.Controls.Add(this.comboBoxRawMaterial);
            this.Name = "ReprocessingForm";
            this.Text = "Переработка";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
