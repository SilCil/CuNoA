using System;
using System.Collections.Generic;

namespace CustomizableAnalysisLibrary.Nodes
{
    [Node("変形/n行で折り返す")]
    public class WrapColumn : ICalculationNode, IOptionNode
    {
        public int N { get; set; } = 1;

        private ReShapeToSingleColumn toSingle = new ReShapeToSingleColumn();

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
            var singleRow = toSingle.Run(data).GetColumn(0);

            var columns = new List<Value[]>();
            var column = new List<Value>();
            foreach(var value in singleRow)
            {
                column.Add(value);

                if (column.Count == N)
                {
                    columns.Add(column.ToArray());
                    column.Clear();
                }
            }

            if (column.Count != 0)
            {
                columns.Add(column.ToArray());
            }

            return Table.CreateFromColumns(columns);
        }
    }
}
