﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Boggle
{
    class BoggleServer
    {
        /// <summary>
        /// Launches a MyBoggleService on port 60000.  Keeps the main
        /// thread active so we can send output to the console.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            new BoggleServer(60000);
            // This is our way of preventing the main thread from
            // exiting while the server is in use
            Console.ReadLine();
        }
        
        // Listens for incoming connection requests
        private TcpListener server;

        private BoggleService InternalBoggleServer;
        /// <summary>
        /// Creates a SimpleChatServer that listens for connection requests on port 4000.
        /// </summary>
        public BoggleServer(int port)
        {
            // A TcpListener listens for incoming connection requests
            server = new TcpListener(IPAddress.Any, port);
            InternalBoggleServer = new BoggleService();
            // Start the TcpListener
            server.Start();

            // Ask the server to call ConnectionRequested at some point in the future when 
            // a connection request arrives.  It could be a very long time until this happens.
            // The waiting and the calling will happen on another thread.  BeginAcceptSocket 
            // returns immediately, and the constructor returns to Main.
            server.BeginAcceptSocket(ConnectionRequested, null);
        }

        /// <summary>
        /// This is the callback method that is passed to BeginAcceptSocket.  It is called
        /// when a connection request has arrived at the server.
        /// </summary>
        private void ConnectionRequested(IAsyncResult result)
        {
            // We obtain the socket corresonding to the connection request.  Notice that we
            // are passing back the IAsyncResult object.
            Socket s = server.EndAcceptSocket(result);

            // We ask the server to listen for another connection request.  As before, this
            // will happen on another thread.
            server.BeginAcceptSocket(ConnectionRequested, null);

            // We create a new ClientConnection, which will take care of communicating with
            // the remote client.
            new ClientConnection(s);
        }
    }

    


    /// <summary>
    /// Represents a connection with a remote client.  Takes care of receiving and sending
    /// information to that client according to the protocol.
    /// </summary>
    class ClientConnection
    {
        // Incoming/outgoing is UTF8-encoded.  This is a multi-byte encoding.  The first 128 Unicode characters
        // (which corresponds to the old ASCII character set and contains the common keyboard characters) are
        // encoded into a single byte.  The rest of the Unicode characters can take from 2 to 4 bytes to encode.
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


        private const string RequestType = @"(POST|PUT|GET) (\/BoggleService\.svc\/)(games|users)(\/\d*)?(\?Brief=)?([a-zA-Z]*)?";
        private const string HostName = @"(Host:) (localhost:60000)";
        private const string AcceptType = @"(Accept:) (application/json)";
        private const string ContentLength = @"(Content-Length:) (\d*)";
        private const string ContentType = @"(Content-Type:) (application/json)";


        // Buffer size for reading incoming bytes
        private const int BUFFER_SIZE = 1024;

        // The socket through which we communicate with the remote client
        private Socket socket;

        // Text that has been received from the client but not yet dealt with
        private StringBuilder incoming;

        // Text that needs to be sent to the client but which we have not yet started sending
        private StringBuilder outgoing;

        // For decoding incoming UTF8-encoded byte streams.
        private Decoder decoder = encoding.GetDecoder();

        // Buffers that will contain incoming bytes and characters
        private byte[] incomingBytes = new byte[BUFFER_SIZE];
        private char[] incomingChars = new char[BUFFER_SIZE];

        // Records whether an asynchronous send attempt is ongoing
        private bool sendIsOngoing = false;

        // For synchronizing sends
        private readonly object sendSync = new object();

        // Bytes that we are actively trying to send, along with the
        // index of the leftmost byte whose send has not yet been completed
        private byte[] pendingBytes = new byte[0];
        private int pendingIndex = 0;

        /// <summary>
        /// Creates a ClientConnection from the socket, then begins communicating with it.
        /// </summary>
        public ClientConnection(Socket s)
        {
            // Record the socket and clear incoming
            socket = s;
            incoming = new StringBuilder();
            outgoing = new StringBuilder();

            // Send a welcome message to the remote client
            SendMessage("Welcome!\r\n");

            // Ask the socket to call MessageReceive as soon as up to 1024 bytes arrive.
            socket.BeginReceive(incomingBytes, 0, incomingBytes.Length,
                                SocketFlags.None, MessageReceived, null);
        }

        /// <summary>
        /// Given the proper parameters read from the input, it runs the correct method in our boggleserver and sends the response message.
        /// This is essentially covering the functionality of IBoggleService from before.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="GameID"></param>
        /// <param name="IsBrief"></param>
        /// <param name="content"></param>
        private void ParseMessage (string Type, string Url, string GameID, string IsBrief, dynamic content)
        {
            HttpStatusCode status;
            string ReturnMessage;

            //Each method we call will return the object we need to encode 
            if (Type == "POST")
            {
                if (Url == "users")
                {
                    
                }
                else if (Url == "games")
                {

                }

            }
            else if (Type == "PUT")
            {
                if (Url == "games")
                {

                }
                else
                {

                }

            }
            //Runs a GET request on the server.
            else
            {

            }

        }

        /// <summary>
        /// Given a status code, compiles and then sends the proper response back to the client.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="content"></param>
        private void CompileMessage(HttpStatusCode status, dynamic content)
        {
            StringBuilder message = new StringBuilder("HTTP/1.1 ");

            //If our response doesnt need to send back a JSON object then we should just send back the status code.
            //This only happens after a cancel game request.
            if (content == null)
            {
                if (status == HttpStatusCode.Forbidden)
                {
                    message.Append("403 Forbidden\r\n");
                    message.Append("content-type:application/json;charset=utf-8 \r\n");
                    message.Append("content-length:0");
                    message.Append("\r\n");
                }
                else if (status == HttpStatusCode.OK)
                {
                    message.Append("200 OK\r\n");
                    message.Append("content-type:application/json;charset=utf-8 \r\n");
                    message.Append("content-length:0");
                    message.Append("\r\n");
                }
            }
            else
            {
                string convertedcontent = JsonConvert.SerializeObject(content);
                int contentlength = encoding.GetByteCount(convertedcontent);

                if (status == HttpStatusCode.Forbidden)
                {
                    message.Append("403 Forbidden\r\n");
                    message.Append("content-type:application/json;charset=utf-8 \r\n");
                    message.Append("content-length:" + contentlength);
                    message.Append("\r\n");
                    message.Append(convertedcontent);
                    message.Append("\r\n");
                }
                else if (status == HttpStatusCode.Conflict)
                {

                }
                else if (status == HttpStatusCode.Created)
                {

                }
                else if (status == HttpStatusCode.Accepted)
                {

                }
                else if (status == HttpStatusCode.OK)
                {
                    
                }

            }

            //Send the message we compiled.
            SendMessage(message.ToString());
        }

        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        private void MessageReceived(IAsyncResult result)
        {
            // Figure out how many bytes have come in
            int bytesRead = socket.EndReceive(result);

            // If no bytes were received, it means the client closed its side of the socket.
            // Report that to the console and close our socket.
            if (bytesRead == 0)
            {
                // Console.WriteLine("Socket closed"); 


                socket.Close();
            }

            // Otherwise, decode and display the incoming bytes.  Then request more bytes.
            else
            {
                // Convert the bytes into characters and appending to incoming
                int charsRead = decoder.GetChars(incomingBytes, 0, bytesRead, incomingChars, 0, false);
                incoming.Append(incomingChars, 0, charsRead);
                Console.WriteLine(incoming);

                //Checks what kind of request was made.
                if (Regex.IsMatch(incoming.ToString(), RequestType))
                {
                    Match match = Regex.Match(incoming.ToString(), RequestType);
                    string request = match.Groups[1].ToString();

                    //If the user has given us a JSON object we need to make sure we get it all.
                    if (Regex.IsMatch(incoming.ToString(), ContentLength) && request != "GET")
                    {
                        Match ContentMatch = Regex.Match(incoming.ToString(), ContentLength);
                        int BodyLength = Int32.Parse(ContentMatch.Groups[2].ToString());
                    }

                }
           
                


                socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, MessageReceived, null);





                //In here we need to parse the incoming message to run whichever service they asked for. (Create User, JoinGame, etc.)



                /*

                                //// Echo any complete lines, after capitalizing them
                                //int lastNewline = -1;
                                //int start = 0;
                                //for (int i = 0; i < incoming.Length; i++)
                                //{
                                //    if (incoming[i] == '\n')
                                //    {
                                //        String line = incoming.ToString(start, i + 1 - start);
                                //        SendMessage(line.ToUpper());
                                //        lastNewline = i;
                                //        start = i + 1;
                                //    }
                                //}
                                //incoming.Remove(0, lastNewline + 1);

                                // call parsemessage around here

                                //Need to reset incoming.
                                //incoming = new StringBuilder();

                            */
            } 
        }

        /// <summary>
        /// Sends a string to the client
        /// </summary>
        private void SendMessage(string lines)
        {
            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // Append the message to the outgoing lines
                outgoing.Append(lines);

                // If there's not a send ongoing, start one.
                if (!sendIsOngoing)
                {
                    //Console.WriteLine("Appending a " + lines.Length + " char line, starting send mechanism");
                    sendIsOngoing = true;
                    SendBytes();
                }
                else
                {
                    //Console.WriteLine("\tAppending a " + lines.Length + " char line, send mechanism already running");
                }
            }
        }

        /// <summary>
        /// Attempts to send the entire outgoing string.
        /// This method should not be called unless sendSync has been acquired.
        /// </summary>
        private void SendBytes()
        {
            // If we're in the middle of the process of sending out a block of bytes,
            // keep doing that.
            if (pendingIndex < pendingBytes.Length)
            {
                Console.WriteLine("\tSending " + (pendingBytes.Length - pendingIndex) + " bytes");
                socket.BeginSend(pendingBytes, pendingIndex, pendingBytes.Length - pendingIndex,
                                 SocketFlags.None, MessageSent, null);
            }

            // If we're not currently dealing with a block of bytes, make a new block of bytes
            // out of outgoing and start sending that.
            else if (outgoing.Length > 0)
            {
                pendingBytes = encoding.GetBytes(outgoing.ToString());
                pendingIndex = 0;
                Console.WriteLine("\tConverting " + outgoing.Length + " chars into " + pendingBytes.Length + " bytes, sending them");
                outgoing.Clear();
                socket.BeginSend(pendingBytes, 0, pendingBytes.Length,
                                 SocketFlags.None, MessageSent, null);
            }

            // If there's nothing to send, shut down for the time being.
            else
            {
                Console.WriteLine("Shutting down send mechanism\n");
                sendIsOngoing = false;
            }
        }

        /// <summary>
        /// Called when a message has been successfully sent
        /// </summary>
        private void MessageSent(IAsyncResult result)
        {
            // Find out how many bytes were actually sent
            int bytesSent = socket.EndSend(result);
            Console.WriteLine("\t" + bytesSent + " bytes were successfully sent");

            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // The socket has been closed
                if (bytesSent == 0)
                {
                    socket.Close();
                    Console.WriteLine("Socket closed");
                }

                // Update the pendingIndex and keep trying
                else
                {
                    pendingIndex += bytesSent;
                    SendBytes();
                }
            }
        }
    }
}