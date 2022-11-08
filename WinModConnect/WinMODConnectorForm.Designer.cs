namespace WinModConnect
{
    partial class WinMODConnectorForm
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
            this.ExportSignals = new System.Windows.Forms.Button();
            this.StartDataExchange = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.motoSIMNames = new System.Windows.Forms.ComboBox();
            this.winMODNames = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comPortTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comHostTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.connectToWinMOD = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ResetMapping = new System.Windows.Forms.Button();
            this.GetCOMElements = new System.Windows.Forms.Button();
            this.SaveMapping = new System.Windows.Forms.Button();
            this.addMapping = new System.Windows.Forms.Button();
            this.MappinglistView = new System.Windows.Forms.ListView();
            this.DataExchange = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CycleRate = new System.Windows.Forms.TextBox();
            this.LogslistView = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.DataExchange.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExportSignals
            // 
            this.ExportSignals.Location = new System.Drawing.Point(9, 41);
            this.ExportSignals.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExportSignals.Name = "ExportSignals";
            this.ExportSignals.Size = new System.Drawing.Size(163, 28);
            this.ExportSignals.TabIndex = 0;
            this.ExportSignals.Text = "Export I/O Signals";
            this.ExportSignals.UseVisualStyleBackColor = true;
            this.ExportSignals.Click += new System.EventHandler(this.ExportSignals_Click);
            // 
            // StartDataExchange
            // 
            this.StartDataExchange.Location = new System.Drawing.Point(199, 18);
            this.StartDataExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StartDataExchange.Name = "StartDataExchange";
            this.StartDataExchange.Size = new System.Drawing.Size(103, 28);
            this.StartDataExchange.TabIndex = 7;
            this.StartDataExchange.Text = "Start";
            this.StartDataExchange.UseVisualStyleBackColor = true;
            this.StartDataExchange.Click += new System.EventHandler(this.StartDataExchange_Click);
            // 
            // motoSIMNames
            // 
            this.motoSIMNames.DropDownWidth = 415;
            this.motoSIMNames.FormattingEnabled = true;
            this.motoSIMNames.Location = new System.Drawing.Point(9, 59);
            this.motoSIMNames.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.motoSIMNames.Name = "motoSIMNames";
            this.motoSIMNames.Size = new System.Drawing.Size(260, 23);
            this.motoSIMNames.TabIndex = 9;
            // 
            // winMODNames
            // 
            this.winMODNames.FormattingEnabled = true;
            this.winMODNames.Location = new System.Drawing.Point(285, 58);
            this.winMODNames.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.winMODNames.Name = "winMODNames";
            this.winMODNames.Size = new System.Drawing.Size(230, 23);
            this.winMODNames.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ExportSignals);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(182, 85);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MotoSIM";
            // 
            // comTimeoutTextBox
            // 
            this.comTimeoutTextBox.Location = new System.Drawing.Point(218, 43);
            this.comTimeoutTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.comTimeoutTextBox.Name = "comTimeoutTextBox";
            this.comTimeoutTextBox.Size = new System.Drawing.Size(50, 23);
            this.comTimeoutTextBox.TabIndex = 5;
            this.comTimeoutTextBox.Text = "1000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Host:";
            // 
            // comPortTextBox
            // 
            this.comPortTextBox.Location = new System.Drawing.Point(132, 43);
            this.comPortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.comPortTextBox.Name = "comPortTextBox";
            this.comPortTextBox.Size = new System.Drawing.Size(71, 23);
            this.comPortTextBox.TabIndex = 3;
            this.comPortTextBox.Text = "40001";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(129, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Port:";
            // 
            // comHostTextBox
            // 
            this.comHostTextBox.Location = new System.Drawing.Point(8, 43);
            this.comHostTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.comHostTextBox.Name = "comHostTextBox";
            this.comHostTextBox.Size = new System.Drawing.Size(111, 23);
            this.comHostTextBox.TabIndex = 1;
            this.comHostTextBox.Text = "localhost";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Timeout(ms)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.connectToWinMOD);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.comHostTextBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.comPortTextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.comTimeoutTextBox);
            this.groupBox3.Location = new System.Drawing.Point(207, 13);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(384, 85);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "WinMOD Connection";
            // 
            // connectToWinMOD
            // 
            this.connectToWinMOD.Location = new System.Drawing.Point(282, 41);
            this.connectToWinMOD.Name = "connectToWinMOD";
            this.connectToWinMOD.Size = new System.Drawing.Size(92, 29);
            this.connectToWinMOD.TabIndex = 6;
            this.connectToWinMOD.Text = "Connect";
            this.connectToWinMOD.UseVisualStyleBackColor = true;
            this.connectToWinMOD.Click += new System.EventHandler(this.connectToWinMOD_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.ResetMapping);
            this.groupBox2.Controls.Add(this.GetCOMElements);
            this.groupBox2.Controls.Add(this.SaveMapping);
            this.groupBox2.Controls.Add(this.addMapping);
            this.groupBox2.Controls.Add(this.MappinglistView);
            this.groupBox2.Controls.Add(this.motoSIMNames);
            this.groupBox2.Controls.Add(this.winMODNames);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(12, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(579, 226);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controller-COM Mapping";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "MotoSIM-Controllers";
            // 
            // ResetMapping
            // 
            this.ResetMapping.Location = new System.Drawing.Point(521, 118);
            this.ResetMapping.Name = "ResetMapping";
            this.ResetMapping.Size = new System.Drawing.Size(48, 37);
            this.ResetMapping.TabIndex = 15;
            this.ResetMapping.Text = "Reset";
            this.ResetMapping.UseVisualStyleBackColor = true;
            this.ResetMapping.Click += new System.EventHandler(this.ResetMapping_Click);
            // 
            // GetCOMElements
            // 
            this.GetCOMElements.Location = new System.Drawing.Point(287, 19);
            this.GetCOMElements.Name = "GetCOMElements";
            this.GetCOMElements.Size = new System.Drawing.Size(138, 28);
            this.GetCOMElements.TabIndex = 14;
            this.GetCOMElements.Text = "Read WinMOD-COM";
            this.GetCOMElements.UseVisualStyleBackColor = true;
            this.GetCOMElements.Click += new System.EventHandler(this.GetCOMElements_Click);
            // 
            // SaveMapping
            // 
            this.SaveMapping.Enabled = false;
            this.SaveMapping.Location = new System.Drawing.Point(203, 192);
            this.SaveMapping.Name = "SaveMapping";
            this.SaveMapping.Size = new System.Drawing.Size(133, 28);
            this.SaveMapping.TabIndex = 13;
            this.SaveMapping.Text = "Save Mapping";
            this.SaveMapping.UseVisualStyleBackColor = true;
            this.SaveMapping.Click += new System.EventHandler(this.SaveMapping_Click);
            // 
            // addMapping
            // 
            this.addMapping.Location = new System.Drawing.Point(521, 53);
            this.addMapping.Name = "addMapping";
            this.addMapping.Size = new System.Drawing.Size(48, 28);
            this.addMapping.TabIndex = 12;
            this.addMapping.Text = "Add";
            this.addMapping.UseVisualStyleBackColor = true;
            this.addMapping.Click += new System.EventHandler(this.addMapping_Click);
            // 
            // MappinglistView
            // 
            this.MappinglistView.Location = new System.Drawing.Point(9, 89);
            this.MappinglistView.Name = "MappinglistView";
            this.MappinglistView.Size = new System.Drawing.Size(506, 97);
            this.MappinglistView.TabIndex = 11;
            this.MappinglistView.UseCompatibleStateImageBehavior = false;
            this.MappinglistView.View = System.Windows.Forms.View.Details;
            // 
            // DataExchange
            // 
            this.DataExchange.Controls.Add(this.label1);
            this.DataExchange.Controls.Add(this.CycleRate);
            this.DataExchange.Controls.Add(this.StartDataExchange);
            this.DataExchange.Enabled = false;
            this.DataExchange.Location = new System.Drawing.Point(12, 339);
            this.DataExchange.Name = "DataExchange";
            this.DataExchange.Size = new System.Drawing.Size(314, 52);
            this.DataExchange.TabIndex = 15;
            this.DataExchange.TabStop = false;
            this.DataExchange.Text = "DataExchange";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Cycle Rate(ms)";
            // 
            // CycleRate
            // 
            this.CycleRate.Location = new System.Drawing.Point(107, 21);
            this.CycleRate.Name = "CycleRate";
            this.CycleRate.Size = new System.Drawing.Size(76, 23);
            this.CycleRate.TabIndex = 8;
            this.CycleRate.Text = "50";
            // 
            // LogslistView
            // 
            this.LogslistView.Location = new System.Drawing.Point(12, 401);
            this.LogslistView.Name = "LogslistView";
            this.LogslistView.Size = new System.Drawing.Size(579, 127);
            this.LogslistView.TabIndex = 10;
            this.LogslistView.UseCompatibleStateImageBehavior = false;
            this.LogslistView.View = System.Windows.Forms.View.Details;
            // 
            // WinMODConnectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 540);
            this.Controls.Add(this.LogslistView);
            this.Controls.Add(this.DataExchange);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WinMODConnectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinMOD Connector";
            this.Load += new System.EventHandler(this.PluginForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.DataExchange.ResumeLayout(false);
            this.DataExchange.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExportSignals;
        private System.Windows.Forms.Button StartDataExchange;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox motoSIMNames;
        private System.Windows.Forms.ComboBox winMODNames;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox comTimeoutTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox comPortTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox comHostTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView MappinglistView;
        private System.Windows.Forms.Button addMapping;
        private System.Windows.Forms.Button SaveMapping;
        private System.Windows.Forms.Button GetCOMElements;
        private System.Windows.Forms.Button ResetMapping;
        private System.Windows.Forms.GroupBox DataExchange;
        private System.Windows.Forms.TextBox CycleRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button connectToWinMOD;
        private System.Windows.Forms.ListView LogslistView;
        private System.Windows.Forms.Label label2;
    }
}