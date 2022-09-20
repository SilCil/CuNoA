
namespace CustomizableAnalysisGUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainFormMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAsNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deplicateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.layoutPanelContents = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelOutputPath = new System.Windows.Forms.Panel();
            this.outputLabel = new System.Windows.Forms.Label();
            this.buttonOutputPath = new System.Windows.Forms.Button();
            this.textBoxOutputPath = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonDetailSetting = new System.Windows.Forms.Button();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.checkBoxIsSingleOutput = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.calculationNodeMenu = new CustomizableAnalysisGUI.NodeMenuStrip();
            this.inputNodeMenu = new CustomizableAnalysisGUI.NodeMenuStrip();
            this.outputNodeMenu = new CustomizableAnalysisGUI.NodeMenuStrip();
            this.mainFormMenuStrip.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelOutputPath.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainFormMenuStrip
            // 
            this.mainFormMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.LibraryToolStripMenuItem});
            this.mainFormMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainFormMenuStrip.Name = "mainFormMenuStrip";
            this.mainFormMenuStrip.Size = new System.Drawing.Size(584, 24);
            this.mainFormMenuStrip.TabIndex = 0;
            this.mainFormMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.openAsNewMenuItem,
            this.saveMenuItem,
            this.deplicateMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "ファイル";
            // 
            // newMenuItem
            // 
            this.newMenuItem.Name = "newMenuItem";
            this.newMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newMenuItem.Size = new System.Drawing.Size(231, 22);
            this.newMenuItem.Text = "新規";
            this.newMenuItem.Click += new System.EventHandler(this.newMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(231, 22);
            this.openMenuItem.Text = "開く";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // openAsNewMenuItem
            // 
            this.openAsNewMenuItem.Name = "openAsNewMenuItem";
            this.openAsNewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.openAsNewMenuItem.Size = new System.Drawing.Size(231, 22);
            this.openAsNewMenuItem.Text = "別ウィンドウで開く";
            this.openAsNewMenuItem.Click += new System.EventHandler(this.openAsNewMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveMenuItem.Size = new System.Drawing.Size(231, 22);
            this.saveMenuItem.Text = "保存";
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // deplicateMenuItem
            // 
            this.deplicateMenuItem.Name = "deplicateMenuItem";
            this.deplicateMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.deplicateMenuItem.Size = new System.Drawing.Size(231, 22);
            this.deplicateMenuItem.Text = "ウィンドウを複製";
            this.deplicateMenuItem.Click += new System.EventHandler(this.deplicateMenuItem_Click);
            // 
            // LibraryToolStripMenuItem
            // 
            this.LibraryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadLibraryToolStripMenuItem,
            this.CodeEditorToolStripMenuItem});
            this.LibraryToolStripMenuItem.Name = "LibraryToolStripMenuItem";
            this.LibraryToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.LibraryToolStripMenuItem.Text = "ライブラリ";
            // 
            // LoadLibraryToolStripMenuItem
            // 
            this.LoadLibraryToolStripMenuItem.Name = "LoadLibraryToolStripMenuItem";
            this.LoadLibraryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.LoadLibraryToolStripMenuItem.Text = "ライブラリ(*.dll)のロード";
            this.LoadLibraryToolStripMenuItem.Click += new System.EventHandler(this.LoadLibraryToolStripMenuItem_Click);
            // 
            // CodeEditorToolStripMenuItem
            // 
            this.CodeEditorToolStripMenuItem.Name = "CodeEditorToolStripMenuItem";
            this.CodeEditorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.CodeEditorToolStripMenuItem.Text = "コードエディター";
            this.CodeEditorToolStripMenuItem.Click += new System.EventHandler(this.CodeEditorToolStripMenuItem_Click);
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.BackColor = System.Drawing.Color.DimGray;
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.layoutPanelContents, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.panelBottom, 0, 2);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 3;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(584, 557);
            this.mainTableLayoutPanel.TabIndex = 1;
            // 
            // layoutPanelContents
            // 
            this.layoutPanelContents.AutoScroll = true;
            this.layoutPanelContents.BackColor = System.Drawing.Color.DimGray;
            this.layoutPanelContents.ColumnCount = 1;
            this.layoutPanelContents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanelContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanelContents.Location = new System.Drawing.Point(10, 10);
            this.layoutPanelContents.Margin = new System.Windows.Forms.Padding(10);
            this.layoutPanelContents.Name = "layoutPanelContents";
            this.layoutPanelContents.RowCount = 1;
            this.layoutPanelContents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelContents.Size = new System.Drawing.Size(564, 437);
            this.layoutPanelContents.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelOutputPath, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 460);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(578, 44);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panelOutputPath
            // 
            this.panelOutputPath.Controls.Add(this.outputLabel);
            this.panelOutputPath.Controls.Add(this.buttonOutputPath);
            this.panelOutputPath.Controls.Add(this.textBoxOutputPath);
            this.panelOutputPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutputPath.Location = new System.Drawing.Point(0, 0);
            this.panelOutputPath.Margin = new System.Windows.Forms.Padding(0);
            this.panelOutputPath.Name = "panelOutputPath";
            this.panelOutputPath.Size = new System.Drawing.Size(578, 44);
            this.panelOutputPath.TabIndex = 0;
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Font = new System.Drawing.Font("Yu Gothic UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.outputLabel.Location = new System.Drawing.Point(6, 14);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(37, 19);
            this.outputLabel.TabIndex = 2;
            this.outputLabel.Text = "出力";
            // 
            // buttonOutputPath
            // 
            this.buttonOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonOutputPath.ForeColor = System.Drawing.Color.DimGray;
            this.buttonOutputPath.Location = new System.Drawing.Point(533, 8);
            this.buttonOutputPath.Name = "buttonOutputPath";
            this.buttonOutputPath.Size = new System.Drawing.Size(38, 29);
            this.buttonOutputPath.TabIndex = 1;
            this.buttonOutputPath.Text = "...";
            this.buttonOutputPath.UseVisualStyleBackColor = false;
            this.buttonOutputPath.Click += new System.EventHandler(this.buttonOutputPath_Click);
            // 
            // textBoxOutputPath
            // 
            this.textBoxOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputPath.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxOutputPath.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxOutputPath.ForeColor = System.Drawing.Color.DimGray;
            this.textBoxOutputPath.Location = new System.Drawing.Point(49, 8);
            this.textBoxOutputPath.Name = "textBoxOutputPath";
            this.textBoxOutputPath.Size = new System.Drawing.Size(482, 29);
            this.textBoxOutputPath.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonDetailSetting);
            this.panelBottom.Controls.Add(this.buttonExecute);
            this.panelBottom.Controls.Add(this.checkBoxIsSingleOutput);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 507);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(584, 50);
            this.panelBottom.TabIndex = 3;
            // 
            // buttonDetailSetting
            // 
            this.buttonDetailSetting.ForeColor = System.Drawing.Color.DimGray;
            this.buttonDetailSetting.Location = new System.Drawing.Point(197, 8);
            this.buttonDetailSetting.Name = "buttonDetailSetting";
            this.buttonDetailSetting.Size = new System.Drawing.Size(108, 34);
            this.buttonDetailSetting.TabIndex = 2;
            this.buttonDetailSetting.Text = "詳細設定";
            this.buttonDetailSetting.UseVisualStyleBackColor = true;
            this.buttonDetailSetting.Click += new System.EventHandler(this.buttonDetailSetting_Click);
            // 
            // buttonExecute
            // 
            this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecute.Font = new System.Drawing.Font("Yu Gothic UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonExecute.ForeColor = System.Drawing.Color.DimGray;
            this.buttonExecute.Location = new System.Drawing.Point(500, 10);
            this.buttonExecute.Margin = new System.Windows.Forms.Padding(10);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(74, 34);
            this.buttonExecute.TabIndex = 0;
            this.buttonExecute.Text = "実行";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // checkBoxIsSingleOutput
            // 
            this.checkBoxIsSingleOutput.AutoSize = true;
            this.checkBoxIsSingleOutput.Font = new System.Drawing.Font("Yu Gothic UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxIsSingleOutput.Location = new System.Drawing.Point(10, 10);
            this.checkBoxIsSingleOutput.Name = "checkBoxIsSingleOutput";
            this.checkBoxIsSingleOutput.Size = new System.Drawing.Size(181, 29);
            this.checkBoxIsSingleOutput.TabIndex = 1;
            this.checkBoxIsSingleOutput.Text = "結果を1つにまとめる";
            this.checkBoxIsSingleOutput.UseVisualStyleBackColor = true;
            this.checkBoxIsSingleOutput.CheckedChanged += new System.EventHandler(this.checkBoxIsSingleOutput_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.RestoreDirectory = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.AddExtension = false;
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.SupportMultiDottedExtensions = true;
            // 
            // calculationNodeMenu
            // 
            this.calculationNodeMenu.Name = "calculationNodeMenu";
            this.calculationNodeMenu.Size = new System.Drawing.Size(61, 4);
            this.calculationNodeMenu.MenuItemClicked += new System.Action<string>(this.AddCalculationNode);
            // 
            // inputNodeMenu
            // 
            this.inputNodeMenu.Name = "inputNodeMenu";
            this.inputNodeMenu.Size = new System.Drawing.Size(61, 4);
            this.inputNodeMenu.MenuItemClicked += new System.Action<string>(this.ReplaceInputNode);
            // 
            // outputNodeMenu
            // 
            this.outputNodeMenu.Name = "outputNodeMenu";
            this.outputNodeMenu.Size = new System.Drawing.Size(61, 4);
            this.outputNodeMenu.MenuItemClicked += new System.Action<string>(this.ReplaceOutputNode);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(584, 581);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Controls.Add(this.mainFormMenuStrip);
            this.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MainMenuStrip = this.mainFormMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "MainForm";
            this.Text = "CuNoA";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.mainFormMenuStrip.ResumeLayout(false);
            this.mainFormMenuStrip.PerformLayout();
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelOutputPath.ResumeLayout(false);
            this.panelOutputPath.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainFormMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.TableLayoutPanel layoutPanelContents;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox checkBoxIsSingleOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonOutputPath;
        private System.Windows.Forms.TextBox textBoxOutputPath;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private NodeMenuStrip calculationNodeMenu;
        private NodeMenuStrip inputNodeMenu;
        private NodeMenuStrip outputNodeMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openAsNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deplicateMenuItem;
        private System.Windows.Forms.Panel panelOutputPath;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonDetailSetting;
        private System.Windows.Forms.ToolStripMenuItem LibraryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CodeEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadLibraryToolStripMenuItem;
    }
}

