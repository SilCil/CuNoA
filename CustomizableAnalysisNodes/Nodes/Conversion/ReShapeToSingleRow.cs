using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/1行に")]
    public class ReShapeToSingleRow : ICalculationNode
    {
        public Table Run(Table data)
        {
            var rows = new List<Value>();
            
            for(int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                rows.AddRange(row);
            }

            return Table.CreateFromRows(rows);
        }
    }
}
