
using APITestFramework;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI
{
    [TestClass]
    public class UserTestSuite
    {
        HttpUtil util = new HttpUtil("App.config.json", "Application/json", false);
        UserHelper apiHelper = new UserHelper();
        UserActionHelper action = new UserActionHelper();
        //TestMethod used for test case in MSTest
        [TestMethod]
        public void Postusertest()
        {
            //Arrange: body for post
           
            //1st way
            //UserModel data = new UserModel
            //{
            //    name = "pawan",
            //    job = "QA"
            //};

            //2nd way
            UserModel data = apiHelper.Generate();
            //data.job = "";

            //3rd way
            //UserModel data = new UserModel();
            //data.name = "";
            //data.job = "";

            //Action
            HttpResponseMessage reponse = util.PostRequest("api/users", data);
            Console.WriteLine(reponse);
            //Verify status code
            reponse.StatusCode.Should().Be(201);

            //verify whole response
            var actu = util.GetResponseBody(reponse);
            UserModel actualdata= JsonConvert.DeserializeObject<UserModel>(actu);
            //List<UserModel> actualdata = JsonConvert.DeserializeObject<List<UserModel>>(actu);
            //for comparison of objects
            // actualdata.name.Should().Be(data.name);
            actualdata.Should().BeEquivalentTo(data);

        }
        [TestMethod]
        public void Getusertest()
        {
            string id = action.AddUser();
            HttpResponseMessage reponse = util.GetRequest("api/users" + id);
            reponse.StatusCode.Should().Be(200);
        }

        [TestMethod]
        public void Putusertest()
        {

            string id=action.AddUser();

            //UserModel data = new UserModel
           // {
           //     name = "pawan1",
           //     job = "QA1"
            //};
            //2nd way
            UserModel data = apiHelper.Generate();
            HttpResponseMessage reponseupsate = util.PutRequest("api/users"+ id, data);

            reponseupsate.StatusCode.Should().Be(200);
            var actu = util.GetResponseBody(reponseupsate);

        }
        [TestMethod]
        public void Deleteusertest()
        {
            string id = action.AddUser();
            HttpResponseMessage reponse = util.DeleteRequest("api/users/"+id);
            reponse.StatusCode.Should().Be(204);
        }


        }


}
