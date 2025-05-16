using MySql.Data.MySqlClient;
using Projetoasp.net.Models;
using System.Configuration;
using System.Data;

namespace Projetoasp.net.Repositorio
{
    // Define a classe responsável por interagir com os dados de clientes no banco de dados
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        // Declara uma variável privada somente leitura para armazenar a string de conexão com o MySQL
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");
        public void Cadastrar(Produto produto)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para inserir dados na tabela 'produto'
                MySqlCommand cmd = new MySqlCommand("insert into produto (Nome,Descricao,Preco,Quantidade) values (@nome, @descricao, @preco,@quantidade)", conexao); // @: PARAMETRO
                                                                                                                                                                                         // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                cmd.Parameters.Add("@nomeprod", MySqlDbType.VarChar).Value = produto.Nome;
                // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                cmd.Parameters.Add("@descricaoprod", MySqlDbType.VarChar).Value = produto.Descricao;
                // Adiciona um parâmetro para o descricao, definindo seu tipo e valor
                cmd.Parameters.Add("@preco", MySqlDbType.Int32).Value = produto.Preco;
                // Adiciona um parâmetro para o preco, definindo seu tipo e valor
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                // Adiciona um parâmetro para o quantidade, definindo seu tipo e valor

                // Executa o comando SQL de inserção e retorna o número de linhas afetadas
                cmd.ExecuteNonQuery();
                // Fecha explicitamente a conexão com o banco de dados (embora o 'using' já faça isso)
                conexao.Close();
            }

        }
        public IEnumerable<Produto> TodosProdutos()
        {
            // Cria uma nova lista para armazenar os objetos Produto
            List<Produto> Produtolist = new List<Produto>();

            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os registros da tabela 'produto'
                MySqlCommand cmd = new MySqlCommand("SELECT * from produto", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                // interage sobre cada linha (DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Cria um novo objeto Cliente e preenche suas propriedades com os valores da linha atual
                    Produtolist.Add(
                                new Produto
                                {
                                    Id = Convert.ToInt32(dr["Id"]), // Converte o valor da coluna "CodProd" para inteiro
                                    Nome = ((string)dr["Nome"]), // Converte o valor da coluna "NomeProd" para string
                                    Quantidade = Convert.ToInt32(dr["Quantidade"]), // Converte o valor da coluna "QuantProd" para inteiro
                                    Preco = Convert.ToInt32(dr["Preco"]), // Converte o valor da coluna "PrecoProd" para inteiro
                                    Descricao = ((string)dr["Descricao"]), // Converte o valor da coluna "DescricaoProd" para string
                                });
                }
                // Retorna a lista de todos os produtos
                return Produtolist;
            }
        }

        // Método para buscar um produto específico pelo seu código (Codigo)
        public Produto ObterProduto(int Id)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar um registro da tabela 'produto' com base no código
                MySqlCommand cmd = new MySqlCommand("SELECT * from produto where Id=@Id ", conexao);

                // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@Id", Id);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Declara um leitor de dados do MySQL
                MySqlDataReader dr;
                // Cria um novo objeto Produto para armazenar os resultados
                Produto produto = new Produto();

                /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // Lê os resultados linha por linha
                while (dr.Read())
                {
                    // Preenche as propriedades do objeto Produto com os valores da linha atual
                    produto.Id = Convert.ToInt32(dr["Id"]);
                    produto.Nome = ((string)dr["Nome"]);
                    produto.Quantidade = Convert.ToInt32(dr["Quantindade"]);
                    produto.Preco= Convert.ToInt32(dr["Preco"]);
                    produto.Descricao = ((string)dr["Descricao"]);
                }
                // Retorna o objeto Cliente encontrado (ou um objeto com valores padrão se não encontrado)
                return produto;
            }
        }
        // Método para Editar (atualizar) os dados de um produto existente no banco de dados
        public bool Atualizar(Produto produto)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'produto' com base no código
                    MySqlCommand cmd = new MySqlCommand("Update produto set Nome=@nome, Preco=@preco, Descricao=@descricao, Quantidade=@quantidade" + " where Id=@id ", conexao);
                    // Adiciona um parâmetro para o código do produto a ser atualizado, definindo seu tipo e valor
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = produto.Id;
                    // Adiciona um parâmetro para o novo nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    // Adiciona um parâmetro para o novo preco, definindo seu tipo e valor
                    cmd.Parameters.Add("@preco", MySqlDbType.Int32).Value = produto.Preco;
                    // Adiciona um parâmetro para o novo descricao, definindo seu tipo e valor
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    // Adiciona um parâmetro para o novo descricao, definindo seu tipo e valor
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada

                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false; // Retorna false em caso de erro

            }
        }
        public void ExcluirProduto(int Id)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();

                // Cria um novo comando SQL para deletar um registro da tabela 'Produto' com base no código
                MySqlCommand cmd = new MySqlCommand("delete from produto where CodProd=@codigo", conexao);

                // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@Id", Id);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close(); // Fecha  a conexão com o banco de dados
            }
        }
    }
}