﻿using GTANetworkAPI;
using Mirror.Database;
using Mirror.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mirror.Models
{
    public class Money
    {
        public int Id { get; set; }
        public double ServerHardcap { get; set; } = 500000000; // 500,000,000;
        public double CurrentEconomyPrinted { get; set; } = 0;
        public double Tax { get; set; } = 0.05;
    }

    public static class TransactionProccess
    {
        public static Money ServerFunds { get; set; }

        /// <summary>
        /// Initialize the Money Service on Server Start.
        /// </summary>
        public static void Initialize()
        {
            if (!DatabaseUtilities.Exists<Money>())
            {
                // Create Money Entry
                ServerFunds = new Money();
                DatabaseUtilities.Upsert(ServerFunds);
                Console.WriteLine(Exceptions.UtilityMoneyEstablished);
                return;
            }

            // Get Money Entry
            var collection = DatabaseUtilities.GetCollection<Money>();
            ServerFunds = collection.First();

            if (ServerFunds == null)
                return;

            Console.WriteLine(Exceptions.UtilityMoneyLoaded);
            Console.WriteLine(Exceptions.Prefix + ServerFunds.CurrentEconomyPrinted.ToString());
        }

        public static void UpdateFunds()
        {
            DatabaseUtilities.UpdateData(ServerFunds);
        }

        public static void Transaction(Client client, string toPlayer, double amount)
        {
            if (amount <= 0)
                return;

            if (!client.HasData("Mirror_Account"))
                return;

            if (!(client.GetData("Mirror_Account") is Account account))
                return;

            // Check Player Money
            if (account.Money < amount)
            {
                client.SendChatMessage(Exceptions.MoneyNotEnoughBalance);
                return;
            }

            Client target = null;
            Client[] clients = NAPI.Pools.GetAllPlayers().ToArray();

            for (int i = 0; i < clients.Length; i++)
            {
                if (clients[i].Name.ToLower().Contains(toPlayer.ToLower()))
                {
                    target = clients[i];
                    break;
                }
            }

            if (target == null)
            {
                client.SendChatMessage(Exceptions.MoneyUserNotFound);
                return;
            }

            if (!target.HasData("Mirror_Account"))
                return;

            if (!(target.GetData("Mirror_Account") is Account targetAccount))
                return;

            account.Money -= amount;
            Account.PlayerUpdateEvent.Trigger(client, account);

            double taxed = CalculateTax(amount);
            ServerFunds.CurrentEconomyPrinted += taxed;
            UpdateFunds();

            double targetRecieved = amount - taxed;

            targetAccount.Money += targetRecieved;
            Account.PlayerUpdateEvent.Trigger(target, targetAccount);
        }

        public static void ServerTransactionTo(Client client, double amount)
        {
            if (amount <= 0)
                return;

            if (!client.HasData("Mirror_Account"))
                return;

            if (!(client.GetData("Mirror_Account") is Account account))
                return;

            ServerFunds.CurrentEconomyPrinted += amount;
            UpdateFunds();
            account.Money += amount;
            Account.PlayerUpdateEvent.Trigger(client, account);
        }

        public static void ServerTransactionFrom(Client client, double amount)
        {
            if (amount <= 0)
                return;

            if (!client.HasData("Mirror_Account"))
                return;

            if (!(client.GetData("Mirror_Account") is Account account))
                return;

            ServerFunds.CurrentEconomyPrinted -= amount;
            UpdateFunds();
            account.Money -= amount;
            Account.PlayerUpdateEvent.Trigger(client, account);
        }

        public static double CalculateTax(double amount)
        {
            return (amount * ServerFunds.Tax);
        }
    }
}
