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


async Task main()
{
    var coinList = await getAllCoins();
    if (coinList != null)
    {
        foreach (Coin coin in coinList)
        {
            Console.WriteLine(coin);

            foreach (var kvp in coin.Platforms)
            {
                Console.WriteLine($"{kvp.Key} : {kvp.Value}");
            }
        }
    }
    else
    {
        Console.WriteLine("The Coin List is Empty!");
    }
}

await main();

class Coin
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
