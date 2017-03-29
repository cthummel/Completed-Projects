﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {

        private static readonly object sync = new object();

        /// <summary>
        /// Creates a PendingGame object
        /// </summary>
        public static class PendingGame
        {

        }

        /// <summary>
        /// Creates a Game object
        /// </summary>
        public class Game
        {

        }

        /// <summary>
        /// Creates a user for the Boggle game
        /// </summary>
        public string CreateUser(string username)
        {
            string UserToken = null;

            return UserToken;
        }

        /// <summary>
        /// Invokes a user token to join the game
        /// </summary>
        public string JoinGame(GameInfo Info)
        {
            string GameID = null;

            return GameID;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelJoinRequest()
        {

        }

        public int PlayWord(string GameID)
        {
            int Score = 0;


            return Score;
        }

        public void GetGameStatus(string GameID)
        {

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

        // <To be deleted later>
        // <summary>
        // Demo.  You can delete this.
        // </summary>
         public string WordAtIndex(int n)
         {
            if (n < 0)
            {
                SetStatus(Forbidden);
                return null;
            }
        
            string line;
            using (StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (n == 0) break;
                    n--;
                }
            }

            if (n == 0)
            {
                SetStatus(OK);
                return line;
            }
            else
            {
                SetStatus(Forbidden);
                return null;
            }
        } 
    } 
}
