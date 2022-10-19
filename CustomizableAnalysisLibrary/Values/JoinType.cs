using System.Collections.Generic;

namespace CustomizableAnalysisLibrary
{
    public enum JoinType
    {
        Row,
        Column,
    }

    internal static class JoinTypeExtension
    {
        internal static Table JoinTables(this JoinType type, IEnumerable<Table> results)
        {
            return type switch
            {
                JoinType.Row => JoinRow(results),
                JoinType.Column => JoinColumn(results),
                _ => throw new System.NotImplementedException(),
            };
        } 

        private static Table JoinRow(IEnumerable<Table> results)
        {
            var rows = new List<IReadOnlyList<Value>>();
            foreach (var result in results)
            {
                for (int i = 0; i < result.RowCount; ++i)
                {
                    rows.Add(result.GetRow(i));
                }
            }
            var table = Table.CreateFromRows(rows);
            return table;
        }

        private static Table JoinColumn(IEnumerable<Table> results)
        {
            var columns = new List<IReadOnlyList<Value>>();
            foreach (var result in results)
            {
                for (int i = 0; i < result.ColumnCount; ++i)
                {
                    columns.Add(result.GetColumn(i));
                }
            }
            var table = Table.CreateFromColumns(columns);
            return table;
        }
    }
}
