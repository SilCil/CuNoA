using CustomizableAnalysisLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public partial class OptionItem : UserControl
    {
        public OptionItem()
        {
            InitializeComponent();
        }

        public event Action OnValueChanged;

        private CustomizableAnalysisLibrary.ValueType valueType = default;

        public void SetValue(string name, Value value)
        {
            valueType = value.ValueType;

            tableLayoutPanel1.SuspendLayout();
            DeactivateItems();

            switch (valueType)
            {
                case CustomizableAnalysisLibrary.ValueType.Int:
                    labelInt.Visible = true;
                    labelInt.Text = name;
                    numericUpDownInt.Visible = true;
                    numericUpDownInt.Value = value.IntValue;
                    break;
                case CustomizableAnalysisLibrary.ValueType.Double:
                    labelDouble.Visible = true;
                    labelDouble.Text = name;
                    numericUpDownDouble.Visible = true;
                    numericUpDownDouble.Value = (decimal)value.DoubleValue;
                    break;
                case CustomizableAnalysisLibrary.ValueType.String:
                    labelString.Visible = true;
                    labelString.Text = name;
                    textBoxString.Visible = true;
                    textBoxString.Text = value.StringValue;
                    break;
                case CustomizableAnalysisLibrary.ValueType.Bool:
                    checkBoxBool.Visible = true;
                    checkBoxBool.Text = name;
                    checkBoxBool.Checked = value.BoolValue;
                    break;
            }

            this.Height = labelInt.Height;
            tableLayoutPanel1.ResumeLayout();
        }

        public Value GetValue()
        {
            return valueType switch
            {
                CustomizableAnalysisLibrary.ValueType.Int => new Value((int)numericUpDownInt.Value),
                CustomizableAnalysisLibrary.ValueType.Double => new Value((double)numericUpDownDouble.Value),
                CustomizableAnalysisLibrary.ValueType.String => new Value(textBoxString.Text),
                CustomizableAnalysisLibrary.ValueType.Bool => new Value(checkBoxBool.Checked),
                _ => throw new NotImplementedException(),
            };
        }

        private void CallValueChanged(object sender, EventArgs e) => OnValueChanged?.Invoke();

        private void DeactivateItems()
        {
            labelDouble.Visible = false;
            labelInt.Visible = false;
            labelString.Visible = false;
            numericUpDownDouble.Visible = false;
            numericUpDownInt.Visible = false;
            textBoxString.Visible = false;
            checkBoxBool.Visible = false;
        }

        private bool isSuspendingTextChanged = false;
        private void textBoxString_TextChanged(object sender, EventArgs e)
        {
            if (isSuspendingTextChanged) return;

            isSuspendingTextChanged = true;
            textBoxString.Text = string.IsNullOrEmpty(textBoxString.Text) ? string.Empty : textBoxString.Lines[0];
            isSuspendingTextChanged = false;
        }
    }
}
