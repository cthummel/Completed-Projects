using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boggle
{
    /// <summary>
    /// Used for encoding and reading GameInfo JSON objects.
    /// </summary>
    public class GameInfo
    {
        public string UserToken { get; set; }
        public int TimeLimit { get; set; }
    }

    /// <summary>
    /// Used for encoding and reading UserID JSON objects.
    /// </summary>
    public class UserID
    {
        public string UserToken { get; set; }
    }

    /// <summary>
    /// Used for encoding and reading UserID JSON objects.
    /// </summary>
    public class WordInfo
    {
        public string UserToken { get; set; }
        public string Word { get; set; }
    }

    /// <summary>
    /// Creates a PendingGame object
    /// </summary>
    public class PendingGame
    {
        public string GameID { get; set; }
        public string Player1Token { get; set; }
        public int TimeLimit { get; set; }
    }

    /// <summary>
    /// Creates a Game object
    /// </summary>
    public class Game
    {
        //public int GameID { get; set; }
        public string GameState { get; set; }
        public string Player1Token { get; set; }
        public string Player2Token { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public Dictionary<string, int> Player1WordList { get; set; }
        public Dictionary<string, int> Player2WordList { get; set; }
        public int TimeLimit { get; set; }
        public int TimeRemaining { get; set; }
        public string GameBoard { get; set; }
    }
}