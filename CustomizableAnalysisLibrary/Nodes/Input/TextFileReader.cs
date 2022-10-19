using System;
using System.Collections.Generic;
using System.IO;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("入力/テキストファイル")]
    public class TextFileReader : IInputNode, IOptionNode
    {
        private const StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries;

        public char[] Separators { get; set; } = new char[] { ' ', '\t', ',' };
        public string CommentSymbol { get; set; } = "#";

        public InputType InputType => InputType.File;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("区切り文字", new Value(new string(Separators)));
            yield return ("コメント文字", new Value(CommentSymbol));
        }

        public void SetOptions(params Value[] options)
        {
            Separators = options[0].ToString().ToCharArray();
            CommentSymbol = options[1].ToString();
        }

        public Table Load(string path)
        {
            var rows = new List<Value[]>();

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (IsSkipLine(line)) continue;
                var words = line.Split(Separators, stringSplitOptions);
                var values = words.ToValueArray();
                rows.Add(values);
            }

            return Table.CreateFromRows(rows);
        }

        private bool IsSkipLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return true;
            if (line.Trim().StartsWith(CommentSymbol)) return true;
            return false;
        }
    }
}
