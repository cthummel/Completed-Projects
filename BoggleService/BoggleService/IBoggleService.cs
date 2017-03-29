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
        string Createuser(UserInfo user);

        [WebInvoke(Method = "POST", UriTemplate = "games")]
        string JoinGame(GameInfo Info);

        [WebInvoke(Method = "PUT", UriTemplate = "games")]
        string CancelJoinRequest();

        [WebInvoke(Method = "PUT", UriTemplate = "games/{GameID}")]
        string PlayWord();

        [WebInvoke(Method = "GET", UriTemplate = "games/{GameID}")]
        string GetGameStatus();


        /// <summary>
        /// Returns the nth word from dictionary.txt.  If there is
        /// no nth word, responds with code 403. This is a demo;
        /// you can delete it.
        /// </summary>
         [WebGet(UriTemplate = "/word?index={n}")]
         string WordAtIndex(int n); 
    }
}
