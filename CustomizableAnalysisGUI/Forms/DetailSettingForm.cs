using CustomizableAnalysisLibrary;
using CustomizableAnalysisLibrary.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public partial class DetailSettingForm : Form
    {
        private readonly static Color DisableColor = SystemColors.ControlDark;
        private Color defaultColor = default;

        private Execution _execution = default;
        private bool isSuspending = false;

        private ComboBoxItem<JoinType>[] joinTypeItems = default;
        private ComboBoxItem<OutputSuffixType>[] suffixTypeItems = default;

        private class ComboBoxItem<T>
        {
            public readonly T value = default;
            public readonly string name = default;

            public ComboBoxItem(T value, string name)
            {
                this.value = value;
                this.name = name;
            }

            public override string ToString() => name;
        }

        public DetailSettingForm(Execution execution)
        {
            InitializeComponent();

            defaultColor = comboBoxJoinType.BackColor;

            comboBoxJoinType.SuspendLayout();
            comboBoxJoinType.Items.Clear();
            joinTypeItems = new ComboBoxItem<JoinType>[] 
            {
                new ComboBoxItem<JoinType>(JoinType.Row, "行"),
                new ComboBoxItem<JoinType>(JoinType.Column, "列"),
            };
            comboBoxJoinType.Items.AddRange(joinTypeItems);
            comboBoxJoinType.ResumeLayout();

            comboBoxSuffixType.SuspendLayout();
            comboBoxSuffixType.Items.Clear();
            suffixTypeItems = new ComboBoxItem<OutputSuffixType>[] 
            {
                new ComboBoxItem<OutputSuffixType>(OutputSuffixType.Number, "連番"),
                new ComboBoxItem<OutputSuffixType>(OutputSuffixType.FileName, "入力ファイル名"),
            };
            comboBoxSuffixType.Items.AddRange(suffixTypeItems);
            comboBoxSuffixType.ResumeLayout();

            _execution = execution;
            ExecutionToView();
        }

        private void ExecutionToView()
        {
            isSuspending = true;

            checkBoxIsSingleOutput.Checked = _execution.IsSingleOutput;
            checkBoxIsAddingOutputPath.Checked = _execution.IsAddFileNameColumnToOutput;
            comboBoxJoinType.Enabled = _execution.IsSingleOutput;
            comboBoxJoinType.BackColor = (comboBoxJoinType.Enabled) ? defaultColor : DisableColor;
            comboBoxJoinType.SelectedItem = joinTypeItems.First(x => x.value == _execution.SingleOutputJoinType);
            comboBoxSuffixType.Enabled = !_execution.IsSingleOutput;
            comboBoxSuffixType.BackColor = (comboBoxSuffixType.Enabled) ? defaultColor : DisableColor;
            comboBoxSuffixType.SelectedItem = suffixTypeItems.First(x => x.value == _execution.OutputSuffixType);

            isSuspending = false;
        }

        private void checkBoxIsSingleOutput_CheckedChanged(object sender, EventArgs e)
        {
            if (isSuspending) return;
            _execution.IsSingleOutput = checkBoxIsSingleOutput.Checked;
            ExecutionToView();
        }

        private void comboBoxJoinType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSuspending) return;

            var item = comboBoxJoinType.SelectedItem as ComboBoxItem<JoinType>;
            if (item is null) return;

            _execution.SingleOutputJoinType = item.value;
            ExecutionToView();
        }

        private void comboBoxSuffixType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSuspending) return;

            var item = comboBoxSuffixType.SelectedItem as ComboBoxItem<OutputSuffixType>;
            if (item is null) return;

            _execution.OutputSuffixType = item.value;
            ExecutionToView();
        }

        private void checkBoxIsAddingOutputPath_CheckedChanged(object sender, EventArgs e)
        {
            if (isSuspending) return;

            _execution.IsAddFileNameColumnToOutput = checkBoxIsAddingOutputPath.Checked;
            ExecutionToView();
        }
    }
}
