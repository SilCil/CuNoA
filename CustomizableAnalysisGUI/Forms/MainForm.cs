using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CustomizableAnalysisLibrary.Services;

namespace CustomizableAnalysisGUI
{
    public partial class MainForm : Form
    {
        private NodeLoader loader = default;
        private Execution execution = default;

        private NodeBlock inputNodeBlock = default;
        private List<NodeBlock> calcNodeBlocks = default;
        private Button addCalcNodeButton = default;
        private NodeBlock outputNodeBlock = default;

        public event Action NewMenuItemClicked;
        public event Action<Execution> OpenAsNewMenuItemClicked;

        public MainForm(NodeLoader loader, Execution execution = default)
        {
            this.loader = loader;
            this.execution = execution;
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            execution ??= ExecutionFactory.CreateDefault();

            // Contents.
            InitializeNodeBlocks();
            ApplyFromExecution();

            // Node Menus.
            SetupNodeMenus();
        }

        private void SetupNodeMenus()
        {
            inputNodeMenu.SetPaths(loader.GetInputNodePaths());
            calculationNodeMenu.SetPaths(loader.GetCalculationNodePaths());
            outputNodeMenu.SetPaths(loader.GetOutputNodePaths());
        }

        private void InitializeNodeBlocks()
        {
            inputNodeBlock = new NodeBlock();
            inputNodeBlock.ReplaceClicked += _ => inputNodeMenu.Show(Cursor.Position);

            calcNodeBlocks = new List<NodeBlock>();

            addCalcNodeButton = new Button();
            addCalcNodeButton.Text = "+";
            addCalcNodeButton.TextAlign = ContentAlignment.MiddleCenter;
            addCalcNodeButton.Margin = new Padding(0, 10, 0, 10);
            addCalcNodeButton.Height = 30;
            addCalcNodeButton.BackColor = SystemColors.ControlDark;
            addCalcNodeButton.ForeColor = SystemColors.ControlText;
            addCalcNodeButton.Click += (_, __) => calculationNodeMenu.Show(Cursor.Position);

            outputNodeBlock = new NodeBlock();
            outputNodeBlock.ReplaceClicked += _ => outputNodeMenu.Show(Cursor.Position);
        }

        private bool isSuspending = false;

        private void ApplyToExecution()
        {
            if (isSuspending) return;
            if (execution is null) return;
            execution.IsSingleOutput = checkBoxIsSingleOutput.Checked;
        }

        private void ApplyFromExecution()
        {
            if (execution is null) return;

            isSuspending = true;
            layoutPanelContents.SuspendLayout();

            // +----------+
            // | Input    |
            // +----------+
            // | Calc     |
            // +----------+
            // |    +     |
            // +----------+
            // | Output   |
            // +----------+

            layoutPanelContents.RowStyles.Clear();
            layoutPanelContents.Controls.Clear();

            // Input.
            inputNodeBlock.SetInputNode(loader.GetPath(execution.InputNode), execution.InputNode);
            AddContent(inputNodeBlock);

            // Calc.
            while (calcNodeBlocks.Count < execution.CalculationNodes.Count)
            {
                var item = new NodeBlock();
                item.RemoveClicked += RemoveCalclationNode;
                item.MoveUpClicked += MoveUpCalculationNode;
                item.MoveDownClickd += MoveDownCalculationNode;
                calcNodeBlocks.Add(item);
            }
            while (calcNodeBlocks.Count > execution.CalculationNodes.Count) calcNodeBlocks.RemoveAt(0);
            for (int i = 0; i < calcNodeBlocks.Count; ++i)
            {
                var node = execution.CalculationNodes[i];
                var block = calcNodeBlocks[i];
                block.SetCalculationNode(loader.GetPath(node), node, i, execution.CalculationNodes.Count);
                AddContent(block);
            }

            // +.
            AddContent(addCalcNodeButton);

            // Output.
            outputNodeBlock.SetOutputNode(loader.GetPath(execution.OutputNode), execution.OutputNode);
            AddContent(outputNodeBlock);
            
            layoutPanelContents.ResumeLayout();

            checkBoxIsSingleOutput.Checked = execution.IsSingleOutput;
            isSuspending = false;
        }

        private void AddContent(Control control)
        {
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            control.Width = layoutPanelContents.Width - 20;
            layoutPanelContents.Controls.Add(control);
        }

        private void checkBoxIsSingleOutput_CheckedChanged(object sender, EventArgs e)
        {
            ApplyToExecution();
        }

        private void AddCalculationNode(string path) => ModifyNode(() => execution.AddCalclationNode(loader.InstantiateCalculationNode(path)));
        private void ReplaceInputNode(string path) => ModifyNode(() => execution.InputNode = loader.InstantiateInputNode(path));
        private void ReplaceOutputNode(string path) => ModifyNode(() => execution.OutputNode = loader.InstantiateOutputNode(path));

        private void RemoveCalclationNode(NodeBlock remove) => ModifyNode(() => 
        {
            var index = calcNodeBlocks.IndexOf(remove);
            if (index < 0) return;
            var removeNode = execution.CalculationNodes[index];
            execution.RemoveCalculationNode(removeNode);
        });

