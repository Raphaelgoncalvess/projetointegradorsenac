﻿using System.Data.SqlClient;

namespace ProjetoIntegradorSenac
{
    public class Conexao
    {
        public SqlConnection conn = new SqlConnection("Persist Security Info=False;User ID=bernardo;Password=bernardo30!;Initial Catalog=db_bernardo;Data Source=dbaulabanco.ce9eq7mml3ie.sa-east-1.rds.amazonaws.com");
        public void Conectar()
        {
            conn.Open();
        }
        public void Desconectar()
        {
            conn.Close();
        }

        public void CadastroTitular(Usuario usuario, Endereco endereco)
        {
            CadastrarUsuario(usuario);
            BuscarIdUsuario(usuario);
            CadastrarEnderecoUsuario(endereco, usuario);
            InserirExame(usuario);
            InserirTitular(usuario);
            BuscarIdTitular(usuario);
            InserirMensalidade(usuario);
        }

        /*Método para validar o LOGIN*/
        public bool ValidarLogin(string Email, string Senha, bool EhTitular)
        {
            /*Faz um SELECT buscando um email e senha específico*/
            string sql = $"SELECT * FROM UsuarioPI WHERE email = '{Email}' and senha = '{Senha}'";
            SqlCommand comando = new SqlCommand(sql, conn);
            /*Executa o comando*/
            var retorno = comando.ExecuteReader();
            /*Se ele ler algum dado na busca, retorna TRUE*/
            if (retorno.Read())
                return true;
            /*Senão retorna FALSE*/
            return false;
        }
        /*Método para cadastrar dados do usuário*/
        public void CadastrarUsuario(Usuario usuario)
        {
            /*Faz um INSERT na tabela UsuarioPI*/
            string sql = $"INSERT INTO UsuarioPI (nome, senha, email, cpf, genero, dataNascimento, ehTitular) "
                       + $"VALUES ('{usuario.Nome}', '{usuario.Senha}', '{usuario.Email}', '{usuario.Cpf}', '{usuario.Genero}', '{usuario.DataNascimento}', '{usuario.EhTitular}')";
            SqlCommand comando = new SqlCommand(sql, conn);
            comando.ExecuteNonQuery();
        }
        /*Método para buscar o Id do Usuario*/
        public int BuscarIdUsuario(Usuario usuario)
        {
            /*Faz um SELECT para buscar o Id do Usuario*/
            string sql = $"SELECT id FROM UsuarioPI WHERE cpf = '{usuario.Cpf}'";
            SqlCommand comando = new SqlCommand(sql, conn);
            usuario.IdUsuario = Convert.ToInt32(comando.ExecuteScalar());
            return usuario.IdUsuario;

        }
        /*Método para cadastrar o endereço do usuário*/
        public void CadastrarEnderecoUsuario(Endereco endereco, Usuario usuario)
        {
            /*Faz um INSERT na tabela EnderecoPI*/
            string sql = $"INSERT INTO EnderecoPI ( cep, bairro, rua, numero, complemento, idUsuario) "
                       + $"VALUES ('{endereco.Cep}', '{endereco.Bairro}', '{endereco.Rua}', '{endereco.Numero}', '{endereco.Complemento}', '{usuario.IdUsuario}')";
            SqlCommand comando = new SqlCommand(sql, conn);
            comando.ExecuteNonQuery();
        }
       

        //CRIAR MÉTODO QUE INSERE O ID NA TABELA TITULAR
        public void InserirTitular(Usuario usuario)
        {
            string sql = $"INSERT INTO TitularPI (idUsuario) VALUES ('{usuario.IdUsuario}')";
            SqlCommand comando = new SqlCommand(sql, conn);
            comando.ExecuteNonQuery();
        }
        /*Método para buscar o Id do Titular*/
        public int BuscarIdTitular(Usuario usuario)
        {
            /*Faz um SELECT para buscar o Id do Usuario*/
            string sql = $"SELECT id FROM TitularPI WHERE idUsuario = '{usuario.IdUsuario}'";
            SqlCommand comando = new SqlCommand(sql, conn);
            usuario.IdTitular = Convert.ToInt32(comando.ExecuteScalar());
            return usuario.IdTitular;

        }

        //CRIAR MÉTODO QUE INSERE DADOS NA TABELA MENSALIDADE
        public void InserirMensalidade(Usuario usuario) 
        {
            string sql = $"INSERT MensalidadePI (descricao, valor, dataEmissao, dataVencimento, taPaga, idTitular)\r\n"
                       + $"VALUES ('Mensalidade Plano Sócio JP', '89.90', '2023-07-17', '2023-08-15', 0, '{usuario.IdTitular}'),\r\n" 
                       + $"('Mensalidade Plano Sócio JP', '89.90', '2023-07-17', '2023-09-15', 0, '{usuario.IdTitular}'),\r\n" 
                       + $"('Mensalidade Plano Sócio JP', '89.90', '2023-07-17', '2023-10-15', 0, '{usuario.IdTitular}'),\r\n" 
                       + $"('Mensalidade Plano Sócio JP', '89.90', '2023-07-17', '2023-11-15', 0, '{usuario.IdTitular}');";
            SqlCommand comando = new SqlCommand(sql, conn);
            comando.ExecuteNonQuery();
        }
        //CRIAR MÉTODO QUE INSERE DADOS NA TABELA EXAMES
        public void InserirExame(Usuario usuario)
        {
            string sql = $"INSERT INTO ExamePI (nome, descricao, estaApto, situacao, IdUsuario) " 
                       + $"VALUES ('Exame Dermatológico', 'Exame para entrar na piscina do clube',0,'Pendente','{usuario.IdUsuario}')";
            SqlCommand comando = new SqlCommand(sql, conn);
            comando.ExecuteNonQuery();

        }
        /*Método para buscar o nome do usuário*/
        public string BuscarNomeUsuarioLogado(Usuario usuario)
        {
            /*Faz um SELECT para buscar o nome*/
            string sql = $"SELECT nome FROM UsuarioPI WHERE email = '{usuario.Email}' and senha = '{usuario.Senha}'";
            SqlCommand comando = new SqlCommand(sql, conn);
            /*Armazena o dado*/
            usuario.Nome = comando.ExecuteScalar().ToString();
            /*Retorna esse dado*/
            return usuario.Nome;
        }
        /*Método para verificar se o usuário é Titular*/
        public bool BuscarUsuarioLogadoEhTitular(Usuario usuario)
        {
            /*Faz um SELECT para ver se é Titular ou Dependente*/
            string sql = $"SELECT ehTitular FROM UsuarioPI WHERE email = '{usuario.Email}' and senha = '{usuario.Senha}'";
            SqlCommand comando = new SqlCommand(sql, conn);
            /*Armazena o dado*/
            usuario.EhTitular = Convert.ToBoolean(comando.ExecuteScalar());
            /*Retorna esse dado*/
            return usuario.EhTitular;
        }
    }
}
