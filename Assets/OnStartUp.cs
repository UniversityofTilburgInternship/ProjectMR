﻿using UnityEngine;

public class OnStartUp : MonoBehaviour
{
  void Awake()
  {
      ActionsParser.ParseEventsActions();
      ActionsParser.ParseInteractions();
      ActionsParser.ParseNormalActions();
      ActionsParser.ParseEvents();
      ActionsParser.ParseReactions();

      //This parses the spawnpositions and modelnames from settings.xml
      SettingsParser.ParseAll();
      PlayerParser.ParsePersons();
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              