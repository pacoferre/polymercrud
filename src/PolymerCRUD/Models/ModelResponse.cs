using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolymerCRUD.Models
{
    public class ModelResponse
    {
        public bool ok { get; set; } = false;
        public string message { get; set; } = "";
        public object model { get; set; } = null;
    }
}
