using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.Model
{
    public class B4BResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object SecretKey { get; set; }
    }
}
