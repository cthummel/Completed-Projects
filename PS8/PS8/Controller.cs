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




        /// <summary>
        /// For canceling the current operation
        /// </summary>
        private CancellationTokenSource tokenSource;


        public Controller(IAnalysisView window)
        {
            this.window = window;


            window.GameStart += StartMatch;
            window.CancelGame += Cancel;
            window.WordEntered += NewWord;

        }

        /// <summary>
        /// Creates an HttpClient for communicating with GitHub.  The GitHub API requires specific information
        /// to appear in each request header.
        /// </summary>
        public static HttpClient CreateClient()
        {
            // Create a client whose base address is the GitHub server
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://cs3500-boggle-s17.azurewebsites.net/BoggleService.svc/");

            // Tell the server that the client will accept this particular type of response data
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            // This is an authorization token that you can create by logging in to your GitHub account.
            client.DefaultRequestHeaders.Add("Authorization", "token " + TOKEN);

            // When an http request is made from a browser, the user agent describes the browser.
            // Github requires the email address of the authenticated user.
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", Uri.EscapeDataString(EMAIL));

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
        /// Starts a new match.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="player"></param>
        private void StartMatch(string player, int time)
        {
            using(HttpClient client = CreateClient())
            {
                // An ExpandoObject is one to which in which we can set arbitrary properties.
                // To create a new public repository, we must send a request parameter which
                // is a JSON object with various properties of the new repo expressed as
                // properties.
                dynamic data = new ExpandoObject();
                data.name = "TestRepo";
                data.description = "A test repository for CS 3500";
                data.has_issues = false;

                //List<dynamic> list = new List<dynamic>();
                //list.Add(data);
                //JsonConvert.SerializeObject(list);

                // To send a POST request, we must include the serialized parameter object
                // in the body of the request.
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("/user/repos", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // The deserialized response value is an object that describes the new repository.
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic newRepo = JsonConvert.DeserializeObject(result);
                    Console.WriteLine("New repository: ");
                    Console.WriteLine(newRepo);
                }
                else
                {
                    Console.WriteLine("Error creating repo: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
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
