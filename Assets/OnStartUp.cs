using Assets;
using UnityEngine;

public class OnStartUp : MonoBehaviour
{
    void Awake()
    {
        ActionsParser.ParseEventsActions();
        ActionsParser.ParseInteractions();
        ActionsParser.ParseReactions();
        ActionsParser.ParseNormalActions();
        ActionsParser.ParseEvents();

        //This parses the spawnpositions and modelnames from settings.xml
        SettingsParser.ParseAll();
        PersonParser.ParsePersons();
        ApplyPlayerSettings();
    }

    private static void ApplyPlayerSettings()
    {
        var playerObject = GameObject.Find("Player");
        var playerObjectComponent = playerObject.GetComponent<PlayerObject>();

        var newNavMeshSize = playerObjectComponent.AllowMovement ? new Vector3(2, 2, 2) : new Vector3(6, 2, 6);

        playerObject.GetComponent<NavMeshObstacle>().size = newNavMeshSize;
        playerObject.GetComponent<FPSInputController>().enabled = playerObjectComponent.AllowMovement;
    }
}      