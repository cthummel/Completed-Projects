using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        private Dictionary<string, string> UserIDs = new Dictionary<string, string>();
        private Dictionary<int, Game> GameList = new Dictionary<int, Game>();
        private PendingGame CurrentPendingGame = new PendingGame();
        private int CurrentGameID = 0;
        private static readonly object sync = new object();
        
        /// <summary>
        /// Creates a user for the Boggle game
        /// </summary>
        public UserID CreateUser(string username)
        {
            lock (sync)
            {
                if (username == "stall")
                {
                    Thread.Sleep(5000);
                }

                if (username == null || username.Trim().Length == 0)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else
                {
                    string userID = Guid.NewGuid().ToString();
                    UserIDs.Add(userID, username.Trim());
                    SetStatus(Created);
                    UserID ID = new UserID();
                    ID.UserToken = userID;
                    return ID;
                }
            }
        }

        /// <summary>
        /// Invokes a user token to join the game
        /// </summary>
        public string JoinGame(GameInfo Info)
        {
            lock (sync)
            {
                string nickname;
                
                if (UserIDs.TryGetValue(Info.UserToken, out nickname) || Info.TimeLimit < 5 || Info.TimeLimit > 120)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else if (CurrentPendingGame.Player1Token == Info.UserToken)
                {
                    SetStatus(Conflict);
                    return null;
                }
                else if (CurrentPendingGame.Player1Token == null)
                {
                    //Establishes the new PendingGame.
                    string NewGameID = (CurrentGameID + 1).ToString();
                    CurrentPendingGame.GameID = NewGameID;
                    CurrentPendingGame.Player1Token = Info.UserToken;
                    CurrentPendingGame.TimeLimit = Info.TimeLimit;

                    //Increments GameID so the next time we need a pending game we can have a unique ID.
                    CurrentGameID += 1;

                    //Sets status and retuns the game ID.
                    SetStatus(Accepted);
                    return NewGameID;
                }
                else
                {
                    int PendingGameID = Int32.Parse(CurrentPendingGame.GameID);
                    BoggleBoard Board = new BoggleBoard();

                    //Sets up a new game;
                    Game NewGame = new Game();
                    
                    NewGame.GameState = "active";
                    NewGame.Player1Token = CurrentPendingGame.Player1Token;
                    NewGame.Player2Token = Info.UserToken;
                    NewGame.Player1Score = 0;
                    NewGame.Player2Score = 0;
                    NewGame.Player1WordList = new Dictionary<string, int>();
                    NewGame.Player2WordList = new Dictionary<string, int>();
                    NewGame.TimeLimit = ((CurrentPendingGame.TimeLimit + Info.TimeLimit) / 2);
                    NewGame.TimeRemaining = NewGame.TimeLimit;
                    NewGame.GameBoard = Board.ToString();

                    //Resets PendingGameBoard so that we have an empty one waiting.
                    CurrentPendingGame.GameID = null;
                    CurrentPendingGame.Player1Token = null;
                    CurrentPendingGame.TimeLimit = 0;

                    //Final cleanup before sending back GameID.
                    GameList.Add(PendingGameID, NewGame);
                    SetStatus(Created);
                    return PendingGameID.ToString();
                }
            }
        }

        /// <summary>
        /// Cancels a JoinGame request.
        /// </summary>
        public void CancelJoinRequest(UserID UserToken)
        {
            if (!UserIDs.ContainsKey(UserToken.UserToken) || UserToken.UserToken != CurrentPendingGame.Player1Token)
            {
                SetStatus(Forbidden);
               
            }
            else
            {
                CurrentPendingGame.Player1Token = null;
                SetStatus(OK);
            }
        }

        public int PlayWord(WordInfo InputObject, string GameID)
        {
            Game CurrentGame;
            
            //All the failure cases for bad input.
            if (InputObject.Word == null || InputObject.Word.Trim().Length == 0)
            {
                SetStatus(Forbidden);
                return 0;
            }
            else if (!GameList.TryGetValue(Int32.Parse(GameID), out CurrentGame) || UserIDs.ContainsKey(InputObject.UserToken))
            {
                SetStatus(Forbidden);
                return 0;
            }
            else if (CurrentGame.Player1Token != InputObject.UserToken || CurrentGame.Player2Token != InputObject.UserToken)
            {
                SetStatus(Forbidden);
                return 0;
            }
            else if (CurrentGame.GameState != "active")
            {
                SetStatus(Conflict);
                return 0;
            }

            // Records the word as being played.
            int Score = 0;
            return Score;
        }

        /// <summary>
        /// Returns game status information if the GameID is valid
        /// </summary>
        /// <param name="GameID"></param>
        public void GetGameStatus(string GameID)
        {
            if (!GameID.Equals(CurrentGameID))
            {
                SetStatus(Forbidden);
            }
            else
            {
                SetStatus(OK);
            }
        }

        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }
    } 
}
