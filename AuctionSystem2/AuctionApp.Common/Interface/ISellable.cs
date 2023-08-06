namespace AuctionApp.Common.Interface
{
    public interface ISellable
    {
        string Name { get; set; }
        string Description { get; set; }
        float StartPrice { get; set; }
    }
}