using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task AdToGroupAsnyc(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task SendBidAsnyc(string groupName,string user,string bid)
        {
            await Clients.Group(groupName).SendAsync("Bids",user,bid);
        }

    }
}
