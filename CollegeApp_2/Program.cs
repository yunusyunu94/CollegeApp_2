using System;
using CollegeApp_2.Mylogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// Logger ;
var builder = WebApplication.CreateBuilder(args);
// bir kayit chazini kullanmak ;
// Once konsoru temizlicez ;
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Yalnizca konsol icin
//builder.Logging.AddDebug(); // Yalnizca hata ayiklama giris yapmak icin

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



// Add services to the container.

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/// Postmanda Json istek d�s�nda fakl� format istek alinirsa hada mesaji dondermemiz gerekir bunun icin AddControllers() icine " Options => Options.ReturnHttpNotAcceptable = true " yazmaliyiz ;
/// Json formati icin   ; AddNewtonsoftJson()
/// XML formati icin    ; AddXmlDataContractSerializerFormatters()

builder.Services.AddControllers(Options => Options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/// ---------------------- 2. Loosely coumpled ------------------------

// DemoController devami

// builder.Services.AddScoped<IMyLogger, LogToFile>();     // �stege ( reguest ) 1 neste uretir 
builder.Services.AddSingleton<IMyLogger, LogToFile>();  // Her ayri istege ( reguest ) neste uretir 
//builder.Services.AddTransient<IMyLogger, LogToFile>();  // Her �stege ( reguest ) ayri ayri neste uretir 


// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------- Logger ------------------------------------------------------------------

// Console  ( Konsol Gunluk Kaydi  ) ;
// StudentController de loglamayi yaptik konsol ekraninda g�rabiliriz


// Debug    ( Hata ay�klama Gunluk Kaydi  );
// Sadece bir kayit chazini kullanmak istiyorsak yukaridaki  " var builder = WebApplication.CreateBuilder(args); " kisnida adimlari izlicez


// EventSource  ( OlayKayna�� Gunluk Kaydi  )
// EventLog (EventLog (yaln�zca Windows) Gunluk Kaydi )

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