        private void MoveUpCalculationNode(NodeBlock node) => ModifyNode(() => execution.SwapCalculationNode(node.NodeIndex, node.NodeIndex - 1));
        private void MoveDownCalculationNode(NodeBlock node) => ModifyNode(() => execution.SwapCalculationNode(node.NodeIndex, node.NodeIndex + 1));

        private void ModifyNode(Action action)
        {
            ApplyToExecution();
            action?.Invoke();
            ApplyFromExecution();
        }

        private void buttonOutputPath_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "出力ファイルを選択";
            saveFileDialog.Filter = default;
            saveFileDialog.DefaultExt = default;
            saveFileDialog.AddExtension = false;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxOutputPath.Text = saveFileDialog.FileName;
            }
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            if (execution is null) return;
            ApplyToExecution();

            OpenInputPathsDialog(out var inputPaths);
            if (inputPaths is null) return;

            var outputPath = textBoxOutputPath.Text;

            using (var waitForm = new WaitExecutingForm())
            {
                try
                {
                    waitForm.Execute(this, execution, inputPaths, outputPath);
                }
                catch(Exception ex)
                {
                    Utility.ShowErrorMessage(ex);
                }
            }
        }

        private void OpenInputPathsDialog(out string[] paths)
        {
            paths = null;
            switch(execution.InputNode.InputType)
            {
                case CustomizableAnalysisLibrary.InputType.File:
                    openFileDialog.Multiselect = true;
                    openFileDialog.Title = "処理するファイルを選択（複数選択可）";
                    openFileDialog.Filter = default;
                    openFileDialog.CheckFileExists = true;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        paths = openFileDialog.FileNames;
                    }
                    break;
                case CustomizableAnalysisLibrary.InputType.Folder:
                    var folderDialog = new FolderBrowserEx.FolderBrowserDialog();
                    folderDialog.Title = "処理するフォルダを選択（複数選択可）";
                    folderDialog.AllowMultiSelect = true;
                    folderDialog.DefaultFolder = folderDialog.InitialFolder = openFileDialog.InitialDirectory;
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        paths = folderDialog.SelectedFolders.ToArray();
                        openFileDialog.InitialDirectory = Path.GetDirectoryName(folderDialog.SelectedFolder);
                    }
                    break;
            }
        }

        private void buttonDetailSetting_Click(object sender, EventArgs e)
        {
            if (execution is null) return;
            ApplyToExecution();

            var clone = ExecutionFactory.CreateClone(execution, loader);
            using (var dialog = new DetailSettingForm(clone))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    execution = clone;
                    ApplyFromExecution();
                }
            }
        }

        private void newMenuItem_Click(object sender, EventArgs e) => NewMenuItemClicked?.Invoke();

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            ApplyToExecution();

            saveFileDialog.Title = "保存先を選択";
            saveFileDialog.Filter = "Customizable analysis files (*.can)|*.can";
            saveFileDialog.DefaultExt = ".can";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var text = ExecutionFactory.CreateJsonText(execution, loader);
                    File.WriteAllText(saveFileDialog.FileName, text);
                }
                catch(Exception ex)
                {
                    Utility.ShowErrorMessage(ex);
                }
            }
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            var obj = OpenCusomizableAnalysisFile();
            if (obj is null) return;
            execution = obj;
            ApplyFromExecution();
        }

        private void openAsNewMenuItem_Click(object sender, EventArgs e)
        {
            var obj = OpenCusomizableAnalysisFile();
            if (obj is null) return;
            OpenAsNewMenuItemClicked?.Invoke(obj);
        }

        private void deplicateMenuItem_Click(object sender, EventArgs e)
        {
            ApplyToExecution();
            var text = ExecutionFactory.CreateJsonText(execution, loader);
            var obj = ExecutionFactory.Create(text, loader);
            OpenAsNewMenuItemClicked?.Invoke(obj);
        }

        private Execution OpenCusomizableAnalysisFile()
        {
            openFileDialog.Title = "ファイルを選択";
            openFileDialog.Filter = "Customizable analysis files (*.can)|*.can|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;

            Execution ret = null;

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var text = File.ReadAllText(openFileDialog.FileName);
                    ret = ExecutionFactory.Create(text, loader);
                }
                catch (Exception ex)
                {
                    Utility.ShowErrorMessage(ex);
                }
            }

            return ret;
        }

        private void CodeEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var editorWindow = new CustomizableAnalysisLibrary.CodeEditor.EditorWindow();
                editorWindow.ShowDialog();
            }
            catch(Exception ex)
            {
                Utility.ShowErrorMessage(ex);
            }
        }

        private void LoadLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "読み込むライブラリを選択（複数選択可）";
            openFileDialog.Filter = "Library (*.dll)|*.dll";
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var paths = openFileDialog.FileNames;

                foreach(var path in paths)
                {
                    try
                    {
                        loader.RegisterNode(path);
                    }
                    catch(Exception ex)
                    {
                        Utility.ShowErrorMessage(ex);
                    }
                }

                SetupNodeMenus();
            }
        }
    }
}
