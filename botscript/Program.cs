using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using botscript;
using botscript.Services;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Net;
using YoutubeSearch;
using System.Reflection;
using botscript.Modules;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

public class Program
{
    public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    private DiscordSocketClient _client;
    private IConfiguration _config;
    private CommandService commands;
    private IServiceProvider services;
    //private functions handler;

    public async Task MainAsync()
    {

        if (!File.Exists("data.sqlite"))
        {
            SQLiteConnection.CreateFile("data.sqlite");
            using (SQLiteConnection con = DataModules.DBConnection())
            {
                con.Open();
                new SQLiteCommand("CREATE TABLE USERS (Id INTEGER NOT NULL PRIMARY KEY, DiscordId INTEGER NOT NULL UNIQUE, Waifu varchar(100), Gender varchar(10), WaifuGender varchar(10))", con).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE COMFORT (Text varchar(2000), Type varchar(50)", con).ExecuteNonQuery();
                new SQLiteCommand("CREATE TABLE LEWD (Text varchar(2000), Type varchar(50)", con).ExecuteNonQuery();
                con.Close();
            }
        }

        string token;
        var filestream = new FileStream("token.txt", FileMode.Open, FileAccess.Read);
        using (var streamreader = new StreamReader(filestream, Encoding.UTF8))
        {
            token = streamreader.ReadLine();
        }

        _client = new DiscordSocketClient();
        _config = BuildConfig();
        commands = new CommandService();

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        services = new ServiceCollection()
                .BuildServiceProvider();

        await InstallCommands();
        await Task.Delay(-1);
    }

    public async Task InstallCommands()
    {
        // Hook the MessageReceived Event into our Command Handler
        _client.MessageReceived += HandleCommand;
        _client.UserLeft += UserLeft;
        _client.UserJoined += UserJoined;
        _client.JoinedGuild += JoinedServer;
        _client.UserBanned += BannedFromServer;
        // Discover all of the commands in this assembly and load them.
        await commands.AddModulesAsync(Assembly.GetEntryAssembly());
    }

    public async Task BannedFromServer(SocketUser user, SocketGuild guild)
    {
        var channels = guild.TextChannels;
        foreach (var i in channels)
        {
            if (i.Name == "server-log")
                await i.SendMessageAsync(("user has been banned from the server").Replace("user", user.Username));
        }
    }

    public async Task JoinedServer(SocketGuild server)
    {
        var pm = await server.Owner.GetOrCreateDMChannelAsync();
        await pm.SendMessageAsync("If you want me to log server activity (such as manual user leaves not tracked by Discord's Audit Log) please manually create a channel named #server-log");
    }

    public async Task UserJoined(SocketGuildUser user)
    {
        var channels = user.Guild.TextChannels;
        foreach (var i in channels)
        {
            if (i.Name == "server-log")
                await i.SendMessageAsync(("user has joined the server").Replace("user", user.Username));
        }
    }

    public async Task UserLeft(SocketGuildUser user)
    {
        var channels = user.Guild.TextChannels;
        foreach (var i in channels)
        {
            if (i.Name == "server-log")
                await i.SendMessageAsync(("user has left the server").Replace("user", user.Username));
        }
    }

    public async Task HandleCommand(SocketMessage messageParam)
    {
        // Don't process the command if it was a System Message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;
        // Determine if the message is a command, based on if it starts with '!' or a mention prefix
        if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
        {
            await Loop(messageParam);
            return;
        }
        // Create a Command Context
        var context = new SocketCommandContext(_client, message);
        // Execute the command. (result does not indicate a return value, 
        // rather an object stating if the command executed successfully)
        var result = await commands.ExecuteAsync(context, argPos, services);
        if (!result.IsSuccess & result.ErrorReason != "Unknown command.")
            await context.Channel.SendMessageAsync(result.ErrorReason);
    }

    private IServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            // Base
            .AddSingleton(_client)
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            // Logging
            .AddLogging()
            .AddSingleton<LogService>()
            // Extra
            .AddSingleton(_config)
            // Add additional services here...
            .BuildServiceProvider();
    }

