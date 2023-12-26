
using APITestFramework;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UsersAPI
{ //Faker class of c# is used for generate fake data
    //public class UserHelper : IModelHelper<UserModel>
        public class UserHelper 
    {
        Faker<UserModel> userfaker;
        //Created constructor to initialize job and name
        public UserHelper()
        {   userfaker = new Faker<UserModel>();
            //RuleFor method for Faker class 
            userfaker.RuleFor(x => x.name, x => x.Name.FullName());
            userfaker.RuleFor(x => x.job, x => x.Random.Word());
        }
        //virtual Generate method of Faker class
        public UserModel Generate()
        {
            //new UserModel
            //{   
            //}
          return  userfaker.Generate();
        }
    }
}
