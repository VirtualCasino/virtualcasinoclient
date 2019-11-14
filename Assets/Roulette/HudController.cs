using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class HudController : MonoBehaviour
{

    public GameObject result;
    public GameObject cash;
    public GameObject bet;
    public GameObject fieldBets;

    private int cashValue = 100;
    private int betValue = 1;
    private int currentBetValue = 1;

    HttpClient httpClient;

    void Start()
    {
        cash.GetComponent<TextMeshProUGUI>().text = cashValue.ToString();
        bet.GetComponent<TextMeshProUGUI>().text = betValue.ToString();
        fieldBets.GetComponent<TextMeshProUGUI>().text = "";
    }

   public void actualizeFieldBets(List<PlayerBet> playersBets)
    {//TODO: zamiast Id player wyświetlać nicki, nie znalazłem endpointu żeby je dostać. A z eventów nie obsługiwałem bo nie zdążyłem.
        if (playersBets.Count > 0)
        {
            List<string> values = new List<string>();
            playersBets.ForEach(playerBet => values.Add(playerBet.ToString()));
            var value = string.Join("/n", values);
            fieldBets.GetComponent<TextMeshProUGUI>().text = value;
        }
        else
        {
            fieldBets.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void increaseBetValue()
    {
        if (betValue + 1 <= cashValue)
        {
            betValue++;
            currentBetValue++;
            bet.GetComponent<TextMeshProUGUI>().text = betValue.ToString();
        }
    }

    public void setBetValueToPlacedBetValue(int value)
    {
            currentBetValue = betValue;
            betValue = value;
            bet.GetComponent<TextMeshProUGUI>().text = betValue.ToString();
    }

    public void resetBetValueToCurrent()
    {
        betValue = currentBetValue;
        bet.GetComponent<TextMeshProUGUI>().text = betValue.ToString();
    }

    public void decreaseBetValue()
    {
        if (betValue - 1 > 0)
        {
            betValue--;
            currentBetValue--;
            bet.GetComponent<TextMeshProUGUI>().text = betValue.ToString();
        }
    }

    public int getBetValue()
    {
        return betValue;
    }

    public void actualizeHudAfterBetPlaced()
    {
        cashValue -= betValue;
        cash.GetComponent<TextMeshProUGUI>().text = cashValue.ToString();
    }

    public void actualizeHudAfterBetCanceled(int value)
    {
        cashValue += value;
        cash.GetComponent<TextMeshProUGUI>().text = cashValue.ToString();
    }
}