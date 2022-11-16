using System.Net.Mail;

namespace SQLConverter
{
    public static class TxTToSQL
    {
        private static readonly string[] SensitiveColumnTexts = new string[] {"name", "title", "address", "phone" };
        private static Random Random = new Random();
        public static List<string> Convert(string[]? data)
        {
            var tableName = data[0];
            var columns = data[1].Split("\t");
            var insertStatment = $"INSERT INTO {tableName} ({string.Join(",", columns)})";

            var sensitiveColumnIds = GetSensitiveColumns(columns);

            var sqlStatments = new List<string>();
            for (int i = 2; i < data.Length; i++)
            {
                var values = data[i].Split("\t");
                var valueStatments = $"VALUES({string.Join(",",GetValues(values, sensitiveColumnIds.ToList()))});";

                sqlStatments.Add(insertStatment);
                sqlStatments.Add(valueStatments);
                sqlStatments.Add(string.Empty);
            }

            return sqlStatments;

        }

        private static IEnumerable<ColumnInfo> GetSensitiveColumns(string[]? columns)
        {
            for (var i = 0; i < columns.Length; i++)
            {
                if (SensitiveColumnTexts.Any(x => columns[i].IndexOf(x, 0, StringComparison.OrdinalIgnoreCase) != -1))
                {
                    yield return new ColumnInfo { Index = i, ColumnName = columns[i] };
                }
            }
        }

        private static IEnumerable<string> GetValues(string[]? values, List<ColumnInfo> sensitiveColumnInfo)
        {
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i].Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    yield return "NULL";
                }
                
                
                if (sensitiveColumnInfo.Any(x => x.Index == i))
                {
                    yield return $"'{sensitiveColumnInfo.FirstOrDefault(x => x.Index == i).ColumnName}-{RandomString(values[i].Length)}'";
                }

                if(MailAddress.TryCreate(values[i], out var email) || values.Contains("@"))
                {
                    yield return $"'{sensitiveColumnInfo.FirstOrDefault(x => x.Index == i).ColumnName}-{RandomString(values[i].Length)}@nomail.com'";
                }

                yield return $"'{values[i].Replace("'", "''")}'";
            }
        }
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
