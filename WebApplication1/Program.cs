var builder = WebApplication.CreateBuilder(args);

// Добавляем YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Включаем проксирование
app.MapReverseProxy();

app.Run();