using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("入力/テキストファイル")]
    public class TextFileReader : IInputNode, IOptionNode
    {
        private const StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries;

        private char[] separators = new char[] { ' ', '\t', ',' };
        private string commentSymbol = "#";

        public InputType InputType => InputType.File;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("区切り文字", new Value(new string(separators)));
            yield return ("コメント文字", new Value(commentSymbol));
        }

        public void SetOptions(params Value[] options)
        {
            separators = options[0].ToString().ToCharArray();
            commentSymbol = options[1].ToString();
        }

        public Table Load(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var rows = new List<IEnumerable<Value>>();

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (IsSkipLine(line)) continue;
                var words = line.Split(separators, stringSplitOptions);
                var values = words.Select(x => new Value(x));
                rows.Add(values);
            }

            return Table.CreateFromRows(rows);
        }

        private bool IsSkipLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return true;
            if (line.Trim().StartsWith(commentSymbol)) return true;
            return false;
        }
    }
}
