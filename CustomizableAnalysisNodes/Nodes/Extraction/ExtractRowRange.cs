using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("抽出/行/数値/範囲を指定する")]
    public class ExtractRowRange : ICalculationNode, IOptionNode
    {
        public int Index { get; set; } = 0;
        public double MinValue { get; set; } = 0.0;
        public double MaxValue { get; set; } = 100.0;

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("列番号", new Value(Index));
            yield return ("最小値", new Value(MinValue));
            yield return ("最大値", new Value(MaxValue));
        }
        
        public void SetOptions(params Value[] options)
        {
            Index = options[0].ToInt(); 
            MinValue = options[1].ToDouble();
            MaxValue = options[2].ToDouble();
        }

        public Table Run(Table data)
        {
            var rows = new List<IReadOnlyList<Value>>();

            for(int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                var value = row[Index].ToDouble();

                if (MinValue <= value && value <= MaxValue)
                {
                    rows.Add(row);
                }
            }

            return Table.CreateFromRows(rows);
        }
    }
}
