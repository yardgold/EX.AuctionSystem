using System;
using System.Linq;
using AuctionApp.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuctionApp.Common.Abstract;
using AuctionApp.Common.Interfaces;

namespace AuctionApp.BL.Class
{
    public class Auction : IAuction
    {
        public Asset Asset { get; set; }
        public List<Agent> RegisteredAgents { get; set; } = new List<Agent>();
        public float CurrentBid { get; set; }

        public event EventHandler<AuctionEventArgs> AuctionEvent;

        protected virtual async Task OnAuctionEvent(float bid)
        {
            if (AuctionEvent != null)
            {
                var handlers = AuctionEvent.GetInvocationList();
                var tasks = new List<Task>();

                foreach (var handler in handlers)
                {
                    var task = Task.Run(() => ((EventHandler<AuctionEventArgs>)handler)(this, new AuctionEventArgs(bid)));
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
            }
        }

        public async Task UpdateBid(float newBid)
        {
            CurrentBid = newBid;
            await OnAuctionEvent(newBid);
        }

        public void RegisterAgent(Agent agent)
        {
            RegisteredAgents.Add(agent);
        }

        public async Task Start()
        {
            await StartAutomaticBidding();
        }

        public async Task StartAutomaticBidding()
        {
            CurrentBid = Asset.StartPrice;
            var biddingRoundInterval = TimeSpan.FromSeconds(5);

            while (true)
            {
                if (RegisteredAgents.Count(agent => agent.Budget >= CurrentBid) == 1)
                {
                    await DisplayWinner(this, CurrentBid);
                    break;
                }
                await Task.Delay(1500);

                var notificationTasks = RegisteredAgents.Select(agent => agent.Notify(CurrentBid, this));
                await Task.WhenAll(notificationTasks);

                var bidTasks = RegisteredAgents.Select(agent => agent.PlaceBid(this));
                await Task.Delay(1000);
                await Task.WhenAll(bidTasks);

                Console.WriteLine($"\n========================================\nCurrent highest bid for {Asset.Name} is {CurrentBid}.\n========================================\n");
                Console.WriteLine("--------------------------------------------");

                bool allBidsPlaced = RegisteredAgents.All(agent => agent.HasPlacedBid);

                if (allBidsPlaced)
                {
                    bool shouldEndAuction = ShouldEndAuction();
                    if (shouldEndAuction)
                    {
                        await DisplayWinner(this, CurrentBid);
                        break;
                    }

                    foreach (var agent in RegisteredAgents)
                    {
                        agent.HasPlacedBid = false;
                        agent.BiddingRounds++;
                    }
                    await Task.Delay(biddingRoundInterval);
                }
            }
            await OnAuctionEvent(CurrentBid);
        }

        public async Task DisplayWinner(Auction auction, float currentBid)
        {
            var winner = RegisteredAgents.FirstOrDefault(agent => agent.Budget >= currentBid);
            if (winner != null)
            {
                Console.WriteLine($"\n=======================================================\nAgent {winner.Name} is the auction winner with a bid of {currentBid}.");
                Console.WriteLine("Congratulations!\n=======================================================");
            }
            else
            {
                Console.WriteLine("No agent can afford the current bid.");
                Console.WriteLine("The auction has ended with no winner.");
            }
            await Task.Delay(5000);
            Environment.Exit(0);
        }

        private bool ShouldEndAuction()
        {
            // simple logic to end the auction after a certain number of bidding rounds (5 for ex.)
            int maxBiddingRounds = 5;
            int completedRounds = RegisteredAgents.Select(agent => agent.BiddingRounds).Max();
            return completedRounds >= maxBiddingRounds;
        }
    }
}
