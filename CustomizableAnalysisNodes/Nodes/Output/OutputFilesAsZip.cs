using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("出力/列を別ファイルに(zip)")]
    public class TextFilesZipWriter : IOptionNode, IOutputNode
    {
        private string commentSymbol = "#";
        private string separator = "\t";
        private string header = string.Empty;

        private string[] comments = default;
        private int[] commonColumnIndices = new int[] { };

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("区切り文字", new Value(separator));
            yield return ("コメント文字", new Value(commentSymbol));
            yield return ("ヘッダー", new Value(header));
            yield return ("共通列の数", new Value(commonColumnIndices.Length));

            for(int i = 0; i < commonColumnIndices.Length; ++i)
            {
                yield return ($"共通列{i}", new Value(commonColumnIndices[i]));
            }
        }

        public void SetOptions(params Value[] options)
        {
            separator = options[0].ToString();
            commentSymbol = options[1].ToString();
            header = options[2].ToString();
            commonColumnIndices = new int[Math.Max(0, options[3].ToInt())];

            for(int i = 0; i < commonColumnIndices.Length; ++i)
            {
                var index = Math.Max(3 + i, options.Length - 1);
                commonColumnIndices[i] = options[index].ToInt();
            }
        }

        public void SetComments(params string[] comments) => this.comments = comments;

        public void Output(string path, Table result)
        {
            var commonColumns = new IReadOnlyList<Value>[commonColumnIndices.Length];
            for(int i = 0; i < commonColumns.Length; ++i)
            {
                commonColumns[i] = result.GetColumn(commonColumnIndices[i]);
            }

            using (ZipArchive zipArchive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                for(int i = 0; i < result.ColumnCount; ++i)
                {
                    var columns = new List<IReadOnlyList<Value>>(commonColumns);
                    columns.Add(result.GetColumn(i));
                    var outputTable = Table.CreateFromColumns(columns);

                    var entry = zipArchive.CreateEntry(string.Format("{0:D6}.txt", i));
                    using (var writer = new StreamWriter(entry.Open()))
                    {
                        foreach (var comment in comments)
                        {
                            writer.Write(commentSymbol);
                            writer.WriteLine(comment);
                        }

                        if (string.IsNullOrEmpty(header) == false)
                        {
                            writer.Write(commentSymbol);
                            writer.WriteLine(header);
                        }

                        for(int rowIndex = 0; rowIndex < outputTable.RowCount; ++rowIndex)
                        {
                            writer.WriteLine(string.Join(separator, outputTable.GetRow(rowIndex)));
                        }
                    }
                }
            }
        }
    }
}
