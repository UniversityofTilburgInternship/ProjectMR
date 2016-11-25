using System.Linq;
using UnityEngine;

namespace Assets
{
    public class EventPlayer
    {
        public static void PlayEventAmbience(EventObject eventObject)
        {
            //we always play the event's sound
            HelperFunctions.PlaySound(eventObject.gameObject);

            switch (eventObject.AnimationName)
            {
                  case "light_switch":
                    Debug.Log("Case light_switch");
                    SwitchLights();
                    break;
                  case "AlarmBell":
                    SetLightColor(Color.red);
                    break;
            }
        }

        public static void RemoveAmbience(EventObject eventObject)
        {
            switch (eventObject.AnimationName)
            {
                //no case for lightswitching since those can be, well, switched
                case "AlarmBell":
                    SetLightColor(Color.white);
                    break;
            }
        }

        private static void SwitchLights()
        {
            foreach (var light in GetAllLights().Select(x => x.GetComponent<Light>()))
            {
                light.enabled = !light.enabled;
            }
        }

        private static void SetLightColor(Color color)
        {
            foreach (var light in GetAllLights().Select(x => x.GetComponent<Light>()))
            {
                if (!light.color.Equals(color))
                    light.color = color;
            }
        }

        private static GameObject[] GetAllLights()
        {
            return GameObject.FindGameObjectsWithTag("ceiling_light");
        }
    }
}                                                                                                                                                                                        