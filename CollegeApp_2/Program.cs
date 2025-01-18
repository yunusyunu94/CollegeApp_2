using System;
using CollegeApp_2.Data;
using CollegeApp_2.Mylogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/// -----------------------------------------   SERÝ LOGGER    ---------------------------------------    

//  Oncelikle Nugetten bu iki paketi kuruyoruz ; Serilog.AspNetCore ve Serilog.Sinks.File
// Sonra assagiidaki kodu SeriLogger githubdan aliyoruz linki ; https://github.com/serilog/serilog-aspnetcore

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information() // Gunluk seviyelerinden bahsetmek istersek
//    .WriteTo.File("Log/log.txt", rollingInterval:RollingInterval.Minute) // buradaki Consolu kaldiriyoruz bir dosya ( File ) saglayicisi istiyoruz. rollingInterval:RollingInterval.Day Hergun icin metin dosyasi olusturur
//    .CreateLogger();

// Yukarýdaki koddan sonra assagidaki builder ediyoruz.

// User this line to override the built-in loggers ;
//builder.Host.UseSerilog();     // Bunda Konsola yansitmaz
// Vaya
//builder.Services.AddSerilog(); // Bunda Konsola yansitmaz

// Use serilog along with built-in loggers ;
//builder.Logging.AddSerilog();   // Bunda Konsol dahil tum saglayicilarda gorebiliriz

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//  -----------------------------------------   LOG4NET    ---------------------------------------------------------------

// Nugetten " Microsoft.Extensions.Logging.Log4Net.AspNetCore " yukluyoruz. Assagýdakikodu yaziyoruz

builder.Logging.ClearProviders();   // Tum yerlesik saglayicilari temizler
builder.Logging.AddLog4Net();

// Sonrasinda Configuration dosyasi icin projeye ADD kismindan NEW ÝTEM oradan WEB CONFÝGURTÝON FÝLE yi seciyoruz ve ismine " log4net.config " yaziyoruz
// SOnra acilam dosyada herseyi siliyoruz ve içindeki kodu yapistiriyoruz

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


// -----------------------------------------   SQL SERVER    ---------------------------------------------------------------

builder.Services.AddDbContext<CollegeDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection")); // Hangi veri tabani kullanmak istedigimizi belirtiyoruz.
                                                                                              // Appsettings.js de danimladigimiz baglandi dizisini
                                                                                              // adini yaziyoruz.
});








// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


// Add services to the container.

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/// Postmanda Json istek dýsýnda faklý format istek alinirsa hada mesaji dondermemiz gerekir bunun icin AddControllers() icine " Options => Options.ReturnHttpNotAcceptable = true " yazmaliyiz ;
/// Json formati icin   ; AddNewtonsoftJson()
/// XML formati icin    ; AddXmlDataContractSerializerFormatters()

builder.Services.AddControllers(Options => Options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/// ---------------------- 2. Loosely coumpled ------------------------

// DemoController devami

// builder.Services.AddScoped<IMyLogger, LogToFile>();     // Ýstege ( reguest ) 1 neste uretir 
builder.Services.AddSingleton<IMyLogger, LogToFile>();  // Her ayri istege ( reguest ) neste uretir 
//builder.Services.AddTransient<IMyLogger, LogToFile>();  // Her Ýstege ( reguest ) ayri ayri neste uretir 



// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------- Logger ------------------------------------------------------------------

// Console  ( Konsol Gunluk Kaydi  ) ;
// StudentController de loglamayi yaptik konsol ekraninda görabiliriz


// Debug    ( Hata ayýklama Gunluk Kaydi  );
// Sadece bir kayit chazini kullanmak istiyorsak yukaridaki  " var builder = WebApplication.CreateBuilder(args); " kisnida adimlari izlicez


// EventSource  ( OlayKaynaðý Gunluk Kaydi  )
// EventLog (EventLog (yalnýzca Windows) Gunluk Kaydi )

// Yukaridakileri dogrudan kullanmamizi saglayan en uste    " var builder = WebApplication.CreateBuilder(args); " yapisi sayesinde Dependency Injection icin dogrudan kullanilabilir olacaktir

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
