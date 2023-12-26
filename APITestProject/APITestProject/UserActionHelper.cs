using APITestFramework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI
{
    public class UserActionHelper
    {
        HttpUtil util = new HttpUtil("App.config.json", "Application/json", false);
        UserHelper apiHelper = new UserHelper();
        //used for prereqvisting of put
        public string AddUser()
        {
            UserModel datapost = apiHelper.Generate();

            HttpResponseMessage reponse = util.PostRequest("api/users", datapost);
            //Verify
            reponse.StatusCode.Should().Be(201);
            string id = "";
            return id;
        }
    }
}
