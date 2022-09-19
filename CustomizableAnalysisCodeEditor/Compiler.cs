using CustomizableAnalysisLibrary;
using CustomizableAnalysisLibrary.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace CustomizableAnalysisLibrary.CodeEditor
{
    internal static class Compiler
    {
        private static readonly Assembly[] roslynPadAssemblies = new[]
        {
            typeof(RoslynCodeEditor).Assembly, // RoslynPad.Editor.Windows
            typeof(GlyphExtensions).Assembly, // RoslynPad.Roslyn.Windows
        };

        private static readonly Assembly[] assemblies = new[]
        {
            typeof(object).Assembly,
            typeof(Enum).Assembly,
            typeof(Enumerable).Assembly,
            typeof(ICalculationNode).Assembly,
            typeof(ExtractColumn).Assembly,
            typeof(MathNet.Numerics.LinearAlgebra.Vector<>).Assembly,
            Assembly.GetExecutingAssembly(),
        };

        private static MetadataReference[] metadataReferences = default;

        private static MetadataReference[] GetMetaDataReferences()
        {
            if (metadataReferences != default) return metadataReferences;

            var list = new List<MetadataReference>();
            list.AddRange(assemblies.Select(x => MetadataReference.CreateFromFile(x.Location)));

            var assemblyDirectoryPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            list.AddRange(new MetadataReference[] 
            {
                MetadataReference.CreateFromFile($"{assemblyDirectoryPath}/mscorlib.dll"),
                MetadataReference.CreateFromFile($"{assemblyDirectoryPath}/netstandard.dll"),
                MetadataReference.CreateFromFile($"{assemblyDirectoryPath}/System.Runtime.dll"),
            });

            metadataReferences = list.ToArray();
            return metadataReferences;
        }

        internal static readonly RoslynHost roslynHost = new RoslynHost(roslynPadAssemblies, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: assemblies), disabledDiagnostics: ImmutableArray.Create("CS7021"));

        public static void CompileToFile(string source, string outputPath)
        {
            var assemblyName = Path.GetFileNameWithoutExtension(outputPath) + ".dll";

            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp8);
            var syntaxTrees = new SyntaxTree[]
            {
                CSharpSyntaxTree.ParseText(source, options)
            };
            
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, GetMetaDataReferences(), compilationOptions);

            using var stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.Read);
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
        }
    }
}
