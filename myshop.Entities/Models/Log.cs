using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class Log
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
