using AuctionApp.Common.Interface;

namespace AuctionApp.Common.Abstract
{
    public abstract class Asset : ISellable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float StartPrice { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasMainRoadAccess { get; set; }
        public bool HasDisabledAccessibility { get; set; }
        public int NumOfRooms { get; set; }
        public float RoomSize { get; set; }
        public int NumOfWC { get; set; }
    }
}