public class CryptoPrice
{
    public long id { get; set; }
    public long assetId { get; set; }
    public string symbol { get; set; }
    public DateTime date { get; set; }
    public decimal open { get; set; }
    public decimal high { get; set; }
    public decimal low { get; set; }
    public decimal close { get; set; }
    public decimal adjClose { get; set; }
    public long volume { get; set; }
    public long unadjustedVolume { get; set; }
    public decimal change { get; set; }
    public decimal changePercent { get; set; }
    public float vwap { get; set; }
    public string label { get; set; }
    public float changeOverTime { get; set; }
    public string asOfDate { get; set; }
}
