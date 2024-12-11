namespace SQLiteViewer
{
    partial class ContractForm
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractForm));
            this.textBoxНомер = new System.Windows.Forms.TextBox();
            this.comboBoxПоставщики = new System.Windows.Forms.ComboBox();
            this.comboBoxПокупатели = new System.Windows.Forms.ComboBox();
            this.dateTimePickerДата = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelНомер = new System.Windows.Forms.Label();
            this.labelПоставщик = new System.Windows.Forms.Label();
            this.labelПокупатель = new System.Windows.Forms.Label();
            this.labelДатаЗаключения = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxНомер
            // 
            this.textBoxНомер.Location = new System.Drawing.Point(150, 30);
            this.textBoxНомер.Name = "textBoxНомер";
            this.textBoxНомер.Size = new System.Drawing.Size(200, 22);
            this.textBoxНомер.TabIndex = 0;
            // 
            // comboBoxПоставщики
            // 
            this.comboBoxПоставщики.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxПоставщики.FormattingEnabled = true;
            this.comboBoxПоставщики.Location = new System.Drawing.Point(150, 70);
            this.comboBoxПоставщики.Name = "comboBoxПоставщики";
            this.comboBoxПоставщики.Size = new System.Drawing.Size(200, 24);
            this.comboBoxПоставщики.TabIndex = 1;
            // 
            // comboBoxПокупатели
            // 
            this.comboBoxПокупатели.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxПокупатели.FormattingEnabled = true;
            this.comboBoxПокупатели.Location = new System.Drawing.Point(150, 110);
            this.comboBoxПокупатели.Name = "comboBoxПокупатели";
            this.comboBoxПокупатели.Size = new System.Drawing.Size(200, 24);
            this.comboBoxПокупатели.TabIndex = 2;
            // 
            // dateTimePickerДата
            // 
            this.dateTimePickerДата.Location = new System.Drawing.Point(150, 150);
            this.dateTimePickerДата.Name = "dateTimePickerДата";
            this.dateTimePickerДата.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerДата.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(150, 190);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(200, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelНомер
            // 
            this.labelНомер.AutoSize = true;
            this.labelНомер.Location = new System.Drawing.Point(19, 30);
            this.labelНомер.Name = "labelНомер";
            this.labelНомер.Size = new System.Drawing.Size(53, 16);
            this.labelНомер.TabIndex = 5;
            this.labelНомер.Text = "Номер:";
            // 
            // labelПоставщик
            // 
            this.labelПоставщик.AutoSize = true;
            this.labelПоставщик.Location = new System.Drawing.Point(19, 70);
            this.labelПоставщик.Name = "labelПоставщик";
            this.labelПоставщик.Size = new System.Drawing.Size(82, 16);
            this.labelПоставщик.TabIndex = 6;
            this.labelПоставщик.Text = "Поставщик:";
            // 
            // labelПокупатель
            // 
            this.labelПокупатель.AutoSize = true;
            this.labelПокупатель.Location = new System.Drawing.Point(19, 110);
            this.labelПокупатель.Name = "labelПокупатель";
            this.labelПокупатель.Size = new System.Drawing.Size(89, 16);
            this.labelПокупатель.TabIndex = 7;
            this.labelПокупатель.Text = "Покупатель:";
            // 
            // labelДатаЗаключения
            // 
            this.labelДатаЗаключения.AutoSize = true;
            this.labelДатаЗаключения.Location = new System.Drawing.Point(19, 150);
            this.labelДатаЗаключения.Name = "labelДатаЗаключения";
            this.labelДатаЗаключения.Size = new System.Drawing.Size(125, 16);
            this.labelДатаЗаключения.TabIndex = 8;
            this.labelДатаЗаключения.Text = "Дата заключения:";
            // 
            // ContractForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 241);
            this.Controls.Add(this.labelДатаЗаключения);
            this.Controls.Add(this.labelПокупатель);
            this.Controls.Add(this.labelПоставщик);
            this.Controls.Add(this.labelНомер);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dateTimePickerДата);
            this.Controls.Add(this.comboBoxПокупатели);
            this.Controls.Add(this.comboBoxПоставщики);
            this.Controls.Add(this.textBoxНомер);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ContractForm";
            this.Text = "Добавить запись в договор";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxНомер;
        private System.Windows.Forms.ComboBox comboBoxПоставщики;
        private System.Windows.Forms.ComboBox comboBoxПокупатели;
        private System.Windows.Forms.DateTimePicker dateTimePickerДата;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelНомер;
        private System.Windows.Forms.Label labelПоставщик;
        private System.Windows.Forms.Label labelПокупатель;
        private System.Windows.Forms.Label labelДатаЗаключения;
    }
}
