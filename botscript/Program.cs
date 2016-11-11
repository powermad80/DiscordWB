using Discord;

class Program
{
    static void Main(string[] args) => new Program().Start();
    private DiscordClient _client;

    public void Start()
    {
        _client = new DiscordClient();
        System.Random random = new System.Random();
        _client.MessageReceived += async (s, e) =>
        {
            if (e.Message.RawText == "Welcome Karen")
                await e.Channel.SendMessage("hey its me ur bot");
                System.Console.WriteLine(e.Message.User.ToString());
            if (e.Message.RawText == "Karen a cute")
                await e.Channel.SendMessage("``/////////////``");
            if (e.Message.User.ToString() == "Accel#4471")
                if (e.Message.RawText == "Karen!")
                    await e.Channel.SendMessage("*slaps Moldy*");
            if (e.Message.RawText == "Say hi, Karen")
                await e.Channel.SendMessage("Hello!");
            if (e.Message.RawText == "#fortune")
            {
                int fortune = random.Next(0, 101);
                if (fortune >= 95)
                    await e.Channel.SendMessage("[大吉] Great Blessing");
                else if (fortune >= 70)
                    await e.Channel.SendMessage("[中吉] Middle Blessing");
                else if (fortune >= 55)
                    await e.Channel.SendMessage("[吉] Blessing");
                else if (fortune >= 30)
                    await e.Channel.SendMessage("[小吉] Small Blessing");
                else if (fortune >= 17)
                    await e.Channel.SendMessage("[小凶] Small Curse");
                else if (fortune >= 8)
                    await e.Channel.SendMessage("[半凶] Half-curse");
                else if (fortune >= 3)
                    await e.Channel.SendMessage("[凶] Curse");
                else if (fortune >= 0)
                    await e.Channel.SendMessage("[大凶] Great Curse");
            }
        };
        _client.ExecuteAndWait(async () =>
        {
            await _client.Connect("MjQ2MzYwMjM2OTY3ODU0MDgw.CwZgbQ.kAr0zxOQ6qXRNbYPu62RUL5Q6oI", TokenType.Bot);
            System.Console.WriteLine("Connection successful");
        });
    }
}
