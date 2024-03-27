# API and Websocket structure for BtcTurk API

<br />
<img width="25" align="left" alt="BtcTurk API Core" src="https://play-lh.googleusercontent.com/Oro51LB-uI56qErIMkUk4hO9ubB7O924ZiC7nSNyMyNeWmpxCvcXTFlEG6MbZTTMMpY" />
<br /><br />

[![GitHub stars](https://img.shields.io/github/stars/suleymanbyzt/BtcTurkApiCore.svg?color=blue)](https://github.com/suleymanbyzt/BtcTurkApiCore/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/suleymanbyzt/BtcTurkApiCore.svg?color=blue)](https://github.com/suleymanbyzt/BtcTurkApiCore/network)


<h2>Built With</h2>

![.NET 8](https://img.shields.io/badge/-.NET%208.0-blueviolet?style=for-the-badge&logoColor=white)

Please open [issues](https://github.com/suleymanbyzt/BtcTurkApiCore/issues) for your questions or bug reports.

# Examples

Firstly, you need to inject the BtcTurkApiCore library into your application. When injecting, you will be required to configure your PublicKey and PrivateKey.


```csharp
builder.Services.AddBtcTurkApiCore(conf =>
{
    conf.PublicKey = "YOUR_PUBLIC_KEY";
    conf.PrivateKey = "YOUR_PRIVATE_KEY";
});
```

Once the setup is complete, you can start utilizing the library in your integration.

Below are some examples you can use in your integration with the library.

Samples.cs
```csharp
     public Samples(
        IBtcTurkApiCore btcTurkApiCore,
        IBtcTurkPublicWebSocketClient btcTurkPublicWebSocketClient,
        IBtcTurkPrivateWebSocketClient btcTurkPrivateWebSocketClient)
    {
        _btcTurkApiCore = btcTurkApiCore;
        _btcTurkPublicWebSocketClient = btcTurkPublicWebSocketClient;
        _btcTurkPrivateWebSocketClient = btcTurkPrivateWebSocketClient;

        //OPTIONAL
        //Public WebSocket Events. These events are triggered when the data is received from the socket.
        _btcTurkPublicWebSocketClient.OnOrderBook += OnOrderBook;
        _btcTurkPublicWebSocketClient.OnTickerAll += OnTickerAll;
        _btcTurkPublicWebSocketClient.OnTickerPair += OnTickerPair;
        _btcTurkPublicWebSocketClient.OnTradeSingle += OnTradeSingle;
        
        //OPTINAL
        //Private WebSocket Events These events are triggered when the data is received from the socket.
        _btcTurkPrivateWebSocketClient.OnOrderInserted += OnOrderInserted;
        _btcTurkPrivateWebSocketClient.OnOrderMatched += OnOrderMatched;
        _btcTurkPrivateWebSocketClient.OnOrderDeleted += OnOrderDeleted;
        _btcTurkPrivateWebSocketClient.OnOrderUpdated += OnOrderUpdated;
        _btcTurkPrivateWebSocketClient.OnUserTrade += OnUserTrade;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Connect to private websocket
            await _btcTurkPrivateWebSocketClient.StartSocketClientAsync(new BtcTurkApiOptions()
            {
                PublicKey = "YOUR_PUBLIC_KEY",
                PrivateKey = "YOUR_PRIVATE_KEY"
            }, stoppingToken);
                

            // Connect to public websocket
            await _btcTurkPublicWebSocketClient.StartSocketClientAsync(stoppingToken);

            //Send subscription request to socket
            await _btcTurkPublicWebSocketClient.SendSubscriptionRequest(Channels.TickerAll, "all");
            await _btcTurkPublicWebSocketClient.SendSubscriptionRequest(Channels.TickerPair, "BTCUSDT");
            await _btcTurkPublicWebSocketClient.SendSubscriptionRequest(Channels.OrderBook, "BTCUSDT");
            await _btcTurkPublicWebSocketClient.SendSubscriptionRequest(Channels.TradeSingle, "BTCUSDT");
            
            // Send request to api
            OrderBook orderBook = await _btcTurkApiCore.GetOrderBookAsync("BTCTRY", 10);
            Ticker tickers = await _btcTurkApiCore.GetTickersAsync();
            ExchangeInfo exchangeInfo = await _btcTurkApiCore.GetExchangeInfoAsync();
            Trade trades = await _btcTurkApiCore.GetTradesAsync("BTCTRY");
            
            // Create order
            CreateOrderResponse createOrderResponse = await _btcTurkApiCore.CreateOrderRequestAsync(new CreateOrderRequest()
            {
                Quantity = "0.001",
                Price = "2000000",
                OrderMethod = OrderMethod.Limit,
                OrderType = OrderType.Buy,
                PairSymbol = "BTCTRY",
                NewOrderClientId = "BtcTurkApiCore"
            });
            
            // Cancel order
            CancelOrderResponse? cancelOrderResponse = await _btcTurkApiCore.CancelOrderRequestAsync(createOrderResponse.Data.Id.ToString());
        }
        catch (Exception e)
        {
            await _btcTurkPrivateWebSocketClient.StopSocketClientAsync();
            await _btcTurkPublicWebSocketClient.StopSocketClientAsync();
            
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private void OnUserTrade(UserTrade obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnOrderUpdated(OrderUpdated obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnOrderDeleted(OrderDeleted obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnOrderMatched(OrderMatched obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnOrderInserted(OrderInserted obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnTickerPair(TickerPair obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnTradeSingle(TradeSingle obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnTickerAll(TickerAll obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }

    private void OnOrderBook(OrderBookFull obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj));
    }
```