using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Dynamic;
using Boggle;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    } 

    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        // Invalid username
        [TestMethod]
        public void CreateUser1()
        {
            string data = null;
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Invalid username
        [TestMethod] 
        public void CreateUser2()
        {
            string data = "             ";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Valid username
        [TestMethod]
        public void CreateUser3()
        {
            string data = "      Hello      ";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
        }

        // Time outside acceptable boundaries
        [TestMethod]
        public void  JoinGame1()
        {
            GameInfo data = new GameInfo();
            data.UserToken = "user";
            data.TimeLimit = 3;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        // Time within acceptable boundaries
        [TestMethod]
        public void JoinGame2()
        {
           
            GameInfo data = new GameInfo();
            data.UserToken = "user";
            data.TimeLimit = 6;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
        }

        // Null username
        [TestMethod]
        public void JoinGame3()
        {
            GameInfo data = new GameInfo();
            data.UserToken = null;
            data.TimeLimit = 7;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void CancelJoin1()
        {
            GameInfo data = new GameInfo();
            data.UserToken = "player";
            data.TimeLimit = 7;
            Response r = client.DoPostAsync("games", data).Result;
            r = client.DoPutAsync("games", data.UserToken).Result;

        }


        [TestMethod]
        public void PlayWord1()
        {

        }

        [TestMethod]
        public void GetGameStatus1()
        {

            
            dynamic data = new ExpandoObject();
            Response r = client.DoGetAsync("games/0").Result;
            Assert.AreEqual(OK, r.Status);
            //dynamic returndata = JsonConvert.DeserializeObject(r.Data);
            Assert.IsNull(r.Data.TimeLimit);
            Assert.IsNull(r.Data.TimeLeft);
            Assert.IsNull(r.Data.Board);
            Assert.IsNull(r.Data.Player1);
            Assert.IsNull(r.Data.Player2);
            Assert.AreEqual(null, r.Data.TimeRemaining);
            Assert.AreEqual("pending", r.Data.GameState.ToString());

        }
    }
}