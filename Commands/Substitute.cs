﻿using System;
using System.Net;
using System.Threading.Tasks;

using Discord.Commands;

using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace ChancyBot.Commands
{
    public class SubstituteCommand : ModuleBase
    {
        [Command("s"), Summary("Subsitutes one word for another in last message.")]
        public async Task Say(string arg1, string arg2)
        {
            //string[] args = argstring.Split(' ');

            List<MsgInfo> list = Program.Instance.messageHist;
            if (list.Count == 0)
            {
                await Context.Channel.SendMessageAsync("Error! No message to replace yet...");
                return;
            }

            string message = GetLastMessageFromUser(list, Context.User.Id);
            if (message == null)
            {
                await Context.Channel.SendMessageAsync("Error! No message to replace yet...");
                return;
            }

            if (!message.Contains(arg1))
            {
                await Context.Channel.SendMessageAsync("Error! Phrase \"" + arg1 + "\" not found in \"" + message +"\"");
                return;
            }

            string newmsg = message.Replace(arg1, arg2);
            await Context.Channel.SendMessageAsync(Context.User.Mention + " meant: " + newmsg);
        }

        private string GetLastMessageFromUser(List<MsgInfo> list, ulong userid)
        {
            for (int i = list.Count-1; i > 0; i--)
            {
                if (list[i].user == userid)
                {
                    return list[i].message;
                }
            }

            return null;
        }
    }

    public class MsgInfo
    {
        public string message;
        public ulong user;

        public override Boolean Equals(Object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }
            else
            {
                MsgInfo msg = (MsgInfo)obj;
                return msg.user == this.user;
            }
        }
    }
}