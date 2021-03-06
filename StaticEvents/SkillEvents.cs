﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mirror.StaticEvents
{
    public static class SkillEvents
    {
        // mp.events.callRemote("EventHandler", "Skill_Event", skillName, target);
        public static void ParseSkillEvent(Client client, params object[] arguments)
        {
            if (arguments.Length < 3) //0, 1, 2
                return;

            string eventName = arguments[0] as string;
            string skillName = arguments[1] as string;

            Vehicle vehicle = null;
            Client target = null;

            switch (skillName)
            {
                case "Medicine":
                    target = arguments[2] as Client;
                    Skills.Intelligence.Medicine.Use(client, target);
                    return;
                case "Drag":
                    target = arguments[2] as Client;
                    Skills.Strength.Drag.Use(client, target);
                    return;
                case "Smash":
                    vehicle = arguments[2] as Vehicle;
                    Skills.Strength.Smash.Use(client, vehicle);
                    return;
                case "Intimidate":
                    target = arguments[2] as Client;
                    Skills.Strength.Intimidate.Use(client, target);
                    return;
            }
        }
    }
}
