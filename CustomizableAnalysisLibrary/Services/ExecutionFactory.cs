using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using CustomizableAnalysisLibrary.Nodes;

namespace CustomizableAnalysisLibrary.Services
{
    public class ExecutionFactory
    {
        public string InputNodePath { get; set; }
        public string[] CalclationNodePaths { get; set; }
        public string OutputNodePath { get; set; }
        public string[][] Options { get; set; }
        public bool IsSingleOutput { get; set; }
        public bool IsAddingOutputPath { get; set; }
        public int JoinType { get; set; }
        public int SuffixType { get; set; }

        private readonly static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            // HTMLなど、不正を防ぐため+など特定のものはUnicodeRanges.Allではエスケープされてしまう.
            // Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            // ファイル保存機能として使う（ウェブ運用はしない）のでUnsafeでOK.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        public static Execution Create(string jsonText, NodeLoader loader)
        {
            var factory = Deserialize(jsonText);
            return factory.Create(loader);
        }

        public static Execution CreateDefault()
        {
            return new Execution(new TextFileReader(), new ICalculationNode[] { }, new TextFileWriter());
        }

        public static Execution CreateEmpty()
        {
            return new Execution(null, new ICalculationNode[] { }, null);
        }

        public static Execution CreateClone(Execution original, NodeLoader loader)
        {
            var factory = CreateFromExecution(original, loader);
            return factory.Create(loader);
        }

        public static string CreateJsonText(Execution execution, NodeLoader loader)
        {
            var factory = CreateFromExecution(execution, loader);
            return factory.Serialize();
        }

        private Execution Create(NodeLoader loader)
        {
            int count = 0;
            void SetOptions(object node)
            {
                if (node is IOptionNode option)
                {
                    option.SetOptions(Options[count].Select(x => new Value(x)).ToArray());
                    ++count;
                }
            }

            var inputNode = loader.InstantiateInputNode(InputNodePath);
            SetOptions(inputNode);

            var calculationNodes = new List<ICalculationNode>();
            foreach (var path in CalclationNodePaths)
            {
                var node = loader.InstantiateCalculationNode(path);
                calculationNodes.Add(node);
                SetOptions(node);
            }

            var outputNode = loader.InstantiateOutputNode(OutputNodePath);
            SetOptions(outputNode);

            var instance = new Execution(inputNode, calculationNodes, outputNode);
            instance.IsSingleOutput = IsSingleOutput;
            instance.IsAddFileNameColumnToOutput = IsAddingOutputPath;
            instance.SingleOutputJoinType = (JoinType)JoinType;
            instance.OutputSuffixType = (OutputSuffixType)SuffixType;

            return instance;
        }

        private static ExecutionFactory CreateFromExecution(Execution execution, NodeLoader loader)
        {
            var settings = new ExecutionFactory();

            settings.InputNodePath = loader.GetPath(execution.InputNode.GetType());
            settings.CalclationNodePaths = execution.CalculationNodes.Select(x => loader.GetPath(x.GetType())).ToArray();
            settings.OutputNodePath = loader.GetPath(execution.OutputNode.GetType());

            var options = new List<IEnumerable<string>>();
            void GetOptions(object node)
            {
                if(node is IOptionNode optionNode)
                {
                    options.Add(optionNode.GetOptions().Select(x => x.Item2.ToString()));
                }
            }

            GetOptions(execution.InputNode);
            foreach(var node in execution.CalculationNodes)
            {
                GetOptions(node);
            }
            GetOptions(execution.OutputNode);

            settings.Options = options.Select(x => x.ToArray()).ToArray();

            settings.IsSingleOutput = execution.IsSingleOutput;
            settings.IsAddingOutputPath = execution.IsAddFileNameColumnToOutput;
            settings.JoinType = (int)execution.SingleOutputJoinType;
            settings.SuffixType = (int)execution.OutputSuffixType;

            return settings;
        }

        private string Serialize()
        {
            return JsonSerializer.Serialize(this, serializerOptions);
        }

        private static ExecutionFactory Deserialize(string json)
        {
            return JsonSerializer.Deserialize<ExecutionFactory>(json, serializerOptions);
        }
    }
}
