using back_end_for_TMS.Infrastructure.Api;
using back_end_for_TMS.Infrastructure.Business;
using back_end_for_TMS.Infrastructure.Database;
using back_end_for_TMS.Infrastructure.Security;
using back_end_for_TMS.Infrastructure.Tenancy;

// Register services to the DI container
// Infra (Database, Caching, Logging) > Security (AuthN, AuthZ) > Business > API

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseServices(builder.Configuration);

builder.Services.AddCorsServices(builder.Configuration);
builder.Services.AddAuthNServices(builder.Configuration);
builder.Services.AddAuthZServices(builder.Configuration);
builder.Services.AddTenancyServices(builder.Configuration);

builder.Services.AddBusinessServices(builder.Configuration);

builder.Services.AddApiServices(builder.Configuration);

// Configure the HTTP request pipeline / middleware
// Protect > Check > Run

var app = builder.Build();

app.UseExceptionHandler();

await app.CheckDatabaseConnection();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontEnd");
app.UseAuthentication();
app.UseTenantResolution();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Application is ready").AllowAnonymous();

app.Run();
