namespace ConsoleApplication3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Program
    {
        private static Dictionary<string, string> colors;

        static string Method(Match match)
        {
            var s = match.Value.ToLower();
            string replace = string.Empty;
            if (s[0] == '#')
            {
                if (s.Length == 7)
                {
                    return colors.ContainsKey(s) ? colors[s] : s;
                }
                else
                {                    
                    replace += s[0].ToString() + s[1].ToString() + s[1].ToString() + s[2].ToString() + s[2].ToString() + s[3].ToString() + s[3].ToString();
                    return colors.ContainsKey(replace) ? colors[replace] : s;
                }
            }
            else
            {
                replace += '#';
                var digit = string.Empty;
                foreach (var c in s)
                {
                    if (char.IsDigit(c)) digit += c;
                    else
                    {
                        if (digit != string.Empty)
                        {
                            if (digit.Length == 1)
                            {
                                replace += '0';
                            }

                            replace += Convert.ToString(Convert.ToInt32(digit), 16);
                        }

                        digit = string.Empty;
                    }
                }

                return colors.ContainsKey(replace) ? colors[replace] : s;
            }
        }

        public static void Main()
        {
            var path = "../../Data/colors.txt";
            colors = new Dictionary<string, string>();
            using (var sr = File.OpenText(path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    var color = s.Split(null as string[], StringSplitOptions.RemoveEmptyEntries);
                    colors.Add(color[1].ToLower(), color[0]);
                }
            }

            var pat = @"rgb\((\d{1,3},\s*){2}\d{1,3}\)|#[a-f,0-9,A-F]{6}|#[a-f,0-9,A-F]{3}";
            path = "../../Data/source.txt";
            var text = File.ReadAllText(path);
            text = Regex.Replace(text, pat, Method);
            System.IO.File.WriteAllText(@"../../Data/output.txt", text);
        }
    }
}