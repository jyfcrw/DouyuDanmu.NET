using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using SuperSocket.ClientEngine;

namespace DouyuDanmu
{
    public class DouyuClient
    {
        private EasyClient client;
        private Timer ticker;

        public string Host = "openbarrage.douyutv.com";
        public Int32 Port = 8601;
        public string RoomId;

        public DouyuClient(string roomId, Action<Dictionary<string, object>> messageHandler)
        {
            RoomId = roomId;
            client = new EasyClient();
            client.Initialize(new DouyuPackageFilter(), (package) => {
                messageHandler(DouyuUtility.Deserialize(package.Data));
            });
        }

        public async void Connect()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Host);
            var connected = await client.ConnectAsync(new IPEndPoint(ips.First(), Port));

            if (connected)
            {
                Login();
                JoinGroup();
                ticker = new Timer((state) => { Tick(); }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            }
        }

        public void Send(Dictionary<string, object> args)
        {
            DouyuPackageInfo package = new DouyuPackageInfo();
            string argsStr = DouyuUtility.Serialize(args);
            byte[] data = package.Encode(argsStr);

            client.Send(new ArraySegment<byte>(data));
        }

        public void Login()
        {
            Send(
                new Dictionary<string, object>
                {
                    { "type", "loginreq" },
                    { "roomid", RoomId }
                }
            );
        }

        public void JoinGroup()
        {
            Send(
                new Dictionary<string, object>
                {
                    { "type", "joingroup" },
                    { "rid", RoomId },
                    { "gid", -9999 }
                }
            );
        }

        public void Tick()
        {
            Send(
                new Dictionary<string, object>
                {
                    { "type", "keeplive" },
                    { "tick", DouyuUtility.UnixTimestamp().ToString() }
                }
            );
        }

        public void Logout()
        {
            Send(
                new Dictionary<string, object>
                {
                    { "type", "logout" }
                }
            );
        }

        public void Close()
        {
            if (ticker != null)
            {
                ticker.Dispose();
            }

            if (client != null && client.IsConnected)
            {
                Logout();
                client.Close();
            }
        }
    }



}
