using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Raspagem_API.data;
using Raspagem_API.Models;
using System;
using System.Diagnostics;
using static Raspagem_API.data.LogContext;

public class MagazineLuizaScraper
{
    public Produto ObterPreco(string descricaoProduto, int idProduto)
    {
        try
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--disable-logging");

            // Inicializa o ChromeDriver
            using (IWebDriver driver = new ChromeDriver(chromeOptions))
            {
                // Abre a página
                driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                System.Threading.Thread.Sleep(5000);

                // Encontra o elemento que possui o atributo data-testid
                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));
                IWebElement nameElement = driver.FindElement(By.CssSelector("[data-testid='product-title']"));
                IWebElement linkElement = driver.FindElement(By.CssSelector("a[data-testid='product-card-container']"));

                // Verifica se o elemento foi encontrado
                if (priceElement != null && nameElement!=null)
                {

                    Produto magaluProd = new Produto();
                    // Obtém o preço do primeiro produto
                    //string firstProductPrice = priceElement.Text;
                    magaluProd.Id = idProduto;
                    magaluProd.preco = priceElement.Text;
                    magaluProd.Nome = nameElement.Text;
                    magaluProd.link = linkElement.GetAttribute("href");


                    // Registra o log com o ID do produto
                    RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    

                    return magaluProd;
                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    // Registra o log com o ID do produto
                    RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            // Registra o log com o ID do produto
            RegistrarLog("Xbox", "amandexspeed", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    private static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
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