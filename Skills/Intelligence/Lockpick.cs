﻿using GTANetworkAPI;
using Mirror.Events;
using Mirror.Levels;
using Mirror.Models;
using Mirror.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mirror.Skills.Intelligence
{
    public class Lockpick : Script
    {
        private readonly string VariableName = "IsLockpickReady";
        private readonly string Notification = "Notification";

        public Lockpick()
        {
            RankEvents.PassiveEvent.PassiveEventTrigger += CheckPassive;
        }

        private void CheckPassive(object source, Client client)
        {
            if (!client.HasData(VariableName + Notification))
                client.SetData(VariableName + Notification, false);

            if (!(client.GetData("Mirror_Account") is Account account))
                return;

            LevelRanks levelRanks = account.GetLevelRanks();

            if (levelRanks.Lockpick <= 0)
                return;

            LevelRankCooldowns levelRankCooldowns = LevelRankCooldowns.GetCooldowns(client);
            levelRankCooldowns.UpdateCooldownTime(client, VariableName, SkillCooldowns.Lockpick - (levelRanks.Lockpick * 5));

            if (!levelRankCooldowns.IsLockpickReady)
            {
                client.SetData(VariableName + Notification, false);
                return;
            }

            if (client.GetData(VariableName + Notification))
                return;

            client.SetData(VariableName + Notification, true);
            client.SendNotification("~b~Lockpick~n~~w~Your next lockpick will have a higher chance to succeed.");
        }
    }
}