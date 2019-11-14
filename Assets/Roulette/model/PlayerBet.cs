using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class PlayerBet : IComparable<PlayerBet>
{

    public PlayerBet(string playerId, int value)
    {
        this.playerId = playerId;
        this.value = value;
    }

    public string playerId;
    public int value;

    public int CompareTo(PlayerBet other)
    {
        if (other == null)
        {
            return 1;
        }

        return value - other.value;
    }

    public int GetValue()
    {
        return value;
    }
}
