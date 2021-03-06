﻿using System.Net.Sockets;
using System.Threading.Tasks;

namespace System.IoFx.Sockets
{
    static class SocketOperationsEx
    {
        private delegate bool SocketOperationDelegate(Socket socket, SocketAsyncEventArgs args);

        private static readonly SocketOperationDelegate AcceptAsyncHandler = (s, e) => s.AcceptAsync(e);
        private static readonly SocketOperationDelegate ConnectAsyncHandler = (s, e) => s.ConnectAsync(e);
        private static readonly SocketOperationDelegate ReceiveAsyncHandler = (s, e) => s.ReceiveAsync(e);
        private static readonly SocketOperationDelegate SendAsyncHandler = (s, e) => s.SendAsync(e);

        //TODO: Fix alloction per socket accept and remo task.
        public static async Task<Socket> AcceptSocketAsync(this Socket listenSocket, SocketAwaitableEventArgs awaitable)
        {
            await AcceptAsync(listenSocket, awaitable);
            var acceptSocket = awaitable.AcceptSocket;
            awaitable.AcceptSocket = null;
            return acceptSocket;
        }

        public static SocketAwaitableEventArgs AcceptAsync(this Socket socket, SocketAwaitableEventArgs awaitable)
        {
            return OperationAsync(socket, awaitable, AcceptAsyncHandler);
        }
        
        public static SocketAwaitableEventArgs ConnectSocketAsync(this Socket socket, SocketAwaitableEventArgs awaitable)
        {
            return OperationAsync(socket, awaitable, ConnectAsyncHandler);
        }

        public static SocketAwaitableEventArgs ReceiveSocketAsync(this Socket socket, SocketAwaitableEventArgs awaitable)
        {
            return OperationAsync(socket, awaitable, ReceiveAsyncHandler);
        }

        public static SocketAwaitableEventArgs SendSocketAsync(this Socket socket, SocketAwaitableEventArgs awaitable)
        {
            return OperationAsync(socket, awaitable, SendAsyncHandler);
        }

        static SocketAwaitableEventArgs OperationAsync(this Socket socket, SocketAwaitableEventArgs awaitable, SocketOperationDelegate socketFunc)
        {
            awaitable.StartOperation();

            if (!socketFunc(socket, awaitable))
            {
                awaitable.CompleteOperation();
            }

            return awaitable;
        }
    }

}
