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
        private Dictionary<int, Timer> TimerList = new Dictionary<int, Timer>();
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
        public GameInfo JoinGame(GameInfo Info)
        {
            lock (sync)
            {                
                if (Info.UserToken == null || Info.UserToken.Trim().Equals("") || Info.TimeLimit < 5 || Info.TimeLimit > 120)
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
                    GameInfo info = new GameInfo();
                    info.GameID = NewGameID;
                    return info;
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
                    NewGame.Player1.Nickname = UserIDs[CurrentPendingGame.Player1Token];
                    NewGame.Player2.Nickname = UserIDs[Info.UserToken];
                    NewGame.Player1.Score = 0;
                    NewGame.Player2.Score = 0;
                    NewGame.Player1.WordsPlayed = new Dictionary<string, int>();
                    NewGame.Player2.WordsPlayed = new Dictionary<string, int>();
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
                    GameInfo info = new GameInfo();
                    info.GameID = CurrentPendingGame.GameID;
                    return info;
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
            lock (sync)
            {
                Game CurrentGame;
                int score = 0;

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
                else if (CurrentGame.Player1.Nickname != InputObject.UserToken || CurrentGame.Player2.Nickname != InputObject.UserToken)
                {
                    SetStatus(Forbidden);
                    return 0;
                }
                else if (CurrentGame.GameState != "active")
                {
                    SetStatus(Conflict);
                    return 0;
                }
                else
                {
                    CurrentGame = new Game();
                    GameList.TryGetValue(Int32.Parse(GameID), out CurrentGame);
                    string word = InputObject.Word.Trim();

                    BoggleBoard Board = new BoggleBoard(CurrentGame.GameBoard);

                    //If its player 1 playing the word.
                    if (CurrentGame.Player1.Nickname == InputObject.UserToken)
                    {
                        if (Board.CanBeFormed(word))
                        {
                            if (CurrentGame.Player1.WordsPlayed.ContainsKey(word))
                            {
                                score = 0;
                            }
                            else if (word.Length <= 3)
                            {
                                score = 0;
                            }
                            else if (word.Length > 3 && word.Length <= 5)
                            {
                                score = 1;
                            }
                            else if (word.Length == 6)
                            {
                                score = 3;
                            }
                            else if (word.Length == 7)
                            {
                                score = 5;
                            }
                            else if (word.Length > 7)
                            {
                                score = 11;
                            }
                            else
                            {
                                score = -1;
                            }
                            CurrentGame.Player1.WordsPlayed.Add(word, score);
                            CurrentGame.Player1.Score += score;
                        }
                    }

                    //If its player 2 playing the word.
                    if (CurrentGame.Player2.Nickname == InputObject.UserToken)
                    {
                        if (Board.CanBeFormed(word))
                        {
                            if (CurrentGame.Player2.WordsPlayed.ContainsKey(word))
                            {
                                score = 0;
                            }
                            else if (word.Length <= 3)
                            {
                                score = 0;
                            }
                            else if (word.Length > 3 && word.Length <= 5)
                            {
                                score = 1;
                            }
                            else if (word.Length == 6)
                            {
                                score = 3;
                            }
                            else if (word.Length == 7)
                            {
                                score = 5;
                            }
                            else if (word.Length > 7)
                            {
                                score = 11;
                            }
                            else
                            {
                                score = -1;
                            }
                            CurrentGame.Player2.WordsPlayed.Add(word, score);
                            CurrentGame.Player2.Score += score;
                        }
                    }
                }

                // Records the word as being played.
                SetStatus(OK);
                return score;
            }
        }

        /// <summary>
        /// Returns game status information if the GameID is valid
        /// </summary>
        /// <param name="GameID"></param>
        public Game GetGameStatus(string GameID, string isBrief)
        {
            lock (sync)
            {
                Game CurrentGame = new Game();
                Game ReturnGame = new Game();

                //If the game in question is our pending game.
                if (CurrentPendingGame.GameID == GameID)
                {
                    SetStatus(OK);
                    CurrentGame.GameState = "pending";
                }
                //If the game in question is a currently running game.
                else if (GameList.TryGetValue(Int32.Parse(GameID), out CurrentGame))
                {
                    
                    //If the game is active
                    if (CurrentGame.GameState == "active")
                    {
                        ReturnGame.GameState = "active";

                        if (isBrief == "yes")
                        {
                            
                            ReturnGame.TimeRemaining = CurrentGame.TimeRemaining;
                            ReturnGame.Player1.Score = CurrentGame.Player1.Score;
                            ReturnGame.Player2.Score = CurrentGame.Player2.Score;

                        }
                        else
                        {
                            ReturnGame.GameBoard = CurrentGame.GameBoard;
                            ReturnGame.TimeLimit = CurrentGame.TimeLimit;
                            ReturnGame.TimeRemaining = CurrentGame.TimeRemaining;
                            ReturnGame.Player1.Nickname = CurrentGame.Player1.Nickname;
                            ReturnGame.Player1.Score = CurrentGame.Player1.Score;
                            ReturnGame.Player2.Nickname = CurrentGame.Player2.Nickname;
                            ReturnGame.Player2.Score = CurrentGame.Player2.Score;
                        }

                    }
                    //if the game is completed.
                    else
                    {
                        ReturnGame.GameState = "completed";

                        if (isBrief == "yes")
                        {
                            ReturnGame.TimeRemaining = 0;
                            ReturnGame.Player1.Score = CurrentGame.Player1.Score;
                            ReturnGame.Player2.Score = CurrentGame.Player2.Score;
                        }
                        else
                        {

                        }
                    }
                    SetStatus(OK);
                }
                //Invalid Game ID case.
                else
                {
                    SetStatus(Forbidden);
                }

                return ReturnGame;
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
