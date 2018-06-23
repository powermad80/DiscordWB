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
        public static string gush(long id, string name, string waifu, string type)
        {
            if (waifu == "User's waifu not registered.")
                return "I don't know who your waifu is, baka.";
            Random random = new Random();
            string output;
            UserObj user;
            using (IDbConnection con = DataModules.DBConnection())
            {
                con.Open();
                List<string> lines = con.Query<string>("SELECT Text FROM " + type).ToList<string>();
                user = con.Query<UserObj>("SELECT * FROM USERS WHERE DiscordId = " + id).ToList().First();
                con.Close();
                output = lines[random.Next(0, lines.Count)];
            }

            return pronounFiller(output.Replace("user", name).Replace("waifu", waifu), user.Gender, user.WaifuGender);
        }

        private static string pronounFiller(string response, string gender, string waifugender)
        {
            List<string> Filler = new List<string>();
            List<string> Replacer = new List<string>();

            Filler.Add("Uthey");
            Filler.Add("Uthem");
            Filler.Add("Utheir");
            Filler.Add("Wthey");
            Filler.Add("Wthem");
            Filler.Add("Wtheir");

            switch (gender)
            {
                case "M":
                    Replacer.Add("he");
                    Replacer.Add("him");
                    Replacer.Add("his");
                    break;

                case "F":
                    Replacer.Add("she");
                    Replacer.Add("her");
                    Replacer.Add("her");
                    break;
            }

            switch (waifugender)
            {
                case "M":
                    Replacer.Add("he");
                    Replacer.Add("him");
                    Replacer.Add("his");
                    break;

                case "F":
                    Replacer.Add("she");
                    Replacer.Add("her");
                    Replacer.Add("her");
                    break;
            }

            for (int i = 0; i < Replacer.Count; i++)
            {
                response = response.Replace(Filler[i], Replacer[i]);
            }

            return response;
        }
    }
}
