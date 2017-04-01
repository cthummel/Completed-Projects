using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Boggle
{
    /// <summary>
    /// Used for encoding and reading GameInfo JSON objects. The Data members just mean we dont have to use all the parameters when we return.
    /// </summary>
    [DataContract]
    public class GameInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public string GameID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UserToken { get; set; }

        [DataMember(EmitDefaultValue = false)]
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

        //[DataMember(EmitDefaultValue = false)]
        //public int Player1Score { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public int Player2Score { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public Dictionary<string, int> Player1WordList { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public Dictionary<string, int> Player2WordList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeRemaining { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string GameBoard { get; set; }
    }

    [DataContract]
    public class Player
    {
        [DataMember(EmitDefaultValue = false)]
        public string Nickname { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int Score { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, int> WordsPlayed { get; set; }
    }
}