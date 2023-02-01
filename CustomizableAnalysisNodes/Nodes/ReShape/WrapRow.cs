using System;
using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変形/n列で折り返す")]
    public class WrapRow : ICalculationNode, IOptionNode
    {
        public int N { get; set; } = 1;

        private ReShapeToSingleRow toSingle = new ReShapeToSingleRow();

        public IEnumerable<(string label, Value)> GetOptions()
        {
            yield return ("n", new Value(N));
        }

        public void SetOptions(params Value[] options)
        {
            N = Math.Max(options[0].ToInt(), 1);
        }

        public Table Run(Table data)
        {
            var singleRow = toSingle.Run(data).GetRow(0);

            var rows = new List<Value[]>();
            var row = new List<Value>();
            foreach(var value in singleRow)
            {
                row.Add(value);

                if (row.Count == N)
                {
                    rows.Add(row.ToArray());
                    row.Clear();
                }
            }

            if (row.Count != 0)
            {
                rows.Add(row.ToArray());
            }

            return Table.CreateFromRows(rows);
        }
    }
}
