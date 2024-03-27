using BtcTurkApiCore.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BtcTurkApiCore.Extensions;

public static class SocketMessageExtension
{
    public static int? SocketMessageId(this string resultMessage)
    {
        if (string.IsNullOrWhiteSpace(resultMessage))
        {
            return null;
        }

        JArray resultArray = JArray.Parse(resultMessage);

        if (resultArray.Count == 0)
        {
            return 0;
        }

        return resultArray.First().Value<int>();
    }

    public static Task<T?> GetResponse<T>(this string resultMessage)
    {
        JArray resultArray = JArray.Parse(resultMessage);

        if (resultArray.Count == 0)
        {
            return Task.FromResult<T?>(default);
        }

        return Task.FromResult(resultArray.Last().ToObject<T>());
    }
    
    public static string JoinRequest(Channels channel, string? @event)
    {
        if (channel == Channels.OrderBook)
        {
            object[] joinMessage = 
            {
                151, new
                {
                    type = 151,
                    channel = "orderbook",
                    join = true,
                    @event = @event
                }
            };
            
            return JsonConvert.SerializeObject(joinMessage);
        }
        else if (channel == Channels.TickerPair)
        {
            object[] joinMessage = 
            {
                151, new
                {
                    type = 151,
                    channel = "ticker",
                    join = true,
                    @event = @event
                }
            };
            
            return JsonConvert.SerializeObject(joinMessage);
        }
        else if (channel == Channels.TickerAll)
        {
            object[] joinMessage = 
            {
                151, new
                {
                    type = 151,
                    channel = "ticker",
                    join = true,
                    @event = "all"
                }
            };
            
            return JsonConvert.SerializeObject(joinMessage);
        }
        else if (channel == Channels.TradeSingle)
        {
            object[] joinMessage = 
            {
                151, new
                {
                    type = 151,
                    channel = "trade",
                    join = true,
                    @event = @event
                }
            };
            
            return JsonConvert.SerializeObject(joinMessage);
        }
        else if (channel == Channels.TradingView)
        {
            object[] joinMessage = 
            {
                151, new
                {
                    type = 151,
                    channel = "tradingview",
                    join = true,
                    @event = @event
                }
            };
            
            return JsonConvert.SerializeObject(joinMessage);
        }
        else
        {
            throw new ArgumentException("Invalid channel type");
        }
    }
}