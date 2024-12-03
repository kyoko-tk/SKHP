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
            this.comboBoxДоговор = new System.Windows.Forms.ComboBox();
            this.comboBoxСклад = new System.Windows.Forms.ComboBox();
            this.comboBoxСырье = new System.Windows.Forms.ComboBox();
            this.textBoxОбъем = new System.Windows.Forms.TextBox();
            this.dateTimePickerДатаПоставки = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelДоговор = new System.Windows.Forms.Label();
            this.labelСклад = new System.Windows.Forms.Label();
            this.labelСырье = new System.Windows.Forms.Label();
            this.labelОбъем = new System.Windows.Forms.Label();
            this.labelДатаПоставки = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxДоговор
            // 
            this.comboBoxДоговор.FormattingEnabled = true;
            this.comboBoxДоговор.Location = new System.Drawing.Point(130, 30);
            this.comboBoxДоговор.Name = "comboBoxДоговор";
            this.comboBoxДоговор.Size = new System.Drawing.Size(200, 21);
            this.comboBoxДоговор.TabIndex = 0;
            // 
            // comboBoxСклад
            // 
            this.comboBoxСклад.FormattingEnabled = true;
            this.comboBoxСклад.Location = new System.Drawing.Point(130, 70);
            this.comboBoxСклад.Name = "comboBoxСклад";
            this.comboBoxСклад.Size = new System.Drawing.Size(200, 21);
            this.comboBoxСклад.TabIndex = 1;
            // 
            // comboBoxСырье
            // 
            this.comboBoxСырье.FormattingEnabled = true;
            this.comboBoxСырье.Location = new System.Drawing.Point(130, 110);
            this.comboBoxСырье.Name = "comboBoxСырье";
            this.comboBoxСырье.Size = new System.Drawing.Size(200, 21);
            this.comboBoxСырье.TabIndex = 2;
            // 
            // textBoxОбъем
            // 
            this.textBoxОбъем.Location = new System.Drawing.Point(130, 150);
            this.textBoxОбъем.Name = "textBoxОбъем";
            this.textBoxОбъем.Size = new System.Drawing.Size(200, 20);
            this.textBoxОбъем.TabIndex = 3;
            // 
            // dateTimePickerДатаПоставки
            // 
            this.dateTimePickerДатаПоставки.Location = new System.Drawing.Point(130, 190);
            this.dateTimePickerДатаПоставки.Name = "dateTimePickerДатаПоставки";
            this.dateTimePickerДатаПоставки.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerДатаПоставки.TabIndex = 4;
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
            this.labelДоговор.Location = new System.Drawing.Point(30, 30);
            this.labelДоговор.Name = "labelДоговор";
            this.labelДоговор.Size = new System.Drawing.Size(54, 13);
            this.labelДоговор.TabIndex = 6;
            this.labelДоговор.Text = "Договор:";
            // 
            // labelСклад
            // 
            this.labelСклад.AutoSize = true;
            this.labelСклад.Location = new System.Drawing.Point(30, 70);
            this.labelСклад.Name = "labelСклад";
            this.labelСклад.Size = new System.Drawing.Size(40, 13);
            this.labelСклад.TabIndex = 7;
            this.labelСклад.Text = "Склад:";
            // 
            // labelСырье
            // 
            this.labelСырье.AutoSize = true;
            this.labelСырье.Location = new System.Drawing.Point(30, 110);
            this.labelСырье.Name = "labelСырье";
            this.labelСырье.Size = new System.Drawing.Size(43, 13);
            this.labelСырье.TabIndex = 8;
            this.labelСырье.Text = "Сырье:";
            // 
            // labelОбъем
            // 
            this.labelОбъем.AutoSize = true;
            this.labelОбъем.Location = new System.Drawing.Point(30, 150);
            this.labelОбъем.Name = "labelОбъем";
            this.labelОбъем.Size = new System.Drawing.Size(45, 13);
            this.labelОбъем.TabIndex = 9;
            this.labelОбъем.Text = "Объем:";
            // 
            // labelДатаПоставки
            // 
            this.labelДатаПоставки.AutoSize = true;
            this.labelДатаПоставки.Location = new System.Drawing.Point(30, 190);
            this.labelДатаПоставки.Name = "labelДатаПоставки";
            this.labelДатаПоставки.Size = new System.Drawing.Size(91, 13);
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
            this.Controls.Add(this.dateTimePickerДатаПоставки);
            this.Controls.Add(this.textBoxОбъем);
            this.Controls.Add(this.comboBoxСырье);
            this.Controls.Add(this.comboBoxСклад);
            this.Controls.Add(this.comboBoxДоговор);
            this.Name = "SupplyForm";
            this.Text = "Добавить поставку";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxДоговор;
        private System.Windows.Forms.ComboBox comboBoxСклад;
        private System.Windows.Forms.ComboBox comboBoxСырье;
        private System.Windows.Forms.TextBox textBoxОбъем;
        private System.Windows.Forms.DateTimePicker dateTimePickerДатаПоставки;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelДоговор;
        private System.Windows.Forms.Label labelСклад;
        private System.Windows.Forms.Label labelСырье;
        private System.Windows.Forms.Label labelОбъем;
        private System.Windows.Forms.Label labelДатаПоставки;
    }
}
