using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.Model
{
    public class B4BToken
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string SecretKey { get; set; }
    }

    public class B4BRequest
    {
        public string SecretKey { get; set; }
        public IntegrationLoginRequest IntegrationLoginRequest { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class IntegrationLoginRequest
    {
        public string UserCode { get; set; }
        public string Password { get; set; }
    }

    public class Order
    {
        public string OrderNo { get; set; }
        public string Notes { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public string OrderDetailNo { get; set; }
        public string ProductCode { get; set; }
        public string NetPrice { get; set; }
        public string Currency { get; set; }
        public string Manufacturer { get; set; }
        public string Quantity { get; set; }
        public bool IsDeleted { get; set; }
    }


}
