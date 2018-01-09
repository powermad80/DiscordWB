using System;

namespace botscript
{
    static class Fortune
    {
        public static string fortune()
        {
            System.Random random = new Random();
            int fortune = random.Next(0, 101);
            if (fortune >= 95)
                return "[大吉] Great Blessing";
            else if (fortune >= 70)
                return "[中吉] Middle Blessing";
            else if (fortune >= 55)
                return "[吉] Blessing";
            else if (fortune >= 30)
                return "[小吉] Small Blessing";
            else if (fortune >= 17)
                return "[小凶] Small Curse";
            else if (fortune >= 8)
                return "[半凶] Half-curse";
            else if (fortune >= 3)
                return "[凶] Curse";
            else
                return "[大凶] Great Curse";
        }
    }
}
