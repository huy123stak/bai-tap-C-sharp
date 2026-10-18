namespace CryptoStockTicker;

public class PriceTick
{
    public DateTime Time { get; set; }
    public decimal Price { get; set; }
    public decimal Change { get; set; }
    public decimal Percent { get; set; }
    public string Direction { get; set; }
}
