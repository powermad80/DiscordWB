using System;

namespace botscript
{
    class EightBall
    {
        public string Shake()
        {
            Random random = new Random();
            int result = random.Next(0, 12);
            switch (result)
            {
                case 0:
                    return "Ara ara~";
                case 1:
                    return "It is certain.";
                case 2:
                    return "Without a doubt.";
                case 3:
                    return "You may rely on it.";
                case 4:
                    return "Most likely.";
                case 5:
                    return "Reply hazy, try again later.";
                case 6:
                    return "Better not tell you now.";
                case 7:
                    return "Cannot predict now.";
                case 8:
                    return "Don't count on it.";
                case 9:
                    return "My sources say no.";
                case 10:
                    return "Outlook not so good.";
                case 11:
                    return "Very doubtful.";
                default:
                    return "error";

            }
        }
    }
}