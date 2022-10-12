using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/1列に")]
    public class ReShapeToSingleColumn : ICalculationNode
    {
        public Table Run(Table data)
        {
            var columns = new List<Value>();
            
            for(int i = 0; i < data.ColumnCount; ++i)
            {
                var column = data.GetColumn(i);
                columns.AddRange(column);
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
