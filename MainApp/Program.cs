using System.Dynamic;
using Newtonsoft.Json;
//using System.Text.Json;


async Task<List<Coin>> getAllCoins()
{
    using HttpClient client = new HttpClient();  // Instantiate a HTTP Client
    try
    {
        HttpResponseMessage response = await client.GetAsync("https://api.coingecko.com/api/v3/coins/list?include_platform=true");

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Coin>>(responseData);
            //return JsonSerializer.Deserialize<List<Coin>>(responseData);
        }
        else
        {
            throw new Exception("No Response!");
        }
    }
    catch (Exception err)
    {
        Console.WriteLine(err);
        throw new Exception("No Response!");
    }
}


async Task<DynamicBase> getCoinMarketData(string Id)
{
    using HttpClient client = new HttpClient();  // Instantiate a HTTP Client
    try
    {
        HttpResponseMessage response = await client.GetAsync("https://api.coingecko.com/api/v3/coins/binance-bitcoin?localization=true&tickers=false&market_data=true&community_data=true&developer_data=true&sparkline=false");

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DynamicBase>(responseData);
        }
        else
        {
            throw new Exception("No Response!");
        }
    }
    catch (Exception err)
    {
        Console.WriteLine(err);
        throw new Exception("No Response!");
    }
}


async Task main()
{
    List<Coin> coinList = await getAllCoins();

    dynamic binanceBitcoinMarketData = await getCoinMarketData("binance-bitcoin");

    try
    {
        Console.WriteLine(binanceBitcoinMarketData.web_slug);
        Console.WriteLine(binanceBitcoinMarketData.symbol);
        Console.WriteLine(binanceBitcoinMarketData.description);
    }
    catch (Exception err)
    {
        Console.WriteLine($"{err.Message}");
    }


    if (coinList != null)
    {
        //foreach (Coin coin in coinList)
        //{
        //    Console.WriteLine(coin);

        //    foreach (var kvp in coin.Platforms)
        //    {
        //        Console.WriteLine($"{kvp.Key} : {kvp.Value}");
        //    }
        //}
    }
    else
    {
        Console.WriteLine("The Coin List is Empty!");
    }
}

await main();


public class DynamicBase : DynamicObject
{
    //dynamic dynamicObject = new CustomDynamicObject();
    //dynamicObject.FirstName = "John";
    //dynamicObject.LastName = "Doe";
    //// Accessing dynamic properties
    //Console.WriteLine(dynamicObject.FirstName); // Outputs: John
    //Console.WriteLine(dynamicObject.LastName); // Outputs: Doe

    private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        return _properties.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        _properties[binder.Name] = value;
        return true;
    }
}


class MarketData : DynamicBase
{
}


class Coin : DynamicBase
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }

    public Dictionary<string, string> Platforms { get; set; }

    public override string ToString()
    {
        return $"ID={ID} | Name={Name} | Symbol={Symbol}";
    }
}
