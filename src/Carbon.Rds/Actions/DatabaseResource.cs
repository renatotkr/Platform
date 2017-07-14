using System;

namespace Carbon.Rds.Services
{
    public struct DatabaseResource
    {
        public DatabaseResource(string schemaName, string tableName = "*")
        {
            SchemaName = schemaName;
            TableName  = tableName;
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public DatabaseResource Parse(string text)
        {
            #region Preconditions

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            #endregion

            var segments = text.Split('.');

            return new DatabaseResource(segments[0], segments[1]);
        }
        
        // *.*

        public override string ToString()
        {
            return SchemaName + "." + TableName;
        }
    }

    // *.*

    // TODO: FunctionName...

}