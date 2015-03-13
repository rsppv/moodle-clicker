using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Entities;

namespace MoodleClicker
{
  [TestClass]
  public class UnitTest1
  {
    public IWebDriver chrome;
    public IUser user;
    
    [TestMethod]
    public void TestMethod1()
    {
      chrome = new ChromeDriver();
      // Чтобы создать своего юзера, нужно
      // создать класс, реализующий интерфейс IUser,
      // в конструкторе задать username и password,
      // положить его в папку ConcreteUsers 
      user = new UserRaspopov();

      // Заходим на сайт
      chrome.Navigate().GoToUrl("http://stud.lms.tpu.ru/");

      // Логинимся
      IWebElement login = chrome.FindElement(By.Id("login_username"));
      IWebElement password = chrome.FindElement(By.Id("login_password"));
      login.SendKeys(user.Username);
      password.SendKeys();
      password.Submit();
      Console.WriteLine(chrome.FindElement(By.ClassName("logininfo")).Text);

      // Открываем Книгу (Лекция, Теория, Зеленая книжка такая)
      chrome.Navigate().GoToUrl("http://stud.lms.tpu.ru/mod/book/view.php?id=20800");
      
      // Пролистываем ее до конца
      IWebElement link;
      IWebElement navbar;
      while (chrome.FindElement(By.XPath("//div[@class='navtop']"))
        .FindElements(By.XPath(".//a[@title='Следующая']")).Count != 0)
      {
        navbar = chrome.FindElement(By.XPath("//div[@class='navtop']"));
        link = navbar.FindElement(By.XPath(".//a[@title='Следующая']"));
        link.Click();
        Console.WriteLine(chrome.FindElement(By.CssSelector("div.book_content h3")).Text);
      }
      navbar = chrome.FindElement(By.XPath("//div[@class='navtop']"));
      link = navbar.FindElement(By.XPath(".//a[@title='Покинуть книгу']"));
      link.Click();

      // На главной должна появиться галочка о пройденном этапе. У меня появилась когда перелогинился.
      Console.WriteLine("Завершение теста");
    }


    [TestCleanup]
    public void TearDown()
    {
      chrome.Quit();
    }

  }
}
