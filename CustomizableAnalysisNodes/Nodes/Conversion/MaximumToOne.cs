using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変換/数値/最大値を1にスケール")]
    public class MaximumToOne : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToIntValue().IntValue;
        }

        public Table Run(Table data)
        {
            var columns = new List<IEnumerable<Value>>();
            for(int i = 0; i < data.ColumnCount; ++i)
            {
                if(i == Index)
                {
                    var values = data.GetColumn(i).Select(x => x.ToDoubleValue().DoubleValue).ToArray();
                    var min = values.Max();
                    columns.Add(values.Select(x => new Value(x / min)));
                }
                else
                {
                    columns.Add(data.GetColumn(i));
                }
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
