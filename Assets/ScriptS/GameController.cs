using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType { Normal, WorldTilt }
public enum WallType { Normal, Punishing }

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ControlType controlType;
    public WallType wallType;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    //To toggle between punishing walls on or off
    public void ToggleWallType(bool _punishing)
    {
        if (_punishing)
            wallType = WallType.Punishing;
        else
            wallType = WallType.Normal;
    }
    //Toggles our control type between world tilt and normal
    public void ToggleWorldTilt(bool _tilt)
    {
        if (_tilt)
            controlType = ControlType.WorldTilt;
        else
            controlType = ControlType.Normal;
    }
}

