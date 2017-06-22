using Carbon.Data;

namespace Carbon.Kms
{
    internal static class SqlHelper
    {
        public static string GetCurrentValueAndIncrement<T>(string columnName)
        {
            var dataset = DatasetInfo.Get<T>();

            var column = dataset[columnName];

            return
                $@"SELECT `{column.Name}` FROM `{dataset.Name}` WHERE id = @id FOR UPDATE;
                   UPDATE `{dataset.Name}`
                   SET `{column.Name}` = `{columnName}` + 1
                   WHERE id = @id";
        }
    }
}