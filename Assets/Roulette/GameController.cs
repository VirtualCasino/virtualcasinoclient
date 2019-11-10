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
    public GameObject result;
    public GameObject coins;

    public GameObject cash;
    public GameObject betValue;

    private int cashValue = 100;
    private int betValueI = 10;

    private List<Transform> bets = new List<Transform>();
    private string playerChoose;
    HttpClient httpClient;

    void Start()
    {
        initHudView();
        initCursor(); 
        httpClient = new HttpClient();
        //initActualPlayersBets();
    }

    private void initHudView()
    {
        cash.GetComponent<TextMeshProUGUI>().text = cashValue.ToString();
        betValue.GetComponent<TextMeshProUGUI>().text = betValueI.ToString();
    }

    private void initCursor()
    {
        Transform start = gameFields.transform.Find("NUMBER_0");
        cursor.transform.SetParent(start);
        cursor.transform.position = start.position;
    }

    private void initActualPlayersBets()
    {
        Transform start = gameFields.transform.Find("NUMBER_0");
        doOtherPlayerBet(start.gameObject);
        Transform start20 = gameFields.transform.Find("NUMBER_20");
        doOtherPlayerBet(start20.gameObject);
        Transform start13 = gameFields.transform.Find("NUMBER_13");
        doOtherPlayerBet(start13.gameObject);
    }

    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    void Update()
    {
        myTime = myTime + Time.deltaTime; 

        if (Input.GetButton("Fire1") && myTime > nextFire / 20 && isNotEndGame())
        {
            nextFire = myTime + fireDelta;
            fieldChooser.MoveCursorToClosestFieldOnThe(Side.LEFT);
        }
        if (Input.GetButton("Fire2") && myTime > nextFire / 20 && isNotEndGame())
        {
            nextFire = myTime + fireDelta;
            fieldChooser.MoveCursorToClosestFieldOnThe(Side.RIGHT);
        }
        if (Input.GetButton("Fire3") && myTime > nextFire / 10 && isNotEndGame())
        {
            nextFire = myTime + fireDelta;
            if (!isMyBetOn())
            {
                doMyBet();
            }
            else
            {
                removeBet();
            }
        }

        if (Input.GetButton("Jump") && myTime > nextFire / 10 && isNotEndGame())
        {
            nextFire = myTime + fireDelta;
            fieldChooser.MoveCursorToClosestFieldOnThe(Side.UP);
        }

        if (Input.GetButton("Submit") && myTime > nextFire / 10 && isNotEndGame())
        {
            nextFire = myTime + fireDelta;
            fieldChooser.MoveCursorToClosestFieldOnThe(Side.DOWN);
        }
        nextFire = nextFire - myTime;
        myTime = 0.0F;
    }

    private bool isNotEndGame()
    {
        return true;//!string.Equals(countText.GetComponent<TextMeshProUGUI>().text, "0") && !string.Equals(countText.GetComponent<TextMeshProUGUI>().text, randomResult.ToString());
    }

    private void doMyBet()
    {
        int tokensCount = 10;
        string clientId = PlayerPrefs.GetString("Id");
        string tableId = PlayerPrefs.GetString("TableId");
        PlaceRouletteBet placeRouletteBet = new PlaceRouletteBet(new RouletteGameId(tableId), new RoulettePlayerId(clientId), cursor.transform.name, tokensCount);
        string placeRouletteBetJson = JsonUtility.ToJson(placeRouletteBet) ?? "";
        StartCoroutine(placeRouletteBetCorutine(placeRouletteBetJson));
        doBet(cursor, MY_BET_TAG);
        CursorView.setAsHasBets(cursor);
        cash.GetComponent<TextMeshProUGUI>().text = (cashValue - betValueI).ToString();

        cashValue -= betValueI;
    }

    private IEnumerator placeRouletteBetCorutine(string placeRouletteBetJson)
    {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Post("/virtual-casino/roulette-game/games/bets", placeRouletteBetJson), (output) => result = output);
    }

    private void doOtherPlayerBet(GameObject field)
    {
        doBet(field, BET_TAG);
    }

    private void doBet(GameObject field, string tag)
    {

        var newBet = Instantiate(coins, field.transform.position, coins.transform.rotation).transform;
        newBet.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        newBet.tag = tag;
        bets.Add(newBet);
        refreshBetsView();
    }


private void removeBet()
    {
        var cursorPosition = cursor.transform.position;
        GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag(MY_BET_TAG);
        foreach (GameObject current in allMovableThings)
        {
            if (current.transform.position == cursorPosition)
            {
                int indexOfElementToRemove = -1;
                bets.ForEach(bet =>
                {
                    if (bet.position == cursorPosition && bet.tag == "myBet")
                    {
                        indexOfElementToRemove = bets.IndexOf(bet);
                        Debug.Log("REMOVED!!!!");
                    }
                });
                bets.RemoveAt(indexOfElementToRemove);
                Destroy(current);
                CursorView.setAsNoBets(cursor);
            }
        }
        cash.GetComponent<TextMeshProUGUI>().text = (cashValue + betValueI).ToString();

        cashValue += betValueI;
    }

    private IEnumerator cancelRouletteBetCorutine(string cancelRouletteBetJson)
    {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Delete("/virtual-casino/roulette-game/games/bets", cancelRouletteBetJson), (output) => result = output);
    }

    private void refreshBetsView()
    {
        showBets();
    }

    private void showBets()
    {
           bets.ForEach(bet => {
               if (checkIfPosEmpty(bet.position))
               {
                   var newBet = Instantiate(coins, bet.position, bet.rotation, bet);
               }
            });
    }

    private bool isMyBetOn()
    {
        return !checkIfPosEmpty(cursor.transform.position);
    }

    private bool checkIfPosEmpty(Vector3 targetPos)
    {
        GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag(MY_BET_TAG);
        foreach (GameObject current in allMovableThings)
        {
            if (current.transform.position == targetPos)
                return false;
        }
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

/*   float timeLeft = 11.0f;
    public System.Random random = new System.Random();
    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;


        }
        yield return new WaitForSeconds(1.0f);
        randomResult = random.Next(1, 37);
        countText.GetComponent<TextMeshProUGUI>().text = randomResult.ToString();
        SpinResult();
    }

    private void SpinResult()
    {//TODO: Zmienić na podliczenie z serwera.
        result.GetComponent<TextMeshProUGUI>().text = position.ToString().Equals(randomResult) ? "You won!!!" : PlayerPrefs.GetString("Name") + "You are loser :(";
    }
    const float V1 = 0.5f;
        MoveCursorToClosestFieldOnThe(Side.RIGHT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.RIGHT);
       
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.UP);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.UP);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.RIGHT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.UP);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.DOWN);
        yield return new WaitForSeconds(V1);
        currCountdownValue = countdownValue;
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.LEFT);
        yield return new WaitForSeconds(V1);
        doBet(cursor);
        MoveCursorToClosestFieldOnThe(Side.RIGHT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.RIGHT);
        yield return new WaitForSeconds(V1);
        MoveCursorToClosestFieldOnThe(Side.UP);
        yield return new WaitForSeconds(V1);
        */
