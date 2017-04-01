using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();

        [WebInvoke(Method = "POST", UriTemplate = "users")]
        UserID CreateUser(string user);

        [WebInvoke(Method = "POST", UriTemplate = "games")]
        GameInfo JoinGame(GameInfo Info);

        [WebInvoke(Method = "PUT", UriTemplate = "games")]
        void CancelJoinRequest(UserID UserToken);

        [WebInvoke(Method = "PUT", UriTemplate = "games/{GameID}")]
        int PlayWord(WordInfo Word, string GameID);

        [WebInvoke(Method = "GET", UriTemplate = "games/{GameID}?Brief={makebrief}")]
        Game GetGameStatus(string GameID, string makebrief);


        
    }
}
