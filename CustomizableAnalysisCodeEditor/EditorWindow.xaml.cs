using Microsoft.Win32;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomizableAnalysisLibrary.CodeEditor
{
    /// <summary>
    /// EditorWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EditorWindow : Window
    {
        private static SampleCodeList sampleCodeList = default;
        private readonly SaveFileDialog m_saveFileDialog = default;
        
        public EditorWindow()
        {
            if (sampleCodeList is null)
            {
                sampleCodeList = new SampleCodeList();
                sampleCodeList.Setup();
            }

            InitializeComponent();

            m_saveFileDialog = new SaveFileDialog
            {
                Title = "保存先ファイルを選択",
                OverwritePrompt = true,
                ValidateNames = true,
                AddExtension = true,
                DefaultExt = "dll",
                Filter = "Library|*.dll",
                RestoreDirectory = true
            };

            SetSampleCodeMenuItems(sampleCodeList.SampleCodes.Keys);
        }

        private void CodeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            CodeEditor.Initialize(Compiler.roslynHost, new ClassificationHighlightColors(), Directory.GetCurrentDirectory(), string.Empty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (m_saveFileDialog.ShowDialog() != true) return;

            ConsoleTextBox.Text = string.Empty;
            try
            {
                Compiler.CompileToFile(CodeEditor.Text, m_saveFileDialog.FileName);
                ConsoleTextBox.Text = $"Succeeded: {m_saveFileDialog.FileName}";
            }
            catch(Exception ex)
            {
                ConsoleTextBox.Text = ex.Message;
            }
        }

        private void CloseMenu_Click(object sender, RoutedEventArgs e) => Close();

        private void SetSampleCodeMenuItems(IEnumerable<string> paths)
        {
            var items = new Dictionary<string, MenuItem>();
            var topLevelItems = new HashSet<MenuItem>();
            foreach (var path in paths)
            {
                CreateSampleCodeMenuItem(path, ref items, ref topLevelItems);
            }
            foreach(var item in topLevelItems)
            {
                CodeTemplateMenu.Items.Add(item);
            }
        }

        private void CreateSampleCodeMenuItem(string path, ref Dictionary<string, MenuItem> items, ref HashSet<MenuItem> topLevelItems)
        {
            var nodes = path.Split("/");

            MenuItem parentNode = null;
            for(int i = 0; i < nodes.Length; ++i)
            {
                var header = nodes[i];
                var currentPath = string.Join("/", nodes.Take(i + 1));

                items.TryAdd(currentPath, new MenuItem() { Header = header });
                var currentNode = items[currentPath];

                if (parentNode == null)
                {
                    topLevelItems.Add(currentNode);
                }
                else if (parentNode.Items.Contains(currentNode) == false)
                {
                    parentNode.Items.Add(currentNode);
                }

                parentNode = currentNode;
            }

            if (items.ContainsKey(path))
            {
                items[path].Click += (_, __) => SampleCodeMenuItem_Click(path);
            }
        }

        private void SampleCodeMenuItem_Click(string path)
        {
            CodeEditor.Text = sampleCodeList.SampleCodes[path];
        }
    }
}
