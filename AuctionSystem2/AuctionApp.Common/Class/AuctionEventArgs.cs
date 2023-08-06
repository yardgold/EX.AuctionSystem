using System;

namespace AuctionApp.Common
{
    public class AuctionEventArgs : EventArgs
    {
        public float CurrentBid { get; }
        public AuctionEventArgs(float currentBid)
        {
            CurrentBid = currentBid;
        }
    }
}
