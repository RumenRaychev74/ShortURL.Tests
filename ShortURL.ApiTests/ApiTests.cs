using RestSharp;
using System.Net;
using System.Text.Json;

namespace ShortURL.ApiTests
{
    public class ApiTests
    {
        private const string baseUrl = "https://shorturl.nakov.repl.co/api";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
        }

        [Test]
        public void Test_ListAllUrlst()
        {
            //Arenge
            this.request = new RestRequest("/urls");

            //Act
            var response = this.client.Execute(request, Method.Get);
            var urls = JsonSerializer.Deserialize<List<UrlResponse>>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsTrue(urls != null);
        }
        [Test]
        public void Test_FindUrlByShortCode_ValidInput()
        {
            // Arrange
            request = new RestRequest("https://shorturl.nakov.repl.co/api/urls/seldev");
             request.AddUrlSegment("keyword", "seldev");

             // Act
             var response = client.Execute(request, Method.Get);
             var UrlResponse = JsonSerializer.Deserialize<UrlResponse>(response.Content);

             // Assert
             Assert.AreEqual(UrlResponse.shortCode, "seldev");

        }
        [Test]
        public void Test_FindUrlByShortCode_InvalidInput()
        {
            //Arrange
            var request = new RestRequest(baseUrl + "/IbreAbre");
            //Act
            var response = client.Execute(request, Method.Get);
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Test]
        public void Test_ShortUrls_CreateNewUrl()
        {
            //Arenge
            this.request = new RestRequest("/urls");
            request.AddHeader("Content-Type", "application/json");
            var newUrlData = new
            {
                url = "http://peugeot.bg",
                shortCode = "sfa" + DateTime.Now.Ticks, 
            };
            request.AddJsonBody(newUrlData);

            //Act
            var response = this.client.Execute(request, Method.Post);
            
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            CreateUrlResponse? responseUrl = JsonSerializer.Deserialize<CreateUrlResponse>(response.Content);
            Assert.AreEqual("Short code added.", responseUrl.msg);
            Assert.AreEqual(newUrlData.url, responseUrl.url.url);
            Assert.AreEqual(newUrlData.shortCode, responseUrl.url.shortCode);
            Assert.AreEqual(0, responseUrl.url.visits);
        }
        [Test]
        public void Test_DeleteNewUrl_ValidInput()
        {
            // 1. Create new URL
            var request = new RestRequest("/urls", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            string newCode = "del" + DateTime.Now.Ticks;
            var newUrl = new
            {
                url = "https://deleteme.com",
                shortCode = newCode
            };
            request.AddJsonBody(newUrl);
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Delete the created URL
            var delRequest = new RestRequest("/urls/" + newUrl.shortCode, Method.Delete);
            var delResponse = client.Execute(delRequest);

            Assert.AreEqual(HttpStatusCode.OK, delResponse.StatusCode);

        }
     
     }
}