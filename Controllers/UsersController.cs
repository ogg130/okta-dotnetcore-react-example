using Microsoft.AspNetCore.Mvc;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using okta_dotnetcore_react_example.Models;

namespace okta_dotnetcore_react_example.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpPost]
        public async void Post([FromBody]Registration reg)
        {

            var oktaClient = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = "https://dev-9659690.okta.com",
                Token = "00h0b1ntEyYGqXnnHrGKJrTjGxvmEJVQa_bpCcvB0h"
            });

            var user = await oktaClient.Users.CreateUserAsync(
                new CreateUserWithPasswordOptions
                {
                    Profile = new UserProfile
                    {
                        FirstName = reg.FirstName,
                        LastName = reg.LastName,
                        Email = reg.Email,
                        Login = reg.Email
                    },
                    Password = reg.Password,
                    Activate = true
                }
            );
        }
    }
}