using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/数値/最小値を含む行を取り出す")]
    public class ExtractRowMin : ICalculationNode, IOptionNode
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
            IReadOnlyList<Value> minRow = default;
            double minValue = double.MaxValue;

            for(int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                var value = row[Index].ToDoubleValue().DoubleValue;

                if (value < minValue)
                {
                    minValue = value;
                    minRow = row;
                }
            }
            
            return Table.CreateFromRows(minRow);
        }
    }
}
