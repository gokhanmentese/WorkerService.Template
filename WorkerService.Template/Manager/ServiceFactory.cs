using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.Manager
{
    public class ServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPurchaseDetailService CreatePurchaseDetailService() => _serviceProvider.GetRequiredService<IPurchaseDetailService>();
        public IPurchaseDetailSupplyService CreatePurchaseDetailSupplyService() => _serviceProvider.GetRequiredService<IPurchaseDetailSupplyService>();
        public IB4BService CreateB4BService() => _serviceProvider.GetRequiredService<IB4BService>();

    }
}
