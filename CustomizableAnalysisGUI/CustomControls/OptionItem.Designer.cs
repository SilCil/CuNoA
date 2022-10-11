
namespace CustomizableAnalysisGUI
{
    partial class OptionItem
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownDouble = new CustomizableAnalysisGUI.NumericUpDownNoMouseWheel();
            this.labelInt = new System.Windows.Forms.Label();
            this.labelDouble = new System.Windows.Forms.Label();
            this.labelString = new System.Windows.Forms.Label();
            this.checkBoxBool = new System.Windows.Forms.CheckBox();
            this.textBoxString = new System.Windows.Forms.TextBox();
            this.numericUpDownInt = new CustomizableAnalysisGUI.NumericUpDownNoMouseWheel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDouble)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInt)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownDouble, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelInt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDouble, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelString, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxBool, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxString, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownInt, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(300, 150);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numericUpDownDouble
            // 
            this.numericUpDownDouble.DecimalPlaces = 3;
            this.numericUpDownDouble.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownDouble.Location = new System.Drawing.Point(93, 41);
            this.numericUpDownDouble.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownDouble.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownDouble.Name = "numericUpDownDouble";
            this.numericUpDownDouble.Size = new System.Drawing.Size(204, 32);
            this.numericUpDownDouble.TabIndex = 6;
            this.numericUpDownDouble.TabStop = false;
            this.numericUpDownDouble.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownDouble.ThousandsSeparator = true;
            this.numericUpDownDouble.ValueChanged += new System.EventHandler(this.CallValueChanged);
            this.numericUpDownDouble.Leave += new System.EventHandler(this.CallValueChanged);
            // 
            // labelInt
            // 
            this.labelInt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInt.AutoEllipsis = true;
            this.labelInt.Location = new System.Drawing.Point(0, 0);
            this.labelInt.Margin = new System.Windows.Forms.Padding(0);
            this.labelInt.Name = "labelInt";
            this.labelInt.Size = new System.Drawing.Size(90, 38);
            this.labelInt.TabIndex = 0;
            this.labelInt.Text = "Int Value";
            this.labelInt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDouble
            // 
            this.labelDouble.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDouble.AutoEllipsis = true;
            this.labelDouble.Location = new System.Drawing.Point(0, 38);
            this.labelDouble.Margin = new System.Windows.Forms.Padding(0);
            this.labelDouble.Name = "labelDouble";
            this.labelDouble.Size = new System.Drawing.Size(90, 38);
            this.labelDouble.TabIndex = 1;
            this.labelDouble.Text = "Double Value";
            this.labelDouble.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelString
            // 
            this.labelString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelString.AutoEllipsis = true;
            this.labelString.Location = new System.Drawing.Point(0, 76);
            this.labelString.Margin = new System.Windows.Forms.Padding(0);
            this.labelString.Name = "labelString";
            this.labelString.Size = new System.Drawing.Size(90, 38);
            this.labelString.TabIndex = 2;
            this.labelString.Text = "String Value";
            this.labelString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxBool
            // 
            this.checkBoxBool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxBool, 2);
            this.checkBoxBool.Location = new System.Drawing.Point(3, 117);
            this.checkBoxBool.Name = "checkBoxBool";
            this.checkBoxBool.Size = new System.Drawing.Size(294, 30);
            this.checkBoxBool.TabIndex = 4;
            this.checkBoxBool.TabStop = false;
            this.checkBoxBool.Text = "Bool Value";
            this.checkBoxBool.UseVisualStyleBackColor = true;
            this.checkBoxBool.CheckedChanged += new System.EventHandler(this.CallValueChanged);
            // 
            // textBoxString
            // 
            this.textBoxString.AcceptsReturn = true;
            this.textBoxString.AcceptsTab = true;
            this.textBoxString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxString.Location = new System.Drawing.Point(93, 79);
            this.textBoxString.Multiline = true;
            this.textBoxString.Name = "textBoxString";
            this.textBoxString.Size = new System.Drawing.Size(204, 32);
            this.textBoxString.TabIndex = 7;
            this.textBoxString.TabStop = false;
            this.textBoxString.WordWrap = false;
            this.textBoxString.TextChanged += new System.EventHandler(this.textBoxString_TextChanged);
            this.textBoxString.Leave += new System.EventHandler(this.CallValueChanged);
            // 
            // numericUpDownInt
            // 
            this.numericUpDownInt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownInt.Location = new System.Drawing.Point(93, 3);
            this.numericUpDownInt.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownInt.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownInt.Name = "numericUpDownInt";
            this.numericUpDownInt.Size = new System.Drawing.Size(204, 32);
            this.numericUpDownInt.TabIndex = 8;
            this.numericUpDownInt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownInt.ThousandsSeparator = true;
            this.numericUpDownInt.ValueChanged += new System.EventHandler(this.CallValueChanged);
            // 
            // OptionItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Yu Gothic UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(300, 0);
            this.Name = "OptionItem";
            this.Size = new System.Drawing.Size(300, 150);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDouble)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelInt;
        private System.Windows.Forms.Label labelDouble;
        private System.Windows.Forms.Label labelString;
        private System.Windows.Forms.CheckBox checkBoxBool;
        private System.Windows.Forms.TextBox textBoxString;
        private NumericUpDownNoMouseWheel numericUpDownInt;
        private NumericUpDownNoMouseWheel numericUpDownDouble;
    }
}
