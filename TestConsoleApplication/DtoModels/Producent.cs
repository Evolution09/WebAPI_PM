using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.DtoModels
{
    public class Producent : IDTOModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public int? AddressID { get; set; }

        public Address Addr { get; set; }
    }
}
