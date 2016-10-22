﻿using System;

namespace SteamBot
        static string senderName;
        static ChatterBotSession botSession;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Gray;
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamFriends.FriendsListCallback>(OnFriendsList);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            manager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);
            manager.Subscribe<SteamFriends.FriendMsgCallback>(OnChatMessage);
            manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            manager.Subscribe<SteamFriends.FriendAddedCallback>(OnFriendAdded);

            Console.WriteLine("[SteamBot] Successfully logged on!");
            Thread.Sleep(TimeSpan.FromSeconds(3));

            Console.ForegroundColor = ConsoleColor.Red;
            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callback.JobID,

                FileName = callback.FileName,

                BytesWritten = callback.BytesToWrite,
                FileSize = fileSize,
                Offset = callback.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callback.OneTimePassword,

                SentryFileHash = sentryHash,
            });
        {
            steamFriends.SetPersonaState(EPersonaState.Online);
        }

        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {

                var msg = callback.Message.Trim();
                var msg2 = msg.Split(' ');
                senderName = steamFriends.GetFriendPersonaName(callback.Sender);

                Console.WriteLine($"[SteamBot] Message received: {senderName}: {callback.Message}");
                string[] args;
                switch (msg2[0].ToLower())
                {
                    case "Hello":
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "Hello");
                        break;

                    default:
                        string botReply;
                        ChatterBotFactory factory = new ChatterBotFactory();
                        ChatterBot bot = factory.Create(ChatterBotType.CLEVERBOT);
                        botSession = bot.CreateSession();
                        try
                        {
                           botReply = botSession.Think(callback.Message);
                        }
                        catch (Exception)
                        {
                           return;
                        }

                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, botReply);
                        Console.WriteLine($"[SteamBot] Responded to {senderName}: {botReply}");

                    break;
                }

            }
        }
        {
            int friendCount = steamFriends.GetFriendCount();

            Console.WriteLine($"[SteamBot] Bot has {0} friends", friendCount);

            for (int x = 0; x < friendCount; x++)
            {
                SteamID steamIdFriend = steamFriends.GetFriendByIndex(x);

                Console.WriteLine($"[SteamBot] Friend: {0}", steamIdFriend.Render());
            }


            foreach (var friend in callback.FriendList)
            {
                if (friend.Relationship == EFriendRelationship.RequestRecipient)
                {
                    steamFriends.AddFriend(friend.SteamID);
                }
            }
        }
        {
            if (callback.Result == EResult.OK)
            {
                Console.WriteLine($"[SteamBot] {0} added friend.", callback.PersonaName);
                steamFriends.SendChatMessage(callback.SteamID, EChatEntryType.ChatMsg, "Thank you for adding me friend.");
            }
        }

        public static string[] Separate(int numberOfArguments, string command)
        {
            string[] separated = new string[4]; // will contain command split by space delimeter

            int length = command.Length;
            int partitionLength = 0;

            int i = 0;
            foreach (char c in command)
            {
                if (i != numberOfArguments)
                {
                    if (partitionLength > length || numberOfArguments > 5)
                    { // error handling
                        separated[0] = "-1";
                        return separated;
                    }
                    else if (c == ' ')
                    {
                        separated[i++] = command.Remove(command.IndexOf(c));
                        command = command.Remove(0, command.IndexOf(c) + 1);
                    }
                    partitionLength++;
                    if (partitionLength == length && i != numberOfArguments)
                    {
                        separated[0] = "-1";
                        return separated;
                    }
                }
                else
                {
                    separated[i] = command;
                }
            }
            return separated;
        }
