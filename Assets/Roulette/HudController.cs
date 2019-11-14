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

    private int cashValue = 100;
    private int betValue = 1;
    private int currentBetValue = 1;

    HttpClient httpClient;

    void Start()
    {
        cash.GetComponent<TextMeshProUGUI>().text = cashValue.ToString();
        bet.GetComponent<TextMeshProUGUI>().text = betValue.ToString();
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