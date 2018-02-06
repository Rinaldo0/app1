using System.Windows.Forms;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class frmConsultaCliente : Form
    {
        private frm_CadastroCLi frm_CadastroCLi;

        public frmConsultaCliente(frm_CadastroCLi frm_CadastroCLi)
        {
            this.frm_CadastroCLi = frm_CadastroCLi;

         
            
        }

        public object dgvCliente { get; private set; }

        private void frmConsultaCliente_Load(object sender, EventArgs e)
        {

            //declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conCliente = conexao.getConnection();

            //cria a instrução sql, parametrizada para selecionar todos os clientes em ordem crescente por nome
            sqlQuery = "SELECT id_cliente, nome, cpf, data_nasc FROM cliente ORDER BY nome";

            //declara um DataAdapter
            SqlDataAdapter dta = new SqlDataAdapter(sqlQuery, conCliente);

            //Declara um DataTable
            DataTable dt = new DataTable();

            //Tratamento de exceções
            try
            {
                //chama o método Fill() do DataAdapter passando como parâmetro o DataTable dt
                dta.Fill(dt);

               

            

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao listar clientes " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }
        }
    }


}
