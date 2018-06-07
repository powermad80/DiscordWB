using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using YoutubeSearch;

namespace botscript.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
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
!yt [search terms] - returns first search result on youtube
```");
        }

        [Command("fortune"), Summary("Draw a fortune")]
        public async Task Draw(params string[] text)
        {
            await Context.Channel.SendMessageAsync(Fortune.fortune());
        }

        [Command("8ball"), Summary("Shake the 8-ball")]
        public async Task Shake(params string[] text)
        {
            await Context.Channel.SendMessageAsync(EightBall.Shake());
        }

        [Command("test"), Summary("test")]
        public async Task test()
        {
            IGuildUser user = Context.Message.Author as IGuildUser;
            await Context.Channel.SendMessageAsync(user.Nickname);
        }

        [Command("register"), Summary("Register waifu to user")]
        public async Task register(string waifu, string gender, string waifugender)
        {

            if (gender != "M" && gender != "F" || waifugender != "M" && waifugender != "F")
            {
                await Context.Channel.SendMessageAsync("Incorrect syntax. Command format: !register [waifu] [your gender (M/F)] [waifu/husbando gender (M/F)]");
                return;
            }

            UserObj send = new UserObj();
            send.DiscordId = Convert.ToInt64(Context.User.Id);
            send.Waifu = waifu;
            send.Gender = gender;
            send.WaifuGender = waifugender;
            await Context.Channel.SendMessageAsync(waifuReg.Register(send));
        }

        [Command("comfort"), Summary("Sweet thoughts")]
        public async Task Load(params string[] text)
        {
            if (Context.Channel is IDMChannel)
            {
                SocketUser user = Context.Message.Author;
                await Context.Channel.SendMessageAsync(comfort.gush(Convert.ToInt64(user.Id), user.Username, waifuReg.getWife(Convert.ToInt64(user.Id)), "COMFORT"));
                return;
            }

            SocketGuildUser guildUser = Context.Message.Author as SocketGuildUser;
            string name = guildUser.Nickname;
            if (name == null)
                name = guildUser.Username;
            await Context.Channel.SendMessageAsync(comfort.gush(Convert.ToInt64(guildUser.Id), name, waifuReg.getWife(Convert.ToInt64(guildUser.Id)), "COMFORT"));
        }

        [Command("lewd"), Summary("Dirty thoughts")]
        public async Task Succ(params string[] text)
        {
            if (Context.Channel is IDMChannel)
            {
                SocketUser user = Context.Message.Author;
                await Context.Channel.SendMessageAsync(comfort.gush(Convert.ToInt64(user.Id), user.Username, waifuReg.getWife(Convert.ToInt64(user.Id)), "LEWD"));
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

            await Context.Channel.SendMessageAsync(Context.User.ToString());

        }

        [Command("yt"), Summary("Posts first results of a youtube search for given terms")]
        public async Task yt(params string[] text)
        {
            var search = Context.Message.ToString().Substring(4);  
            await Context.Channel.SendMessageAsync(Program.YTSearch(search));
        }

        [Command("ytResults"), Summary("Posts top 5 results of a youtube search, allowing the user to select one")]
        public async Task YTResults(params string[] text)
        {
            var search = Context.Message.ToString().Substring(4);
            YoutubeSearchResults a = new YoutubeSearchResults();

            var task = Task.Run(() => a.YTSearch(Context, search));

            if (task.Wait(TimeSpan.FromSeconds(15)))
                return;
            else
            {
                await Context.Channel.SendMessageAsync("Search selection timed out");
                a = null;
            }

        }
    }
}
