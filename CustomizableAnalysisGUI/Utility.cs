using System;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public static class Utility
    {
        public static void ShowErrorMessage(Exception ex) => ShowErrorMessage(ex.Message);

        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
