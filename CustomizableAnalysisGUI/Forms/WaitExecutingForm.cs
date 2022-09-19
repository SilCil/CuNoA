using CustomizableAnalysisLibrary.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public partial class WaitExecutingForm : Form
    {
        public WaitExecutingForm()
        {
            InitializeComponent();
        }

        /// <remarks>モーダルでウィンドウを表示</remarks>
        public void Execute(IWin32Window owner, Execution execution, string[] inputPaths, string outputPath)
        {
            Task.Run(() =>
            {
                DialogResult = DialogResult.None;
                Task.Delay(100).Wait();
                try
                {
                    execution.Run(inputPaths, outputPath);
                    DialogResult = DialogResult.OK;
                }
                catch(Exception ex)
                {
                    Utility.ShowErrorMessage(ex);
                    DialogResult = DialogResult.Cancel;
                }
            });
            ShowDialog(owner);
        }

        private int tickCount = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            tickCount = (tickCount + 1) % 3;
            string text = "Processing";
            for(int i = 0; i < tickCount + 1; ++i) text += ".";
            label.Text = text;
        }
    }
}
