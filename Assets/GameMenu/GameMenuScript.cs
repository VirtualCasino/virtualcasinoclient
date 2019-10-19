using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;

public class GameMenuScript : MonoBehaviour
{
    
    public GameObject nickValue;
    public GameObject tokensValue;

    public void OnEnable()
    {
        string nick = PlayerPrefs.GetString("Nick");
        int tokensCount = PlayerPrefs.GetInt("Tokens");
        nickValue.GetComponent<TextMeshProUGUI>().text = nick;
        tokensValue.GetComponent<TextMeshProUGUI>().text = Convert.ToString(tokensCount);
    }

    public void createGame() {
        
    }

}
