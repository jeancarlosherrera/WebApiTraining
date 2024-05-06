using FluentValidation;
using IssueTracker.Api.Catalog;
using Marten;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsSoftwareAdmin", policy =>
    {
        policy.RequireRole("SoftwareCenter");

    });
});

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IValidator<CreateCatalogItemRequest>, CreateCatalogItemRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCatalogItemRequestValidator>();

var connectionString = builder.Configuration.GetConnectionString("data") ?? throw new Exception("Can't start, need a connection string");
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
}).UseLightweightSessions();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // This adds the ability to get the OpenAPI Spec through the API at https://localhost:1338/swagger/v1/swagger.json
    app.UseSwaggerUI(); // This adds "SwaggerUI" - a web application that reads that OpenAPI spec above and puts a pretty UI on it.
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); // come back to this.

app.MapControllers(); // create the call sheet. 

app.Run(); // start the process and block here waiting for requests.

public partial class Program { }