using Discord;

class Program
{
    static void Main(string[] args) => new Program().Start();
    private DiscordClient _client;

    public void Start()
    {
        _client = new DiscordClient();
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

        };
        _client.ExecuteAndWait(async () =>
        {
            await _client.Connect("MjQ2MzYwMjM2OTY3ODU0MDgw.CwZgbQ.kAr0zxOQ6qXRNbYPu62RUL5Q6oI", TokenType.Bot);
            System.Console.WriteLine("Connection successful");
        });
    }
}
