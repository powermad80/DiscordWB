using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using YoutubeSearch;

namespace botscript.Modules
{
    public class InfoModule : InteractiveBase<SocketCommandContext>
    {

        [Command("info")]
        public async Task Info()
        {
            await Context.Channel.SendMessageAsync(@"
```
Commands:
!fortune - Draw a fortune
!8ball - shake the 8-ball
!register [waifu] [your gender (M/F)] [her/his gender (M/F)] - Register your waifu/husbando
!comfort - sweet thoughts
!lewd - dirty thoughts
!yt [search terms] - returns youtube search results, pick from top 5
```");
        }

        [Command("fortune"), Summary("Draw a fortune")]
        public async Task Draw(params string[] text)
        {
            await ReplyAsync(Fortune.fortune());
        }

        [Command("8ball"), Summary("Shake the 8-ball")]
        public async Task Shake(params string[] text)
        {
            await ReplyAsync(EightBall.Shake());
        }

        [Command("test"), Summary("test")]
        public async Task test()
        {
            IGuildUser user = Context.Message.Author as IGuildUser;
            await ReplyAsync(user.Nickname);
        }

        [Command("register"), Summary("Register waifu to user")]
        public async Task register(params string[] args/*string waifu, string gender, string waifugender*/)
        {
            if (args.Count() < 3)
            {
                await ReplyAsync("Incorrect syntax. Command format: !register [waifu] [your gender (M/F)] [waifu/husbando gender (M/F)]");
                return;
            }

            string waifu = args[0];
            string gender = args[1];
            string waifugender = args[2];

            if (gender != "M" && gender != "F" || waifugender != "M" && waifugender != "F")
            {
                await ReplyAsync("Incorrect syntax. Command format: !register [waifu] [your gender (M/F)] [waifu/husbando gender (M/F)]");
                return;
            }

            UserObj send = new UserObj();
            send.DiscordId = Convert.ToInt64(Context.User.Id);
            send.Waifu = waifu;
            send.Gender = gender;
            send.WaifuGender = waifugender;
            await ReplyAsync(waifuReg.Register(send));
        }

        [Command("comfort"), Summary("Sweet thoughts")]
        public async Task Load(params string[] text)
        {
            if (Context.Channel is IDMChannel)
            {
                SocketUser user = Context.Message.Author;
                await ReplyAsync(comfort.gush(Convert.ToInt64(user.Id), user.Username, waifuReg.getWife(Convert.ToInt64(user.Id)), "COMFORT"));
                return;
            }

            SocketGuildUser guildUser = Context.Message.Author as SocketGuildUser;
            string name = guildUser.Nickname;
            if (name == null)
                name = guildUser.Username;
            await ReplyAsync(comfort.gush(Convert.ToInt64(guildUser.Id), name, waifuReg.getWife(Convert.ToInt64(guildUser.Id)), "COMFORT"));
        }

        [Command("lewd"), Summary("Dirty thoughts")]
        public async Task Succ(params string[] text)
        {
            if (Context.Channel is IDMChannel)
            {
                SocketUser user = Context.Message.Author;
                await ReplyAsync(comfort.gush(Convert.ToInt64(user.Id), user.Username, waifuReg.getWife(Convert.ToInt64(user.Id)), "LEWD"));
                return;
            }

            SocketGuildUser guildUser = Context.Message.Author as SocketGuildUser;
            string name = guildUser.Nickname;
            if (name == null)
                name = guildUser.Username;
            var pm = await guildUser.GetOrCreateDMChannelAsync();
            await pm.SendMessageAsync(comfort.gush(Convert.ToInt64(guildUser.Id), name, waifuReg.getWife(Convert.ToInt64(guildUser.Id)), "LEWD"));
        }

        [Command("pest"), Summary("for testing things")]
        public async Task QuestForJesus(string one, string two, string three)
        {

            await ReplyAsync(Context.User.ToString());

        }

        [Command("yt", RunMode = RunMode.Async), Summary("Posts top 5 results of a youtube search, allowing the user to select one")]
        public async Task YTResults(params string[] text)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(10);
            var search = Context.Message.ToString().Substring(4);
            Criteria<string> b = new Criteria<string>();
            
            YoutubeSearchResults a = new YoutubeSearchResults();

            await ReplyAndDeleteAsync(a.YTSearch(search), timeout: timeout);
            var reply = await NextMessageAsync(timeout: timeout);
            if (reply == null)
            {
                await ReplyAndDeleteAsync("Command timed out.", timeout: TimeSpan.FromSeconds(5));
            }
            else if (reply.Content == "1" || reply.Content == "2" || reply.Content == "3" || reply.Content == "4" || reply.Content == "5")
            {
                int n = Int32.Parse(reply.Content);
                await ReplyAsync(a.ResultsList[n - 1].Url);
            }
            else
            {
                await ReplyAndDeleteAsync("Invalid response", timeout: TimeSpan.FromSeconds(5));
            }
            a = null;

        }
    }
}
