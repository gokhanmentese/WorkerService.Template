using Microsoft.Extensions.Options;
using WorkerService.Template.Enumarations;
using WorkerService.Template.Manager;
using WorkerService.Template.Model;

namespace WorkerService.Template
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IB4BService _b4BService;
        private readonly IPurchaseDetailService _purchaseDetailService;
        private readonly IPurchaseDetailSupplyService _purchaseDetailSupplyService;
        private readonly AppSettings _appSettings;
        private readonly ServiceFactory _serviceFactory;

        public Worker(ILogger<Worker> logger, AppSettings appSettings, ServiceFactory serviceFactory)
        {
            _logger = logger;
            _appSettings = appSettings;
            _serviceFactory = serviceFactory;

            _purchaseDetailService = _serviceFactory.CreatePurchaseDetailService();
            _purchaseDetailSupplyService = _serviceFactory.CreatePurchaseDetailSupplyService();
            _b4BService = serviceFactory.CreateB4BService();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
              
                try
                {
                    var purchaseDetails = await _purchaseDetailService.GetUnsentToB4B();
                    if (purchaseDetails != null && purchaseDetails.Count != 0)
                    {
                        var groupedByOrderNo = purchaseDetails.GroupBy(x => x.OrderNumber).ToList();

                        foreach (var orderitems in groupedByOrderNo)
                        {
                            try
                            {
                                var orders = new List<Order>();

                                var order = new Order();
                                order.OrderNo = orderitems.Key;
                                order.Notes = string.Empty;

                                var orderDetails = new List<OrderDetail>();

                                foreach (var orderitem in orderitems)
                                {
                                    if (orderitem.ApprovalStatus != null && orderitem.ApprovalStatus.Value == (int)ApprovalStatus.Onay)
                                    {
                                        var supplies = await _purchaseDetailSupplyService.GetSuppliesByPurchaseDetailId(orderitem.Id);
                                        if (supplies != null && supplies.Count != 0)
                                        {
                                            var approveds = supplies
                                                .Where(a => a.StatusCode.HasValue && a.StatusCode.Value == (int)PurchaseDetailSupplyStatus.Onaylandi).ToList();
                                            if (approveds != null && approveds.Count != 0)
                                            {
                                                if (approveds.Count == 1)
                                                {
                                                    orderDetails.Add(new OrderDetail
                                                    {
                                                        OrderDetailNo = orderitem.OrderNumber,
                                                        ProductCode = approveds[0].ProductCode,
                                                        Quantity = (approveds[0].ApprovedQuantity != null ? approveds[0].ApprovedQuantity.Value.ToString() : "0"),
                                                        Currency = approveds[0].CurrencyName,
                                                        Manufacturer = approveds[0].ManufacturerCode,
                                                        NetPrice = (approveds[0].SuggestedSalesPrice != null ? approveds[0].SuggestedSalesPrice.Value.ToString() : "0"),
                                                        IsDeleted = false
                                                    });
                                                }
                                                else
                                                {
                                                    for (int i = 0; i < approveds.Count; i++)
                                                    {
                                                        orderDetails.Add(new OrderDetail
                                                        {
                                                            OrderDetailNo = i == 0 ? orderitem.OrderNumber : "-1",
                                                            ProductCode = approveds[i].ProductCode,
                                                            Quantity = (approveds[i].ApprovedQuantity != null ? approveds[i].ApprovedQuantity.Value.ToString() : "0"),
                                                            Currency = approveds[i].CurrencyName,
                                                            Manufacturer = approveds[i].ManufacturerCode,
                                                            NetPrice = (approveds[i].SuggestedSalesPrice != null ? approveds[i].SuggestedSalesPrice.Value.ToString() : "0"),
                                                            IsDeleted = false
                                                        });
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //Tedarikçi satırı olmadan onaylananlar
                                                orderDetails.Add(new OrderDetail
                                                {
                                                    OrderDetailNo = orderitem.OrderNumber,
                                                    ProductCode = orderitem.ProductCode,
                                                    Quantity = orderitem.Quantity,
                                                    Currency = orderitem.ParaBirimiStr,
                                                    Manufacturer = orderitem.ManufacturerCode,
                                                    NetPrice = orderitem.TahminiFiyat,
                                                    IsDeleted = false
                                                });
                                            }
                                        }
                                    }
                                    else if (orderitem.ApprovalStatus != null && orderitem.ApprovalStatus.Value == (int)ApprovalStatus.Red)
                                    {
                                        orderDetails.Add(new OrderDetail
                                        {
                                            OrderDetailNo = orderitem.OrderNumber,
                                            ProductCode = orderitem.ProductCode,
                                            Quantity = orderitem.Quantity,
                                            Currency = orderitem.ParaBirimiStr,
                                            Manufacturer = orderitem.ManufacturerCode,
                                            NetPrice = orderitem.TahminiFiyat,
                                            IsDeleted = true
                                        });
                                    }
                                }

                                order.OrderDetails = orderDetails;
                                orders.Add(order);

                                var response = await _b4BService.Send(orders);

                                if (response != null && response.Status)
                                {
                                    foreach (var item in orderitems)
                                    {
                                        await _purchaseDetailService.SendToB4B(item.Id);
                                    }
                                }
                                else
                                {
                                    _logger.LogError(response?.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

                await Task.Delay(1000 * 60 * _appSettings.Interval, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker service başlatılıyor...");
            // Başlangıç işlemleri burada yapılabilir (örneğin veri tabanı bağlantıları)
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker service durduruluyor...");
            // Servis durdurulmadan önce yapılacak temizleme işleri burada olabilir
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Worker service kaynakları serbest bırakılıyor...");
            // Kaynak temizleme işlemleri
            base.Dispose();
        }
    }
}
