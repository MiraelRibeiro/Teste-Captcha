using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;

namespace CredValue_teste
{
    public class LocalVotacao
    {
        public static void Acesso()
        {
            IWebDriver driver = new ChromeDriver();
            string site = "https://www.tse.jus.br/servicos-eleitorais/titulo-e-local-de-votacao/titulo-e-local-de-votacao";

            driver.Navigate().GoToUrl(site);

            InserirDados(driver);
        }

        public static void InserirDados(IWebDriver driver)
        {
            string nome = "Mirael do Carmo Ribeiro";
            string nascomento = "16/09/1997";
            string mae = "Maria Aparecida da Silva Ribeiro";

            StreamWriter sw = new StreamWriter($"E:\\descartavel\\Teste{nome}.txt");

            try
            {
                Thread.Sleep(5000);

                IWebElement nomeTituloCPF = driver.FindElement(By.XPath("/html/body/main/div/div/article/div/article/main/section/form/fieldset/div[1]/input"));
                nomeTituloCPF.SendKeys(nome);

                IWebElement dataNascimento = driver.FindElement(By.XPath("/html/body/main/div/div/article/div/article/main/section/form/fieldset/div[2]/div/input"));
                dataNascimento.SendKeys(nascomento);

                IWebElement nomeMae = driver.FindElement(By.XPath("/html/body/main/div/div/article/div/article/main/section/form/fieldset/div[3]/div/input"));
                nomeMae.SendKeys(mae);

                var time = TimeSpan.FromSeconds(10);

                Console.WriteLine(time);

                Thread.Sleep(5000);
                // clique no botão 
                new WebDriverWait(driver, time).Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/main/div/div/article/div/article/main/section/form/fieldset/button"))).Click();

                Thread.Sleep(5000);

                // acessa os dados de retorno e salva em txt
                ObterDados(driver, nome);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                sw.WriteLine($"Não foi possivel acessar os dados referente ao nome {nome}");
                sw.Close();
                throw;
            }
        }


        public static void ObterDados(IWebDriver driver, string nome)
        {
            StreamWriter sw = new StreamWriter($"E:\\descartavel\\Teste{nome}.txt");
            var retorno = driver.FindElement(By.XPath("/html/body/main/div/div/article/div/article/main/section/div[2]")).FindElements(By.TagName("p"));

            if(retorno.Count > 0)
            {
                foreach (var tagP in retorno)
                {
                    sw.WriteLine(tagP.Text);
                }
            }
            else
            {
                sw.WriteLine("Não há dados referentes ao usuário");
            }
            sw.Close();
        }
        
    }
}
