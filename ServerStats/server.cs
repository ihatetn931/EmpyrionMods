using Eleon.Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;


namespace ServerData
{
    public class ServerData
    {
        public float fps;
        public int mem;
        public int players;
        public ulong ticks;
        public int uptime;
    }

    public class ServerInfo : ModInterface
    {
        ModGameAPI GameAPI;
        public void Game_Start(ModGameAPI dediAPI)
        {
            GameAPI = dediAPI;
            GameAPI.Console_Write("Empyrion Server Info By ihatetn931/shadowpot Loaded");
        }

        private void ServerStats(DediStats data)
        {
           
            var obj = new ServerData
            {
                    fps = data.fps,
                    mem = data.mem,
                    players = data.players,
                    ticks = data.ticks,
                    uptime = data.uptime,
            };
            if (data.uptime > 0)
            {
                var json = JsonUtility.ToJson(obj);
                File.WriteAllText(@"E:\empyrionDedicatedserver\Content\Mods\ServerInfo\server.json", json);
                Thread.Sleep(1000);
            }
        }

        public void Game_Event(CmdId eventId, ushort seqNr, object data)
        {
            try
            {
                switch (eventId)
                {
                    case CmdId.Event_Dedi_Stats:
                        DediStats info = (DediStats)data;
                        ServerStats(info);
                        break;
                        
                }
            }
            catch (Exception ex)
            {
                GameAPI.Console_Write(ex.Message);
            }
        }

        public void Game_Update()
        {
            GameAPI.Game_Request(CmdId.Request_Dedi_Stats, (ushort)6, new DediStats());
        }

        public void Game_Exit()
        {
            
        }
    }
}