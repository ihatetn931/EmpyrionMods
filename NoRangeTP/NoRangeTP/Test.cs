using Eleon;
using Eleon.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NoRangeTP
{
    public class NoRangeMain : ModInterface, IMod
    {

        IModApi modApi;
        ModGameAPI legacyModApi;

        public void Game_Event(CmdId eventId, ushort seqNr, object data)
        {
            throw new NotImplementedException();
        }

        public void Game_Exit()
        {
            throw new NotImplementedException();
        }

        public void Game_Start(ModGameAPI dediAPI)
        {
            legacyModApi = dediAPI;
        }

        public void Game_Update()
        {
            throw new NotImplementedException();
        }
        interface IStructure
        {

        }
        public void Init(IModApi modAPI)
        {
            modApi = modAPI;
            
        }

        public void Shutdown()
        {
            
        }
    }
    class FileInfo : IStructure
    {

    }
}
