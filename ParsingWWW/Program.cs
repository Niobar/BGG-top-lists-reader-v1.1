using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ParsingWWW
{
    class Program
    {
        static void Main(string[] args)
        {
            #region variables
            //string geekListID = "233613";
            //string geekListID = "233625";
            //string geekListID = "230158";
            string geekListID = "224225";
            //string geekListID = "231127";

            Regex lastPageCheckRegex = new Regex(@"/geeklist/\d+/.*?/page/(?<page>\d+)");
            int bglLenght = 0;
            string downloadedPage;
            List<int> boardGameslist = new List<int>();
            List<string> boardGameNameList = new List<string>();
            List<int> boardGamesListIndexes = new List<int>();
            List<BoardGameDB> boardGameDBList = new List<BoardGameDB>();
            string fileNameForDownloadedList = @"C:\Users\biuro\source\repos\ParsingWWW\ParsingWWW\geeklist" + geekListID + ".txt";
            string fileNameForListDB = @"C:\Users\biuro\source\repos\ParsingWWW\ParsingWWW\geeklistDB" + geekListID + ".txt";
            WebClient client = new WebClient();
            string urlAdress = client.BaseAddress;
            int pageNumberLast = 0;
            #endregion

            System.IO.File.WriteAllText(fileNameForDownloadedList, string.Empty);
            downloadedPage = client.DownloadString($"https://boardgamegeek.com/geeklist/{geekListID}/page/1");
            System.IO.File.AppendAllText(fileNameForDownloadedList, downloadedPage);
            MatchCollection mcLastPageCheck = lastPageCheckRegex.Matches(downloadedPage);
            foreach (Match mLastPageCheck in mcLastPageCheck)
            {

                string pageNumberString = mLastPageCheck.Groups["page"].Value;
                int pageNumber = Int32.Parse(pageNumberString);
                if (pageNumber > pageNumberLast)
                {
                    pageNumberLast = pageNumber;
                }


            }

            for (int i = 2; i <= pageNumberLast; i++)
            {
                downloadedPage = client.DownloadString($"https://boardgamegeek.com/geeklist/{geekListID}/page/{i}");
                System.IO.File.AppendAllText(fileNameForDownloadedList, downloadedPage);
            }






            GeekListSearch();
            SaveToFile();
            Writer();



            void Writer()
            {
                for (int i = 0; i < boardGameDBList.Count; i++)
                {

                    Console.WriteLine($"Board game: {boardGameDBList[i].gameName} is game number {i + 1}");
                }
                Console.WriteLine(urlAdress);
            }

            List<BoardGameDB> GeekListSearch()
            {

                string geekList = System.IO.File.ReadAllText(fileNameForDownloadedList);
                Regex exLine = new Regex(@"<a\s*href=""(?<link>(/boardgame/|/boardgamefamily/)(?<id>\d+)/[^""]*).*?>(?<name>.*?)<");
                MatchCollection mcLine = exLine.Matches(geekList);
                foreach (Match mLine in mcLine)
                {
                    var link = mLine.Groups["link"].Value;
                    var id = mLine.Groups["id"].Value;
                    var name = mLine.Groups["name"].Value;
                    var gameIndex = mLine.Groups["link"].Index;
                    var gameChecker = geekList.Substring(gameIndex - 57, 1);

                    if (int.TryParse(gameChecker, out int x) == true)
                    {
                        boardGameNameList.Add(name);
                        boardGameslist.Add(Int32.Parse(id));
                    }

                }
                bglLenght = boardGameslist.Count;


                for (int i = 0; i < bglLenght; i++)
                {
                    BoardGameDB temp = new BoardGameDB(boardGameslist[i], boardGameNameList[i]);
                    boardGameDBList.Add(temp);
                }
                return boardGameDBList;
            }


            void SaveToFile()
            {
                System.IO.File.WriteAllLines(fileNameForListDB, boardGameDBList.Select(aa => aa.ToString()));
            }

        }
    }
}
