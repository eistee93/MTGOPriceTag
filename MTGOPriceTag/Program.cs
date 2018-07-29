using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace GoatBots
{
    class Program
    {
        private static String goatUrl = "https://www.goatbots.com/card/{0}";

        static void Main(string[] args)
        {
            string deckHtml = GetHtml("https://www.cardhoarder.com/d/5b30f9c57ed61");

            //string input = Console.ReadLine().Replace(" ", "-");
            string html = GetHtml(String.Format(goatUrl, input));
            List<decimal> result = GetPrices(html);

            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.WriteLine(result.Min());
            Console.ReadKey();
        }

        public static string GetHtml(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

        public static List<decimal> GetPrices(string html)
        {
            List<decimal> prices = new List<decimal>();

            string startButtonString = "<button class=\"thumbnail\"";
            string endButtonString = "</button>";
            string startFoilString = "data-foil=\"";
            string endFoilString = "\"";
            string startPriceString = "data-price=\"";
            string endPriceString = "\"";

            int startButtonIndex = html.IndexOf(startButtonString);
            System.Globalization.CultureInfo parseCulture = new System.Globalization.CultureInfo("en-US");

            while (startButtonIndex > -1)
            {
                int endButtonIndex = html.IndexOf(endButtonString, startButtonIndex) + endButtonString.Length;
                string buttonSubString = html.Substring(startButtonIndex, endButtonIndex - startButtonIndex);

                int startFoilIndex = buttonSubString.IndexOf(startFoilString) + startFoilString.Length;
                int endFoilIndex = buttonSubString.IndexOf(endFoilString, startFoilIndex);
                string foil = buttonSubString.Substring(startFoilIndex, endFoilIndex - startFoilIndex);

                if (foil == "0")
                {
                    int startPriceIndex = buttonSubString.IndexOf(startPriceString) + startPriceString.Length;
                    int endPriceIndex = buttonSubString.IndexOf(endPriceString, startPriceIndex);
                    string price = buttonSubString.Substring(startPriceIndex, endPriceIndex - startPriceIndex);
                    prices.Add(decimal.Parse(price, parseCulture));
                }

                startButtonIndex = html.IndexOf(startButtonString, startButtonIndex + 1);
            }

            return prices;
        }

        public static decimal GetLowestPrice(string html)
        {
            return GetPrices(html).Min();
        }

        public static List<decimal> GetCards(string html)
        {
            List<string> cards = new List<string>();

            string startCardListString = "<div class=\"row\" id=\"deck - view\">";
            string endCardListString = "<div class=\"deck - footer\">";
            string startCardString = "<li>";
            string endCardString = "</li>";

            int startCardListIndex = html.IndexOf(startCardListString);
            System.Globalization.CultureInfo parseCulture = new System.Globalization.CultureInfo("en-US");

            while (startButtonIndex > -1)
            {
                int endButtonIndex = html.IndexOf(endButtonString, startButtonIndex) + endButtonString.Length;
                string buttonSubString = html.Substring(startButtonIndex, endButtonIndex - startButtonIndex);

                int startFoilIndex = buttonSubString.IndexOf(startFoilString) + startFoilString.Length;
                int endFoilIndex = buttonSubString.IndexOf(endFoilString, startFoilIndex);
                string foil = buttonSubString.Substring(startFoilIndex, endFoilIndex - startFoilIndex);

                if (foil == "0")
                {
                    int startPriceIndex = buttonSubString.IndexOf(startPriceString) + startPriceString.Length;
                    int endPriceIndex = buttonSubString.IndexOf(endPriceString, startPriceIndex);
                    string price = buttonSubString.Substring(startPriceIndex, endPriceIndex - startPriceIndex);
                    prices.Add(decimal.Parse(price, parseCulture));
                }

                startButtonIndex = html.IndexOf(startButtonString, startButtonIndex + 1);
            }

            return prices;
        }
    }
}
