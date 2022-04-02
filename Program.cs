using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MinimalBackend;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters
    .Add(new JsonStringEnumConverter()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();
    await context.InsertSampleData();
}

app.UseSwagger();
app.ConfigureEndpoints();
app.UseSwaggerUI();
app.Run();