using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/数値/最大値を含む行を取り出す")]
    public class ExtractRowMax : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
        }

        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt();
        }

        public Table Run(Table data)
        {
            IReadOnlyList<Value> maxRow = default;
            double maxValue = double.MinValue;

            for(int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                var value = row[Index].ToDouble();

                if (value > maxValue)
                {
                    maxValue = value;
                    maxRow = row;
                }
            }
            
            return Table.CreateFromRows(maxRow);
        }
    }
}
