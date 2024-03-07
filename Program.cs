using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Raspagem_API.data;
using Raspagem_API.Models;
using Raspagem_API.Send;

namespace Raspagem_API
{
    class Program
    {


        static List<Produto> produtosVerificados = new List<Produto>();

        static void Main(string[] args)
        {

            int intervalo = 360000;
            Timer timer = new Timer(VerificarNovoProduto, null, 0, intervalo);

            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();

        }


        static async void VerificarNovoProduto(object state)
        {

            string username = "11164448";
            string senha = "60-dayfreetrial";
            string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

            try
            {
                using (HttpClient client = new HttpClient())
                {

                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {

                        string responseData = await response.Content.ReadAsStringAsync();

                        List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                        foreach (Produto produto in novosProdutos)
                        {

                            if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                            {
                                // Se é um novo produto, faça algo com ele
                                Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                                // Adicionar o produto à lista de produtos verificados
                                produtosVerificados.Add(produto);

                                // Registra um log no banco de dados apenas se o produto for novo
                                if (!ProdutoJaRegistrado(produto.Id))
                                {
                                    RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                                    MercadoLivreScraper mercadoLivreScraper = new MercadoLivreScraper();
                                    Produto mercadoPro = /*double.Parse(tirarPrefixo*/mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);

                                    MagazineLuizaScraper magazineLuizaScraper = new MagazineLuizaScraper();
                                    Produto magaluPro = /*double.Parse(tirarPrefixo(*/magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id)/*))*/;

                                    email Email = new email(mercadoPro, magaluPro);

                                    //Console.WriteLine("Mercadu livri: "+mercadoPre);
                                    //Console.WriteLine("Magalu: "+magaluPre);

                                    //Console.WriteLine($"Melhor compra: {((mercadoPre <  magaluPre) ?  "Mercado Livre":"Magazine Luiza" ) }");


                                }
                            }
                        }
                    }
                    else
                    {

                        Console.WriteLine($"Erro: {response.StatusCode}");

                    }


                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao fazer requisição: {ex.Message}");

            }
        }

        static List<Produto> ObterNovosProdutos(string responseData)
        {

            List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
            return produtos;

        }

        // Método para verificar se o produto já foi registrado no banco de dados
        static bool ProdutoJaRegistrado(int idProduto)
        {
            using (var context = new LogContext())
            {
                return context.Logrobo.Any(log => log.IdProdutoAPI == idProduto && log.CodigoRobo == "Xbox");
            }
        }


        static void RegistrarLog(string codRob, String usuRob, DateTime dateLog, string Processo, string InfLog, int IdProd)
        {

            using (var context = new LogContext())
            {

                var log = new Log
                {

                    CodigoRobo = codRob,
                    UsuarioRobo = usuRob,
                    DateLog = dateLog,
                    Etapa = Processo,
                    InformacaoLog = InfLog,
                    IdProdutoAPI = IdProd

                };

                context.Logrobo.Add(log);
                context.SaveChanges();

            }

        }
    }
}
