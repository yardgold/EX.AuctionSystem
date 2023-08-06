using System;
using System.Threading.Tasks;
using AuctionApp.Common.Interface;
using AuctionApp.Common.Interfaces;

namespace AuctionApp.Common.Abstract
{
    public abstract class Agent
    {
        public abstract (bool, string) WantsToParticipate(ISellable sellableItem);
        public string Name { get; set; }
        public float Budget { get; set; }
        public bool PrefersAirConditioning { get; set; }
        public bool PrefersMainRoadAccess { get; set; }
        public bool PrefersDisabledAccessibility { get; set; }
        public int PreferredNumOfRooms { get; set; }
        public float PreferredRoomSize { get; set; }
        public int PreferredNumOfWC { get; set; }
        public bool HasPlacedBid { get; set; }
        public int BiddingRounds { get; set; }

        public virtual async Task Notify(float currentBid, IAuction auction)
        {
            Console.WriteLine($"\nAgent {Name} is notified about the current bid: {currentBid}");
            await Task.CompletedTask;
        }

        public virtual async Task PlaceBid(IAuction auction)
        {
            if (Budget > auction.CurrentBid)
            {
                float bidAmount = auction.CurrentBid + 10;
                Console.WriteLine($"Agent {Name} places a bid of {bidAmount}");
                auction.CurrentBid = bidAmount;
                HasPlacedBid = true;
            }
            else
            {
                Console.WriteLine($"Agent {Name} can't place a bid as it's over their budget.");
            }
            await Task.CompletedTask;
        }
    }
}