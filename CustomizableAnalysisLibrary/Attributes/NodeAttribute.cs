using System;

namespace CustomizableAnalysisLibrary
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeAttribute : Attribute
    {
        public readonly string path = default;

        public NodeAttribute(string path)
        {
            this.path = path;
        }
    }
}
