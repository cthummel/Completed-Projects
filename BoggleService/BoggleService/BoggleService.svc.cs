﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;

using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        private readonly static Dictionary<string, string> UserIDs = new Dictionary<string, string>();
        private readonly static Dictionary<int, Game> GameList = new Dictionary<int, Game>();
        private static System.Timers.Timer ServerTimer = new System.Timers.Timer(1000);
        private static Game CurrentPendingGame = new Game();
        private static readonly object sync = new object();
        
        static BoggleService()
        {
            CurrentPendingGame.GameState = "pending";
            CurrentPendingGame.Player1Token = null;
            CurrentPendingGame.TimeLimit = 0;
            //GameList.Add(1, CurrentPendingGame);
            ServerTimer.Elapsed += UpdateTimeRemaining;
            ServerTimer.Start();
            
        }


        /// <summary>
        /// Server side timer ticks every second and updates time remaining on all games.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UpdateTimeRemaining(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (sync)
            {
                foreach (int GameID in GameList.Keys)
                {
                    if (GameList[GameID].GameState == "active")
                    {
                        if (GameList[GameID].TimeLeft == 1)
                        {
                            GameList[GameID].GameState = "completed";
                            GameList[GameID].TimeLeft = 0;
                        }
                        else
                        {
                            GameList[GameID].TimeLeft -= 1;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Creates a user for the Boggle game
        /// </summary>
        public UserID CreateUser(NameInfo username)
        {
            lock (sync)
            {
                if (username.Nickname == "stall")
                {
                    Thread.Sleep(5000);
                }

                if (username.Nickname == null || username.Nickname.Trim().Length == 0)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                else
                {
                    string userID = Guid.NewGuid().ToString();
                    UserIDs.Add(userID, username.Nickname.Trim());
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
        public GameIDReturn JoinGame(GameInfo Info)
        {
            lock (sync)
            {
                GameIDReturn ReturnInfo = new GameIDReturn();
                //string ReturnInfo = string.Empty;

                string nickname;
                //!UserIDs.ContainsKey(Info.UserToken) ||
                if ( Info.TimeLimit < 5 || Info.TimeLimit > 120)
                {
                    SetStatus(Forbidden);
                    return ReturnInfo;
                }
                else if (!UserIDs.TryGetValue(Info.UserToken, out nickname))
                {
                    SetStatus(Forbidden);
                    return ReturnInfo;
                }
                //If the same player tries to join the pending game against himself.
                else if (CurrentPendingGame.Player1Token == Info.UserToken)
                {
                    SetStatus(Conflict);
                    return ReturnInfo;
                }
                //If the pending game has a player 1 waiting.
                else if (CurrentPendingGame.Player1Token != null)
                {
                    Game NewGame = new Game();
                    NewGame.Player1 = new Player();
                    NewGame.Player2 = new Player();
                    string P1Nickname;
                    UserIDs.TryGetValue(CurrentPendingGame.Player1Token, out P1Nickname);

                    //Game NewPendingGame = new Game();
                    BoggleBoard Board = new BoggleBoard();
                    string PendingGameID = GameList.Keys.Count.ToString();

                    //Start new active game.
                    NewGame.GameState = "active";
                    NewGame.Board = Board.ToString();
                    NewGame.Player1Token = CurrentPendingGame.Player1Token;
                    NewGame.Player2Token = Info.UserToken;
                    NewGame.TimeLimit = (Info.TimeLimit + CurrentPendingGame.TimeLimit) / 2;
                    NewGame.TimeLeft = NewGame.TimeLimit;
                    NewGame.Player1.Nickname = P1Nickname;
                    NewGame.Player1.Score = 0;
                    NewGame.Player1.WordsPlayed = new Dictionary<string, int>();
                    NewGame.Player2.Nickname = UserIDs[Info.UserToken];
                    NewGame.Player2.Score = 0;
                    NewGame.Player2.WordsPlayed = new Dictionary<string, int>();

                    //Add an empty pending game.
                    CurrentPendingGame.Player1Token = null;
                    CurrentPendingGame.TimeLimit = 0;


                    //Send back information to client.
                    GameList.Add(GameList.Keys.Count + 1, NewGame);
                    //GameList.Add(GameList.Keys.Count + 1, NewPendingGame);

                    //ReturnInfo.GameID = (GameList.Keys.Count - 1).ToString();
                    ReturnInfo.GameID = (GameList.Keys.Count).ToString();
                    SetStatus(Created);
                    return ReturnInfo;
                }
                //If the pending game is empty.
                else if (CurrentPendingGame.Player1Token == null)
                {
                    //Inputs user data into the pending game.
                    CurrentPendingGame.GameState = "pending";
                    CurrentPendingGame.Player1Token = Info.UserToken;
                    CurrentPendingGame.TimeLimit = Info.TimeLimit;
                    

                    //Returns info back to the user.
                    //ReturnInfo.GameID = GameList.Keys.Count.ToString();
                    ReturnInfo.GameID = (GameList.Keys.Count + 1).ToString();
                    SetStatus(Accepted);
                    return ReturnInfo;
                }
                return ReturnInfo;
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

        public ScoreReturn PlayWord(WordInfo InputObject, string GameID)
        {
            lock (sync)
            {
                Game CurrentGame;
                ScoreReturn Score = new ScoreReturn();
                Score.Score = 0;
                int internalscore = 0;

                //All the failure cases for bad input.
                if (InputObject.Word == null || InputObject.Word.Trim().Length == 0)
                {
                    SetStatus(Forbidden);
                    return Score;
                }
                //Playing a word in a pending game.
                if ((GameList.Keys.Count + 1).ToString() == GameID)
                {
                    SetStatus(Conflict);
                    return Score;
                }
                //Invalid GameID
                if (!GameList.TryGetValue(Int32.Parse(GameID), out CurrentGame) || !UserIDs.ContainsKey(InputObject.UserToken))
                {
                    SetStatus(Forbidden);
                    return Score;
                }
                else if (CurrentGame.Player1Token != InputObject.UserToken && CurrentGame.Player2Token != InputObject.UserToken)
                {
                    SetStatus(Forbidden);
                    return Score;
                }
                else if (CurrentGame.GameState != "active")
                {
                    SetStatus(Conflict);
                    return Score;
                }
                else
                {
                    CurrentGame = new Game();
                    GameList.TryGetValue(Int32.Parse(GameID), out CurrentGame);
                    string word = InputObject.Word.Trim();

                    BoggleBoard Board = new BoggleBoard(CurrentGame.Board);

                    //If its player 1 playing the word.
                    if (CurrentGame.Player1Token == InputObject.UserToken)
                    {
                        if (word.Length < 3)
                        {
                            internalscore = 0;
                        }
                        else if (Board.CanBeFormed(word))
                        {
                            if (CurrentGame.Player1.WordsPlayed.ContainsKey(word))
                            {
                                internalscore = 0;
                            }
                            else if (word.Length == 3 || word.Length == 4)
                            {
                                internalscore = 1;
                            }
                            else if (word.Length == 5)
                            {
                                internalscore = 2;
                            }
                            else if (word.Length == 6)
                            {
                                internalscore = 3;
                            }
                            else if (word.Length == 7)
                            {
                                internalscore = 5;
                            }
                            else if (word.Length > 7)
                            {
                                internalscore = 11;
                            }
                           
                            CurrentGame.Player1.WordsPlayed.Add(word, internalscore);
                            CurrentGame.Player1.Score += internalscore;
                        }
                        else
                        {
                            internalscore = -1;
                            CurrentGame.Player1.WordsPlayed.Add(word, internalscore);
                            CurrentGame.Player1.Score += internalscore;
                        }
                        GameList[Int32.Parse(GameID)] = CurrentGame;
                    }

                    //If its player 2 playing the word.
                    if (CurrentGame.Player2Token == InputObject.UserToken)
                    {
                        if (word.Length < 3)
                        {
                            internalscore = 0;
                        }
                        else if (Board.CanBeFormed(word))
                        {
                            if (CurrentGame.Player2.WordsPlayed.ContainsKey(word))
                            {
                                internalscore = 0;
                            }
                            else if (word.Length == 3 || word.Length == 4)
                            {
                                internalscore = 1;
                            }
                            else if (word.Length == 5)
                            {
                                internalscore = 2;
                            }
                            else if (word.Length == 6)
                            {
                                internalscore = 3;
                            }
                            else if (word.Length == 7)
                            {
                                internalscore = 5;
                            }
                            else if (word.Length > 7)
                            {
                                internalscore = 11;
                            }
                            
                            CurrentGame.Player2.WordsPlayed.Add(word, internalscore);
                            CurrentGame.Player2.Score += internalscore;
                        }
                        else
                        {
                            internalscore = -1;
                            CurrentGame.Player2.WordsPlayed.Add(word, internalscore);
                            CurrentGame.Player2.Score += internalscore;
                        }
                        GameList[Int32.Parse(GameID)] = CurrentGame;
                    }
                }

                // Records the word as being played.
                SetStatus(OK);
                Score.Score = internalscore;
                return Score;
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
                int InputID;
                Game CurrentGame = new Game();
                Game ReturnGame = new Game();
                ReturnGame.Player1 = new Player();
                ReturnGame.Player2 = new Player();

                //If the game in question is our pending game.
                if ((GameList.Keys.Count + 1).ToString() == GameID)
                {
                    SetStatus(OK);
                    CurrentGame.GameState = "pending";
                    return CurrentGame;
                }
                else if (!Int32.TryParse(GameID, out InputID))
                {
                    SetStatus(Forbidden);
                    return null;
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
                            ReturnGame.TimeLeft = CurrentGame.TimeLeft;
                            ReturnGame.Player1.Score = CurrentGame.Player1.Score;
                            ReturnGame.Player2.Score = CurrentGame.Player2.Score;
                        }
                        else
                        {
                            ReturnGame.Board = CurrentGame.Board;
                            ReturnGame.TimeLimit = CurrentGame.TimeLimit;
                            ReturnGame.TimeLeft = CurrentGame.TimeLeft;
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
                            ReturnGame.TimeLeft = 0;
                            ReturnGame.Player1.Score = CurrentGame.Player1.Score;
                            ReturnGame.Player2.Score = CurrentGame.Player2.Score;
                        }
                        else
                        {
                            ReturnGame.Board = CurrentGame.Board;
                            ReturnGame.TimeLimit = CurrentGame.TimeLimit;
                            ReturnGame.TimeLeft = 0;
                            ReturnGame.Player1.Nickname = CurrentGame.Player1.Nickname;
                            ReturnGame.Player1.Score = CurrentGame.Player1.Score;
                            ReturnGame.Player1.WordsPlayed = CurrentGame.Player1.WordsPlayed;
                            ReturnGame.Player2.Nickname = CurrentGame.Player2.Nickname;
                            ReturnGame.Player2.Score = CurrentGame.Player2.Score;
                            ReturnGame.Player2.WordsPlayed = CurrentGame.Player2.WordsPlayed;
                        }
                    }
                    SetStatus(OK);
                }
                //Invalid Game ID case.
                else
                {
                    SetStatus(Forbidden);
                    return null;
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
