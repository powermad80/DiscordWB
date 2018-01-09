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
        public Task Info()
                => ReplyAsync("command successful");

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
        public async Task register(params string[] text)
        {
            string waifu = Context.Message.ToString().Substring(10);
            await Context.Channel.SendMessageAsync(waifuReg.Register(waifu, Context.User.ToString()));
        }

        [Command("comfort"), Summary("Sweet thoughts")]
        public async Task Load(params string[] text)
        {
            IGuildUser user = Context.Message.Author as IGuildUser;
            string name = user.Nickname;
            if (name == null)
                name = user.Username;

            await Context.Channel.SendMessageAsync(comfort.gush(name, waifuReg.getWife(user.ToString()), "comfort.txt"));
        }

        [Command("lewd"), Summary("Dirty thoughts")]
        public async Task Succ(params string[] text)
        {
            IGuildUser user = Context.Message.Author as IGuildUser;
            string name = user.Nickname;
            if (name == null)
                name = user.Username;
            var pm = await user.GetOrCreateDMChannelAsync();
            await pm.SendMessageAsync(comfort.gush(name, waifuReg.getWife(user.ToString()), "lewd.txt"));
        }

        [Command("pest"), Summary("for testing things")]
        public async Task QuestForJesus()
        {
            await Context.Channel.SendMessageAsync("no test loaded");
        }

        [Command("postnudes"), Summary("make Karen post nudes")]
        public async Task postNudes()
        {
            var channels = Context.Guild.TextChannels;
                foreach (var i in channels)
            {
                if (i.Name.ToLower().Contains("lewd"))
                    await i.SendMessageAsync(comfort.gush("nothing", "placeholder", "nudes.txt"));
            }
        }

        [Command("yt"), Summary("Posts first results of a youtube search for given terms")]
        public async Task yt(params string[] text)
        {
            var search = Context.Message.ToString().Substring(4);  
            await Context.Channel.SendMessageAsync(Program.YTSearch(search));
        }

        //[Command("color"), Summary("Generates role that colors someone's name")]
        //public async Task color(params string[] text)
        //{
        //    if (!(Context.Guild.Id == 237082695623114752))
        //        return;
        //    var userRoles = (Context.Message.Author as SocketGuildUser).Roles;
        //}
    }
}
