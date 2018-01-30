using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using System.Data;

namespace botscript
{
    static class waifuReg
    {
        public static string Register(UserObj data)
        {
            using (IDbConnection con = DataModules.DBConnection())
            {
                con.Open();

                List<int> Id = con.Query<int>("SELECT Id FROM USERS WHERE DiscordId = " + data.DiscordId).ToList<int>();

                if (Id.Count == 0)
                {
                    con.Insert<UserObj>(data);
                }

                else
                {
                    data.Id = Id[0];
                    con.Update<UserObj>(data);
                }

                con.Close();
                return "Success!";
            }
        }

        public static string getWife(long user)
        {
            string result = "User's waifu not registered.";
            using (IDbConnection con = DataModules.DBConnection())
            {
                con.Open();
                List<string> temp = con.Query<string>("SELECT WAIFU FROM USERS WHERE DiscordId = " + user).ToList<string>();
                if (temp.Count > 0)
                {
                    result = temp.First();
                }
                con.Close();
            }
            return result;

        }
        
    }
}
