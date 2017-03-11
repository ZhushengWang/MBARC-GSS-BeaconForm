namespace nScriptHostTest
{
    partial class AddReferenceDialog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.dotNetTabPage = new System.Windows.Forms.TabPage();
            this._gacListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.browseTabPage = new System.Windows.Forms.TabPage();
            this._removeFileButton = new System.Windows.Forms.Button();
            this._addFileButton = new System.Windows.Forms.Button();
            this._filesListBox = new System.Windows.Forms.ListBox();
            this._buttonPanel = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.dotNetTabPage.SuspendLayout();
            this.browseTabPage.SuspendLayout();
            this._buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.dotNetTabPage);
            this.tabControl1.Controls.Add(this.browseTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(409, 337);
            this.tabControl1.TabIndex = 0;
            // 
            // dotNetTabPage
            // 
            this.dotNetTabPage.Controls.Add(this._gacListView);
            this.dotNetTabPage.Location = new System.Drawing.Point(4, 22);
            this.dotNetTabPage.Name = "dotNetTabPage";
            this.dotNetTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.dotNetTabPage.Size = new System.Drawing.Size(401, 311);
            this.dotNetTabPage.TabIndex = 0;
            this.dotNetTabPage.Text = ".NET";
            this.dotNetTabPage.UseVisualStyleBackColor = true;
            // 
            // _gacListView
            // 
            this._gacListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._gacListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this._gacListView.FullRowSelect = true;
            this._gacListView.Location = new System.Drawing.Point(6, 3);
            this._gacListView.Name = "_gacListView";
            this._gacListView.ShowGroups = false;
            this._gacListView.Size = new System.Drawing.Size(387, 257);
            this._gacListView.TabIndex = 2;
            this._gacListView.UseCompatibleStateImageBehavior = false;
            this._gacListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Component Name";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Runtime";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Path";
            this.columnHeader4.Width = 200;
            // 
            // browseTabPage
            // 
            this.browseTabPage.Controls.Add(this._removeFileButton);
            this.browseTabPage.Controls.Add(this._addFileButton);
            this.browseTabPage.Controls.Add(this._filesListBox);
            this.browseTabPage.Location = new System.Drawing.Point(4, 22);
            this.browseTabPage.Name = "browseTabPage";
            this.browseTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.browseTabPage.Size = new System.Drawing.Size(401, 311);
            this.browseTabPage.TabIndex = 1;
            this.browseTabPage.Text = "Browse";
            this.browseTabPage.UseVisualStyleBackColor = true;
            // 
            // _removeFileButton
            // 
            this._removeFileButton.Location = new System.Drawing.Point(227, 237);
            this._removeFileButton.Name = "_removeFileButton";
            this._removeFileButton.Size = new System.Drawing.Size(94, 23);
            this._removeFileButton.TabIndex = 2;
            this._removeFileButton.Text = "Remove File...";
            this._removeFileButton.UseVisualStyleBackColor = true;
            this._removeFileButton.Click += new System.EventHandler(this.RemoveFileButton_Click);
            // 
            // _addFileButton
            // 
            this._addFileButton.Location = new System.Drawing.Point(79, 237);
            this._addFileButton.Name = "_addFileButton";
            this._addFileButton.Size = new System.Drawing.Size(94, 23);
            this._addFileButton.TabIndex = 1;
            this._addFileButton.Text = "Add File...";
            this._addFileButton.UseVisualStyleBackColor = true;
            this._addFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // _filesListBox
            // 
            this._filesListBox.FormattingEnabled = true;
            this._filesListBox.Location = new System.Drawing.Point(8, 6);
            this._filesListBox.Name = "_filesListBox";
            this._filesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this._filesListBox.Size = new System.Drawing.Size(385, 225);
            this._filesListBox.TabIndex = 0;
            // 
            // _buttonPanel
            // 
            this._buttonPanel.Controls.Add(this._cancelButton);
            this._buttonPanel.Controls.Add(this._okButton);
            this._buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._buttonPanel.Location = new System.Drawing.Point(0, 288);
            this._buttonPanel.Name = "_buttonPanel";
            this._buttonPanel.Size = new System.Drawing.Size(409, 49);
            this._buttonPanel.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(322, 14);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(234, 14);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // AddReferenceDialog
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(409, 337);
            this.Controls.Add(this._buttonPanel);
            this.Controls.Add(this.tabControl1);
            this.Name = "AddReferenceDialog";
            this.Text = "AddReferenceDialog";
            this.tabControl1.ResumeLayout(false);
            this.dotNetTabPage.ResumeLayout(false);
            this.browseTabPage.ResumeLayout(false);
            this._buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage dotNetTabPage;
        private System.Windows.Forms.TabPage browseTabPage;
        private System.Windows.Forms.Panel _buttonPanel;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.ListView _gacListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button _addFileButton;
        private System.Windows.Forms.ListBox _filesListBox;
        private System.Windows.Forms.Button _removeFileButton;
    }
}