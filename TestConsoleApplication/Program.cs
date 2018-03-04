using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using TestConsoleApplication.DtoModels;

namespace TestConsoleApplication
{
    class Program
    {
        #region Constans

        private const string BaseAddress = "http://localhost:62141/";
        private const string ProductController = "api/product";
        private const string ProdTemp = "?Code={0}";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Args"></param>
        static void Main(string[] Args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List all products
            var prodList = SendGet(client);

            // Update prod
            var changedProduct = (Product) prodList[0];
            changedProduct.Name += " changed";
            changedProduct.Price = 12;
            changedProduct.CategoryID = 0;
            changedProduct.EAN = "1234";

            SendPut(client, changedProduct);

            // Add prod
            var newProd = new Product();
            newProd.Name = "Created by client";
            newProd.Code = "Test99";
            newProd.CategoryID = 0;
            newProd.Price = 11;
            newProd.EAN = "1234";

            SendPost(client, newProd);

            prodList = SendGet(client);
            Console.WriteLine(prodList.ToString());

            // Delete prod
            //SendDelete(client, "Test99");

            Console.ReadLine();
        }

        private static JArray SendGet(HttpClient Client)
        {
            JArray list = null;

            HttpResponseMessage response = Client.GetAsync(ProductController).Result;
            if (response.IsSuccessStatusCode)
                list = response.Content.ReadAsAsync<JArray>().Result;
            else
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            return list;
        }

        private static void SendPut(HttpClient Client, Product Product)
        {
            string uri = string.Concat(ProductController, string.Format(ProdTemp, Product.Code));
            HttpResponseMessage response = Client.PutAsJsonAsync(uri, Product).Result;

            string msg = response.IsSuccessStatusCode ? 
                "Success!" : 
                string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            Console.WriteLine(msg);
        }

        private static void SendPost(HttpClient Client, Product Product)
        {
            HttpResponseMessage response = Client.PostAsJsonAsync(ProductController, Product).Result;

            string msg = response.IsSuccessStatusCode ?
                "Success!" :
                string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            Console.WriteLine(msg);
        }

        private static void SendDelete(HttpClient Client, string Code)
        {
            string uri = string.Concat(ProductController, string.Format(ProdTemp, Code));
            HttpResponseMessage response = Client.DeleteAsync(uri).Result;

            string msg = response.IsSuccessStatusCode ?
                "Success!" :
                string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            Console.WriteLine(msg);
        }

    }
}
