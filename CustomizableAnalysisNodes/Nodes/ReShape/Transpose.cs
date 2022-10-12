using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変形/行と列を入替")]
    public class Transpose : ICalculationNode
    {
        public Table Run(Table data)
        {
            var rows = new List<IReadOnlyList<Value>>();
            for(int i = 0; i < data.ColumnCount; ++i)
            {
                rows.Add(data.GetColumn(i));
            }
            return Table.CreateFromRows(rows);
        }
    }
}
