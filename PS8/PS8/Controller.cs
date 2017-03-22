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
        private System.Windows.Forms.Timer timer;

        private string Player1ID;
        private string Player2ID;
        private string GameID;


        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private CancellationTokenSource tokenSource;


        public Controller(IAnalysisView window)
        {
            this.window = window;
            ServerName = "http://cs3500-boggle-s17.azurewebsites.net/BoggleService.svc/";
            Player1ID = "Player1";
            Player2ID = "Player2";

            window.GameStart += StartMatch;
            window.Register += RegisterUser;
            window.CancelGame += Cancel;
            window.WordEntered += NewWord;
            
            


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

            // This is an authorization token that you can create by logging in to your GitHub account.
            //client.DefaultRequestHeaders.Add("Authorization", "token " + TOKEN);

            // When an http request is made from a browser, the user agent describes the browser.
            // Github requires the email address of the authenticated user.
            //client.DefaultRequestHeaders.UserAgent.Clear();
            //client.DefaultRequestHeaders.Add("User-Agent", Uri.EscapeDataString(EMAIL));

            // There is more client configuration to do, depending on the request.
            return client;
        }


        /// <summary>
        /// Cancels the current operation (currently unimplemented)
        /// </summary>
        private void Cancel()
        {
            tokenSource.Cancel();
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



            }


        }
        



        /// <summary>
        /// Starts a new match.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="player"></param>
        private void StartMatch(string player, int time)
        {
            
            using(HttpClient client = CreateClient(ServerName))
            {
                
                dynamic data = new ExpandoObject();
                data.UserToken = Player1ID;
                data.TimeLimit = time;
                
                // To send a POST request, we must include the serialized parameter object
                // in the body of the request.
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("games", content).Result;
               
                

                if (response.IsSuccessStatusCode)
                {
                    RunTimer();


                    // The deserialized response value is an object that describes the new repository.
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic newRepo = JsonConvert.DeserializeObject(result);
                    GameID = newRepo.GameID;

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
                    Player1ID = newRepo.UserToken;
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

        }

        /// <summary>
        /// Asks server for game letters and sends them to the view to update the gameboard.
        /// </summary>
        private void ReturnLetters()
        {
            //Asks server for letters and parses them.
            //Saves them into a returnletters list before sending to the view to update.
            var returnletters = new List<string>();




            window.SetLetters(returnletters);
        }


    }
}
