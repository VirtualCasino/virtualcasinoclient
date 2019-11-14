using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class RouletteGameView
{

    public RouletteGameView(string id, string rouletteGameViewId, List<PlayerView> playersViews, string spinState)
    {
        this.id = id;
        this.rouletteGameViewId = rouletteGameViewId;
        this.playersViews = playersViews;
        this.spinState = spinState;
    }

    public string id;
    public string rouletteGameViewId;
    public List<PlayerView> playersViews;
    public string spinState;

    public string GetId()
    {
        return id;
    }

    public List<PlayerView> GetPlayersViews()
    {
        return playersViews;
    }
}