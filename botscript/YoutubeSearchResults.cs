using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using YoutubeSearch;

namespace botscript
{
    class YoutubeSearchResults
    {
        public async Task YTSearch(ISocketMessageChannel channel, string search)
        {
            var results = new VideoSearch();
            var first = results.SearchQuery(search, 1);
            string searchresults = first[0].Url;

            for (int i = 1; i < 5; i++)
            {
                searchresults = searchresults + Environment.NewLine + first[i].Url;
            }
            
            await channel.SendMessageAsync(searchresults);
        }
    }
}
