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

                sqlStatements.Add(valueStatements);
            }

            return sqlStatements;
        }
    }
}
