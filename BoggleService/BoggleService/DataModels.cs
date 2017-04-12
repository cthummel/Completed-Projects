using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Web;

namespace Boggle
{
    /// <summary>
    /// Used for encoding and reading GameInfo JSON objects. The Data members just mean we dont have to use all the parameters when we return.
    /// </summary>
    public class GameInfo
    {
        public string UserToken { get; set; }

        public int TimeLimit { get; set; }
    }

    public class NameInfo
    {
        public string Nickname { get; set; }
    }

    public class ScoreReturn
    {
        public int Score { get; set; }
    }

    public class GameIDReturn
    {
        public string GameID { get; set; }
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

    ///// <summary>
    ///// Creates a PendingGame object
    ///// </summary>
    //public class PendingGame
    //{
    //    public PendingGame()
    //    {
    //        GameID = "1";
    //        Player1Token = string.Empty;
    //        TimeLimit = 0;
    //    }

    //    public string GameID { get; set; }
    //    public string Player1Token { get; set; }
    //    public int TimeLimit { get; set; }
    //}

    /// <summary>
    /// Creates a Game object
    /// </summary>
    [DataContract]
    public class Game
    {

        [DataMember]
        public string GameState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Player Player1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Player Player2 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Player1Token { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Player2Token { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeRemaining { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string GameBoard { get; set; }
    }

    /// <summary>
    /// Creates a Player object for use in the Game object. Holds individual player stats.
    /// </summary>
    
    public class Player
    {
        
        public string Nickname { get; set; }
        
        public int Score { get; set; }
        
        public Dictionary<string, int> WordsPlayed { get; set; }
    }

   


}