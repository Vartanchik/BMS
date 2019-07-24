using BMS.WebAPI.Models;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BMS.IntegrationTest
{
    public class AccountApiIntegrationTest
    {
        [Fact]
        public async Task RegisterNewUser_IsSuccessful()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/Account/Register", new StringContent(
                    JsonConvert.SerializeObject(new RegisterModel
                    {
                        Name = RandomString(6),
                        Email = RandomString(4) + "@test.com",
                        Password = "123456",
                        ConfirmPassword = "123456"
                    }),
                    Encoding.UTF8,
                    "application/json"));

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[new Random().Next(s.Length)])
                                        .ToArray());
        }
    }
}
