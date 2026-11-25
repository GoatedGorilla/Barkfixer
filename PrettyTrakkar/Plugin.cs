using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using GorillaNetworking;
using Photon.Pun;
using PrettyTrakkar;
using GorillaGameModes;

namespace PrettyTrakkar
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        // 👇 paste your webhook URL here
        private const string WebhookUrl = "https://discord.com/api/webhooks/XXXX/YYYY";

        Webhook whInstance = new Webhook();

        void Awake()
        {
            Harmony.CreateAndPatchAll(GetType().Assembly, Info.Metadata.GUID);
            whInstance = new Webhook();
            SetupWebhook();
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
        }

        void OnPlayerSpawned() => NetworkSystem.Instance.OnJoinedRoomEvent += OnJoin;

        void SetupWebhook()
        {
            if (string.IsNullOrWhiteSpace(WebhookUrl) || WebhookUrl.Contains("XXXX"))
            {
                Debug.LogError("[Pretty Trakkar] No webhook URL set! Edit WebhookUrl in Plugin.cs and rebuild.");
                return;
            }

            whInstance.Name = "gorilla";
            whInstance.AvatarURL = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/pticon.png?raw=true";
            whInstance.Url = WebhookUrl;

            Debug.Log("[Pretty Trakkar] Webhook set successfully!");
        }

        // ...OnJoin stays exactly the same
    }

        //i feel like this code is so bad but it works please make a pr making this cleaner ill just leave it like it is right now
        void OnJoin()
        {
            //gathering info
            string name = NetworkSystem.Instance.GetMyNickName();
            string room = NetworkSystem.Instance.RoomName;
            string howmanymonke = NetworkSystem.Instance.RoomPlayerCount.ToString();
            string gameMode = NetworkSystem.Instance.GameModeString;
            string actualGameMode = "";
            string imageUrl = "";

            if (gameMode.ToLower().Contains("casual")) actualGameMode = "Casual";
            if (gameMode.ToLower().Contains("infection")) actualGameMode = "Infection";
            if (gameMode.ToLower().Contains("guardian")) actualGameMode = "Guardian";
            if (gameMode.ToLower().Contains("paintbrawl")) actualGameMode = "Paintbrawl";
            if (gameMode.Contains("SuperCasual")) actualGameMode = "SUPER Casual";
            if (gameMode.Contains("SuperInfection")) actualGameMode = "SUPER Infection";


            switch (actualGameMode)
            {
                case "Casual":
                    imageUrl = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/Casual.png?raw=true";
                    break;

                case "Infection":
                    imageUrl = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/Infection.png?raw=true";
                    break;

                case "Guardian":
                    imageUrl = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/Guardian.png?raw=true";
                    break;

                case "Paintbrawl":
                    imageUrl = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/Paintbrawl.png?raw=true";
                    break;

                case "SUPER Casual":
                    imageUrl = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/SCasual.png?raw=true";
                    break;

                case "SUPER Infection":
                    imageUrl = "https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/SInfection.png?raw=true";
                    break;
            }

            string message = $@"
            {{
              ""flags"": 32768,
              ""username"": ""Tracker"",
              ""avatar_url"": ""https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/pticon.png?raw=true"",
              ""components"": [
                {{
                  ""type"": 17,
                  ""components"": [
                    {{
                      ""type"": 9,
                      ""components"": [
                        {{
                          ""type"": 10,
                          ""content"": ""# Player Tracked ! [JOIN]\n**👤 | Name: {name}**\n**🚪 | Room: {room}**\n**👥 | Player Count: {howmanymonke}**\n""
                        }}
                      ],
                      ""accessory"": {{
                        ""type"": 11,
                        ""media"": {{
                          ""url"": ""https://github.com/neboskriptDEV/PrettyTrakkar/blob/main/pticon.png?raw=true""
                        }},
                        ""description"": ""github.com/neboskriptDEV/PrettyTrakkar/""
                      }}
                    }},
                    {{
                      ""type"": 12,
                      ""items"": [
                        {{
                          ""media"": {{
                            ""url"": ""{imageUrl}""
                          }}
                        }}
                      ]
                    }},
                    {{
                      ""type"": 10,
                      ""content"": ""Tracked by [***Pretty Tracker***](https://github.com/neboskriptDEV/PrettyTrakkar/) (WIP) • made by [***neboskript***](https://github.com/neboskriptDEV/) with love <3""
                    }}
                  ]
                }}
              ]
            }}";

            whInstance.SendMessage(message);
            Debug.Log("[Pretty Trakkar] Lobby Logged!");
            Debug.Log("[Pretty Trakkar] NAME: " + name + " ROOM: " + room + " PLAYER COUNT: " + howmanymonke + " GAMEMODE: " + actualGameMode + " RAW GAMEMODE NAME: " + gameMode);
        }


    }
}
