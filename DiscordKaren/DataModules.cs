using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using Discord.Addons.Interactive;

namespace botscript
{
    public static class DataModules
    {
        public static SQLiteConnection DBConnection()
        {
            return new SQLiteConnection("Data Source=data.sqlite;Version=3;");
        }
    }

    [Table("USERS")]
    public class UserObj
    {
        [Key]
        public int Id { get; set; }

        public long DiscordId { get; set; }

        public string Waifu { get; set; }

        public string Gender { get; set; }

        public string WaifuGender { get; set; }
    }

    [Table("COMFORT")]
    public class ComfortObj
    {
        public string Text { get; set; }

        public bool Yuri { get; set; }

        public bool Yaoi { get; set; }
    }
}
