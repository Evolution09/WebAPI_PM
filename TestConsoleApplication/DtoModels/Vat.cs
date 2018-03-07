using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.DtoModels
{
    public class Vat : IDTOModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
    }
}
