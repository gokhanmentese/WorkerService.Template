🌍 Worker Service Template ✈️

Bu proje, arka plan işlemlerinde kullandığımız servis yazılımının şablonudur. 


🚀 Projede Kullanılan Teknolojiler

🛠️ .NET Core: Proje, bu .NET 8 'i kullanır.

🔑 Loglama: Projede SeriLog ile dosyaya loglama işlemi kullanılmıştır.

✨ Uygulama Ayarları: Uygulamada kullanılacak değerlerin appsettings.json üzerinden alınması sağlanmıştır.

🎨 Dependeny Injection: ServiceFactory ,projede kullanılan bazı servislerin merkezi bir noktadan alınmasını kolaylaştırmak için oluşturulmuş bir sınıftır. Dependency Injection (bağımlılık enjeksiyonu) ile IServiceProvider üzerinden ilgili servisler resolve edilir. Böylece ilgili servislerin örneklerine erişim kolaylaştırılmış olur..
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

⚡ Özellik Yönetimi (Faeture Management): Uygulamanın belirli özelliklerini dinamik olarak açıp kapatmana olanak tanır.İlgili özelliği Microsoft.FeatureManagement kullanılarak aktif edilmiştir.Bu projede, LoggingEnabled özelliğini kontrol ederek loglamayı dinamik olarak yönetiyoruz.
