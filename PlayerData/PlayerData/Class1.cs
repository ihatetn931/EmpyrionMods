using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Eleon.Modding;
using UnityEngine;

namespace PlayerData
{
    public class PlayerDataMain : IMod, ModInterface
    {
        IModApi modApi;
        ModGameAPI legacyModApi;

        void UpdateAll()
        {

        }
        void EnteredGame(bool entered)
        {
            if (entered)
            {
                if (modApi.Application.Mode == ApplicationMode.DedicatedServer)
                {
                    Console.WriteLine("Player Connected1");
                }
                
            }
        }

        void OnLoadedPlayfield(IPlayfield playfield)
        {
            playfield.SpawnTestPlayer(new Vector3(0, 0, 0));
            modApi.Log("Test Player Spawned");
        }

        void OnUnLoadedPlayfield(IPlayfield playfield)
        {

            Console.WriteLine("PlayField {0} pvp", playfield);
        }
        void EventGame(GameEventType type, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null)
        {
            Console.WriteLine("type: {0}", type);
        }

        public void Game_Start(ModGameAPI dediAPI)
        {
            legacyModApi = dediAPI;
        }
        void IMod.Init(IModApi modAPI)
        {
            modApi = modAPI;
            modApi.GameEvent += EventGame;
            
            modApi.Application.OnPlayfieldLoaded += OnLoadedPlayfield;
            modApi.Application.OnPlayfieldUnloading += OnUnLoadedPlayfield;
            modApi.Application.GameEntered += EnteredGame;
            modApi.Application.Update += UpdateAll;
            modApi.Log("ModApi Loaded");
            Console.WriteLine("ModApi Loaded");
        }

        void IMod.Shutdown()
        {
            modApi.Log("ModApi Unloaded");
            Console.WriteLine("ModApi Unloaded");
        }

        public void Game_Update()
        {
           // throw new NotImplementedException();
        }

        public void Game_Exit()
        {
            Console.WriteLine("Game_Exit()");
        }

        public void Game_Event(CmdId eventId, ushort seqNr, object data)
        {
            //Console.WriteLine("Game_Event: {0} : {1}",eventId, data);
            try
            {
                switch (eventId)
                {
                    case CmdId.Event_GlobalStructure_List :
                        {
                             sList = (GlobalStructureInfo)data;
                            //pInfo.playfield
                            foreach(var h in sList.)
                            {
                                h.Value.ForEach(Action < GlobalStructureInfo>).;
                            }
                            pInfo.playfield.SpawnTestPlayer(new Vector3(0, 0, 0));

                        }
                        break;
                    case CmdId.Event_Playfield_List :
                        {
                            PlayfieldList pList = (PlayfieldList)data;
                            foreach(var a in pList.playfields)
                            {
                                a.
                                Console.WriteLine("Playfields Loaded{0}", plist);
                            }
                        }
                        break;
                        
                }
            }

            catch (Exception ex)
            {
                legacyModApi.Console_Write(ex.Message);
            }
        }
    }
}
