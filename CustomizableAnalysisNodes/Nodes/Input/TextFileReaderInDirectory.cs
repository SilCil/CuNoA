using System;
using System.Collections.Generic;
using System.IO;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("入力/フォルダ内のテキストファイル")]
    public class TextFileReaderInDirectory : IOptionNode, IInputNode
    {
        public char[] Separators { get; set; } = new char[] { ' ', '\t', ',' };
        public string CommentSymbol { get; set; } = "#";
        public string SearchPattern { get; set; } = "*.dat";
        public bool IncludeSubDirectory { get; set; } = true;
        public bool CombineMultipleFiles { get; set; } = true;

        public InputType InputType => InputType.Folder;

        private TextFileReader m_textFileReader = new TextFileReader();

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("区切り文字", new Value(new string(Separators)));
            yield return ("コメント文字", new Value(CommentSymbol));
            yield return ("ファイル名", new Value(SearchPattern));
            yield return ("サブフォルダを含める", new Value(IncludeSubDirectory));
            yield return ("複数の場合は列を結合", new Value(CombineMultipleFiles));
        }

        public void SetOptions(params Value[] options)
        {
            Separators = options[0].ToString().ToCharArray();
            CommentSymbol = options[1].ToString();
            SearchPattern = options[2].ToString();
            IncludeSubDirectory = options[3].ToBool();
            CombineMultipleFiles = options[4].ToBool();

            ApplyOptions();
        }

        private void ApplyOptions()
        {
            m_textFileReader.Separators = Separators;
            m_textFileReader.CommentSymbol = CommentSymbol;
        }

        public Table Load(string path)
        {
            ApplyOptions();

            var dir = Path.ChangeExtension(Path.GetFullPath(path), "");
            var option = (IncludeSubDirectory) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var filePaths = Directory.GetFiles(dir, SearchPattern, option);

            if (filePaths is null || filePaths.Length == 0)
            {
                throw new Exception($"{SearchPattern} not found.");
            }

            if (CombineMultipleFiles)
            {
                var columns = new List<IReadOnlyList<Value>>();

                foreach (var filePath in filePaths)
                {
                    var table = m_textFileReader.Load(filePath);

                    for(int i = 0; i < table.ColumnCount; ++i)
                    {
                        columns.Add(table.GetColumn(i));
                    }
                }

                return Table.CreateFromColumns(columns);
            }
            else
            {
                return m_textFileReader.Load(filePaths[0]);
            }
        }
    }
}
