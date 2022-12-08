using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIRAtoDF
{
    class SQLConnector
    {
        private const string CONNECTION_STRING = @"Data Source = database.windows.net; Initial Catalog = ProdDB; User Id = admin; Password = password";
        private const string TABLE_NAME = "common.Status";
        private SqlConnection getConnection()
        {
            return new SqlConnection(CONNECTION_STRING);
        }

        public bool BulkInsertDT(DataTable dt, bool cleanUp)
        {
            var connection = getConnection();
            connection.Open();

            if (cleanUp)
            {
                var command = new SqlCommand($"DELETE FROM {TABLE_NAME}", connection);
                command.ExecuteNonQuery();
            }

            SqlBulkCopy objbulk = new SqlBulkCopy(connection);

            objbulk.DestinationTableName = TABLE_NAME;

            foreach(var column in dt.Columns)
            {
                objbulk.ColumnMappings.Add(column.ToString(), column.ToString());
            }

            try
            {                
                objbulk.WriteToServer(dt);
                connection.Close();
                return true;
            }
            catch (Exception e)
            {                
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}
