using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("出力/テキストファイル")]
    public class TextFileWriter : IOptionNode, IOutputNode
    {
        private string commentSymbol = "#";
        private string separator = "\t";
        private string header = string.Empty;

        private string[] comments = default;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("区切り文字", new Value(separator));
            yield return ("コメント文字", new Value(commentSymbol));
            yield return ("ヘッダー", new Value(header));
        }

        public void SetOptions(params Value[] options)
        {
            separator = options[0].ToStringValue().StringValue;
            commentSymbol = options[1].ToStringValue().StringValue;
            header = options[2].ToStringValue().StringValue;
        }

        public void SetComments(params string[] comments) => this.comments = comments;

        public void Output(string path, Table result)
        {
            var output = new StringBuilder();

            foreach(var comment in comments)
            {
                output.Append(commentSymbol);
                output.AppendLine(comment);
            }

            if (string.IsNullOrEmpty(header) == false)
            {
                output.Append(commentSymbol);
                output.AppendLine(header);
            }

            for(int i = 0; i < result.RowCount; ++i)
            {
                output.AppendLine(string.Join(separator, result.GetRow(i)));
            }

            File.WriteAllText(path, output.ToString());
        }
    }
}
