using System.Net.Http.Headers;
using BtcTurkApiCore.Client;
using BtcTurkApiCore.Handlers;
using BtcTurkApiCore.Models.Options;
using BtcTurkApiCore.Websocket;
using BtcTurkApiCore.Websocket.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BtcTurkApiCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBtcTurkApiCore(this IServiceCollection services, Action<BtcTurkApiOptions> configureOptions)
        {
            BtcTurkApiOptions? options = new BtcTurkApiOptions();
            configureOptions(options);

            if (options == null || string.IsNullOrEmpty(options.PublicKey) || string.IsNullOrEmpty(options.PrivateKey))
            {
                throw new ArgumentNullException(nameof(options), "BtcTurkApiOptions is required. Please provide PublicKey and PrivateKey.");
            }

            services.AddHttpClient<IBtcTurkApiCore, Client.BtcTurkApiCore>(client =>
                {
                    client.BaseAddress = new Uri("https://api.btcturk.com");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddHttpMessageHandler(_ => new AuthenticatedHttpClientHandler(options.PublicKey, options.PrivateKey, options.Nonce));

            services.AddSingleton<IBtcTurkPublicWebSocketClient, BtcTurkPublicWebsocketClient>();
            services.AddSingleton<IBtcTurkPrivateWebSocketClient, BtcTurkPrivateWebsocketClient>();

            return services;
        }
    }
}