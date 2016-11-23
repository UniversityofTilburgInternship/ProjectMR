﻿using Assets;
using UnityEngine;

public class UnityPlayer : MonoBehaviour
{
    private PlayerObject _playerObject;

    public Vector3 Position
    {
        get { return _playerObject.transform.position; }
    }

    public static UnityPlayer Spawn()
    {
        var unityPlayer = new UnityPlayer {_playerObject = PlayerObject.Instantiate()};
        return unityPlayer;
    }

    public bool IsLookingAt(GameObject Object)
    {
        return _playerObject.IsLookingAt(Object);
    }

    public void TriggerEvent(EventObject eventObject)
    {
        _playerObject.TriggerEvent(eventObject);

    }
}                                                                                     