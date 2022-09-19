using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomizableAnalysisLibrary.Services
{
    public class Execution
    {
        public IInputNode InputNode { get; set; }
        public IReadOnlyList<ICalculationNode> CalculationNodes => calculationNodes;
        public IOutputNode OutputNode { get; set; }
        public bool IsSingleOutput { get; set; } = false;
        public JoinType SingleOutputJoinType { get; set; } = JoinType.Row;
        public OutputSuffixType OutputSuffixType { get; set; } = OutputSuffixType.Number;
        public bool IsAddFileNameColumnToOutput { get; set; } = false;

        private List<ICalculationNode> calculationNodes = default;

        internal Execution(IInputNode inputNode, IEnumerable<ICalculationNode> calculationNodes, IOutputNode outputNode)
        {
            this.InputNode = inputNode;
            this.calculationNodes = new List<ICalculationNode>(calculationNodes);
            this.OutputNode = outputNode;
        }

        public void AddCalclationNode(ICalculationNode node) => calculationNodes.Add(node);
        public void RemoveCalculationNode(ICalculationNode node) => calculationNodes.Remove(node);
        
        public void SwapCalculationNode(int i, int j) 
        {
            var node = calculationNodes[i];
            calculationNodes[i] = calculationNodes[j];
            calculationNodes[j] = node;
        }

        public void Run(string[] inputPaths, string outputPath)
        {
            if (IsSingleOutput && inputPaths.Length > 1)
            {
                RunToSingleOutput(inputPaths, outputPath);
            }
            else if (inputPaths.Length == 1)
            {
                RunSingle(inputPaths[0], outputPath);
            }
            else
            {
                var dir = Path.GetDirectoryName(outputPath);
                var name = Path.GetFileNameWithoutExtension(outputPath);
                var extension = Path.GetExtension(outputPath);
                for(int i = 0; i < inputPaths.Length; ++i)
                {
                    string suffix = OutputSuffixType switch
                    {
                        OutputSuffixType.Number => i.ToString(),
                        OutputSuffixType.FileName => Path.GetFileNameWithoutExtension(inputPaths[i]),
                        _ => throw new System.NotImplementedException(),
                    };

                    RunSingle(inputPaths[i], $"{dir}{Path.DirectorySeparatorChar}{name}_{suffix}{extension}");
                }
            }
        }

        private void RunSingle(string inputPath, string outputPath)
        {
            var table = GetResult(inputPath);
            OutputNode.SetComments($"Input file path: {inputPath}");
            OutputNode.Output(outputPath, table);
        }

        private void RunToSingleOutput(IEnumerable<string> inputPaths, string outputPath)
        {
            var comments = new List<string>();
            var results = new List<Table>();

            comments.Add("Input file list;");
            foreach (var inputPath in inputPaths)
            {
                comments.Add($"    {comments.Count}: {inputPath}");
                results.Add(GetResult(inputPath));
            }

            Table table = SingleOutputJoinType.JoinTables(results);
            OutputNode.SetComments(comments.ToArray());
            OutputNode.Output(outputPath, table);
        }

        private Table GetResult(string inputPath)
        {
            var table = InputNode.Load(inputPath);
            foreach (var node in calculationNodes)
            {
                table = node.Run(table);
            }

            if (IsAddFileNameColumnToOutput == false) return table;

            var columns = new List<IReadOnlyList<Value>>();
            for(int i = 0; i < table.ColumnCount; ++i)
            {
                columns.Add(table.GetColumn(i));
            }

            int maxLength = columns.Max(x => x.Count);
            var nameValue = new Value(Path.GetFileNameWithoutExtension(inputPath)); 
            var nameColumn = Enumerable.Repeat(nameValue, maxLength);
            var addedColumns = columns.Prepend(nameColumn);
            return Table.CreateFromColumns(addedColumns);
        }
    }
}
