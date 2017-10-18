using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace EthereumDemoApp.Models
{
    public class Login
    {
  
        public Login(){}

        public Member ExecuteLogin(string pass)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(API_Dictionary.ApiLogin);
                var json = JsonConvert.SerializeObject(new
                {
                    token = pass
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =  client.PostAsync(uri, content).Result;
                string ans =  response.Content.ReadAsStringAsync().Result;
                Member req = JsonConvert.DeserializeObject<Member>(ans);
                return req;
            }
        }

    }
}
