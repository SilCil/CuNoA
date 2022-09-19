using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CustomizableAnalysisLibrary.CodeEditor
{
    internal class SampleCodeList
    {
        private const string NodeTemplateDirectory = "Nodes";

        private readonly Dictionary<string, string> m_sampleCodes = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> SampleCodes => m_sampleCodes;

        public void Setup()
        {
            var solutionDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var targetDir = Path.Combine(solutionDir, NodeTemplateDirectory);
            var sourceFiles = Directory.GetFiles(targetDir, "*.cs", SearchOption.AllDirectories);

            var generator = new SampleCodeGenerator();
            foreach (var sourceFile in sourceFiles)
            {
                var source = File.ReadAllText(sourceFile);

                var originalTree = CSharpSyntaxTree.ParseText(source);
                var root = originalTree.GetCompilationUnitRoot();

                generator.ClearNodePath();
                var convertedRoot = generator.Visit(root);
                var path = generator.GetNodePath();

                if (string.IsNullOrWhiteSpace(path)) continue;
                m_sampleCodes[path] = convertedRoot.ToFullString();
            }
        }

        private class SampleCodeGenerator : CSharpSyntaxRewriter
        {
            private string nodePath = default;

            public string GetNodePath() => nodePath;
            public void ClearNodePath() => nodePath = null;

            public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
            {
                var original = base.VisitNamespaceDeclaration(node) as NamespaceDeclarationSyntax;
                var newName = SyntaxFactory.ParseName($"{original.Name}.Custom");
                return original.WithName(newName);
//                return original.Members.FirstOrDefault();
            }

            public override SyntaxNode VisitAttribute(AttributeSyntax node)
            {
                var original = base.VisitAttribute(node) as AttributeSyntax;

                if (original.Name.ToString() != "Node") return original;

                var oldArgumentList = original.ArgumentList;
                var oldArg = oldArgumentList.Arguments[0];
                var oldExpression = oldArg.Expression;
                var oldToken = oldExpression.GetFirstToken();
                nodePath = oldToken.ValueText;

                var newToken = SyntaxFactory.ParseToken($"\"カスタム/{oldToken.ValueText}\"");
                var newExpression = oldExpression.ReplaceToken(oldToken, newToken);
                var newArg = oldArg.WithExpression(newExpression);
                var newArgumentList = oldArgumentList.ReplaceNode(oldArg, newArg);

                return original.WithArgumentList(newArgumentList);
            }
        }
    }
}
