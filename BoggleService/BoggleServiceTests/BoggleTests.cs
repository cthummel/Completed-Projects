using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    } 

    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }



        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        /// <summary>
        /// Creates an HttpClient for communicating with the boggle server.
        /// </summary>
        private static HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60000");
            //client.BaseAddress = new Uri("http://cs3500-boggle-s17.azurewebsites.net");
            return client;
        }

        /// <summary>
        /// Helper for serializaing JSON.
        /// </summary>
        private static StringContent Serialize(dynamic json)
        {
            return new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// All legal words
        /// </summary>
        private static readonly ISet<string> dictionary;

        static BoggleTests()
        {
            dictionary = new HashSet<string>();
            using (StreamReader words = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"/dictionary.txt"))
            {
                string word;
                while ((word = words.ReadLine()) != null)
                {
                    dictionary.Add(word.ToUpper());
                }
            }
        }


        /// <summary>
        /// Given a board configuration, returns all the valid words.
        /// </summary>
        private static List<string> AllValidWords(string board)
        {
            BoggleBoard bb = new BoggleBoard(board);
            List<string> validWords = new List<string>();
            foreach (string word in dictionary)
            {
                if (word.Length > 2 && bb.CanBeFormed(word))
                {
                    validWords.Add(word);
                }
            }
            return validWords;
        }

        /// <summary>
        /// Given a board configuration, returns as many words of different lengths as possible.
        /// </summary>
        private static List<string> DifferentLengthWords(string board)
        {
            List<string> variety = new List<string>();
            List<string> allWords = AllValidWords(board);
            for (int i = 3; i <= 10; i++)
            {
                int length = i;
                string word = allWords.Find(w => w.Length == length);
                if (word != null)
                {
                    variety.Add(word);
                }
            }
            return variety;
        }

        /// <summary>
        /// Returns the score for a word.
        /// </summary>
        private static int GetScore(string word)
        {
            if (!dictionary.Contains(word) && word.Length >= 3)
            {
                return -1;
            }
            switch (word.Length)
            {
                case 1:
                case 2:
                    return 0;
                case 3:
                case 4:
                    return 1;
                case 5:
                    return 2;
                case 6:
                    return 3;
                case 7:
                    return 5;
                default:
                    return 11;
            }
        }

        public void Reset()
        {
            string player1 = MakeUser("a").Result;
            string player2 = MakeUser("b").Result;
            if (JoinGameStatus(player1).Result == Accepted)
            {
                JoinGameStatus(player2).Wait();
            }
        }
        /// <summary>
        /// Makes a user and asserts that the resulting status code is equal to the
        /// status parameter.  Returns a Task that will produce the new userID.
        /// </summary>
        private async Task<string> MakeUser(String nickname, HttpStatusCode status = 0)
        {
            dynamic name = new ExpandoObject();
            name.Nickname = nickname;

            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.PostAsync("/BoggleService.svc/users", Serialize(name));
                if (status != 0) Assert.AreEqual(status, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    dynamic user = JsonConvert.DeserializeObject(result);
                    return user.UserToken;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Joins the game and asserts that the resulting status code is equal to the parameter status.
        /// Returns a Task that will produce the new GameID.
        /// </summary>
        private async Task<string> JoinGame(String player, int timeLimit, HttpStatusCode status = 0)
        {
            dynamic user = new ExpandoObject();
            user.UserToken = player;
            user.TimeLimit = timeLimit;

            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.PostAsync("/BoggleService.svc/games", Serialize(user));
                if (status != 0) Assert.AreEqual(status, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    dynamic game = JsonConvert.DeserializeObject(result);
                    return game.GameID;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Joins the game returns a Task that will produce the resulting status.
        /// </summary>
        private async Task<HttpStatusCode> JoinGameStatus(String player)
        {
            dynamic user = new ExpandoObject();
            user.UserToken = player;
            user.TimeLimit = 10;

            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.PostAsync("/BoggleService.svc/games", Serialize(user));
                return response.StatusCode;
            }
        }

        /// <summary>
        /// Cancels the pending game and asserts that the resulting status code is
        /// equal to the parameter status.
        /// </summary>
        private async Task CancelGame(String player, HttpStatusCode status = 0)
        {
            dynamic user = new ExpandoObject();
            user.UserToken = player;

            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.PutAsync("/BoggleService.svc/games", Serialize(user));
                if (status != 0) Assert.AreEqual(status, response.StatusCode);
            }
        }

        /// <summary>
        /// Gets the status for the specified game and value of brief.  Asserts that the resulting
        /// status code is equal to the parameter status.  Returns a task that produces the object
        /// returned by the service.
        /// </summary>
        private async Task<dynamic> GetStatus(String game, string brief, HttpStatusCode status = 0)
        {
            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.GetAsync("/BoggleService.svc/games/" + game + "?brief=" + brief);
                if (status != 0) Assert.AreEqual(status, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject(result);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Plays a word and asserts that the resulting status code is equal to the parameter
        /// status.  Returns a task that will produce the score of the word.
        /// </summary>
        private async Task<int> PlayWord(String player, String game, String word, HttpStatusCode status = 0)
        {
            dynamic play = new ExpandoObject();
            play.UserToken = player;
            play.Word = word;

            using (HttpClient client = CreateClient())
            {
                HttpResponseMessage response = await client.PutAsync("/BoggleService.svc/games/" + game, Serialize(play));
                if (status != 0) Assert.AreEqual(status, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    String result = await response.Content.ReadAsStringAsync();
                    dynamic score = JsonConvert.DeserializeObject(result);
                    return score.Score;
                }
                else
                {
                    return -2;
                }
            }
        }



        // Invalid username
        [TestMethod]
        public void CreateUser1()
        {
            string data = string.Empty;
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Invalid username
        [TestMethod] 
        public void CreateUser2()
        {
            string data = "             ";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Valid username
        [TestMethod]
        public void CreateUser3()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "      Hello      ";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
        }

        // Time outside acceptable boundaries
        [TestMethod]
        public void  JoinGame1()
        {
            GameInfo data = new GameInfo();
            data.UserToken = "user";
            data.TimeLimit = 3;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Time within acceptable boundaries
        [TestMethod]
        public void JoinGame2()
        {
           
            GameInfo data = new GameInfo();
            data.UserToken = "user";
            data.TimeLimit = 6;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Null username
        [TestMethod]
        public void JoinGame3()
        {
            GameInfo data = new GameInfo();
            data.UserToken = "";
            data.TimeLimit = 7;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }
        //Joined Pending game successfully
        [TestMethod]
        public void JoinGame4()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "Player 1";
            Response r = client.DoPostAsync("users", data).Result;
            string Token = r.Data.UserToken;

            dynamic data2 = new ExpandoObject();
            data2.Nickname = "Player 2";
            Response r2 = client.DoPostAsync("users", data2).Result;
            string Token2 = r2.Data.UserToken;

            dynamic data3 = new ExpandoObject();
            data3.UserToken = Token;
            data3.TimeLimit = 30;
            Response r3 = client.DoPostAsync("games", data3).Result;
            Assert.AreEqual(Accepted, r3.Status);

            dynamic data4 = new ExpandoObject();
            data4.UserToken = Token2;
            data4.TimeLimit = 30;
            Response r4 = client.DoPostAsync("games", data4).Result;
            Assert.AreEqual(Created, r4.Status);

        }
        [TestMethod]
        public void JoinGame5()
        {
            Reset();
            String player1 = MakeUser("Player 1").Result;
            JoinGame(player1, 10, Accepted).Wait();
        }


        [TestMethod]
        public void CancelJoin1()
        {
            GameInfo data = new GameInfo();
            data.UserToken = "player";
            data.TimeLimit = 7;
            Response r = client.DoPostAsync("games", data).Result;
            r = client.DoPutAsync("games", data.UserToken).Result;

        }
        

        [TestMethod]
        public void PlayWord1()
        {

        }

        [TestMethod]
        public void GetGameStatus1()
        {

            
            dynamic data = new ExpandoObject();
            Response r = client.DoGetAsync("games/1").Result;
            Assert.AreEqual(OK, r.Status);
            //dynamic returndata = JsonConvert.DeserializeObject(r.Data);
            Assert.IsNull(r.Data.TimeLimit);
            Assert.IsNull(r.Data.TimeLeft);
            Assert.IsNull(r.Data.Board);
            Assert.IsNull(r.Data.Player1);
            Assert.IsNull(r.Data.Player2);
            Assert.AreEqual(null, r.Data.TimeRemaining);
            Assert.AreEqual("pending", r.Data.GameState.ToString());

        }
    }
}