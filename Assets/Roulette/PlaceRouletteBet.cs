using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class PlaceRouletteBet
{

    public PlaceRouletteBet(RouletteGameId rouletteGameId, RoulettePlayerId playerId, string field, int value)
    {
        this.gameId = gameId;
        this.playerId = playerId;
        this.field = field;
        this.value = value;
    }

    public RouletteGameId gameId;
    public RoulettePlayerId playerId;
    public string field;
    public int value;
}
