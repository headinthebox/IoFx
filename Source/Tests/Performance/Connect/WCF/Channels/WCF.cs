﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Connect.WCF.Channels
{
    class WCF :TestScenario
    {
        private TcpChannelServer _server;
        private DuplexChannelManager _clients;
        public override void Run()
        {
            if (ConnectArguments.IsServer())
            {
                _server = new TcpChannelServer(ConnectArguments.CreateNetTcpAddress());
                _server.StartServer();
            }
            else
            {
                _clients  = new DuplexChannelManager(ConnectArguments.ConnectionLimit,
                                                        ConnectArguments.MessageRate,
                                                        new NetTcpBinding() { Security = { Mode = SecurityMode.None } },
                                                        ConnectArguments.CreateNetTcpAddress());
                _clients.Start();
            }
        }
    }
}
