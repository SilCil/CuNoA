using System.Collections.Generic;

namespace CustomizableAnalysisLibrary
{
    public interface IOptionNode
    {
        IEnumerable<(string label, Value)> GetOptions();
        void SetOptions(params Value[] options);
    }
}
