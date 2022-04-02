using MinimalBackend;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();

app.ConfigureEndpoints();

app.UseSwaggerUI();
app.Run();