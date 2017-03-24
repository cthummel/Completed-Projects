using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS8
{
    public class Controller
    {
        private IAnalysisView window;
        private string ServerName;
        private string UserName;
        private System.Windows.Forms.Timer timer;
        private string UserID;
        private string Player1ID;
        private string Player2ID;
        private int Player1Score;
        private int Player2Score;
        private string GameID;
        private bool FirstUpdate;

        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private CancellationTokenSource tokenSource;

        public Controller(IAnalysisView window)
        {
            this.window = window;
            ServerName = "http://cs3500-boggle-s17.azurewebsites.net/BoggleService.svc/";
            Player2ID = "Player2";
            Player1Score = 0;
            Player2Score = 0;
            FirstUpdate = true;
            timer = new System.Windows.Forms.Timer();
            tokenSource = new CancellationTokenSource();
            window.GameStart += StartMatch;
            window.Register += RegisterUser;
            window.CancelGame += QuitGame;
            window.WordEntered += NewWord;
            timer.Tick += WaitingForGame;
        }

        /// <summary>
        /// Creates an HttpClient for communicating with the boggle server.
        /// </summary>
        public static HttpClient CreateClient(string server)
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(server);

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        /// <summary>
        /// Cancels the current game.
        /// </summary>
        private void QuitGame()
        {
            tokenSource.Cancel();
            timer.Stop();
            window.GameOver();
        }

        /// <summary>
        /// Starts a timer for a game.
        /// </summary>
        private void RunTimer()
        {
            //Starts a timer for checking the game.
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += WaitingForGame;
            timer.Start();
        }

        /// <summary>
        /// Pings server looking for current status of the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitingForGame(object sender, EventArgs e)
        {
            using (HttpClient client = CreateClient(ServerName))
            {
                dynamic data = new ExpandoObject();
                HttpResponseMessage response = client.GetAsync(String.Format("games/{0}", GameID)).Result;

                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic GameData = JsonConvert.DeserializeObject(result);

                    if ((string)GameData.GameState == "active")
                    {
                        if (FirstUpdate == true)
                        {
                            window.SetLetters((string)GameData.Board);
                            FirstUpdate = false;
                        }

                        int timeleft = GameData.TimeLeft;
                        int ScoreP1 = GameData.Player1.Score;
                        int ScoreP2 = GameData.Player2.Score;

                        string[] UpdateParameters = new string[3];
                        UpdateParameters[0] = timeleft.ToString();
                        UpdateParameters[1] = ScoreP1.ToString();
                        UpdateParameters[2] = ScoreP2.ToString();

                        window.Update(UpdateParameters);
                    }


                    //Display final word lists if the game is over.
                    if((string)GameData.GameState == "completed")
                    {
                        var WordsDictionary = new Dictionary<string, List<string>>();
                        var Player1Words = new List<string>();
                        var Player2Words = new List<string>();
                        int timeleft = GameData.TimeLeft;
                        int ScoreP1 = GameData.Player1.Score;
                        int ScoreP2 = GameData.Player2.Score;

                        string[] UpdateParameters = new string[3];
                        UpdateParameters[0] = timeleft.ToString();
                        UpdateParameters[1] = ScoreP1.ToString();
                        UpdateParameters[2] = ScoreP2.ToString();

                        foreach (dynamic s in GameData.Player1.WordsPlayed)
                        {
                            string word = s.Word;
                            string score = s.Score;
                            Player1Words.Add("Word: " + word + " Score: " + score);
                        }
                        foreach (dynamic s in GameData.Player2.WordsPlayed)
                        {
                            string word = s.Word;
                            string score = s.Score;
                            Player2Words.Add("Word: " + word + " Score: " + score);
                        }

                        WordsDictionary.Add((string)GameData.Player1.Nickname, Player1Words);
                        WordsDictionary.Add((string)GameData.Player2.Nickname, Player2Words);
                        window.FinalWords(WordsDictionary, (string)GameData.Player1.Nickname, (string)GameData.Player2.Nickname);

                        QuitGame();
                    }
                }
            }
        }
        
        /// <summary>
        /// Starts a new match.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="player"></param>
        private void StartMatch(string player, int time)
        {
            FirstUpdate = true;
            using(HttpClient client = CreateClient(ServerName))
            {
                dynamic data = new ExpandoObject();
                data.UserToken = UserID;
                data.TimeLimit = time;
                
                // To send a POST request, we must include the serialized parameter object
                // in the body of the request.
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("games", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // The deserialized response value is an object that describes the new repository.
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic newRepo = JsonConvert.DeserializeObject(result);
                    GameID = newRepo.GameID;

                    //Now we need to update the view with the game information.
                    HttpResponseMessage response2 = client.GetAsync(String.Format("games/{0}", GameID)).Result;

                    if (response2.IsSuccessStatusCode)
                    {
                        String Getresult = response2.Content.ReadAsStringAsync().Result;
                        dynamic GetData = JsonConvert.DeserializeObject(Getresult);

                        //Pull data out.
                        if ((string)GetData.GameState != "pending")
                        {
                            int ScoreP1 = GetData.Player1.Score;
                            int ScoreP2 = GetData.Player2.Score;
                            int timeleft = GetData.TimeLeft;
                            Player1ID = GetData.Player1.Nickname;
                            Player2ID = GetData.Player2.Nickname;

                            //Compile for view update
                            string[] UpdateParameters = new string[5];
                            UpdateParameters[0] = timeleft.ToString();
                            UpdateParameters[1] = ScoreP1.ToString();
                            UpdateParameters[2] = ScoreP2.ToString();
                            UpdateParameters[3] = Player1ID.ToString();
                            UpdateParameters[4] = Player2ID.ToString();

                            //Update view.
                            window.SetLetters((string)GetData.Board);
                            window.Update(UpdateParameters);
                        }
                    }
                    // Starts our internal timer for pinging server.
                    RunTimer();
                }
                else
                {
                    
                }
            }
        }

        /// <summary>
        /// Registers a new User with the server.
        /// </summary>
        /// <param name="username"></param>
        private void RegisterUser(string username, string server)
        {
            ServerName = server;
            UserName = username;

            using(HttpClient client = CreateClient(ServerName))
            {
                dynamic data = new ExpandoObject();
                data.Nickname = username;

                tokenSource = new CancellationTokenSource();
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("users", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic newRepo = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    UserID = newRepo.UserToken;
                }
                else
                {
                    //ERRORS GO HERE
                }
            }

        }

        /// <summary>
        /// Tells the server that a new word was entered. Updates score and wordlist in view if successful.
        /// </summary>
        /// <param name="word"></param>
        private void NewWord(string word)
        {
            using (HttpClient client = CreateClient(ServerName))
            {
                dynamic data = new ExpandoObject();
                data.UserToken = UserID;
                data.Word = word;

                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(String.Format("games/{0}", GameID), content).Result;

                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic newRepo = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    Player1Score += newRepo.Score;
                }
            }
        }
    }
}