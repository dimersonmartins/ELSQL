using MySql.Data.MySqlClient;
using System.Data;

namespace Vendor.Data
{
    public class VMySql
    {
        private MySqlConnection Conexao()
        {
            return new MySqlConnection("server=localhost;user=root;database=sgestao;port=3306;password=");
        }

        public DataSet QueryBuilder(string pQuery, MySqlCommand pMySqlCommand)
        {
            try
            {
                var conexao = Conexao();

                conexao.Open();

                pMySqlCommand.Connection = conexao;
                pMySqlCommand.CommandText = pQuery;
                pMySqlCommand.CommandType = CommandType.Text;

                MySqlDataAdapter mySqlData = new MySqlDataAdapter(pMySqlCommand);

                DataSet dataSet = new DataSet();

                mySqlData.Fill(dataSet);

                conexao.Close();

                return dataSet;
            }
            catch
            {
                return new DataSet();
            }
        }

        public DataSet QueryBuilder(string pQuery)
        {
            try
            {
                var conexao = Conexao();

                conexao.Open();

                MySqlCommand mySqlCommand = new MySqlCommand(pQuery, conexao);

                mySqlCommand.CommandType = CommandType.Text;

                MySqlDataAdapter mySqlData = new MySqlDataAdapter(mySqlCommand);

                DataSet dataSet = new DataSet();

                mySqlData.Fill(dataSet);

                conexao.Close();

                return dataSet;
            }
            catch
            {
                return new DataSet();
            }

        }
    }
}
