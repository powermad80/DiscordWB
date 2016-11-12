using Discord;
using botscript;

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
            string post = e.Message.RawText;
            if (post == "Welcome Karen")
                await e.Channel.SendMessage("hey its me ur bot");

            if (post == "Karen a cute")
                await e.Channel.SendMessage("``/////////////``");

            if (e.Message.User.ToString() == "Accel#4471")
                if (post == "Karen!")
                    await e.Channel.SendMessage("*slaps Moldy*");

            if (post == "Say hi, Karen")
                await e.Channel.SendMessage("Hello!");

            if (post.ToLower() == "goodnight" || post.ToLower() == "good night")
                await e.Channel.SendMessage("Goodnight!");

            if (post == "#fortune")
            {
                Fortune F = new Fortune();
                await e.Channel.SendMessage(F.fortune());
            }

            if (post == "#8ball")
            {
                EightBall EB = new EightBall();
                await e.Channel.SendMessage(EB.Shake());
            }

        };
        _client.ExecuteAndWait(async () =>
        {
            await _client.Connect("MjQ2MzYwMjM2OTY3ODU0MDgw.CwZgbQ.kAr0zxOQ6qXRNbYPu62RUL5Q6oI", TokenType.Bot);
            System.Console.WriteLine("Connection successful");
        });
    }
}
