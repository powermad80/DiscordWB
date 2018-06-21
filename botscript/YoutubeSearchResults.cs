using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Addons.Interactive;
using YoutubeSearch;

namespace botscript
{
    class YoutubeSearchResults
    {
        public List<VideoInformation> ResultsList { get; set; }
        public string YTSearch(string search)
        {

            var results = new VideoSearch();
            ResultsList = results.SearchQuery(search, 1);
            string searchresults = "```1. " + ResultsList[0].Title + " " + ResultsList[0].Duration;

            for (int i = 1; i < 5; i++)
            {
                searchresults = searchresults + Environment.NewLine + (i + 1).ToString() + ". " + ResultsList[i].Title + " " + ResultsList[i].Duration;
            }

            searchresults = searchresults + "```";
            return searchresults;

        }
    }
}

