using System;
using AuctionApp.BL.Class;
using System.Threading.Tasks;
using AuctionApp.Common.Class;
using System.Collections.Generic;

namespace AuctionApp.UI
{
    public class AuctionSetup
    {
        private Auction _auction;

        public async Task InitiateAuction()
        {
            _auction = new Auction();

            Console.WriteLine("==== Auction System ====\n");

            SetupAssetForAuction();
            SetupAgentsForAuction();

            await _auction.Start();
        }

        private void SetupAssetForAuction()
        {
            Console.WriteLine("Setting up asset for the auction:\n");

            ConcreteAsset asset = new ConcreteAsset();

            Console.WriteLine("Enter asset name:");
            asset.Name = Console.ReadLine();

            Console.WriteLine("Enter asset start price:");
            asset.StartPrice = float.Parse(Console.ReadLine());

            Console.WriteLine("Does the asset have air conditioning? (yes/no):");
            asset.HasAirConditioning = Console.ReadLine().ToLower() == "yes";

            Console.WriteLine("Is the asset accessible from the main road? (yes/no):");
            asset.HasMainRoadAccess = Console.ReadLine().ToLower() == "yes";

            Console.WriteLine("Is the asset accessible for disabled individuals? (yes/no):");
            asset.HasDisabledAccessibility = Console.ReadLine().ToLower() == "yes";

            Console.WriteLine("Enter the number of rooms:");
            asset.NumOfRooms = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter average room size:");
            asset.RoomSize = float.Parse(Console.ReadLine());

            Console.WriteLine("Enter number of WC:");
            asset.NumOfWC = int.Parse(Console.ReadLine());

            _auction.Asset = asset;

            Console.Clear();
        }

        private void SetupAgentsForAuction()
        {
            Console.WriteLine("Setting up agents for the auction:\n");
            
            Console.WriteLine("Enter number of agents:");
            int numOfAgents = int.Parse(Console.ReadLine());

            for (int i = 0; i < numOfAgents; i++)
            {
                Console.Clear();
                Console.WriteLine($"Setting up agent {i + 1}:\n");

                ConcreteAgent agent = new ConcreteAgent();

                Console.WriteLine("Enter agent name:");
                agent.Name = Console.ReadLine();

                Console.WriteLine("Enter agent budget:");
                agent.Budget = float.Parse(Console.ReadLine());

                Console.WriteLine("Does the agent prefer air conditioning? (yes/no):");
                agent.PrefersAirConditioning = Console.ReadLine().ToLower() == "yes";

                Console.WriteLine("Does the agent prefer main road access? (yes/no):");
                agent.PrefersMainRoadAccess = Console.ReadLine().ToLower() == "yes";

                Console.WriteLine("Does the agent prefer disabled accessibility? (yes/no):");
                agent.PrefersDisabledAccessibility = Console.ReadLine().ToLower() == "yes";

                Console.WriteLine("Enter the agent's preferred number of rooms:");
                agent.PreferredNumOfRooms = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter the agent's preferred room size:");
                agent.PreferredRoomSize = float.Parse(Console.ReadLine());

                Console.WriteLine("Enter the agent's preferred number of WC:");
                agent.PreferredNumOfWC = int.Parse(Console.ReadLine());

                var (shouldRegister, reason) = agent.WantsToParticipate(_auction.Asset);
                if (shouldRegister)
                {
                    _auction.RegisterAgent(agent);
                    Console.WriteLine($"{agent.Name} registered successfully for the auction.");
                }
                else
                {
                    Console.WriteLine($"\n\n{agent.Name} did not register for the auction because:");
                    Console.WriteLine(reason);
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            Console.Clear();
        }

        public static void RegisterAgentsToAuction(Auction auction, List<ConcreteAgent> agents)
        {   
            foreach (var agent in agents)
            {
                var (shouldRegister, reason) = agent.WantsToParticipate(auction.Asset);
                if (shouldRegister)
                {
                    auction.RegisterAgent(agent);
                    Console.WriteLine($"{agent.Name} registered for the auction.");
                }
                else
                {
                    Console.WriteLine($"{agent.Name} did not registered for the auction because the asset {reason}.");
                }
            }
        }

    }
}