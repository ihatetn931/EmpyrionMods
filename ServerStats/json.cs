using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Eleon.Modding;
using Newtonsoft.Json;
using UnityEngine;

namespace ServerStatus
{
    public class Json
    {
        public class PlayerServerData
        {
            public int clientId { get; set; }
            public int entityId { get; set; }
            public string steamId { get; set; }
            public string steamOwnerId { get; set; }
            public string playerName { get; set; }
            public string playfield { get; set; }
            public string startPlayfield { get; set; }
            public byte factionGroup { get; set; }
            public int factionId { get; set; }
            public byte factionRole { get; set; }
            public int origin { get; set; }
            public float health { get; set; }
            public float healthMax { get; set; }
            public float oxygen { get; set; }
            public float oxygenMax { get; set; }
            public float stamina { get; set; }
            public float staminaMax { get; set; }
            public float kills { get; set; }
            public float died { get; set; }
            public double credits { get; set; }
            public int exp { get; set; }
            public int upgrade { get; set; }
            public float bpRemainingTime { get; set; }
            public string bpInFactory { get; set; }
            public int ping { get; set; }
        }

        public class RootObject
        {
            public int totalPlayers { get; set; }
            public List<PlayerServerData> playerData { get; set; }
        }

        private static readonly string plyrData = "playerdata.json";
        public static string plyrDataPath = Path.Combine(GetAssemblyDirectory, plyrData);

        private static string GetAssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static void CreateJson()
        {
            var settings = new JsonSerializerSettings
            {
                CheckAdditionalContent = true,
                Formatting = Formatting.Indented
            };
            List<PlayerServerData> BiomeNames = new List<PlayerServerData>();
            var root = new RootObject { playerData = BiomeNames };

            string json = JsonConvert.SerializeObject(root, settings);
            File.AppendAllText(plyrDataPath, json);
        }

        public static void AddToList(PlayerInfo data)
        {
            bool found = true;
            var settings = new JsonSerializerSettings { CheckAdditionalContent = true, Formatting = Formatting.Indented };
            var read = File.ReadAllText(plyrDataPath);
            var json = JsonConvert.DeserializeObject<RootObject>(read);

            json.totalPlayers = json.playerData.Count + 1;

            foreach (var c in json.playerData)
            {
                if (c.playerName.Equals(data.playerName))
                {
                    found = false;
                    if (!found)
                    {
                        c.clientId = data.clientId;
                        c.playerName = data.playerName;
                        c.ping = data.ping;
                        c.died = data.died;
                        c.kills = data.kills;
                        c.health = data.health;
                        c.steamId = data.steamId;
                        c.steamOwnerId = data.steamOwnerId;
                        c.playfield = data.playfield;
                        c.startPlayfield = data.startPlayfield;
                        c.entityId = data.entityId;
                        c.factionGroup = data.factionGroup;
                        c.origin = data.origin;
                        c.healthMax = data.healthMax;
                        c.stamina = data.stamina;
                        c.staminaMax = data.staminaMax;
                        c.credits = data.credits;
                        c.exp = data.exp;
                        c.upgrade = data.upgrade;
                        c.bpRemainingTime = data.bpRemainingTime;
                        c.bpInFactory = data.bpInFactory;
                        break;
                    }
                }
            }
            if (found)
            {
                json.playerData.Add(new PlayerServerData
                {
                    clientId = data.clientId,
                    playerName = data.playerName,
                    ping = data.ping,
                    died = data.died,
                    kills = data.kills,
                    health = data.health,
                    steamId = data.steamId,
                    steamOwnerId = data.steamOwnerId,
                    playfield = data.playfield,
                    startPlayfield = data.startPlayfield,
                    entityId = data.entityId,
                    factionGroup = data.factionGroup,
                    origin = data.origin,
                    healthMax = data.healthMax,
                    stamina = data.stamina,
                    staminaMax = data.staminaMax,
                    credits = data.credits,
                    exp = data.exp,
                    upgrade = data.upgrade,
                    bpRemainingTime = data.bpRemainingTime,
                    bpInFactory = data.bpInFactory,
                });

                string jsonString1 = JsonConvert.SerializeObject(json, settings);
                File.WriteAllText(plyrDataPath, jsonString1);
            }
        }

        public static void AddAndUpdatePlayer(PlayerInfo data)
        {
            if (!File.Exists(plyrDataPath))
            {
                CreateJson();
            }
            else if (data.playerName != null)
            {
                AddToList(data);
            }
        }
    }
}



