using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace WindowsFormsApp5
{
   public class conexao
    {

        public static SqlConnection getConnection()
        {
            SqlConnection cnn = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = BD_CadastroCliente; Integrated Security = True; Connect Timeout = 30; Encrypt = False; " +
                                        "TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False   ");
            return cnn;
        }

    }
    
 }
