﻿// Written by Joe Zachary for CS 3500, November 2012
// Revised by Joe Zachary April 2016
// Revised extensively by Joe Zachary April 2017
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
namespace CustomNetworking
{
    /// <summary>
    /// The type of delegate that is called when a StringSocket send has completed.
    /// </summary>
    public delegate void SendCallback(bool wasSent, object payload);
    /// <summary>
    /// The type of delegate that is called when a receive has completed.
    /// </summary>
    public delegate void ReceiveCallback(String s, object payload);
    /// <summary> 
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings. (As opposed to Sockets, which read and write raw bytes.)  
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchronization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refrains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// We can write a string to a StringSocket ss by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameter to the callback is the payload.  
    /// 
    /// We can read a string from a StringSocket ss by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a string and the payload.  The string is the requested string (with the newline removed).
    /// </summary>
    public class StringSocket : IDisposable
    {
        // Underlying socket
        private Socket socket;
        // Encoding used for sending and receiving
        private Encoding encoding;
        private Decoder decoder;
        // Lock for BeginSend
        private object sendLock = new object();
        // Lock for BeginReceive
        private object receiveLock = new object();
        // Text that has been received from the client but not yet dealt with
        private StringBuilder incomingString;
        // Buffer size for reading incoming bytes
        private const int BUFFER_SIZE = 1024;
        // Buffers that will contain incoming bytes and characters
        private byte[] incomingBytes = new byte[BUFFER_SIZE];
        private char[] incomingChars = new char[BUFFER_SIZE];
        // Records whether an asynchronous send attempt is ongoing
        // Private bool sendIsOngoing = false;
        // Bytes that we are actively trying to send, along with the
        // index of the leftmost byte whose send has not yet been completed
        private byte[] pendingBytes = new byte[0];
        // private int pendingIndex = 0;
        // For holding callbacks
        private Queue<ReceiveCallback> ReceiveQueue;
        private Queue<SendCallback> SendQueue;
        private Queue<object> SendPayLoadQueue;
        private Queue<object> ReceivePayLoadQueue;
        private Queue<int> LengthQueue;

        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// StringSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        internal StringSocket(Socket s, Encoding e)
        {
            socket = s;
            encoding = e;
            decoder = encoding.GetDecoder();
            incomingString = new StringBuilder();
            ReceiveQueue = new Queue<ReceiveCallback>();
            SendQueue = new Queue<SendCallback>();
            SendPayLoadQueue = new Queue<object>();
            ReceivePayLoadQueue = new Queue<object>();
            LengthQueue = new Queue<int>();
        }
        /// <summary>
        /// We can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload)
        ///     
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, it 
        /// invokes the callback.  The parameters to the callback are a string and the payload.  
        /// The string is the requested string (with the newline removed).
        /// 
        /// Alternatively, we can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload, length)
        ///     
        /// If length is negative or zero, this behaves identically to the first case.  If length
        /// is positive, then it reads and decodes length bytes from the underlying Socket, yielding
        /// a string s.  The parameters to the callback are s and the payload
        ///
        /// In either case, if there are insufficient bytes to service a request because the underlying
        /// Socket has closed, the callback is invoked with null and the payload.
        /// 
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// 
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.
        /// </summary>
        public void BeginReceive(ReceiveCallback callback, object payload, int length = 0)
        {
            // Only locking here to make sure that the callback and its payload will always stay together. 
            // If there is no lock then its possible that another entry could messup our order in the queue.
            lock (receiveLock)
            {
                ReceiveQueue.Enqueue(callback);
                ReceivePayLoadQueue.Enqueue(payload);
                LengthQueue.Enqueue(length);
            }
            //Since we know we have a request for a string we can begin looking for it.
            socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, ReceiveAsync, null);


        }
        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        private void ReceiveAsync(IAsyncResult result)
        {
            // Figure out how many bytes have come in
            int bytesRead = socket.EndReceive(result);
            // Convert the bytes into characters and appending to incoming
            int charsRead = decoder.GetChars(incomingBytes, 0, bytesRead, incomingChars, 0, false);
            incomingString.Append(incomingChars, 0, charsRead);
            int lastNewline = -1;
            int start = 0;
            for (int i = 0; i < incomingString.Length; i++)
            {
                if (incomingString[i] == '\n')
                {
                    String line = incomingString.ToString(start, i - start);
                    // Pops the callback off the queue and gives it the proper line and payload.
                    ReceiveCallback returncallback = ReceiveQueue.Dequeue();
                    object returnpayload = ReceivePayLoadQueue.Dequeue();
                    returncallback(line, returnpayload);
                    lastNewline = i;
                    start = i + 1;
                }
            }
            //ReceiveQueue.Dequeue();
            incomingString.Remove(0, lastNewline + 1);
            //If the recieve queue has more callbacks in it then it needs to keep reading until all the recieve calls are complete.
            if (ReceiveQueue.Count != 0)
            {
                socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, ReceiveAsync, null);
            }
        }
        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket it invokes the callback.  
        /// The parameters to the callback are true and the payload.
        /// 
        /// If it is impossible to send because the underlying Socket has closed, the callback 
        /// is invoked with false and the payload as parameters.
        ///
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        public void BeginSend(String s, SendCallback callback, object payload)
        {
            byte[] stringBytes = encoding.GetBytes(s);
            lock (sendLock)
            {
                SendQueue.Enqueue(callback);
                SendPayLoadQueue.Enqueue(payload);
                SendCallback returncallback = SendQueue.Dequeue();
                object returnpayload = SendPayLoadQueue.Dequeue();
                object outgoingPayload = new Tuple<byte[], SendCallback, object>(stringBytes, callback, payload);
                socket.BeginSend(stringBytes, 0, stringBytes.Length, SocketFlags.None, SendAsync, outgoingPayload);
            }

        }
        /// <summary>
        /// Called when a message has been successfully sent
        /// </summary>
        private void SendAsync(IAsyncResult result)
        {
            lock (sendLock)
            {
                var internalPayload = (Tuple<byte[], SendCallback, object>)result.AsyncState;
                byte[] stringBytes = internalPayload.Item1;
                SendCallback callback = internalPayload.Item2;
                object outgoingPayload = internalPayload.Item3;
                callback(true, outgoingPayload);
            }
        }
        /// <summary>
        /// Shuts down this StringSocket.
        /// </summary>
        public void Shutdown(SocketShutdown mode)
        {
            socket.Shutdown(mode);
        }
        /// <summary>
        /// Closes this StringSocket.
        /// </summary>
        public void Close()
        {
            socket.Close();
        }

        /// <summary>
        /// Frees resources associated with this StringSocket.
        /// </summary>
        public void Dispose()
        {
            Shutdown(SocketShutdown.Both);
            Close();
        }
    }
}