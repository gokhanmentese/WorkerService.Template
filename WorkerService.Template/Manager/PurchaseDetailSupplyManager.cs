using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Template.DTOs;
using WorkerService.Template.Model;
using WorkerService.Template.Model.Results;

namespace WorkerService.Template.Manager
{
    public interface IPurchaseDetailSupplyService
    {
        Task<List<PurchaseDetailSupplyDTO>> GetSuppliesByPurchaseDetailId(Guid purchaseDetailId);

    }

    public class PurchaseDetailSupplyManager : IPurchaseDetailSupplyService
    {
        private readonly CrmServiceSettings _crmServiceSettings;

        public PurchaseDetailSupplyManager(CrmServiceSettings crmServiceSettings)
        {
            _crmServiceSettings = crmServiceSettings;
        }

        public async Task<List<PurchaseDetailSupplyDTO>> GetSuppliesByPurchaseDetailId(Guid purchaseDetailId)
        {
            var client = new HttpClient();

            var requestUri = string.Format("{0}{1}/{2}", _crmServiceSettings.ApiURL, "purchasedetailsupplies/GetByPurchaseDetailId", purchaseDetailId.ToString("D"));

            var request = new HttpRequestMessage(HttpMethod.Get,requestUri);
            request.Headers.Add(_crmServiceSettings.KeyName, _crmServiceSettings.KeyValue);

            var content = new StringContent("", null, "text/plain");
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string to a list of PurchaseDetailDTO objects
            var purchaseDetails = JsonConvert.DeserializeObject<DataResult<List<PurchaseDetailSupplyDTO>>>(responseStr);

            if (purchaseDetails != null && purchaseDetails.Success)
                return purchaseDetails.Data;

            return new List<PurchaseDetailSupplyDTO>();
        }
    }
}
