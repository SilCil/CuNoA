using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    internal static class Utility
    {
        private static HashSet<string> UsedAssemblyNames = new HashSet<string>();
        private static Dictionary<string, Assembly> CashedAssemblies = new Dictionary<string, Assembly>();

        public static Assembly LoadFromSource(string key, string source)
        {
            if (CashedAssemblies.TryGetValue(key, out var assembly)) return assembly;

            var assemblyName = string.Empty;
            while (UsedAssemblyNames.Contains(assemblyName = Path.ChangeExtension(Path.GetRandomFileName(), ".dll")));
            UsedAssemblyNames.Add(assemblyName);

            CashedAssemblies[key] = Compile(source, assemblyName);
            return CashedAssemblies[key];
        }

        private static Assembly Compile(string source, string assemblyName)
        {
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp8);
            var syntaxTrees = new SyntaxTree[]
            {
                CSharpSyntaxTree.ParseText(source, options)
            };
            var assemblyDirectoryPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enum).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile($"{assemblyDirectoryPath}/netstandard.dll"),
                MetadataReference.CreateFromFile($"{assemblyDirectoryPath}/System.Runtime.dll"),
                MetadataReference.CreateFromFile(typeof(MathNet.Numerics.LinearAlgebra.Vector<>).Assembly.Location),
            };
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, references, compilationOptions);

            using var stream = new MemoryStream();
            var emitResult = compilation.Emit(stream);
            if (emitResult.Success == false)
            {
                var sb = new StringBuilder();
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    var pos = diagnostic.Location.GetLineSpan();
                    var location = $"({pos.Path}@Line{(pos.StartLinePosition.Line + 1)}:{(pos.StartLinePosition.Character + 1)})";
                    sb.AppendLine($"[{diagnostic.Severity}, {location}]{diagnostic.Id}, {diagnostic.GetMessage()}");
                }
                throw new Exception(sb.ToString());
            }

            stream.Seek(0, SeekOrigin.Begin);
            return AssemblyLoadContext.Default.LoadFromStream(stream);
        }
    }
}
