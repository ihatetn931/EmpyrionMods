using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Eleon.Modding;
using System.Linq;
using System.Threading;

namespace ServerStatus
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
        public static List<int> playerIds = new List<int>();
        public static int playerOfflineCount = 0;
        ModGameAPI GameAPI;

        public void Game_Start(ModGameAPI dediAPI)
        {
            GameAPI = dediAPI;
            GameAPI.Console_Write("Empyrion Dedi Server Info By ihatetn931/shadowpot Loaded");
        }

        public void Game_Event(CmdId eventId, ushort seqNr, object data)
        {
            try
            {
                switch (eventId)
                {
                    case CmdId.Event_Dedi_Stats:
                        DediStats dStats = (DediStats)data;
                        ServerStats(dStats);
                        break;
                    case CmdId.Event_Player_Connected:
                        {
                            int entityId = ((Id)data).id;
                            lock (playerIds)
                            {
                                playerIds.Add(entityId);
                            }
                        }
                        break;

                    case CmdId.Event_Player_Disconnected:
                        {
                            int entityId = ((Id)data).id;
                            lock (playerIds)
                            {
                                playerIds.Remove(entityId);
                                
                            }
                        }
                        break;

                    case CmdId.Event_Player_List:
                        {
                            if (data != null)
                            {  // empyt list is null?!
                                lock (playerIds)
                                {
                                    playerIds = ((IdList)data).list;
                                }
                                for (int i = 0; i < playerIds.Count; i++)
                                {
                                    //GameAPI.Console_Write(string.Format("{0} Player with id {1}", i + 1, playerIds[i]));
                                    if (i > 0)
                                    {
                                        GetPlayerInfo();
                                    }
                                }
                            }
                            else
                            {
                                GameAPI.Console_Write("No players connected");
                            }
                        }
                        break;

                    case CmdId.Event_Player_Info:
                        {
                            // DediStats sStats = (DediStats)data;
                            PlayerInfo pInfo = (PlayerInfo)data;
                            if (pInfo == null)
                                break;
                            Json.AddAndUpdatePlayer(pInfo);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                GameAPI.Console_Write(ex.Message);
            }
        }

        public void Get_PlayerList()
        {
            GameAPI.Game_Request(CmdId.Request_Player_List, (ushort)CmdId.Request_Player_List, null);
        }

        private void GetPlayerInfo()
        {
            lock (playerIds)
            {
                foreach (int id in playerIds)
                {
                    GameAPI.Game_Request(CmdId.Request_Player_Info, (ushort)Eleon.Modding.CmdId.Request_Player_Info, new Eleon.Modding.Id(id));
                }
            }
        }

        public void Game_Update()
        {
            GameAPI.Game_Request(CmdId.Request_Dedi_Stats, (ushort)6, new DediStats());
            Get_PlayerList();
        }
        public void Game_Exit()
        {
            GameAPI.Console_Write("Empyrion Dedi Server Info By ihatetn931/shadowpot Unloaded");
        }
        public  void ServerStats(DediStats data)
        {
            var sData = new ServerData
            {
                fps = data.fps,
                mem = data.mem,
                players = data.players,
                ticks = data.ticks,
                uptime = data.uptime,
            };
            if (data.players == 0)
            {
                playerOfflineCount++;
                Thread.Sleep(1000);
                if (playerOfflineCount == 3600)
                {
                    String sayCommand = "say Server Shutting Down Now in 5 mins";
                    String command = "saveandexit 5";
                    GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)CmdId.Request_InGameMessage_AllPlayers, new PString(sayCommand));
                    GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)CmdId.Request_InGameMessage_AllPlayers, new PString(command));
                   // Console.WriteLine("Commands: {0}", command);
                    //Console.WriteLine("ConsoleCommand: {0}", GameAPI.Game_Request(CmdId.Request_ConsoleCommand, (ushort)CmdId.Request_InGameMessage_AllPlayers, new Eleon.Modding.PString(command)));
                }
            }
            else
            {
                playerOfflineCount = 0;
            }
            var json = JsonUtility.ToJson(sData, true);
            File.WriteAllText(@"E:\empyrionDedicatedserver\Content\Mods\ServerInfo\serverdata.json", json);
        }
    }
}