using Microsoft.EntityFrameworkCore;
using myFlightPerformance;
using myFlightPerformance.Models;
using myFlightPerformance.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure the SQLite database connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Register your bulk insert service
builder.Services.AddScoped<ObservationBulkInsertService>();
builder.Services.AddScoped<MalfunctionBulkInsertService>();
builder.Services.AddScoped<OtherBulkInsertService>();
builder.Services.AddScoped<ProcedureBulkInserService>();
builder.Services.AddScoped<ISegInputService, SegInputService>();
builder.Services.AddScoped<ISegEditDeleteService, SegEditDeleteService>();
builder.Services.AddScoped<IFormInputService, FormInputService>();
builder.Services.AddScoped<IFormReportDeleteService, FormReportDeleteService>();


var app = builder.Build();

// Apply the CORS policy
app.UseCors("AllowAll");


//Uncomment to reset the database
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    
    // I only want to rest a specific table
    // dbContext.Database.ExecuteSqlRaw("DELETE FROM ObservedEvents");
    // dbContext.Database.ExecuteSqlRaw("DELETE FROM ObservedGrades");
    // dbContext.Database.ExecuteSqlRaw("DELETE FROM ObservedGradesInstructorCritiques");

    // Uncomment the following lines to reset the entire database

    // Step 1: Drop all tables (delete all data)
    // dbContext.Database.EnsureDeleted();

    // Step 2: Recreate all tables by applying migrations
    // dbContext.Database.Migrate(); 
// }

// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }

app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


