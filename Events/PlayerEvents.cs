﻿using System;
using System.Collections.Generic;
using System.Text;
using Mirror.Models;
using GTANetworkAPI;

namespace Mirror.Events
{
    public static class PlayerEvents
    {
        /// <summary>
        /// Uses the interval client-side to make attacks on a player.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="arguments"></param>
        public static void AttackPlayer(Client client, params object[] arguments)
        {
            Client target = arguments[1] as Client;

            if (client.Position.DistanceTo2D(target.Position) > 5)
                return;

            if (target.IsInVehicle)
                return;

            if (!client.HasData("LastAttack"))
            {
                client.SetData("LastAttack", DateTime.UtcNow.Millisecond);
            }

            if ((Int32)client.GetData("LastAttack") + 2500 > DateTime.UtcNow.Millisecond)
                return;

            client.SetData("LastAttack", DateTime.UtcNow.Millisecond);

            if (Talent.Skillcheck.CheckStrAgainstOpponent(client, target))
            {
                int damage = Talent.Dice.RollDamage(10, 0);
                //target.SendChatMessage($"You were attacked by {client.Name} and he hit you.");
                target.TriggerEvent("eventLastDamage", damage);
                client.TriggerEvent("eventTargetDamage", damage);
                //client.SendChatMessage($"You attacked {target.Name} and hit him.");
                target.Health -= damage;
                return;
            }

            target.TriggerEvent("eventLastDamage", 0);
            client.TriggerEvent("eventTargetDamage", 0);
        }


    }
}