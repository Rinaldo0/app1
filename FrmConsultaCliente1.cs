using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace WindowsFormsApp5
{
    public partial class frm_ConsultaCli : Form
    {

        frm_CadastroCLi frm_ConsulataCli;
        public frm_ConsultaCli(frm_CadastroCLi frm_ConsultaCli)
        {

            this.frm_ConsulataCli = frm_ConsulataCli;

           

            InitializeComponent();
        }

        public void FrmConsultaCliente1_Load(object sender, EventArgs e)
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

                //configura a fonte de dados no DataGridView
                dgvCliente.DataSource = dt;

                //altera a cor das linhas alternadas no grid
                dgvCliente.RowsDefaultCellStyle.BackColor = Color.White;
                dgvCliente.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;

                //cria as colunas no DataGridView e coloca o texto do nome das colunas
                dgvCliente.Columns[0].HeaderCell.Value = "Código do Cliente"; //primeira coluna
                dgvCliente.Columns[1].HeaderCell.Value = "Nome"; //segunda coluna
                dgvCliente.Columns[2].HeaderCell.Value = "CPF"; //terciera coluna
                dgvCliente.Columns[3].HeaderCell.Value = "Dt. Nasc."; //Quarta coluna
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
        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            //variável para guardar o código do cliente selecionado no DataGridView
            string codigoCliente;

            //seleciona o código da linha atual da coluna[0] do DataGridView
            codigoCliente = dgvCliente.CurrentRow.Cells[0].Value.ToString();

            //declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conClienteConsulta = conexao.getConnection();

            //declara e inicializa um DataReader como null
            SqlDataReader dtr = null;

            //cria a instrução sql, parametrizada
            sqlQuery = "SELECT id_cliente, nome, cpf, data_nasc FROM cliente WHERE id_cliente = @id_cliente";

            //tratamento de exceções
            try
            {
                //abrir conexão
                conClienteConsulta.Open();

                // cria um command para esta conexão
                SqlCommand cmd = new SqlCommand(sqlQuery, conClienteConsulta);

                //adiciona um parâmetro
                cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(codigoCliente)));

                //Atribui o comando ao DataReader
                dtr = cmd.ExecuteReader();

                //verifica se retornou registro na consulta SQL
                //se retornou, preenche a tela do frmCadastroCliente com os dados armazenados no DataReader dtr
                //chama o método Read( ) para ler os registros no dtr
                if (dtr.Read())
                {
                    //para o Form frmCadastroCliente
                    //atribui ao TextBox o valor do campo ID_CLIENTE do banco de dados retornado na consulta
                    frm_CadastroCli.txt_Codigo.Text = dtr["ID_CLIENTE"].ToString();

                    frm_CadastroCli.txt_Nome.Text = dtr["NOME"].ToString();
                    frm_CadastroCli.msk_Data.Text = dtr["DATA_NASC"].ToString();
                    frm_CadastroCli.msk_Cpf.Text = dtr["CPF"].ToString();
                }
            }
            //se houve exceção as instruções do bloco catch serão executadas
            catch (Exception ex)
            {
                //exibe a mensagem sobre a exceção ocorrida no bloco try
                MessageBox.Show(ex.ToString());
            }
            //as instruções de finally sempre serão executadas independente se houve exceção ou não
            finally
            {
                //fecha o DataReader se for diferente de null, ou sej,a se estiver conectado
                if (dtr != null)
                {
                    dtr.Close();
                }
                //fecha a conexão se for diferente de null
                if (conClienteConsulta != null)
                {
                    conClienteConsulta.Close();
                }
            }

            //fecha o form
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
