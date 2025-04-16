using WorkerService.Template.DTOs;
using WorkerService.Template.Model.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Template.Model;

namespace WorkerService.Template.Manager
{
    public interface IPurchaseDetailService
    {
        Task<List<PurchaseDetailDTO>> GetUnsentToB4B();

        Task<IResult> SendToB4B(Guid id);
    }

    public class PurchaseDetailManager : IPurchaseDetailService
    {
        private readonly CrmServiceSettings _crmServiceSettings;

        public PurchaseDetailManager(CrmServiceSettings crmServiceSettings)
        {
            _crmServiceSettings = crmServiceSettings;
        }

        public async Task<List<PurchaseDetailDTO>> GetUnsentToB4B()
        {
            var client = new HttpClient();

            var requestUri= string.Format("{0}{1}", _crmServiceSettings.ApiURL, "purchasedetails/GetUnsentToB4B");

            var request = new HttpRequestMessage(HttpMethod.Get,requestUri);
            request.Headers.Add(_crmServiceSettings.KeyName,_crmServiceSettings.KeyValue);

            var content = new StringContent("", null, "text/plain");
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON string to a list of PurchaseDetailDTO objects
            var purchaseDetails = JsonConvert.DeserializeObject<DataResult<List<PurchaseDetailDTO>>>(responseStr);

            if (purchaseDetails != null && purchaseDetails.Success)
                return purchaseDetails.Data;

            return new List<PurchaseDetailDTO>();
        }

        public async Task<IResult> SendToB4B(Guid id)
        {
            var client = new HttpClient();

            var requestUri = string.Format("{0}{1}/{2}", _crmServiceSettings.ApiURL, "purchasedetails/sendtob4b",id.ToString("D"));

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Add(_crmServiceSettings.KeyName, _crmServiceSettings.KeyValue);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Result>(responseStr);

            return result?? new Result { Success=false};
        }
    }
}
