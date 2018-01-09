using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace botscript
{
    public class functions
    {

        //private CommandService commands;
        //private DiscordSocketClient client;
        //private IDependencyMap map;

        //public async Task Install(IDependencyMap _map)
        //{
        //    client = _map.Get<DiscordSocketClient>();
        //    commands = new CommandService();
        //    map = _map;

        //    await commands.AddModulesAsync(Assembly.GetEntryAssembly());

        //    client.MessageReceived += HandleCommand;

        //    client.UserLeft += UserLeft;
        //    client.UserJoined += UserJoined;
        //    client.JoinedGuild += JoinedServer;
        //}



        

        //public async Task HandleCommand(SocketMessage parameterMessage)
        //{
        //    var message = parameterMessage as SocketUserMessage;
        //    var thisguild = message.Channel as SocketGuildChannel;

        //    //await message.Channel.SendMessageAsync(message.Attachments.FirstOrDefault().Url);
        //    //if (url != null)
        //    //Program.DLoad(url, message.Channel.ToString(), (message.Channel as SocketGuildChannel).Guild.Name);

        //    Console.WriteLine(message.Content);
        //    if (message.Attachments.FirstOrDefault() != null && thisguild.Guild.Id != 230963929008832514)
        //       {
        //       Program.DLoad(message.Attachments.FirstOrDefault().Url, message.Channel);
        //       }

        //    int argPos = 0;
        //    if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix('!', ref argPos))) return;
        //    var context = new CommandContext(client, message);
        //    var result = await commands.ExecuteAsync(context, argPos, map);

        //    if (!result.IsSuccess && result.ErrorReason != "Unknown command.")
        //    {
        //        await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
        //    }
                
        //}
    }
}
