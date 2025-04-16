using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.DTOs
{

    [Serializable]
    public class PurchaseDetailDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("OrderDetailNo")]
        public string OrderDetailNo { get; set; }


        [JsonProperty("OrderNumber")]
        public string OrderNumber { get; set; }


        [JsonProperty("ProductCode")]
        public string ProductCode { get; set; }


        [JsonProperty("TahminiFiyat")]
        public string TahminiFiyat { get; set; }


        [JsonProperty("ParaBirimiStr")]
        public string ParaBirimiStr { get; set; }


        [JsonProperty("ManufacturerCode")]
        public string ManufacturerCode { get; set; }


        [JsonProperty("Adet")]
        public string Quantity { get; set; }


        [JsonProperty("IsSentB4B")]
        public bool? IsSentB4B { get; set; } = false;

        [JsonProperty("ApprovalStatus")]
        public int? ApprovalStatus { get; set; }
    }
}
