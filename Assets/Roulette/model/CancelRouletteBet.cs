using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class CancelRouletteBet
{

    public CancelRouletteBet(RouletteGameId rouletteGameId, RoulettePlayerId playerId, string field)
    {
        this.gameId = gameId;
        this.playerId = playerId;
        this.field = field;
    }

    public RouletteGameId gameId;
    public RoulettePlayerId playerId;
    public string field;
}
