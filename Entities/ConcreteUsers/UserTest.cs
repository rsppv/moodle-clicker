using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ConcreteUsers
{
  class UserTest
  {

      private string _username;
      private string _password;

      public string Username
      {
        get { return _username; }
      }

      public string Password
      {
        get { return _password; }
      }

      public UserTest()
      {
        _username = "TestUserName";
        _password = "TestPassword";
      }

  }
}
