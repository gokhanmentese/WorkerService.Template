🌍 Worker Service Template ✈️

.NET 8 ile yazılmış bir Worker Service (Background Service) uygulamasıdır. Belirli aralıklarla çalışan zamanlanmış görevleri yerine getirir ve arka plan işlemlerini yönetmek için tasarlanmıştır.


🚀 Projede Kullanılan Teknolojiler

🛠️ .NET Core:  [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) ile geliştirilmiş bir Worker Service (Background Service) uygulamasıdır. Arka planda zamanlanmış görevleri periyodik olarak çalıştırmak için kullanılır.

🔑 Loglama: Projede SeriLog ile dosyaya loglama işlemi kullanılmıştır. Birkaç konfigurasyon değişikliğiyle ElasticSearche de loglama yapılabilir.

✨ Uygulama Ayarları: Uygulamada kullanılacak değerlerin appsettings.json üzerinden alınması sağlanmıştır.

🎨 ServiceFactory: Projede kullanılan bazı servislerin merkezi bir noktadan alınmasını kolaylaştırmak için oluşturulmuş bir sınıftır. Dependency Injection (bağımlılık enjeksiyonu) ile IServiceProvider üzerinden ilgili servisler resolve edilir. Böylece ilgili servislerin örneklerine erişim kolaylaştırılmış olur..
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

⚡ Özellik Yönetimi (Faeture Management): Uygulamanın belirli özelliklerini dinamik olarak açıp kapatmana olanak tanır.İlgili özellik Microsoft.FeatureManagement kullanılarak aktif edilmiştir.Bu projede, LoggingEnabled özelliğini kontrol ederek loglamayı dinamik olarak yönetiyoruz.
