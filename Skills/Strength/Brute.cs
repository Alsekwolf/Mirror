﻿using GTANetworkAPI;
using Mirror.Events;
using Mirror.Levels;
using Mirror.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mirror.Skills.Strength
{
    // Increased Health
    public class Brute : Script
    {
        public Brute()
        {
            RankEvents.PassiveEvent.PassiveEventTrigger += CheckPassive;
        }

        public void CheckPassive(object source, Client client)
        {
            if (!(client.GetData("Mirror_Account") is Account account))
                return;

            LevelRanks levelRanks = account.GetLevelRanks();

            if (levelRanks.Brute <= 0)
                return;

            LevelRankCooldowns levelRankCooldowns = LevelRankCooldowns.GetCooldowns(client);

            if (!levelRankCooldowns.IsRegenerateReady)
            {
                levelRankCooldowns.UpdateCooldownTime(client, "IsBruteReady", 60);
                return;
            }

            if (client.Armor > levelRanks.Brute)
                return;

            client.SendNotification("~r~Brute~n~~w~You feel your skin tighten.");
            client.Armor = levelRanks.Brute;
        }
    }
}
