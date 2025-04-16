using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.DTOs
{
    [Serializable]
    public class PurchaseDetailSupplyDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("ProductCode")]
        public string ProductCode { get; set; }

        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("ManufacturerCode")]
        public string ManufacturerCode { get; set; }

        [JsonProperty("SuggestedSalesPrice")]
        public decimal? SuggestedSalesPrice { get; set; }

        [JsonProperty("CurrencyId")]
        public Guid? CurrencyId { get; set; }

        [JsonProperty("CurrencyName")]
        public string? CurrencyName { get; set; }

        [JsonProperty("SupplierQuantity")]
        public decimal? SupplierQuantity { get; set; }

        [JsonProperty("StatusCode")]
        public int? StatusCode { get; set; }


        [JsonProperty("ApprovedQuantity")]
        public decimal? ApprovedQuantity { get; set; }


    }
}
