using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add support for multiple content types
    c.OperationFilter<SwaggerContentTypeOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Custom Operation Filter for Content Types
public class SwaggerContentTypeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var contentTypes = new List<string>
        {
            "application/json",
            "application/xml",
            "application/yaml",
            "application/x-yaml",
            "text/csv"
        };

        if (operation.Responses.ContainsKey("200"))
        {
            var response = operation.Responses["200"];
            response.Content = contentTypes.ToDictionary(
                contentType => contentType,
                contentType => new OpenApiMediaType()
            );
        }
    }
}