using System.Threading.Tasks;

namespace AuctionApp.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AuctionSetup setup = new AuctionSetup();
            await setup.InitiateAuction();
        }
    }
}