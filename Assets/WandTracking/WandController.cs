using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;

    public class WandController : MonoBehaviour
    {
        public void Update()
        {
            var WandKeyPressed = GetPressedWandKeyIfAny();


            if (WandKeyPressed != null)
            {
                var eventForWandKeyId = EventController.PlayerEvents[0];

                //the component has to be instantiated first by CNV so we cant make triggerPlayerEvent() static
                var playerGameObject = GameObject.Find("Player");
                UnityPlayer unityPlayer = playerGameObject.GetComponent<UnityPlayer>();

                unityPlayer.TriggerPlayerEvent(eventForWandKeyId.Id);
            }
        }

        private ControllerButton GetPressedWandKeyIfAny()
        {
            Debug.Log("AnyWandKeyPressed()");
            var allControllerButtons = gameObject.GetComponents<ControllerButton>();
            var allPressedControllerButtons = allControllerButtons.Where(x => x.isPressed()).ToList();

            if (allPressedControllerButtons.Any())
                return allPressedControllerButtons[Random.Range(0, allPressedControllerButtons.Count)];
            else
                return null;
        }
    }
