using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public class NumericUpDownNoMouseWheel : NumericUpDown
    {
        protected override void OnMouseWheel(MouseEventArgs e) { }
    }
}
