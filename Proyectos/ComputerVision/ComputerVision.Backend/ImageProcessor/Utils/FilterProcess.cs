using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImageProcessor.Utils
{
    public static class FilterProcess
    {
        const string _matchEmailPattern = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

        const string _matchUrlPattern = @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]
        +[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/
        (?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";

        const string _matchPhoneNumberPattern = @"\d{8,11}";

        public static List<string> GetEmails(List<string> textList)
        {
            List<string> emails = new();
            foreach (var text in textList.ToList())
            {
                Regex rx = new Regex(_matchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(text);

                foreach (Match match in matches)
                {
                    emails.Add(match.Value.ToString());
                }

                if (matches.Count > 0)
                {
                    textList.Remove(text);
                }
            }

            return emails;
        }

        public static List<string> GetUrls(List<string> textList)
        {
            List<string> urls = new();
            foreach (var text in textList.ToList())
            {
                Regex rx = new Regex(_matchUrlPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(text);

                foreach (Match match in matches)
                {
                    urls.Add(match.Value.ToString());
                }

                if (matches.Count > 0)
                {
                    textList.Remove(text);
                }
            }

            return urls;
        }

        public static List<string> GetPhoneNumbers(List<string> textList)
        {
            List<string> phoneNumbers = new();
            foreach (var text in textList.ToList())
            {
                var numbers = text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("+", "");
                Regex rx = new Regex(_matchPhoneNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(numbers);

                foreach (Match match in matches)
                {
                    string val = match.Value.ToString();
                    if (match.Value.ToString().StartsWith("591"))
                    {
                        val = "+" + val;
                    }
                    phoneNumbers.Add(val);
                }

                if (matches.Count > 0)
                {
                    textList.Remove(text);
                }
            }

            return phoneNumbers;
        }
    }
}
