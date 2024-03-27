using System.Text;
using BtcTurkApiCore.Models.Api.Requests;
using BtcTurkApiCore.Models.Api.Responses;
using Newtonsoft.Json;

namespace BtcTurkApiCore.Client;

public class BtcTurkApiCore : IBtcTurkApiCore
{
    private readonly HttpClient _httpClient;

    public BtcTurkApiCore(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeInfo> GetExchangeInfoAsync()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/v2/server/exchangeinfo");
        return await DeserializeObject<ExchangeInfo>(request);
    }
    
    public async Task<Ticker> GetTickersAsync()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/v2/ticker");
        return await DeserializeObject<Ticker>(request);
    }
    
    public async Task<OrderBook> GetOrderBookAsync(string pairSymbol, int? limit)
    {
        List<string> query = new List<string>();

        if (!string.IsNullOrWhiteSpace(pairSymbol))
        {
            query.Add($"pairSymbol={pairSymbol}");
        }

        if (limit != null)
        {
            query.Add($"limit={limit}");
        }

        string queryString = string.Join("&", query);

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/v2/orderbook?{queryString}");
        return await DeserializeObject<OrderBook>(request);
    }
    
    public async Task<Trade> GetTradesAsync(string pairSymbol)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/v2/trades?pairSymbol={pairSymbol}");
        return await DeserializeObject<Trade>(request);
    }
    
    public async Task<OpenOrder> GetOpenOrdersAsync(string pairSymbol)
    {
        string query = null;

        if (!string.IsNullOrWhiteSpace(pairSymbol))
        {
            query = $"pairSymbol={pairSymbol}";
        }

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/openOrders?{query}");
        return await DeserializeObject<OpenOrder>(request);
    }
    
    public async Task<UserTransactionTrade> GetUserTransactionTradesAsync(long? orderId)
    {
        string query = null;

        if (orderId != null)
        {
            query = $"orderId={orderId}";
        }

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/users/transactions/trade?{query}");
        return await DeserializeObject<UserTransactionTrade>(request);
    }
    
    public async Task<UserTransactionTrade> GetUserTransactionTradesAsync(string pairSymbol, long? startDate, long? endDate)
    {
        List<string> query = new List<string>();

        if (!string.IsNullOrWhiteSpace(pairSymbol))
        {
            query.Add($"pairSymbol={pairSymbol}");
        }

        if (startDate != null && endDate != null)
        {
            query.Add($"startDate={startDate}");
            query.Add($"endDate={endDate}");
        }

        var queryString = string.Join("&", query);

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/users/transactions/trade?{queryString}");
        return await DeserializeObject<UserTransactionTrade>(request);
    }
    
    public async Task<UserBalance> GetBalancesAsync()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/v1/users/balances");
        return await DeserializeObject<UserBalance>(request);
    }

    public async Task<CreateOrderResponse> CreateOrderRequestAsync(CreateOrderRequest createOrderRequest)
    {
        string serialize = JsonConvert.SerializeObject(createOrderRequest);
        StringContent content = new StringContent(serialize, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage()
        {
            Content = content,
            Method = HttpMethod.Post,
            RequestUri = new Uri(_httpClient.BaseAddress + "api/v1/order")
        };

        return await DeserializeObject<CreateOrderResponse>(request);
    }

    public async Task<CancelOrderResponse?> CancelOrderRequestAsync(string id)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/order?id={id}");

        return await DeserializeObject<CancelOrderResponse>(request);
    }
    
    public async Task<List<Ohlc>> GetOhlc(QueryOhlcRequest request)
    {
        List<string> parameters = new List<string>();

        if (String.IsNullOrWhiteSpace(request.PairSymbol) || (request.From == null && request.To == null))
        {
            throw new ApplicationException("parameters cannot be null");
        }

        parameters.Add($"pair={request.PairSymbol}");
        parameters.Add($"from={request.From}");
        parameters.Add($"to={request.To}");

        string queryString = string.Join("&", parameters);
        
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/v1/ohlcs?{queryString}");
        return await DeserializeObject<List<Ohlc>>(requestMessage);
    }

    public async Task<Kline> GetKlines(QueryKlineRequest request)
    {
        List<string> parameters = new List<string>();

        if (String.IsNullOrWhiteSpace(request.PairSymbol) || (request.From == null && request.To == null) || request.Resolution == null)
        {
            throw new ApplicationException("parameters cannot be null");
        }

        parameters.Add($"symbol={request.PairSymbol}");
        parameters.Add($"from={request.From}");
        parameters.Add($"to={request.To}");
        parameters.Add($"resolution={request.Resolution}");

        string queryString = string.Join("&", parameters);
        
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"api/v1/klines/history?{queryString}");
        return await DeserializeObject<Kline>(requestMessage);
    }

    private async Task<T> DeserializeObject<T>(HttpRequestMessage request)
    {
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        string content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content);
    }
}