namespace SQLConverter
{
    public static class TxTToSQL
    {
        public static List<string> Convert(string[]? data)
        {
            var tableName = data[0];
            var insertStatement = $"INSERT INTO {tableName} ({data[1].Replace("\t", ",")})";

            var sqlStatements = new List<string> {
            insertStatement,
            "VALUES"
            };
            for (int i = 2; i < data.Length; i++)
            {
                var values = $"{data[i].Replace("\t", "','")}";
                var valueStatements = $"('{values}'),".Replace("'NULL'", "NULL");
                if (i == data.Length - 1)
                {
                    valueStatements = valueStatements.Remove(valueStatements.Length - 1);
                }
                sqlStatements.Add(valueStatements);
            }

            return sqlStatements;
        }

        public static string Cleanup(string[]? data)
        {
            var tableName = data[0];
            var statement = $"DELETE FROM {tableName} WHERE Id IN";

            var ids = new List<string>();
            for (int i = 2; i < data.Length; i++)
            {
                var splits = data[i].Split("\t");
                if (!string.IsNullOrWhiteSpace(splits[0]) && Guid.TryParse(splits[0], out var x))
                {
                    ids.Add(splits[0]);
                }
            }

            return ids.Any() ? $"{statement} ('{string.Join("','", ids)}')" : "";
        }
    }
}
