
namespace CustomizableAnalysisGUI
{
    partial class DetailSettingForm
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
            this.comboBoxJoinType = new System.Windows.Forms.ComboBox();
            this.labelJoinType = new System.Windows.Forms.Label();
            this.checkBoxIsSingleOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxIsAddingOutputPath = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxSuffixType = new System.Windows.Forms.ComboBox();
            this.labelSuffixType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxJoinType
            // 
            this.comboBoxJoinType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxJoinType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxJoinType.Location = new System.Drawing.Point(154, 65);
            this.comboBoxJoinType.Name = "comboBoxJoinType";
            this.comboBoxJoinType.Size = new System.Drawing.Size(146, 33);
            this.comboBoxJoinType.TabIndex = 0;
            this.comboBoxJoinType.SelectedIndexChanged += new System.EventHandler(this.comboBoxJoinType_SelectedIndexChanged);
            // 
            // labelJoinType
            // 
            this.labelJoinType.AutoSize = true;
            this.labelJoinType.Location = new System.Drawing.Point(43, 68);
            this.labelJoinType.Name = "labelJoinType";
            this.labelJoinType.Size = new System.Drawing.Size(75, 25);
            this.labelJoinType.TabIndex = 1;
            this.labelJoinType.Text = "まとめ方";
            // 
            // checkBoxIsSingleOutput
            // 
            this.checkBoxIsSingleOutput.AutoSize = true;
            this.checkBoxIsSingleOutput.Location = new System.Drawing.Point(23, 27);
            this.checkBoxIsSingleOutput.Name = "checkBoxIsSingleOutput";
            this.checkBoxIsSingleOutput.Size = new System.Drawing.Size(181, 29);
            this.checkBoxIsSingleOutput.TabIndex = 2;
            this.checkBoxIsSingleOutput.Text = "結果を1つにまとめる";
            this.checkBoxIsSingleOutput.UseVisualStyleBackColor = true;
            this.checkBoxIsSingleOutput.CheckedChanged += new System.EventHandler(this.checkBoxIsSingleOutput_CheckedChanged);
            // 
            // checkBoxIsAddingOutputPath
            // 
            this.checkBoxIsAddingOutputPath.AutoSize = true;
            this.checkBoxIsAddingOutputPath.Location = new System.Drawing.Point(23, 159);
            this.checkBoxIsAddingOutputPath.Name = "checkBoxIsAddingOutputPath";
            this.checkBoxIsAddingOutputPath.Size = new System.Drawing.Size(286, 29);
            this.checkBoxIsAddingOutputPath.TabIndex = 2;
            this.checkBoxIsAddingOutputPath.Text = "入力ファイル名を1列目に出力する";
            this.checkBoxIsAddingOutputPath.UseVisualStyleBackColor = true;
            this.checkBoxIsAddingOutputPath.CheckedChanged += new System.EventHandler(this.checkBoxIsAddingOutputPath_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonOK.Location = new System.Drawing.Point(380, 169);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(74, 42);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // comboBoxSuffixType
            // 
            this.comboBoxSuffixType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSuffixType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxSuffixType.Location = new System.Drawing.Point(154, 108);
            this.comboBoxSuffixType.Name = "comboBoxSuffixType";
            this.comboBoxSuffixType.Size = new System.Drawing.Size(146, 33);
            this.comboBoxSuffixType.TabIndex = 4;
            this.comboBoxSuffixType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSuffixType_SelectedIndexChanged);
            // 
            // labelSuffixType
            // 
            this.labelSuffixType.AutoSize = true;
            this.labelSuffixType.Location = new System.Drawing.Point(43, 111);
            this.labelSuffixType.Name = "labelSuffixType";
            this.labelSuffixType.Size = new System.Drawing.Size(104, 25);
            this.labelSuffixType.TabIndex = 5;
            this.labelSuffixType.Text = "ファイル末尾";
            // 
            // DetailSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(466, 223);
            this.Controls.Add(this.labelSuffixType);
            this.Controls.Add(this.comboBoxSuffixType);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxIsAddingOutputPath);
            this.Controls.Add(this.checkBoxIsSingleOutput);
            this.Controls.Add(this.labelJoinType);
            this.Controls.Add(this.comboBoxJoinType);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailSettingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxJoinType;
        private System.Windows.Forms.Label labelJoinType;
        private System.Windows.Forms.CheckBox checkBoxIsSingleOutput;
        private System.Windows.Forms.CheckBox checkBoxIsAddingOutputPath;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxSuffixType;
        private System.Windows.Forms.Label labelSuffixType;
    }
}