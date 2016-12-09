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

    private void ApplyPlayerSettings()
    {
        var playerObject = GameObject.Find("Player");
        var playerObjectComponent = playerObject.GetComponent<PlayerObject>();

        float newNavMeshRadius;

        if (playerObjectComponent.AllowMovement)
            newNavMeshRadius = 1.5f;
        else
            newNavMeshRadius = 4.5f;

        playerObject.GetComponent<NavMeshObstacle>().radius = newNavMeshRadius;
        playerObject.GetComponent<FPSInputController>().enabled = playerObjectComponent.AllowMovement;
    }
}                                                                                                      