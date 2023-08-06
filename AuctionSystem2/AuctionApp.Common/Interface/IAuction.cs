using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuctionApp.Common.Abstract;

namespace AuctionApp.Common.Interfaces
{
    public interface IAuction
    {
        Asset Asset { get; set; }
        List<Agent> RegisteredAgents { get; set; }
        Task Start();
        float CurrentBid { get; set; }

        event EventHandler<AuctionEventArgs> AuctionEvent;

        Task UpdateBid(float newBid);
        void RegisterAgent(Agent agent);
    }
}