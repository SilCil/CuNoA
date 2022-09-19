using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CustomizableAnalysisLibrary.Services
{
    public class NodeLoader
    {
        private static readonly HashSet<string> ExculdeAssemblyPrefix = new HashSet<string>()
        {
            "MathNet",
            "Microsoft",
            "System",
        };

        private Dictionary<string, Type> inputNodeTypes = new Dictionary<string, Type>();
        private Dictionary<string, Type> calculationNodeTypes = new Dictionary<string, Type>();
        private Dictionary<string, Type> outputNodeTypes = new Dictionary<string, Type>();

        public void LoadDefaultAssembly()
        {
            // dllフォルダのパスを取得.
            var exeAssembly = Assembly.GetEntryAssembly();
            var exeDirectory = Path.GetDirectoryName(exeAssembly.Location);

            // dllをロード.
            foreach (var path in Directory.GetFiles(exeDirectory, "*.dll", SearchOption.AllDirectories))
            {
                var name = Path.GetFileName(path);
                if (ExculdeAssemblyPrefix.Any(x => name.StartsWith(x))) continue;

                var assembly = Assembly.LoadFrom(path);
                RegisterNode(assembly);
            }
        }

        public void RegisterNode(string filePath) => RegisterNode(Assembly.LoadFrom(filePath));

        public void RegisterNode(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            foreach (var t in types)
            {
                if (t.IsAbstract) continue;

                var nodeAttribute = t.GetCustomAttribute<NodeAttribute>();
                if (nodeAttribute is null) continue;

                if (typeof(IInputNode).IsAssignableFrom(t))
                {
                    inputNodeTypes[nodeAttribute.path] = t;
                }

                if (typeof(ICalculationNode).IsAssignableFrom(t))
                {
                    calculationNodeTypes[nodeAttribute.path] = t;
                }

                if (typeof(IOutputNode).IsAssignableFrom(t))
                {
                    outputNodeTypes[nodeAttribute.path] = t;
                }
            }
        }

        public IEnumerable<string> GetInputNodePaths() => inputNodeTypes.Keys;
        public IEnumerable<string> GetCalculationNodePaths() => calculationNodeTypes.Keys;
        public IEnumerable<string> GetOutputNodePaths() => outputNodeTypes.Keys;

        public string GetPath(Type type)
        {
            var nodeAttribute = type.GetCustomAttribute<NodeAttribute>();
            return nodeAttribute?.path;
        }

        public string GetPath(object node) => GetPath(node.GetType());

        public IInputNode InstantiateInputNode(string path) => Instantiate<IInputNode>(inputNodeTypes[path]);
        public IOutputNode InstantiateOutputNode(string path) => Instantiate<IOutputNode>(outputNodeTypes[path]);
        public ICalculationNode InstantiateCalculationNode(string path) => Instantiate<ICalculationNode>(calculationNodeTypes[path]);
        private T Instantiate<T>(Type type) => (T) Activator.CreateInstance(type);
    }
}
