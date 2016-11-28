﻿using System.Runtime.InteropServices;
 using UnityEngine;

namespace Assets
{
    public class PlayerObject : MonoBehaviour
    {
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
                                                                                                                                                                                                                                               