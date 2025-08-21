using Microsoft.EntityFrameworkCore;
using myFlightPerformance;
using myFlightPerformance.Models;
using myFlightPerformance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<ObservationBulkInsertService>();
builder.Services.AddScoped<MalfunctionBulkInsertService>();
builder.Services.AddScoped<OtherBulkInsertService>();
builder.Services.AddScoped<ProcedureBulkInserService>();
builder.Services.AddScoped<ISegInputService, SegInputService>();
builder.Services.AddScoped<ISegEditDeleteService, SegEditDeleteService>();
builder.Services.AddScoped<IFormInputService, FormInputService>();
builder.Services.AddScoped<IFormReportDeleteService, FormReportDeleteService>();

var app = builder.Build();

app.UseCors("AllowAll");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// For offline/EXE use, comment out HTTPS redirection if you have no certificate
// app.UseHttpsRedirection();

app.UseDefaultFiles(); // Serves index.html by default
app.UseStaticFiles();  // Serves static Angular files

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Fallback to index.html for Angular routing (deep links)
app.MapFallbackToFile("index.html");

app.Run();