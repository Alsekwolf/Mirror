﻿using GTANetworkAPI;
using Mirror.Handler;
using Mirror.Levels;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Skillsheet = Mirror.Skills.Skills;
using Mirror.Classes.Static;
using Mirror.Classes.Models;
using Mirror.StaticEvents;
using Mirror.Globals;
using Mirror.Utility;

namespace Mirror.Commands
{
    public class Sandbox : Script
    {
        [Command("cv")]
        public void CreateVehicle(Client client, string value, bool locked = false)
        {
            uint vehiclecode = NAPI.Util.GetHashKey(value);

            Vehicle newVeh = NAPI.Vehicle.CreateVehicle(vehiclecode, client.Position.Around(5), 0, 45, 45, "TESTING", 255, locked, true);
        }

        [Command("sethealth")]
        public void SetHealth(Client client, int amount)
        {
            client.Health = amount;
        }

        [Command("time")]
        public void SetTime(Client client, int hour)
        {
            NAPI.World.SetTime(hour, 0, 0);
        }

        [Command("update")]
        public void UpdateClothing(Client client)
        {
            if (!client.HasData(EntityData.Account))
                return;

            Account acc = client.GetData(EntityData.Account) as Account;

            Appearance app = Appearance.RetrieveAppearance(acc);
            app.Attach(client);

            Clothing clothing = Clothing.RetrieveClothing(acc);
            clothing.Attach(client);
        }

        [Command("setpoint")]
        public void CmdSetPoint(Client client, string type, int radius = 5)
        {
            Location.CreatePosition(client, type, radius);
        }


        [Command("additem")]
        public void CmdAddItem(Client client, string name, int amount = 1)
        {
            InventoryHandler.AddItemToInventory(client, name, amount);
        }

        [Command("addtopoutfit")]
        public void CmdAddTopOutfit(Client client, string name, int mask, int undershirt, int torso, int top, int hat, int glasses)
        {
            InventoryItem item = new InventoryItem
            {
                ID = 0,
                Name = name,
                StackCount = 1
            };

            item.CreateTopOutfit(
                new int[] { mask, 0 },
                new int[] { undershirt, 0 },
                new int[] { torso, 0 },
                new int[] { top, 0 },
                new int[] { hat, 0 },
                new int[] { glasses, 0 }
            );
            
            InventoryHandler.AddItemToInventory(client, name, 1, item);
        }

        [Command("addxp")]
        public void CmdSetXP(Client client, int amount) => AccountUtil.AddExperience(client, amount);

        [Command("getpoints")]
        public void GetPoints(Client client)
        {
            Account account = client.GetData(EntityData.Account);

            LevelRanks levelRanks = JsonConvert.DeserializeObject<LevelRanks>(account.LevelRanks);

            client.SendChatMessage($"{levelRanks.GetUnallocatedRankPointCount(account.CurrentExperience)}");
        }

        [Command("allocatepoint")]
        public void CMDAllocatePoint(Client client, string type)
        {
            RankEvents.AllocateRankPoint(client, "", type);
        }

        [Command("tp")]
        public void CmdTP(Client client, string trg)
        {
            Client target = NAPI.Player.GetPlayerFromName(trg);

            if (target == null)
                return;

            client.Position = target.Position.Around(5);
        }

        [Command("weapon")]
        public void CmdWeapon(Client client, string weapon)
        {
            client.RemoveAllWeapons();

            WeaponHash hash = NAPI.Util.WeaponNameToModel(weapon);
            AccountUtil.AddPlayerWeapon(client, hash);
        }

        [Command("transfer")]
        public void CmdTransferMoney(Client client, string target, int amount)
        {
            TransactionProccess.Transaction(client, target, amount);
        }
    }



    public class MyCustomResource : Script
    {
        [Command("sethealth")]
        public void CmdSetHealth(Client client)
        {
            // what we want to do here.
        }
    }






}





