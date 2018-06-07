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
        public async Task YTSearch(SocketCommandContext context, string search)
        {

            var results = new VideoSearch();
            var first = results.SearchQuery(search, 1);
            string searchresults = "1. " + first[0].Title + " " + first[0].Duration;
            string author = context.Message.Author.Id.ToString();

            Func<SocketMessage, Task> ResponseAwaiter = null;
            ResponseAwaiter = async (SocketMessage e) =>
            {
                string post = e.ToString();
                if (e.Author.Id.ToString() == author)
                {
                    if (post == "1" || post == "2" || post == "3" || post == "4" || post == "5")
                    {
                        await context.Channel.SendMessageAsync(first[Convert.ToInt32(post) - 1].Url);
                        context.Client.MessageReceived -= ResponseAwaiter;
                    }
                }
            };

            for (int i = 1; i < 5; i++)
            {
                searchresults = searchresults + Environment.NewLine + (i + 1).ToString() + ". " + first[i].Title + " " + first[i].Duration;
            }

            await context.Channel.SendMessageAsync(searchresults);
            context.Client.MessageReceived += ResponseAwaiter;

        }

        //private async long YTAwaiter(SocketCommandContext Context)
        //{
        //    long response;
        //    Context.Client.MessageReceived += (SocketMessage e) =>
        //    {
        //        string post = e.ToString();
        //        if (e.Author.Id.ToString() == author)
        //        {
        //            if (post == "1" || post == "2" || post == "3" || post == "4" || post == "5")
        //            {
        //                response = Int64.Parse(post);
        //            }
        //        }
        //        return Task.CompletedTask;
        //    };
        //}
            

    }

    class YoutubeReplyListener
    {
        public YoutubeReplyListener(SocketCommandContext context)
        {
            Context = context;
        }

        public SocketCommandContext Context { get; set; }
    }

}

