using System.Collections.Generic;
using System.Linq;

namespace CustomizableAnalysisLibrary
{
    public class Table
    {
        private readonly Value[,] values = default;

        public int RowCount => values.GetLength(0);
        public int ColumnCount => values.GetLength(1);

        public IReadOnlyList<Value> GetRow(int index) => Enumerable.Range(0, ColumnCount).Select(x => values[index, x]).ToArray();
        public IReadOnlyList<Value> GetColumn(int index) => Enumerable.Range(0, RowCount).Select(x => values[x, index]).ToArray();

        private Table(Value[,] values) => this.values = values;

        public static Table CreateFromRows(params IEnumerable<Value>[] rows) => CreateFromRows((IEnumerable<IEnumerable<Value>>)rows);

        public static Table CreateFromRows(IEnumerable<IEnumerable<Value>> rows)
        {
            var array = rows.Select(x => x.ToArray()).ToArray();
            var values = new Value[array.Length, array.Max(x => x.Length)];

            for (int i = 0; i < array.Length; ++i)
            {
                for (int j = 0; j < array[i].Length; ++j)
                {
                    values[i, j] = array[i][j];
                }
            }

            return new Table(values);
        }

        public static Table CreateFromColumns(params IEnumerable<Value>[] columns) => CreateFromColumns((IEnumerable<IEnumerable<Value>>)columns);

        public static Table CreateFromColumns(IEnumerable<IEnumerable<Value>> columns)
        {
            var array = columns.Select(x => x.ToArray()).ToArray();
            var values = new Value[array.Max(x => x.Length), array.Length];

            for(int i = 0; i < array.Length; ++i)
            {
                for(int j = 0; j < array[i].Length; ++j)
                {
                    values[j, i] = array[i][j];
                }
            }

            return new Table(values);
        }

        public static Table CreateFromSingleElement(Value value) => CreateFromRows(new Value[] { value });
    }
}
