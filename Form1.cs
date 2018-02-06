using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp5
{
    public partial class frm_CadastroCLi : Form
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection cnn = new SqlConnection(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = BD_CadastroCliente; Integrated Security = True; Connect Timeout = 30; Encrypt = False; " +
                                        "TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False   ");
            return cnn;
        }

        internal static SqlConnection getConnection()
        {
            throw new NotImplementedException();
        }
        public frm_CadastroCLi()
        {
            InitializeComponent();

        }

        private void habilitar()
        {
            //txtCodigo sempre será desabilitado
            txt_Codigo.Enabled = false;
            //altera a propriedade Enabled dos controles para true, habilitando o controle
            txt_Nome.Enabled = true;
            msk_Cpf.Enabled = true;
            msk_Data.Enabled = true;
        }

        private void desabilitar()
        {
            //txtCodigo sempre será desabilitado
            txt_Codigo.Enabled = false;

            //altera a propriedade Enabled dos controles para ficarem desabilitados
            txt_Nome.Enabled = false;
            msk_Cpf.Enabled = false;
            msk_Data.Enabled = false;
        }

        private void limparControles()
        {
            //desabilita o TextBox
            txt_Codigo.Enabled = false;

            //limpa os textos dos TextBox e MaskedTextBox
            txt_Nome.Clear();
            txt_Codigo.Clear();
            msk_Cpf.Clear();
            msk_Data.Clear();
            //coloca o foco no mskCPF
            msk_Cpf.Focus();
        }


        private bool validaDados()
        {
            //verificar se mskCPF está preenchido, se não estiver preenchido
            if (string.IsNullOrEmpty(msk_Cpf.Text))
            {
                //mensagem ao usuário
                MessageBox.Show("Preenchimento de campo obrigatório", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //limpa o mskCPF
                msk_Cpf.Clear();

                //coloca o cursor no mskCPF
                msk_Cpf.Focus();

                //retorna falso
                return false;
            }

            //verifica se o que foi digitado em data de nascimento é uma data válida 
            DateTime auxData; //variável auxiliar
                              //se não for uma data válida ou se não digitar nenhuma data
            if (!(DateTime.TryParse(msk_Data.Text, out auxData)))
            {
                //mensagem ao usuário
                MessageBox.Show("Preenchimento de campo obrigatório", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //limpa o mskDtNasc
                msk_Data.Clear();

                //coloca o cursor no mskDtNasc
                msk_Data.Focus();

                //retorna falso
                return false;
            }

            //verifica se o txtNome está preenchido, Se for nulo ou vazio retorna falso
            if (string.IsNullOrEmpty(txt_Nome.Text))
            {
                //mensagem ao usuário
                MessageBox.Show("Preenchimento de campo obrigatório", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //limpa o txtNome
                txt_Nome.Clear();

                //coloca o cursor no txtNome
                txt_Nome.Focus();

                //retorna falso
                return false;
            }

            //se todas as validações passaram no teste, retorna verdadeiro
            return true;
        }

        public void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            habilitar();


        }

        private void btn_Incluir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Codigo.Text))
            {
                //se txtCodigo não estiver vazio, significa que já foi consultado um cliente.
                // a instrução a seguir captura se foi clicado o botão Yes (SIM) como resposta da pergunta.
                if (MessageBox.Show("Você está editando um registro existente. Deseja incluir um registro novo?", "ACR Rental Car", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    limparControles();
                return;   //encerra a sub-rotina
            }

            // antes de incluir é preciso validar os dados de preenchimento obrigatório
            //chama o método para validar a entrada de dados
            //se retornou falso, interrompe o processamento para incluir no banco de dados

            if (validaDados() == false)
            {
                return;  //interrompe a sub-rotina
            }

            //declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conCliente = conexao.getConnection();

            //cria a instrução sql, parametrizada
            sqlQuery = "INSERT INTO cliente(nome,data_nasc,cpf) VALUES(@nome,@data_nasc,@cpf)";

            //Tratamento de exceções
            try
            {
                //abre a conexão com o banco de dados
                conCliente.Open();

                //cria um objeto do tipo SqlCommand com a instrução SQL e a conexão
                SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);

                //define, adiciona os parametros
                cmd.Parameters.Add(new SqlParameter("@nome", txt_Nome.Text));
                cmd.Parameters.Add(new SqlParameter("@data_nasc", Convert.ToDateTime(msk_Data.Text)));
                cmd.Parameters.Add(new SqlParameter("@cpf", msk_Cpf.Text));

                //executa o commando
                //ExecuteNonQuery envia instruções para o banco de dados que estão em cmd
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente incluído com sucesso", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Limpa os campos para nova entrada de dados
                limparControles();
            }
            catch (Exception ex)  // se houve alguma exceção dentro do bloco try
            {
                MessageBox.Show("Problema ao incluir cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally  // independente se houve exceção ou não o bloco try é sempre executado
            {
                //se conexão não for nula, fecha conexão
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }

        }

       

        private void btn_Alterar_Click(object sender, EventArgs e)
        {
            //os campos para serem alterados são preenchidos por meio da consulta no grid do form Consulta de Cliente
            //verifica se tem o código do cliente no txtCodigo. Se o campo estiver vazio, interrompe a sub-rotina
            if (string.IsNullOrEmpty(txt_Codigo.Text))
            {
                MessageBox.Show("Consulte o cliente que deseja alterar clicando no botão consultar", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;   //interrompe a sub-rotina
            }

            // antes de alterar o registro é preciso validar os dados de preenchimento obrigatório
            //chama o método para validar a entrada de dados
            //se retornou falso, interrompe o processamento
            if (validaDados() == false)
            {
                return;
            }

            //declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conCliente = conexao.getConnection();

            //cria a instrução sql, parametrizada
            sqlQuery = "UPDATE cliente set nome=@nome,data_nasc=@data_nasc,cpf=@cpf WHERE id_cliente=@id_cliente";

            //Tratamento de exceções 
            //códigos semelhantes ao botão inserir com diferença na instrução SQL
            try
            {
                conCliente.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);
                //define, adiciona os parametros
                cmd.Parameters.Add(new SqlParameter("@nome", txt_Nome.Text));
                cmd.Parameters.Add(new SqlParameter("@data_nasc", Convert.ToDateTime(msk_Data.Text)));
                cmd.Parameters.Add(new SqlParameter("@cpf", msk_Cpf.Text));
                cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(txt_Codigo.Text)));

                //executa o comando
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente alterado com sucesso", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Limpa os campos para nova entrada de dados
                limparControles();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao alterar cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (conCliente != null)
                {
                    conCliente.Close();
                }
            }
        }

        private void btn_Excluir_Click(object sender, EventArgs e)
        {
            //veririfica se tem o código do cliente no txtCodigo
            if (string.IsNullOrEmpty(txt_Codigo.Text))
            {
                MessageBox.Show("Consulte o cliente que deseja excluir clicando no botão consultar", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //solicita confirmação de exclusão de registro
            if (MessageBox.Show("Deseja excluir permanentemente o registro?", "ACR Rental Car", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //declaração da variável para guardar as instruções SQL
                string sqlQuery;

                //cria conexão chamando o método getConnection da classe Conexao
                SqlConnection conCliente = conexao.getConnection();

                //cria a instrução sql, parametrizada
                sqlQuery = "DELETE FROM cliente WHERE id_cliente=@id_cliente";

                //Tratamento de exceções
                try
                {
                    conCliente.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conCliente);

                    //define, adiciona os parametros
                    cmd.Parameters.Add(new SqlParameter("@id_cliente", Convert.ToInt32(txt_Codigo.Text)));

                    //executa o commando
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cliente excluído com sucesso", "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Limpa os campos para nova entrada de dados
                    limparControles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problema ao incluir cliente " + ex, "ACR Rental Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btn_Fechar_Click(object sender, EventArgs e)
        {
            
            {
                this.Close();
            }
        }

        public void btn_Consultar_Click(object sender, EventArgs e)
        {


            // Cria um novo formulário de frmConsultaCliente, passando como parâmetro uma referência deste form
            Form frm = new frm_ConsultaCli(this);

            // Define quem é o pai dessa janela
            frm.MdiParent = this.MdiParent;

            // Exibe o formulário
            frm.Show();
        }

        public void txt_Codigo_TextChanged(object sender, EventArgs e)
        {

        }

        public void msk_Data_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        public void txt_Nome_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}   
  