    private IConfiguration BuildConfig()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("config.json")
            .Build();
    }


    public static void DLoad(string post, ISocketMessageChannel channel)
    {

        if (post.ToLower().Contains(".jpg") || post.ToLower().Contains(".png") || post.ToLower().Contains(".gif") || post.ToLower().Contains(".jpeg"))
        {

            string imgType = ".bmp";

            if (post.ToLower().Contains(".jpg"))
            {
                imgType = ".jpg";
            }
            if (post.ToLower().Contains(".png"))
            {
                imgType = ".png";
            }

            if (post.ToLower().Contains(".gif"))
            {
                imgType = ".gif";
            }

            if (post.ToLower().Contains(".jpeg"))
            {
                imgType = ".jpeg";
            }


            var server = (channel as SocketGuildChannel);
            System.Uri uri = new System.Uri(post);
            string imgPath = "pics/" + server.Guild.Id.ToString() + '/' + server.Guild.Name + '/' + channel.Name + '/';
            Directory.CreateDirectory(imgPath);
            WebClient webClient = new WebClient();
            string filename = DateTime.Now.ToString("yyyyMMddhhmmss");
            webClient.DownloadFileAsync(uri, imgPath + filename + imgType);
        }
    }

    public static string YTSearch(string search)
    {
        var results = new VideoSearch();
        var first = results.SearchQuery(search, 1)[0];
        return first.Url;
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.FromResult(true);
    }

    //public async Task LeftLog(SocketGuildUser user)
    //{
    //    var channels = user.Guild.TextChannels;
    //    foreach (var i in channels)
    //    {
    //        if (i.Name == "server-log")
    //            await i.SendMessageAsync(("user has left the server").Replace("user", user.Username));
    //    }
    //}

    public async Task Loop(SocketMessage e)
    {

        //_client = new DiscordSocketClient();
        System.Random random = new System.Random();

        string post = e.ToString();
        var thisguild = e.Channel as SocketGuildChannel;
        if (e.Attachments.FirstOrDefault() != null && thisguild.Guild.Id != 230963929008832514 && !e.Author.IsBot)
        {
            Program.DLoad(e.Attachments.FirstOrDefault().Url, e.Channel);
        }

        if (post.Length != 0 && !e.Author.IsBot)
        {

            Console.WriteLine(e.Content);

            if (Uri.IsWellFormedUriString(post, UriKind.Absolute) && thisguild.Guild.Id != 230963929008832514)
            {
                Program.DLoad(post, e.Channel);
            }




            if (post == "Welcome back, Karen")
                await e.Channel.SendMessageAsync("Ohayou!");

            if (post == "Karen a cute")
                await e.Channel.SendMessageAsync("``/////////////``");

            if (post.ToLower() == "k")
                await e.Channel.SendMessageAsync("Potassium");

            if (post == "FULLY")
                await e.Channel.SendMessageAsync("AUTOMATED");

            if (post == "bigthink")
                await e.Channel.SendMessageAsync("http://puu.sh/u5qtU.jpg");

            if (post == "$quit")
            {
                if (e.Author.ToString() == "Accel#4471")
                    System.Environment.Exit(0);
            }

            /*if (post == "test")
            {
                var serv = e.Channel as SocketGuildChannel;
                var channels = serv.Guild.TextChannels;
                foreach (ITextChannel i in channels)
                {
                    if (i.Name == "server-log")
                        await i.SendMessageAsync("log message goes here");
                }
            }*/

            if (post == "[X]")
                await e.Channel.SendMessageAsync("JASON!");

            if (post.Contains("in 2017 LUL"))
            {
                var guild = e.Channel as SocketGuildChannel;
                var lul = guild.Guild.Emotes;
                foreach (var i in lul)
                {
                    if (i.Name == "LUL")
                    {
                        string EmojiMessage = "<:emoji:replacethis>".Replace("replacethis", i.Id.ToString());
                        await e.Channel.SendMessageAsync(EmojiMessage);
                    }
                }
            }

            /*if (post.ToLower().Contains("post nudes karen"))
            {
                await 222932403348307968).SendMessage(comfort.gush(e.User, "placeholder", "nudes.txt"));
            }*/


            if (post == "Say hi, Karen")
                await e.Channel.SendMessageAsync("Hello!");

            /*if (post.Contains("in 2017 LUL"))
            {
                await e.Channel.SendMessageAsync("<:emoji:327277771812372493>");
            }*/

            if (post.Length >= 9)
            {

                if (post.ToLower().Substring(0, 9) == "goodnight" || (post.ToLower() + " ").Substring(0, 10) == "good night")
                    await e.Channel.SendMessageAsync("Goodnight!");
            }
        }
    }
}
