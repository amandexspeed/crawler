using Raspagem_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspagem_API.Send
{
    class email
    {
        static string tirarPrefixo(string texto)
        {
            // Define um padrão de regex para encontrar "R$:" no início do texto
            string padrao = @"^R\$";

            // Remove o padrão correspondente do texto
            string textoFormatado = Regex.Replace(texto, padrao, "");

            return textoFormatado;
        }
        //string nomeProdutoMercadoLivre, precoProdutoMercadoLivre, nomeProdutoMagazineLuiza, precoProdutoMagazineLuiza;

        //Enviar email com o resultado da comparação

        public email(Produto ProdutoMercadoLivre, Produto ProdutoMagazineLuiza)
        {
            // Configurações do servidor SMTP do Gmail
            string smtpServer = "smtp-mail.outlook.com"; // Servidor SMTP do Gmail
            int porta = 587; // Porta SMTP do Gmail para TLS/STARTTLS
            string remetente = "amandextec@hotmail.com"; // Seu endereço de e-mail do Gmail
            string senha = "testeRobo"; // Sua senha do Gmail

            // Configurar cliente SMTP
            using (SmtpClient client = new SmtpClient(smtpServer, porta))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; // Habilitar SSL/TLS

                // Construir mensagem de e-mail
                MailMessage mensagem = new MailMessage(remetente, "amandextec@gmail.com")
                {
                    Subject = "Resultado da comparação de preços",
                    Body = $"Produto do Mercado Livre: {ProdutoMercadoLivre.Nome} - Preço: {ProdutoMercadoLivre.preco}\n" +
                           $"Produto do Magazine Luiza: {ProdutoMagazineLuiza.Nome} - Preço: {ProdutoMagazineLuiza.preco}\n" +
                           $"Melhor compra: {(double.Parse(tirarPrefixo(ProdutoMercadoLivre.preco)) < double.Parse(tirarPrefixo(ProdutoMagazineLuiza.preco)) ? $"Mercado Livre. Link: {ProdutoMercadoLivre.link}" : $"Magazine Luiza. Link: {ProdutoMagazineLuiza.link}")}"

                };

                // Enviar e-mail
                client.Send(mensagem);

                Console.WriteLine("Mercado Livre");
                Console.WriteLine(ProdutoMercadoLivre.Nome);
                Console.WriteLine(ProdutoMercadoLivre.preco);
                Console.WriteLine("Magazine Luiza");
                Console.WriteLine(ProdutoMagazineLuiza.Nome);
                Console.WriteLine(ProdutoMagazineLuiza.preco);

            }

        }

    }
}
