using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public FieldChooser fieldChooser;
    public GameObject gameFields;
    public GameObject cursor; 
    public GameObject countText;
    public GameObject result;
    public GameObject coins;

    private List<Transform> bets = new List<Transform>();
    private string playerChoose;
    private int position;
    private int randomResult = 0;

    void Start()
    {
        initHudView();
        initCursor();
        initActualPlayersBets();
    }

    private void initHudView()
    {
        countText.GetComponent<TextMeshProUGUI>().text = "11";
        StartCoroutine(StartCountdown(timeLeft));
        position = 0;
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
        doBet(start.gameObject);
        Transform start20 = gameFields.transform.Find("NUMBER_20");
        doBet(start20.gameObject);
        Transform start13 = gameFields.transform.Find("NUMBER_13");
        doBet(start13.gameObject);
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
            doBet(cursor);
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
        return !string.Equals(countText.GetComponent<TextMeshProUGUI>().text, "0") && !string.Equals(countText.GetComponent<TextMeshProUGUI>().text, randomResult.ToString());
    }

    private void doBet(GameObject field)
    {
        var rotation = coins.transform.rotation;
        rotation.y += Random.Range(-10.0f, 10.0f);
        var newBet = Instantiate(coins, field.transform.position, coins.transform.rotation).transform;
        newBet.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        newBet.tag = "bet";
        bets.Add(newBet);
        Debug.Log("Bets count" + bets.Count);
        refreshBetsView();
        CursorView.setAsHasBets(cursor);
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

    private bool checkIfPosEmpty(Vector3 targetPos)
    {
        GameObject[] allMovableThings = GameObject.FindGameObjectsWithTag("bet");
        foreach (GameObject current in allMovableThings)
        {
            if (current.transform.position == targetPos)
                return false;
        }
        return true;
    }

    float timeLeft = 11.0f;
    public System.Random random = new System.Random();
    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            countText.GetComponent<TextMeshProUGUI>().text = currCountdownValue.ToString();

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
}


/*  const float V1 = 0.5f;
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
