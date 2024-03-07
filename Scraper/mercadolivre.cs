using HtmlAgilityPack;
using Raspagem_API.data;
using Raspagem_API.Models;
using System;
using System.Runtime.ConstrainedExecution;
using System.Xml;
using static Raspagem_API.data.LogContext;

public class MercadoLivreScraper
{
    public Produto ObterPreco(string descricaoProduto, int idProduto)
    {
        // URL da pesquisa no Mercado Livre com base na descrição do produto
        string url = $"https://lista.mercadolivre.com.br/{descricaoProduto}";

        try
        {
            // Inicializa o HtmlWeb do HtmlAgilityPack
            HtmlWeb web = new HtmlWeb();

            // Carrega a página de pesquisa do Mercado Livre
            HtmlDocument document = web.Load(url);

            // Encontra o elemento que contém o preço do primeiro produto            
            HtmlNode firstProductPriceNode = document.DocumentNode.SelectSingleNode("//span[@class='andes-money-amount__fraction']");

            //HtmlNode firstProductNameNode = document.DocumentNode.SelectSingleNode("//h2[@class='ui-search-item__title']");
            HtmlNode firstProductLinkNode = document.DocumentNode.SelectSingleNode("//a[@class='ui-search-item__group__element ui-search-link__title-card ui-search-link']");

            // Verifica se o elemento foi encontrado
            if (firstProductPriceNode != null && firstProductLinkNode != null)
            {
                Produto produto = new Produto();

                // Obtém o preço do primeiro produto
                //string firstProductPrice = firstProductPriceNode;
                produto.Id = idProduto;
                produto.preco = firstProductPriceNode.InnerText.Trim();
                produto.Nome = firstProductLinkNode.InnerText;
                produto.link = firstProductLinkNode.Attributes["href"].Value;

                // Registra o log com o ID do produto
                RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "WebScraping - Mercado Livre", "Sucesso", idProduto);

                // Retorna o preço
                return produto;
            }
            else
            {
                Console.WriteLine("Preço não encontrado.");

                // Registra o log com o ID do produto
                RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "WebScraping - Mercado Livre", "Produto não encontrado", idProduto);

                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            // Registra o log com o ID do produto
            RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "Web Scraping - Mercado Livre", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    private void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
    {
        using (var context = new LogContext())
        {
            var log = new Log
            {
                CodigoRobo = codRob,
                UsuarioRobo = usuRob,
                DateLog = dateLog,
                Etapa = processo,
                InformacaoLog = infLog,
                IdProdutoAPI = idProd
            };
            context.Logrobo.Add(log);
            context.SaveChanges();
        }
    }
}

