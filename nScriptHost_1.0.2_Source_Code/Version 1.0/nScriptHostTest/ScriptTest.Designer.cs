namespace nScriptHostTest
{
    partial class ScriptTest
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
                _scriptHost.Dispose();
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
            this._sourceCodeTextBox = new System.Windows.Forms.TextBox();
            this._runButton = new System.Windows.Forms.Button();
            this._compileErrorTextBox = new System.Windows.Forms.TextBox();
            this._referencesButton = new System.Windows.Forms.Button();
            this._languageComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._compileButton = new System.Windows.Forms.Button();
            this._haltButton = new System.Windows.Forms.Button();
            this.MsgTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _sourceCodeTextBox
            // 
            this._sourceCodeTextBox.Location = new System.Drawing.Point(12, 12);
            this._sourceCodeTextBox.Multiline = true;
            this._sourceCodeTextBox.Name = "_sourceCodeTextBox";
            this._sourceCodeTextBox.Size = new System.Drawing.Size(610, 353);
            this._sourceCodeTextBox.TabIndex = 0;
            // 
            // _runButton
            // 
            this._runButton.Location = new System.Drawing.Point(245, 371);
            this._runButton.Name = "_runButton";
            this._runButton.Size = new System.Drawing.Size(80, 23);
            this._runButton.TabIndex = 1;
            this._runButton.Text = "Run";
            this._runButton.UseVisualStyleBackColor = true;
            this._runButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // _compileErrorTextBox
            // 
            this._compileErrorTextBox.Location = new System.Drawing.Point(12, 400);
            this._compileErrorTextBox.Multiline = true;
            this._compileErrorTextBox.Name = "_compileErrorTextBox";
            this._compileErrorTextBox.ReadOnly = true;
            this._compileErrorTextBox.Size = new System.Drawing.Size(610, 109);
            this._compileErrorTextBox.TabIndex = 2;
            // 
            // _referencesButton
            // 
            this._referencesButton.Location = new System.Drawing.Point(34, 371);
            this._referencesButton.Name = "_referencesButton";
            this._referencesButton.Size = new System.Drawing.Size(80, 23);
            this._referencesButton.TabIndex = 3;
            this._referencesButton.Text = "References...";
            this._referencesButton.UseVisualStyleBackColor = true;
            this._referencesButton.Click += new System.EventHandler(this.ReferencesButton_Click);
            // 
            // _languageComboBox
            // 
            this._languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._languageComboBox.FormattingEnabled = true;
            this._languageComboBox.Items.AddRange(new object[] {
            "JScript",
            "VBScript"});
            this._languageComboBox.Location = new System.Drawing.Point(492, 372);
            this._languageComboBox.Name = "_languageComboBox";
            this._languageComboBox.Size = new System.Drawing.Size(130, 21);
            this._languageComboBox.TabIndex = 4;
            this._languageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(428, 375);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Language:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _compileButton
            // 
            this._compileButton.Location = new System.Drawing.Point(139, 371);
            this._compileButton.Name = "_compileButton";
            this._compileButton.Size = new System.Drawing.Size(80, 23);
            this._compileButton.TabIndex = 6;
            this._compileButton.Text = "Compile";
            this._compileButton.UseVisualStyleBackColor = true;
            this._compileButton.Click += new System.EventHandler(this.CompileButton_Click);
            // 
            // _haltButton
            // 
            this._haltButton.Location = new System.Drawing.Point(347, 372);
            this._haltButton.Name = "_haltButton";
            this._haltButton.Size = new System.Drawing.Size(75, 23);
            this._haltButton.TabIndex = 7;
            this._haltButton.Text = "Halt";
            this._haltButton.UseVisualStyleBackColor = true;
            this._haltButton.Click += new System.EventHandler(this.HaltButton_Click);
            // 
            // MsgTextBox
            // 
            this.MsgTextBox.Location = new System.Drawing.Point(12, 515);
            this.MsgTextBox.Multiline = true;
            this.MsgTextBox.Name = "MsgTextBox";
            this.MsgTextBox.ReadOnly = true;
            this.MsgTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MsgTextBox.Size = new System.Drawing.Size(610, 109);
            this.MsgTextBox.TabIndex = 2;
            // 
            // ScriptTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 624);
            this.Controls.Add(this._haltButton);
            this.Controls.Add(this._compileButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._languageComboBox);
            this.Controls.Add(this._referencesButton);
            this.Controls.Add(this.MsgTextBox);
            this.Controls.Add(this._compileErrorTextBox);
            this.Controls.Add(this._runButton);
            this.Controls.Add(this._sourceCodeTextBox);
            this.Name = "ScriptTest";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _sourceCodeTextBox;
        private System.Windows.Forms.Button _runButton;
        private System.Windows.Forms.TextBox _compileErrorTextBox;
        private System.Windows.Forms.Button _referencesButton;
        private System.Windows.Forms.ComboBox _languageComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _compileButton;
        private System.Windows.Forms.Button _haltButton;
        private System.Windows.Forms.TextBox MsgTextBox;
    }
}

