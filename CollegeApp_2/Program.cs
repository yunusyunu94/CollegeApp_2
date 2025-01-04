using CollegeApp_2.Mylogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/// Postmanda Json istek dýsýnda faklý format istek alinirsa hada mesaji dondermemiz gerekir bunun icin AddControllers() icine " Options => Options.ReturnHttpNotAcceptable = true " yazmaliyiz ;
/// Json formati icin   ; AddNewtonsoftJson()
/// XML formati icin    ; AddXmlDataContractSerializerFormatters()

builder.Services.AddControllers(Options => Options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/// ---------------------- 2. Loosely coumpled ------------------------

builder.Services.AddScoped<IMyLogger, LogToFile>();     // Ýstege ( reguest ) 1 neste uretir 
builder.Services.AddSingleton<IMyLogger, LogToFile>();  // Her ayri istege ( reguest ) neste uretir 
builder.Services.AddTransient<IMyLogger, LogToFile>();  // Her Ýstege ( reguest ) ayri ayri neste uretir 







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
