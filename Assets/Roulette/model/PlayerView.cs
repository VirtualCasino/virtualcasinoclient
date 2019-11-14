using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class PlayerView
{

    public PlayerView(string playerViewId, int tokensCount, List<BetView> betsViews)
    {
        this.playerViewId = playerViewId;
        this.tokensCount = tokensCount;
        this.betsViews = betsViews;
    }

    public string playerViewId;
    public int tokensCount;
    public List<BetView> betsViews;
}