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

            //Func<SocketMessage, Task> ResponseAwaiter = null;
            //ResponseAwaiter = async (SocketMessage e) =>
            //{
            //    string post = e.ToString();
            //    if (e.Author.Id.ToString() == author)
            //    {
            //        if (post == "1" || post == "2" || post == "3" || post == "4" || post == "5")
            //        {
            //            await context.Channel.SendMessageAsync(first[Convert.ToInt32(post) - 1].Url);
            //            context.Client.MessageReceived -= ResponseAwaiter;
            //        }
            //        else if (post == "cancel")
            //        {
            //            context.Client.MessageReceived -= ResponseAwaiter;
            //        }
            //    }
            //};

            for (int i = 1; i < 5; i++)
            {
                searchresults = searchresults + Environment.NewLine + (i + 1).ToString() + ". " + first[i].Title + " " + first[i].Duration;
            }

            searchresults = searchresults + Environment.NewLine + "[if result not listed, say 'cancel']";

            await context.Channel.SendMessageAsync(searchresults);
            YoutubeReplyListener a = new YoutubeReplyListener(context, first);
            context.Client.MessageReceived += a.ListenForReply;
            await Task.Delay(TimeSpan.FromSeconds(20));

        }
    }

    class YoutubeReplyListener
    {
        public YoutubeReplyListener(SocketCommandContext Context, List<VideoInformation> First)
        {
            context = Context;
            first = First;
            TaskFinished = false;
        }

        private SocketCommandContext context { get; set; }
        private List<VideoInformation> first { get; set; }
        private bool TaskFinished { get; set; }
        public async Task ListenForReply(SocketMessage e)/*SocketCommandContext context, List<VideoInformation> first)*/
        {
            string post = e.ToString();
            if (e.Author.Id.ToString() == context.Message.Author.Id.ToString())
            {
                if (post == "1" || post == "2" || post == "3" || post == "4" || post == "5")
                {
                    await context.Channel.SendMessageAsync(first[Convert.ToInt32(post) - 1].Url);
                    TaskFinished = true;
                    return;
                }
                else if (post == "cancel")
                {
                    TaskFinished = true;
                    return;
                }
            }
        }

        public async Task StartListening()
        {
            context.Client.MessageReceived += this.ListenForReply;
            await Task.Delay(TimeSpan.FromSeconds(20));
        }
    }

}

