using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;

namespace botscript
{
    class comfort
    {
        public static string gush(string name, string waifu, string type)
        {
            if (waifu == "User's waifu not registered.")
                return "I don't know who your waifu is, baka.";
            Random random = new Random();
            int choice = random.Next(1, 12);
            string output;
            var filestream = new FileStream(type, FileMode.Open, FileAccess.Read);
            using (var streamreader = new StreamReader(filestream, Encoding.UTF8))
            {
                string line;
                List<string> outlist = new List<string>();
                while ((line = streamreader.ReadLine()) != "endfile")
                {
                    outlist.Add(line);
                }

                output = outlist[random.Next(0, outlist.Count)];
                output = output.Replace("waifu", waifu);
                return output.Replace("user", name);
            }
        }
    }
}
