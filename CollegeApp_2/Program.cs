using System;
using System.Text;
using CollegeApp_2.Configurations;
using CollegeApp_2.Data;
using CollegeApp_2.Data.Repository;
using CollegeApp_2.Mylogging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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


//  -----------------------------------------    AutoMapper    ---------------------------------------------------------------

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

// VEYA

// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  Bu butun esemlileri getirir

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


// -------------------------  IStudentsRepository  -------------------

builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();




// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// -------------------------  ICollageRepository  -------------------

builder.Services.AddScoped(typeof(ICollageRepository<>), typeof(CollageRepository<>));


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


// ----------------------------------------- Enabling CORS ------------------------------------------------------------------

// https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0 Sitesinden ; " CORS with named policy and middleware " 

// builder.Services.AddCors(options => options.AddPolicy("MyTestCors",
//                      policy =>
//                      {
//                          // Allow all orgins(Tum kokenlere izin verdik)
//                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); // AllowAnyOrigin  ; Herhen gibir kokene izin ver.
//                                                                                     // AllowAnyHeader  ; Herhen gibir basliga izin ver.
//                                                                                     // AllowAnyMethod  ; Herhen gibir yonteme izin ver. Yontemler ; GET,POST,PUT,DELETE

//                          // Allow only few orgins(Yalniz bir kac kokene izin verdik)
//                          policy.WithOrigins("http://localhost:5164");
//                      }));



// Birden fazla politika tanimlamak icin ;

builder.Services.AddCors(options =>
{

    // DEFAULT olarak ;

    // options.AddDefaultPolicy(policy =>
    // {
    //    // Allow all orgins(Tum kokenlere izin verdik)
    //    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    // });


    options.AddPolicy("AllowAll", policy =>
                      {
                          // Allow all orgins(Tum kokenlere izin verdik)
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

                      });

    options.AddPolicy("AllowOnlyLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5164").AllowAnyHeader().AllowAnyMethod();

    });

    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com","http://gmail.com","http://drive.google.com").AllowAnyHeader().AllowAnyMethod();

    });

    options.AddPolicy("AllowOnlyMicrosoft", policy =>
    {
        policy.WithOrigins("http://outlook.com","http://microsoft.com","http://onedrive.google.com").AllowAnyHeader().AllowAnyMethod();

    });

});

// Assagida CORS'u etkinlestirmek gerekir.

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ----------------------------------------- Enabling CORS ------------------------------------------------------------------

app.UseRouting();

// CORS'u etkinlestirmek icin ;
app.UseCors("AllowAll");

// Default Policy icin ;
app.UseCors();

// NOT !!! ; UseAuthorization dan once yapmamiz gerekiyor.

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------- JWT TOKEN ------------------------------------------------------------------

// Ýlk once ;
// Nugetten assagidaki kutuphaneyi yukuyoruz
// Microsoft.AspNetCore.Authentication.JwtBearer

// Ýkinci olarak ;
// 3 tane Controller var hangisini guvenli uc noktasi hangilerinin genel uc noktasi oldugunu belirtmemiz lazim bunun icin ;
// StudentController da [Authorize(Roles ="Superadmin")] Roles ile erisimi kisitlamasi verebiliriz burada yalniz Superadmin erisimi olanlar icerigi gorebilir
// Eger kisitlama olmicaksa  [AllowAnonymous] demeliyiz, StudentController de GetStudents de yaptigimiz gibi erisimde kisitlama olmayacaktir

// Ucuncu olarak ;
// Secret Key gizli anahtara ihtiyacimiz var bunun icin appsettings.json da Secret Key gizli anahtari olusturucaz
// Sonra unu okumak icin Enabling CORS da tanimlamamiz gerek "  builder.Configuration.GetValue<string>("JWTSecret")  " seklinde


// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// ----------------------------------------- JWT Authentication Configuration ------------------------------------------------------------------

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"));

builder.Services.AddAuthentication(options => {

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // Varsayilan kimlik dogrulama semasi
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;     // Varsayilan Medya okuma semasi
}).AddJwtBearer(options =>
{
   // options.RequireHttpsMetadata = false; // HTTP sorunu yasiyorsak, https yerine http de calisir uretim ortaminda falsa yapmamaliyiz
   options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {

        ValidateIssuerSigningKey = true, // Yayincinin imzalama anahtari
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Yayinciyi dogrulamak
        ValidateAudience = false,

    };
});

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

app.UseAuthorization();

// ----------------------------------------- Enabling CORS ------------------------------------------------------------------

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",                                   // Yerel ana bilgisayar
        context => context.Response.WriteAsync("Test Respons"))
        .RequireCors("AllowAll");

    endpoints.MapControllers()                                  // Burada uygulama denetleyicimizi eþler
             .RequireCors("AllowOnlyLocalhost");

    endpoints.MapGet("api/testingendpoint2",                                  // Ýstek ve yanit burada iþlenir
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));
});

//Yukariya  " app.UseCors(); " in üstüne  " app.UseRouting(); "  eklemelisin

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//app.MapControllers();

app.Run();
