using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;

namespace TestConsoleApplication
{
    class Program
    {
        #region Constans

        private const string BASE_ADDRESS = "http://localhost:62141/";
        private const string PRODUCT_CONTROLLER = "api/Products";
        private const string PROD_TEMP = "?Code={0}";

        #endregion

        public partial class DtoProduct
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public int Category { get; set; }
            public decimal Price { get; set; }

            public static explicit operator DtoProduct(JToken JsonProd)
            {
                var prod = new DtoProduct();
                prod.ID = JsonProd["ID"].ToString();
                prod.Name = JsonProd["Name"].ToString();
                prod.Code = JsonProd["Code"].ToString();
                prod.Category = int.Parse(JsonProd["Category"].ToString());
                prod.Price = decimal.Parse(JsonProd["Price"].ToString());

                return prod;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_ADDRESS);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List all products
            var prodList = SendGet(client);

            // Update prod
            var changedProduct = (DtoProduct) prodList[0];
            changedProduct.Name = "changed";
            changedProduct.Price = 12;
            changedProduct.Category = 0;

            SendPut(client, changedProduct);

            // Add prod
            var newProd = new DtoProduct();
            newProd.ID = "9999999999";
            newProd.Name = "Created by client";
            newProd.Code = "Test99";
            newProd.Category = 0;
            newProd.Price = 11;

            SendPost(client, newProd);

            prodList = SendGet(client);
            Console.WriteLine(prodList.ToString());

            // Delete prod
            SendDelete(client, "Test99");

            Console.ReadLine();
        }

        private static JArray SendGet(HttpClient Client)
        {
            JArray list = null;

            HttpResponseMessage response = Client.GetAsync(PRODUCT_CONTROLLER).Result;
            if (response.IsSuccessStatusCode)
                list = response.Content.ReadAsAsync<JArray>().Result;
            else
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            return list;
        }

        private static void SendPut(HttpClient Client, DtoProduct Product)
        {
            string uri = string.Concat(PRODUCT_CONTROLLER, string.Format(PROD_TEMP, Product.Code));
            HttpResponseMessage response = Client.PutAsJsonAsync(uri, Product).Result;

            string msg = response.IsSuccessStatusCode ? 
                "Success!" : 
                string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            Console.WriteLine(msg);
        }

        private static void SendPost(HttpClient Client, DtoProduct Product)
        {
            HttpResponseMessage response = Client.PostAsJsonAsync(PRODUCT_CONTROLLER, Product).Result;

            string msg = response.IsSuccessStatusCode ?
                "Success!" :
                string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            Console.WriteLine(msg);
        }

        private static void SendDelete(HttpClient Client, string Code)
        {
            string uri = string.Concat(PRODUCT_CONTROLLER, string.Format(PROD_TEMP, Code));
            HttpResponseMessage response = Client.DeleteAsync(uri).Result;

            string msg = response.IsSuccessStatusCode ?
                "Success!" :
                string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            Console.WriteLine(msg);
        }

    }
}
