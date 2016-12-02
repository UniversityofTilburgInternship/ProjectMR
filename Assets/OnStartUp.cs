﻿using UnityEngine;

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
    PlayerParser.ParsePersons();
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                    