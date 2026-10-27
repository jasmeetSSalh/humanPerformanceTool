using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myFlightPerformance;
using myFlightPerformance.Models;
using myFlightPerformance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Instructors sign in with a cookie. The Angular app is served from this same
// origin (or through the dev server proxy), so the cookie can be strict and HTTP-only.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "PilotAssess.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        // This is an API, so answer with status codes instead of redirecting to a login page.
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

// Every endpoint requires a signed-in instructor unless it is marked [AllowAnonymous].
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Limit login attempts to 5 per minute per IP address.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("login", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1) }));
});

builder.Services.AddScoped<IPasswordHasher<Instructor>, PasswordHasher<Instructor>>();

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

await PrepareDatabaseAsync(app);

// Swagger is only exposed while developing.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// For offline/EXE use, comment out HTTPS redirection if you have no certificate
// app.UseHttpsRedirection();

app.UseDefaultFiles(); // Serves index.html by default
app.UseStaticFiles();  // Serves static Angular files

app.UseRouting();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Fallback to index.html for Angular routing (deep links). The page itself is
// public so the login screen can load; the API behind it is not.
app.MapFallbackToFile("index.html").AllowAnonymous();

app.Run();

// Applies pending migrations, then creates the first instructor account from
// configuration (InitialInstructor:Username and InitialInstructor:Password)
// if there are no accounts yet.
static async Task PrepareDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await context.Database.MigrateAsync();

    if (await context.Instructors.AnyAsync())
    {
        return;
    }

    var username = app.Configuration["InitialInstructor:Username"]?.Trim().ToLower();
    var password = app.Configuration["InitialInstructor:Password"];

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
        logger.LogWarning(
            "No instructor accounts exist. Set InitialInstructor__Username and InitialInstructor__Password to create the first one.");
        return;
    }

    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Instructor>>();
    var instructor = new Instructor { Username = username, PasswordHash = "", CreatedAt = DateTime.UtcNow };
    instructor.PasswordHash = hasher.HashPassword(instructor, password);

    context.Instructors.Add(instructor);
    await context.SaveChangesAsync();

    logger.LogInformation("Created the first instructor account: {Username}", username);
}
