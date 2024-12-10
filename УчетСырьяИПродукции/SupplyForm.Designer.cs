namespace SQLiteViewer
{
    partial class SupplyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed; otherwise, False.</param>
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
            this.comboBoxПоставщик = new System.Windows.Forms.ComboBox();
            this.comboBoxСклад = new System.Windows.Forms.ComboBox();
            this.comboBoxСырье = new System.Windows.Forms.ComboBox();
            this.textBoxОбъем = new System.Windows.Forms.TextBox();
            this.dateTimePickerДата = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelДоговор = new System.Windows.Forms.Label();
            this.labelСклад = new System.Windows.Forms.Label();
            this.labelСырье = new System.Windows.Forms.Label();
            this.labelОбъем = new System.Windows.Forms.Label();
            this.labelДатаПоставки = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxПоставщик
            // 
            this.comboBoxПоставщик.FormattingEnabled = true;
            this.comboBoxПоставщик.Location = new System.Drawing.Point(130, 30);
            this.comboBoxПоставщик.Name = "comboBoxПоставщик";
            this.comboBoxПоставщик.Size = new System.Drawing.Size(200, 24);
            this.comboBoxПоставщик.TabIndex = 0;
            // 
            // comboBoxСклад
            // 
            this.comboBoxСклад.FormattingEnabled = true;
            this.comboBoxСклад.Location = new System.Drawing.Point(130, 70);
            this.comboBoxСклад.Name = "comboBoxСклад";
            this.comboBoxСклад.Size = new System.Drawing.Size(200, 24);
            this.comboBoxСклад.TabIndex = 1;
            // 
            // comboBoxСырье
            // 
            this.comboBoxСырье.FormattingEnabled = true;
            this.comboBoxСырье.Location = new System.Drawing.Point(130, 110);
            this.comboBoxСырье.Name = "comboBoxСырье";
            this.comboBoxСырье.Size = new System.Drawing.Size(200, 24);
            this.comboBoxСырье.TabIndex = 2;
            // 
            // textBoxОбъем
            // 
            this.textBoxОбъем.Location = new System.Drawing.Point(130, 150);
            this.textBoxОбъем.Name = "textBoxОбъем";
            this.textBoxОбъем.Size = new System.Drawing.Size(200, 22);
            this.textBoxОбъем.TabIndex = 3;
            // 
            // dateTimePickerДата
            // 
            this.dateTimePickerДата.Location = new System.Drawing.Point(130, 190);
            this.dateTimePickerДата.Name = "dateTimePickerДата";
            this.dateTimePickerДата.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerДата.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(130, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelДоговор
            // 
            this.labelДоговор.AutoSize = true;
            this.labelДоговор.Location = new System.Drawing.Point(12, 30);
            this.labelДоговор.Name = "labelДоговор";
            this.labelДоговор.Size = new System.Drawing.Size(82, 16);
            this.labelДоговор.TabIndex = 6;
            this.labelДоговор.Text = "Поставщик:";
            // 
            // labelСклад
            // 
            this.labelСклад.AutoSize = true;
            this.labelСклад.Location = new System.Drawing.Point(12, 70);
            this.labelСклад.Name = "labelСклад";
            this.labelСклад.Size = new System.Drawing.Size(50, 16);
            this.labelСклад.TabIndex = 7;
            this.labelСклад.Text = "Склад:";
            // 
            // labelСырье
            // 
            this.labelСырье.AutoSize = true;
            this.labelСырье.Location = new System.Drawing.Point(12, 110);
            this.labelСырье.Name = "labelСырье";
            this.labelСырье.Size = new System.Drawing.Size(51, 16);
            this.labelСырье.TabIndex = 8;
            this.labelСырье.Text = "Сырье:";
            // 
            // labelОбъем
            // 
            this.labelОбъем.AutoSize = true;
            this.labelОбъем.Location = new System.Drawing.Point(12, 150);
            this.labelОбъем.Name = "labelОбъем";
            this.labelОбъем.Size = new System.Drawing.Size(96, 16);
            this.labelОбъем.TabIndex = 9;
            this.labelОбъем.Text = "Колличество:";
            // 
            // labelДатаПоставки
            // 
            this.labelДатаПоставки.AutoSize = true;
            this.labelДатаПоставки.Location = new System.Drawing.Point(12, 190);
            this.labelДатаПоставки.Name = "labelДатаПоставки";
            this.labelДатаПоставки.Size = new System.Drawing.Size(106, 16);
            this.labelДатаПоставки.TabIndex = 10;
            this.labelДатаПоставки.Text = "Дата поставки:";
            // 
            // SupplyForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 281);
            this.Controls.Add(this.labelДатаПоставки);
            this.Controls.Add(this.labelОбъем);
            this.Controls.Add(this.labelСырье);
            this.Controls.Add(this.labelСклад);
            this.Controls.Add(this.labelДоговор);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dateTimePickerДата);
            this.Controls.Add(this.textBoxОбъем);
            this.Controls.Add(this.comboBoxСырье);
            this.Controls.Add(this.comboBoxСклад);
            this.Controls.Add(this.comboBoxПоставщик);
            this.Name = "SupplyForm";
            this.Text = "Добавить поставку";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxПоставщик;
        private System.Windows.Forms.ComboBox comboBoxСклад;
        private System.Windows.Forms.ComboBox comboBoxСырье;
        private System.Windows.Forms.TextBox textBoxОбъем;
        private System.Windows.Forms.DateTimePicker dateTimePickerДата;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelДоговор;
        private System.Windows.Forms.Label labelСклад;
        private System.Windows.Forms.Label labelСырье;
        private System.Windows.Forms.Label labelОбъем;
        private System.Windows.Forms.Label labelДатаПоставки;
    }
}
