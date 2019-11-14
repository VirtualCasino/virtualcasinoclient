using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private string MY_BET_TAG = "myBet";
    private string BET_TAG = "bet";

    public FieldChooser fieldChooser;
    public GameObject gameFields;
    public GameObject cursor;
    public GameObject coins;

    private List<Transform> bets = new List<Transform>();
    Dictionary<string, List<PlayerBet>> allBets = new Dictionary<string, List<PlayerBet>>();
    private string playerChoose;
    HttpClient httpClient;
    HudController hudController;
    BetController betController;

    void Start()
    {
        initHudView();
        initBetController();
        initCursor();
        httpClient = new HttpClient();
        StartCoroutine(getGameView());
    }

    private void initHudView()
    {
        GameObject gameObject = GameObject.Find("HUD");
        hudController = gameObject.GetComponent<HudController>();
    }

    private void initBetController()
    {
        GameObject gameObject = GameObject.Find("BetController");
        betController = gameObject.GetComponent<BetController>();
    }

    private void initCursor()
    {
        Transform start = gameFields.transform.Find("NUMBER_0");
        cursor.transform.SetParent(start);
        cursor.transform.position = start.position;
    }

    private IEnumerator getGameView()
    {
        Debug.Log("GameViewId");
        string gameId = PlayerPrefs.GetString("TableId");
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Get("/virtual-casino/casino-services/roulette-games/" + gameId), (output) => result = output);
        Debug.Log("Got GameView: " + result.response);
        gotGameView(result);
    }

    private void gotGameView(HttpResponse result)
    {
        RouletteGameView rouletteGameView = JsonUtility.FromJson<RouletteGameView>(result.response);
        betController.init(rouletteGameView.GetPlayersViews());
    }

    private void initActualPlayersBets()
    {
        Transform start = gameFields.transform.Find("NUMBER_0");
        //doOtherPlayerBet(start.gameObject);
        Transform start20 = gameFields.transform.Find("NUMBER_20");
        //doOtherPlayerBet(start20.gameObject);
        Transform start13 = gameFields.transform.Find("NUMBER_13");
        //doOtherPlayerBet(start13.gameObject);
    }

    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    void Update()
    {
        myTime = myTime + Time.deltaTime;
        if (isNotGameBoardBlocked())
        {
            if (Input.GetButton("Fire1") && myTime > nextFire / 20)
            {
                nextFire = myTime + fireDelta;
                fieldChooser.MoveCursorToClosestFieldOnThe(Side.LEFT);
                betController.setCurrentBet();
            }
            if (Input.GetButton("Fire2") && myTime > nextFire / 20)
            {
                nextFire = myTime + fireDelta;
                fieldChooser.MoveCursorToClosestFieldOnThe(Side.RIGHT);
                betController.setCurrentBet();
            }
            if (Input.GetButton("Fire3") && myTime > nextFire / 10)
            {
                nextFire = myTime + fireDelta;
                betController.bet();
            }

            if (Input.GetButton("Jump") && myTime > nextFire / 10)
            {
                nextFire = myTime + fireDelta;
                fieldChooser.MoveCursorToClosestFieldOnThe(Side.UP);
                betController.setCurrentBet();
            }

            if (Input.GetButton("Submit") && myTime > nextFire / 10)
            {
                nextFire = myTime + fireDelta;
                fieldChooser.MoveCursorToClosestFieldOnThe(Side.DOWN);
                betController.setCurrentBet();
            }

            if (Input.GetButton("Increase") && myTime > nextFire / 5)
            {
                hudController.increaseBetValue();
            }

            if (Input.GetButton("Decrease") && myTime > nextFire / 5)
            {
                hudController.decreaseBetValue();
            }
        }
        nextFire = nextFire - myTime;
        myTime = 0.0F;
    }

    private bool isNotGameBoardBlocked()
    {
        return true;
    }

    public static IEnumerator Run<T>(IEnumerator target, Action<T> output)
    {
        object result = null;
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
        output((T)result);
    }
}