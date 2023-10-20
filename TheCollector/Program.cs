using System.Data;
using System.Net.WebSockets;
using CryptoInterface.BackgroundService;
using CryptoInterface.CryptoCom;
using CryptoInterface.CryptoCom.ResponseHandlers;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Npgsql;
using OWT.SocketClient;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Add(new JsonConfigurationSource(){
    Path = "config.json"
});

var connectionString = builder.Configuration["connectionString"];
builder.Services.AddScoped<IDbConnection>(db =>
    new NpgsqlConnection(connectionString));

builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddTransient<ISocketClient, SocketClient>();
builder.Services.AddTransient<CryptoComMarketClient>();

builder.Services.AddScoped<HeartbeatHandler>();
builder.Services.AddScoped<TickerSaveHandlerPostgre>();
builder.Services.AddSingleton<Queue<JObject>>();

builder.Services.AddHostedService<CryptoComSimpleCollector>();


using IHost host = builder.Build();

await host.RunAsync();
