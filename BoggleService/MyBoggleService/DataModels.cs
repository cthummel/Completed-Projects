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

        [DataMember]
        public int TimeLeft { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Board { get; set; }
    }

    /// <summary>
    /// Creates a Player object for use in the Game object. Holds individual player stats.
    /// </summary>
    public class Player
    {
        public string Nickname { get; set; }
        
        public int Score { get; set; }
        
        public List<WordScore> WordsPlayed { get; set; }
    }

    /// <summary>
    /// Word-score pairings, collectected in one object
    /// </summary>
    public class WordScore
    {
        public string Word { get; set; }
        public int Score { get; set; }
    }
}