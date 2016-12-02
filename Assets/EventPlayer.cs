using System.Linq;
using UnityEngine;

namespace Assets
{
    public class EventPlayer
    {
        public static void PlayEventAmbience(EventObject eventObject)
        {
            GetAudioSource(eventObject.gameObject).Play();
            switch (eventObject.AnimationName)
            {
                  case "light_switch":
                    SwitchLights();
                    break;
                  case "AlarmBell":
                    SetLightColor(Color.red);
                    break;
            }
        }

        public static void RemoveAmbience(EventObject eventObject)
        {
            GetAudioSource(eventObject.gameObject).Stop();
            switch (eventObject.AnimationName)
            {
                //no case for lightswitching since those can be, well, switched
                case "AlarmBell":
                    SetLightColor(Color.white);
                    break;
            }
        }

        //NOTE: You do need to set the audiosource to loop or not loop in the unity editor yourself.
        private static AudioSource GetAudioSource(GameObject gameObject)
        {
            return gameObject.GetComponent<AudioSource>();
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