﻿﻿﻿using System.Runtime.InteropServices;
 using UnityEngine;

namespace Assets
{
    public class PlayerObject : MonoBehaviour
    {
        public bool AllowMovement;

        public static PlayerObject Instantiate()
        {
            var playerGameObjectModelName = GameObject.Find("Player");
            var playerObject = playerGameObjectModelName.GetComponent<PlayerObject>();
            return playerObject;
        }

        public void TriggerPlayerEvent(EventObject eventObject)
        {
            if (!EventController.ActiveEvents.ContainsKey(eventObject.Id))
            {
                EventController.ActiveEvents.Add(eventObject.Id, eventObject);
                EventPlayer.PlayEventAmbience(eventObject);

                //Events that have npc's associated with them as actors have IsReady set by those actors.
                if (eventObject.NpcActionIds.Count == 0)
                    eventObject.IsReady = true;
            }
            else
            {
                EventController.ActiveEvents.Remove(eventObject.Id);
                eventObject.IsReady = false;
            }
        }
    }
}
                                                                                                                                                                                                                                                            