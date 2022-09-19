using CustomizableAnalysisLibrary;
using System;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public partial class NodeBlock : UserControl
    {
        private IOptionNode optionNode = default;

        public NodeBlock()
        {
            InitializeComponent();
        }

        public int NodeIndex { get; private set; }
        public int NodeCount { get; private set; }

        public event Action<NodeBlock> ReplaceClicked;
        public event Action<NodeBlock> RemoveClicked;
        public event Action<NodeBlock> MoveUpClicked;
        public event Action<NodeBlock> MoveDownClickd;

        private void SetIndex(int index, int count)
        {
            NodeIndex = index;
            NodeCount = count;
            SetActive(buttonUp, index != 0);
            SetActive(buttonDown, index != count - 1);
        }

        public void SetInputNode(string path, IInputNode node)
        {
            SetFixedNode(path, node);
        }

        public void SetOutputNode(string path, IOutputNode node)
        {
            SetFixedNode(path, node);
        }

        public void SetCalculationNode(string path, ICalculationNode node, int index, int count)
        {
            SetIndex(index, count);
            SetHeader(path);
            SetActive(buttonRemove, true);
            SetActive(buttonReplace, false);
            BindOptionNode(node);
        }

        private void SetFixedNode(string path, object node)
        {
            NodeIndex = NodeCount = -1;
            SetHeader(path);
            SetActive(buttonUp, false);
            SetActive(buttonDown, false);
            SetActive(buttonRemove, false);
            SetActive(buttonReplace, true);
            BindOptionNode(node);
        }

        private void SetHeader(string path)
        {
            labelHeader.Text = path;
        }

        private void SetActive(Button button, bool isActive)
        {
            button.Enabled = isActive;
            button.Visible = isActive;
        }

        private void BindOptionNode(object node)
        {
            optionNode = node as IOptionNode;
            OptionsToView();
        }

        private bool isSuspending = false;

        private void OptionsToView()
        {
            if (optionNode is null)
            {
                tableLayoutPanelOptions.Controls.Clear();
                return;
            }

            tableLayoutPanelOptions.SuspendLayout();
            isSuspending = true;

            int index = 0;
            foreach ((var label, var value) in optionNode.GetOptions())
            {
                if (index >= tableLayoutPanelOptions.Controls.Count) 
                {
                    var optionItem = new OptionItem();
                    optionItem.OnValueChanged += ViewToOptions;
                    optionItem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    tableLayoutPanelOptions.Controls.Add(optionItem);
                }

                var item = tableLayoutPanelOptions.Controls[index] as OptionItem;
                item.SetValue(label, value);

                ++index;
            }

            while(index < tableLayoutPanelOptions.Controls.Count)
            {
                var removeItem = tableLayoutPanelOptions.Controls[tableLayoutPanelOptions.Controls.Count - 1];
                var option = removeItem as OptionItem;
                option.OnValueChanged -= ViewToOptions;
                tableLayoutPanelOptions.Controls.Remove(removeItem);
            }

            tableLayoutPanelOptions.ResumeLayout();
            isSuspending = false;
        }

        private void ViewToOptions()
        {
            if (isSuspending) return;
            if (optionNode is null) return;

            Value[] values = new Value[tableLayoutPanelOptions.Controls.Count];
            for(int i = 0; i < tableLayoutPanelOptions.Controls.Count; ++i)
            {
                var item = tableLayoutPanelOptions.Controls[i] as OptionItem;
                values[i] = item.GetValue();
            }

            try
            {
                optionNode.SetOptions(values);
            }
            catch(Exception ex)
            {
                Utility.ShowErrorMessage(ex);
            }

            OptionsToView();
        }

        private void buttonUp_Click(object sender, EventArgs e) => MoveUpClicked?.Invoke(this);
        private void buttonDown_Click(object sender, EventArgs e) => MoveDownClickd?.Invoke(this);
        private void buttonRemove_Click(object sender, EventArgs e) => RemoveClicked?.Invoke(this);
        private void buttonReplace_Click(object sender, EventArgs e) => ReplaceClicked?.Invoke(this);
    }
}
