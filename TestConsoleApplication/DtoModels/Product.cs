using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestConsoleApplication.DtoModels
{
    public class Product : IDTOModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string EAN { get; set; }
        public int ProducentID { get; set; }
        public int CategoryID { get; set; }
        public int VATID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public static explicit operator Product(JToken JsonProd)
        {
            var prod = new Product
            {
                ID = int.Parse(JsonProd["ID"].ToString()),
                Name = JsonProd["Name"].ToString(),
                Code = JsonProd["Code"].ToString(),
                CategoryID = int.Parse(JsonProd["CategoryID"].ToString()),
                Price = decimal.Parse(JsonProd["Price"].ToString())
            };

            return prod;
        }
    }
}
