using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuctionApp.Common.Abstract;
using AuctionApp.Common.Interface;
using AuctionApp.Common.Interfaces;

namespace AuctionApp.Common.Class
{
    public class ConcreteAgent : Agent
    {
        public override async Task Notify(float currentBid, IAuction auction)
        {
            Console.WriteLine($"Agent {Name} is notified about the current bid: {currentBid}");
            await Task.CompletedTask;
        }

        public override async Task PlaceBid(IAuction auction)
        {
            if (Budget > auction.CurrentBid)
            {
                float bidAmount = auction.CurrentBid + 10;
                Console.WriteLine($"Agent {Name} places a bid of {bidAmount}");
                auction.CurrentBid = bidAmount;
            }
            else
            {
                Console.WriteLine($"Agent {Name} can't place a bid as it's over their budget.");
            }
            await Task.Delay(1000); // simulate time for the agent to place an asynchronously bid 
        }

        public override (bool, string) WantsToParticipate(ISellable sellableItem)
        {
            if (!(sellableItem is Asset asset))
            {
                return (false, "Item is not an Asset.");
            }

            var reasons = new List<string>();

            if (PrefersAirConditioning && !asset.HasAirConditioning)
                reasons.Add("it does not have Air Conditioning");

            if (PrefersMainRoadAccess && !asset.HasMainRoadAccess)
                reasons.Add("it does not have Main Road Access");

            if (PrefersDisabledAccessibility && !asset.HasDisabledAccessibility)
                reasons.Add("it does not have Disabled Accessibility");

            if (PreferredNumOfRooms > 0 && asset.NumOfRooms < PreferredNumOfRooms)
                reasons.Add($"it has {asset.NumOfRooms} rooms, while {PreferredNumOfRooms} were preferred");

            if (Math.Abs(PreferredRoomSize) > 0.1 && asset.RoomSize < PreferredRoomSize)
                reasons.Add($"its room size is {asset.RoomSize} sqm, while {PreferredRoomSize} sqm were preferred");

            if (PreferredNumOfWC > 0 && asset.NumOfWC < PreferredNumOfWC)
                reasons.Add($"it has {asset.NumOfWC} WCs, while {PreferredNumOfWC} were preferred");


            if (reasons.Any())
                return (false, string.Join(",\nand ", reasons));
            else
                return (true, "Agent registered successfully.");
        }
    }
}