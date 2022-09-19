using CustomizableAnalysisLibrary.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public partial class WaitInitializingForm : Form
    {
        public WaitInitializingForm()
        {
            InitializeComponent();
        }

        /// <remarks>モーダルでウィンドウを表示</remarks>
        public void Setup(IWin32Window owner, out NodeLoader loader)
        {
            var _loader = new NodeLoader();
            Task.Run(() => 
            {
                DialogResult = DialogResult.None;
                Task.Delay(100).Wait();
                _loader.LoadDefaultAssembly();
                DialogResult = DialogResult.OK;
            });

            ShowDialog(owner);
            loader = _loader;
        }

        private int tickCount = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            tickCount = (tickCount + 1) % 3;
            var text = "Initializing";
            for (int i = 0; i < tickCount + 1; ++i) text += ".";
            label.Text = text;
        }
    }
}
