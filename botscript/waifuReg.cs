using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace botscript
{
    static class waifuReg
    {
        public static string Register(string text, string user)
        {
            bool replace = false;
            string lineToReplace = "";
            string entry = user + ' ' + text;
            var fileStream = new FileStream("register.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains(user))
                    {
                        replace = true;
                        lineToReplace = line;
                    }
                }
            }

            if (replace)
            {
                string allText = File.ReadAllText("register.txt");
                allText = allText.Replace(lineToReplace, entry);
                File.WriteAllText("register.txt", allText);
            }
            else
            {
                using (StreamWriter file =
                    new StreamWriter("register.txt", true))
                {
                    file.WriteLine(entry);
                }
            }
            return "Success";
        }

        public static string getWife(string entry)
        {
            var fileStream = new FileStream("register.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (entry.Length < line.Length)
                    {
                        if (entry == line.Substring(0, entry.Length))
                            return line.Substring(entry.Length + 1);
                    }
                }
                return "User's waifu not registered.";
            }
        }
        
    }
}
