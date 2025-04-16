using Microsoft.FeatureManagement;
using Serilog;
using WorkerService.Template;
using WorkerService.Template.Manager;
using WorkerService.Template.Model;


try
{
    var builder = Host.CreateApplicationBuilder(args);

    // Feature Management'ı ekle
    builder.Services.AddFeatureManagement();

    // Loglama işlemini önceki aşamada temizle
    builder.Logging.ClearProviders(); // Mevcut log sağlayıcılarını temizle

   
    // FeatureManager'ı Resolve et
    var featureManager = builder.Services.BuildServiceProvider().GetRequiredService<IFeatureManager>();

    if (featureManager.IsEnabledAsync("LoggingEnabled").Result)
    {
        // Serilog'u appsettings.json'dan yükle
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
            .CreateLogger();

        // Serilog'u doğrudan builder.Logging üzerinden ekle
        builder.Logging.AddSerilog(); // Serilog'u ekle
    }

    // AppSettings ve diğer servisleri ekle
    AddAppSettings(builder);

    // Hosted service ve diğer bağımlılıkları ekle
    AddServices(builder);
  
    if (featureManager.IsEnabledAsync("LoggingEnabled").Result)
        Log.Information("Worker service başlıyor...");

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Worker service başlatılamadı.");
}
finally
{
    Log.CloseAndFlush();
}

void AddAppSettings(HostApplicationBuilder builder)
{
    // Add AppSettings directly to the services
    var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
    if (appSettings == null)
        throw new Exception("AppSettings bölümü appsettings.json dosyasında bulunamadı.");

    builder.Services.AddSingleton(appSettings);

    var b4BSettings = builder.Configuration.GetSection("B4BSettings").Get<B4BSettings>();
    if (b4BSettings == null)
        throw new Exception("B4BSettings bölümü appsettings.json dosyasında bulunamadı.");

    builder.Services.AddSingleton(b4BSettings);

    var crmSettings = builder.Configuration.GetSection("CrmServiceSettings").Get<CrmServiceSettings>();
    if (crmSettings == null)
        throw new Exception("CrmServiceSettings bölümü appsettings.json dosyasında bulunamadı.");

    builder.Services.AddSingleton(crmSettings);
}

void AddServices(HostApplicationBuilder builder)
{
    // Singleton servislerini ekle
    builder.Services.AddSingleton<IB4BService, B4BManager>();
    builder.Services.AddSingleton<IPurchaseDetailService, PurchaseDetailManager>();
    builder.Services.AddSingleton<IPurchaseDetailSupplyService, PurchaseDetailSupplyManager>();
    builder.Services.AddSingleton<ServiceFactory>();

    // Hosted Service ekle
    builder.Services.AddHostedService<Worker>();
}