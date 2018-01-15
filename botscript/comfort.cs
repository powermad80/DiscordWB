using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using System.Data;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace botscript
{
    class comfort
    {
        public static string gush(string name, string waifu, string type)
        {
            if (waifu == "User's waifu not registered.")
                return "I don't know who your waifu is, baka.";
            Random random = new Random();
            string output;
            using (IDbConnection con = DataModules.DBConnection())
            {
                con.Open();
                List<string> lines = con.Query<string>("SELECT Text FROM" + type).ToList<string>();
                con.Close();
                output = lines[random.Next(0, lines.Count)];
            }
            output = output.Replace("user", name);
            return output.Replace("waifu", waifu);
        }
    }
}
