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
        private static HashSet<string> colorsUsed;

        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        public static string ColorsMethod(Match match)
        {
            var matchValue = match.Value.ToLower();
            var replace = string.Empty;
            if (matchValue[0] == '#')
            {
                if (matchValue.Length == 7)
                {
                    if (!colorsUsed.Contains(matchValue) && colors.ContainsKey(matchValue))
                    {
                        colorsUsed.Add(matchValue);
                        File.AppendAllText(@"../../Data/colorUsed.txt", colors[matchValue] + "\n");
                    }

                    return colors.ContainsKey(matchValue) ? colors[matchValue] : matchValue;
                }
                   
                replace += matchValue[0].ToString() + matchValue[1].ToString() + matchValue[1].ToString() + matchValue[2].ToString() + matchValue[2].ToString() + matchValue[3].ToString() + matchValue[3].ToString();
                if (!colorsUsed.Contains(replace) && colors.ContainsKey(replace))
                {
                    colorsUsed.Add(replace);
                    File.AppendAllText(@"../../Data/colorUsed.txt", colors[replace] + "\n");
                }

                return colors.ContainsKey(replace) ? colors[replace] : matchValue;
            }

            replace += '#';
            var digit = string.Empty;
            foreach (var Char in matchValue)
            {
                if (char.IsDigit(Char))
                {
                    digit += Char;
                }
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

            if (!colorsUsed.Contains(replace) && colors.ContainsKey(replace))
            {
                colorsUsed.Add(replace);
                File.AppendAllText(@"../../Data/colorUsed.txt", colors[replace] + "\n");
            }

            return colors.ContainsKey(replace) ? colors[replace] : matchValue;
        }

        public static void Main()
        {
            var pathColor = "../../Data/colors.txt";
            File.WriteAllText(@"../../Data/colorUsed.txt","");
            colors = new Dictionary<string, string>();
            colorsUsed = new HashSet<string>();
            using (var streamReader = File.OpenText(pathColor))
            {
                string s;
                while ((s = streamReader.ReadLine()) != null)
                {
                    var color = s.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    colors.Add(color[1].ToLower(), color[0]);
                }
            }

            var pattern = @"rgb\((\d{1,3},\s*){2}\d{1,3}\)|#[a-f,0-9,A-F]{6}|#[a-f,0-9,A-F]{3}";
            var pathSource = "../../Data/source.txt";
            var text = File.ReadAllText(pathSource);
            text = Regex.Replace(text, pattern, ColorsMethod);
            File.WriteAllText(@"../../Data/output.txt", text);
        }
    }
}