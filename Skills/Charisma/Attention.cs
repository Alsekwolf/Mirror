﻿using GTANetworkAPI;
using Mirror.Events;
using Mirror.Levels;

using Mirror.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Mirror.Classes.Static;
using Mirror.Classes.Models;
using Mirror.StaticEvents;
using Mirror.Globals;
using Mirror.Utility;

namespace Mirror.Skills.Charisma
{
    public class Attention : Script
    {
        private readonly string VariableName = "IsAttentionReady";
        private readonly string Notification = "Notification";

        public Attention()
        {
            RankEvents.PassiveEvent.PassiveEventTrigger += CheckPassive;
        }

        private void CheckPassive(object source, Client client)
        {
            if (!client.HasData(VariableName + Notification))
                client.SetData(VariableName + Notification, false);

            if (!(client.GetData(EntityData.Account) is Account account))
                return;

            LevelRanks levelRanks = account.GetLevelRanks();

            if (levelRanks.Attention <= 0)
                return;

            LevelRankCooldowns levelRankCooldowns = AccountUtil.GetCooldowns(client);
            levelRankCooldowns.UpdateCooldownTime(client, VariableName, SkillCooldowns.Attention);

            if (!levelRankCooldowns.IsAttentionReady)
            {
                client.SetData(VariableName + Notification, false);
                return;
            }

            if (client.GetData(VariableName + Notification))
                return;

            client.SetData(VariableName + Notification, true);
            client.SendChatMessage("~o~Attention ~w~You are able to draw the attention of everyone around you.");
        }
    }
}
