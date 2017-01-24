using System.Collections.Generic;
using System.Linq;
using Assets;
using Casanova.Prelude;
using UnityEngine;

public class WandController : MonoBehaviour
{
    public void Update()
    {
        var allControllerButtons = gameObject.GetComponents<ControllerButton>();
        foreach (var controllerButton in allControllerButtons)
        {
            var WandKeyPressed = controllerButton.isPressed();
            Debug.Log("Wandkeypressed: " + WandKeyPressed);
            if (Input.GetMouseButtonDown(0))
                WandKeyPressed = 1;

            if (WandKeyPressed > 0 && WandKeyPressed < 4)
            {
                //the component has to be instantiated first by CNV so we cant make triggerPlayerEvent() static
                var playerGameObject = GameObject.Find("Player");
                UnityPlayer unityPlayer = playerGameObject.GetComponent<UnityPlayer>();

                unityPlayer.TriggerPlayerEvent(WandKeyPressed - 1);
            }
        }

    }

//    private ControllerButton GetPressedWandKeyIfAny()
//    {
//        Debug.Log("AnyWandKeyPressed()");
//        var allControllerButtons = gameObject.GetComponents<ControllerButton>();
//        var allPressedControllerButtons = allControllerButtons.Where(x => x.isPressed()).ToList();
//        Debug.Log("keys pressed: " + allPressedControllerButtons.Count);
//        if (allPressedControllerButtons.Any())
//        {
//            Debug.Log("button has been pressed");
//            return allPressedControllerButtons[Random.Range(0, allPressedControllerButtons.Count)];
//        }
//        return null;
//    }
}