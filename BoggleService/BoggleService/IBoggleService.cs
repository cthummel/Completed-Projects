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
        string JoinGame(GameInfo Info);

        [WebInvoke(Method = "PUT", UriTemplate = "games")]
        void CancelJoinRequest(UserID UserToken);

        [WebInvoke(Method = "PUT", UriTemplate = "games/{GameID}")]
        int PlayWord(string GameID);

        [WebInvoke(Method = "GET", UriTemplate = "games/{GameID}")]
        void GetGameStatus(string GameID);


        
    }
}
