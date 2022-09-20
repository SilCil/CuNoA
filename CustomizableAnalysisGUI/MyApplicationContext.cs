using CustomizableAnalysisLibrary.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CustomizableAnalysisGUI
{
    public class MyApplicationContext : ApplicationContext
    {
        private NodeLoader loader = default;
        private List<MainForm> forms = new List<MainForm>();

        public MyApplicationContext()
        {
            LoadAssembly();

            Execution execution = null;
            var args = Environment.GetCommandLineArgs();
            // args[0] はこのアプリケーションを指す?
            if(args.Length > 1 && File.Exists(args[1]))
            {
                try
                {
                    var text = File.ReadAllText(args[1]);
                    execution = ExecutionFactory.Create(text, loader);
                }
                catch (Exception ex)
                {
                    Utility.ShowErrorMessage(ex);
                }
            }

            var form = CreateForm(execution);
            this.MainForm = form;
            form.Show();
        }

        private void LoadAssembly()
        {
            using (var loadingForm = new WaitInitializingForm())
            {
                try
                {
                    loadingForm.Setup(null, out loader);
                }
                catch (Exception ex)
                {
                    Utility.ShowErrorMessage(ex);
                }
            }
        }

        private MainForm CreateForm(Execution execution = default)
        {
            var form = new MainForm(loader, execution);
            form.FormClosed += Form_Closed;
            form.NewMenuItemClicked += Form_NewMenuItemClicked;
            form.OpenAsNewMenuItemClicked += Form_OpenAsNewMenuItemClicked;
            forms.Add(form);
            return form;
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            var form = sender as MainForm;
            if (form is null) return;
            forms.Remove(form);
            this.MainForm = forms.FirstOrDefault();
            if (this.MainForm == null) Application.Exit();
        }

        private void Form_OpenAsNewMenuItemClicked(Execution obj)
        {
            var form = CreateForm(obj);
            form.Show();
        }

        private void Form_NewMenuItemClicked()
        {
            var form = CreateForm();
            form.Show();
        }
    }
}
