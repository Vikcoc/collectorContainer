using System.Data;
using System.Net.WebSockets;
using CryptoInterface.BackgroundService;
using CryptoInterface.CryptoCom;
using CryptoInterface.CryptoCom.Deciders;
using CryptoInterface.CryptoCom.ResponseHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OWT.SocketClient;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var connectionString = "";
builder.Services.AddScoped<IDbConnection>(db =>
    new NpgsqlConnection(connectionString));

builder.Services.AddTransient<ClientWebSocket>();
builder.Services.AddTransient<ISocketClient, SocketClient>();
builder.Services.AddTransient<CryptoComMarketClient>();

builder.Services.AddScoped<HeartbeatHandler>();
builder.Services.AddScoped<TickerSaveHandlerPostgre>();

builder.Services.AddScoped(s =>
{
    var res = new CryptoComMarketDtoDecider();
    res.AddHandler(s.GetRequiredService<TickerSaveHandlerPostgre>());
    return res;
});


builder.Services.AddHostedService<CryptoComDataCollector>();


using IHost host = builder.Build();

await host.RunAsync();
