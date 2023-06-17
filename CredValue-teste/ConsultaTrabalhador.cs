using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoCaptcha;

namespace CredValue_teste
{
    public class ConsultaTrabalhador
    {
        public static void Acesso()
        {
            IWebDriver driver = new ChromeDriver();
            string site = "http://www.rais.gov.br/sitio/consulta_trabalhador_identificacao.jsf";

            driver.Navigate().GoToUrl(site);

            PrimeiraPagina(driver, "46102682807", site);
        }

        public static void PrimeiraPagina(IWebDriver driver, string cpf, string site)
        {
            // INSERINDO DADOS NO CAMPO DE CPF
            IWebElement elementCpf = driver.FindElement(By.XPath("/html/body/div[2]/form/div/fieldset/div[2]/div[1]/input"));
            elementCpf.SendKeys(cpf);

            // acessando o checkbox de confirmação de captcha
            Recaptcha(site, driver);

            // CLICANDO NO BOTÃO DE AVANÇAR 
            IWebElement elementAvancar = driver.FindElement(By.XPath("/html/body/div[2]/form/div/div/div/input[1]"));
            elementAvancar.Click();

            BuscarDadosTela(driver, cpf);
        }

        public static void BuscarDadosTela(IWebDriver driver, string cpf)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(2000));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/div[2]/form/div[3]/div/table")));

            var elementTable = driver.FindElement(By.XPath("/html/body/div[2]/form/div[3]/div/table/tbody")).FindElements(By.TagName("tr"));

            StreamWriter sw = new StreamWriter($"E:\\descartavel\\Teste{cpf}.txt");

            if (elementTable.Count > 0)
            {
                foreach (var item in elementTable)
                {
                    var d = item.FindElements(By.TagName("td"));

                    try
                    {
                        sw.WriteLine(d[0].Text);
                        sw.WriteLine(d[1].Text);
                        sw.WriteLine(d[2].Text);
                        sw.WriteLine(d[3].Text);
                        sw.WriteLine(d[4].Text);
                        sw.WriteLine(d[5].Text);
                        sw.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
            else
            {
                sw.WriteLine($"Não há dados referente ao CPF {cpf}");
                sw.Close();
            }
        }

        public static async void Recaptcha(string site, IWebDriver driver)
        {
            // UTILIZANDO A API DO 2CAPTCHA (API PAGA) AO TENTAR VALIDAR O HCAPTCHA DEVE SER INSERIDO A CHAVE DE ACESSO NO LUGAR DO "YOUR_API_KEY" EM "solver.ReCaptchaV2Async" PARA VALIDAR O USUARIO E TER ACESSO A API
            try
            {
                IWebElement elementCb = driver.FindElement(By.XPath("/html/body/div[2]/form/div/fieldset/div[4]/div/div"));
                var captchaKey = elementCb.GetAttribute("data-setkey");
                var solver = new Solve();
                var result = await solver.ReCaptchaV2Async("YOUR_API_KEY", captchaKey, site);

                // INSERIR DADOS DO CAPTCHA NO CAMPO DE TEXTO INVISIVEL DA PAGINA USANDO UM SCRIPT EM JS
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript($"document.querySelector('#h-captcha-response-0e9nazp4ng0q').innerHTML = '{result.Request}'");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
