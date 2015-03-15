using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;


namespace ConsoleClicker
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = 0; // считаем клики


            var login = "";
            var pass = "";
            using (var sr = new StreamReader("logpass.txt")) //кладем файл в аутпут папку с содержаением user;password
            {
                var text = sr.ReadToEnd().Split(';');
                login = text[0];
                pass = text[1];
            }

            var webDriver = new FirefoxDriver();
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            // Чтобы создать своего юзера, нужно
            // создать класс, реализующий интерфейс IUser,
            // в конструкторе задать username и password,
            // положить его в папку ConcreteUsers 

            // Заходим на сайт
            webDriver.Navigate().GoToUrl("http://stud.lms.tpu.ru/");

            // Логинимся
            IWebElement loginField = webDriver.FindElement(By.Id("login_username"));
            IWebElement passwordField = webDriver.FindElement(By.Id("login_password"));
            loginField.SendKeys(login);
            passwordField.SendKeys(pass);
            passwordField.Submit();
            Console.WriteLine(webDriver.FindElement(By.ClassName("logininfo")).Text);

            // Массив книжек
            var books = new List<string>();
            using (var sr = new StreamReader("books.txt"))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    books.Add(line);
                }
            }


            foreach (var book in books)
            {
                // Открываем Книгу (Лекция, Теория, Зеленая книжка такая)
                webDriver.Navigate().GoToUrl(book);
                counter++;
                wait.Until(ExpectedConditions.ElementExists(By.ClassName("navtop")));

                // Пролистываем ее до конца
                var navbar = webDriver.FindElement(By.XPath("//div[@class='navtop']"));
                IWebElement link;
                do
                {
                    wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Следующая']")));
                    link = navbar.FindElement(By.XPath(".//a[@title='Следующая']"));
                    link.Click();
                    counter++;
                    try
                    {
                        wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.book_content h3")));
                        Console.WriteLine(webDriver.FindElement(By.CssSelector("div.book_content h3")).Text);
                    }
                    catch (NoSuchElementException)
                    {
                    }
                    wait.Until(ExpectedConditions.ElementExists(By.ClassName("navtop")));
                    navbar = webDriver.FindElement(By.XPath("//div[@class='navtop']"));
                } while (navbar.FindElements(By.XPath(".//a[@title='Следующая']")).Count > 0);

                wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Покинуть книгу']")));
                link = navbar.FindElement(By.XPath(".//a[@title='Покинуть книгу']"));
                link.Click();
                counter++;

            }

            // На главной должна появиться галочка о пройденном этапе. У меня появилась когда перелогинился.
            Console.WriteLine("Завершение теста");
            Console.WriteLine("Количество кликов: " + counter);
            Console.ReadLine();
        }
    }
}
