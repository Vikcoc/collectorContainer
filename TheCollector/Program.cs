using System.Data;
using System.Net.WebSockets;
using CryptoInterface.BackgroundService;
using CryptoInterface.CryptoCom;
using CryptoInterface.CryptoCom.ResponseHandlers;
using CryptoInterface.Saver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Npgsql;
using OWT.CryptoCom.Dto;
using OWT.SocketClient;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Add(new JsonConfigurationSource(){
    Path = "config.json"
});

builder.Services.AddScoped<IDbConnection>(db => {
    var theConnection = db.GetRequiredService<IConfiguration>()["connectionString"];
    return new NpgsqlConnection(theConnection);
    });

builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddTransient<ISocketClient, SocketClient>();
builder.Services.AddTransient<CryptoComMarketClient>();

builder.Services.AddScoped<HeartbeatHandler>();
builder.Services.AddScoped<TickerSaveHandlerPostgre>();
builder.Services.AddScoped<BulkInsertPostgre>();
builder.Services.AddSingleton<Queue<(CryptoComTickerData, DateTime)>>();

builder.Services.AddHostedService<CryptoComSimpleCollector>();
builder.Services.AddHostedService<CryptoComSaverService>();


using IHost host = builder.Build();

await host.RunAsync();
