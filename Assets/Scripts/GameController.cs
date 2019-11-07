using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private string playerChoose;
    private int position;
    private int randomResult = 0;
    public GameObject gameFields;
    public GameObject cursor;

    public GameObject coins;

    public List<Transform> bets = new List<Transform>();

    void Awake()
    {
        playerChoose = "X";
    }

    public Text countText;
    public Text result;

    float timeLeft = 11.0f;
    // Start is called before the first frame update
    void Start()
    {
        Transform start = gameFields.transform.Find("NUMBER_0");

        Transform start20 = gameFields.transform.Find("NUMBER_20");

        Transform start13 = gameFields.transform.Find("NUMBER_13");

        doBet(start.gameObject);
        doBet(start20.gameObject);
        doBet(start13.gameObject);
        countText.text = "11";
        StartCoroutine(StartCountdown(timeLeft));
        position = 0;
        showBets();
    }

    // Update is called once per frame

    public float fireDelta = 0.5F;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    void Update()
    {
        myTime = myTime + Time.deltaTime; 

        if (Input.GetButton("Fire1") && myTime > nextFire / 20 )
        {
            nextFire = myTime + fireDelta;
            MoveCursorToClosestFieldOnThe(Side.LEFT);
        }
        if (Input.GetButton("Fire2") && myTime > nextFire / 20)
        {
            nextFire = myTime + fireDelta;
            MoveCursorToClosestFieldOnThe(Side.RIGHT);
        }
        if (Input.GetButton("Fire3") && myTime > nextFire / 10)
        {
            nextFire = myTime + fireDelta;
            doBet(cursor);
        }

        if (Input.GetButton("Jump") && myTime > nextFire / 10)
        {
            nextFire = myTime + fireDelta;
            MoveCursorToClosestFieldOnThe(Side.UP);
        }

        if (Input.GetButton("Submit") && myTime > nextFire / 10)
        {
            nextFire = myTime + fireDelta;
            MoveCursorToClosestFieldOnThe(Side.DOWN);
        }
        nextFire = nextFire - myTime;
        myTime = 0.0F;
    }


    //private bool isNotFirstField()
    //{
    //    return position > 0;
    //}

    //private bool isNotLastField()
    //{
    //    return position < 35;
    //}

    //private bool isNotEndGame()
    //{
    //    return !string.Equals(countText.text, "0") && !string.Equals(countText.text, randomResult.ToString());
    //}

    public System.Random random = new System.Random();
    float currCountdownValue;
    public IEnumerator StartCountdown(float countdownValue)
    {
        Transform start = gameFields.transform.Find("NUMBER_0");
        Debug.Log("JEST!!!!!!!!!!!!" + start.name);
        cursor.transform.SetParent(start);
        cursor.transform.position = start.position;
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
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            countText.text = currCountdownValue.ToString();

        }
        yield return new WaitForSeconds(1.0f);
        randomResult = random.Next(1, 37);
        countText.text = randomResult.ToString();
        result.text = position.ToString().Equals(randomResult) ? "Wygrałeś!!!" : PlayerPrefs.GetString("Name") + "Przegrałeś :(";
    }

    void MoveCursorToClosestFieldOnThe(Side side)
    {
        switch (side)
        {
            case Side.LEFT:
                getClosestFieldOnTheLeft();
                break;
            case Side.RIGHT:
                getClosestFieldOnTheRight();
                break;
            case Side.UP:
                getClosestFieldOnTheUp();
                break;
            case Side.DOWN:
                getClosestFieldOnTheDown();
                break;
            default:
                print("Incorrect way.");
                break;
        }
    }

    void getClosestFieldOnTheLeft()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.x < -0.1)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        if (!object.Equals(bestTarget, null))
        {
            Debug.Log("NAME: " + bestTarget.name);
            cursor.transform.position = bestTarget.position;
            if (checkIfPosEmpty(bestTarget.position))
            {
                setAsNoBets(cursor);
            }
            else
            {
                setAsHasBets(cursor);
            }
        }
        else
        {
            Debug.Log("WRONG WAY!!!!!!!!!!!!!!" + Side.LEFT);
        }
    }

    void getClosestFieldOnTheRight()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.x > 0.1)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        if (!object.Equals(bestTarget, null))
        {
            Debug.Log("NAME: " + bestTarget.name);
            cursor.transform.position = bestTarget.position;
            if (checkIfPosEmpty(bestTarget.position))
            {
                setAsNoBets(cursor);
            }
            else
            {
                setAsHasBets(cursor);
            }
        }
        else
        {
            Debug.Log("WRONG WAY!!!!!!!!!!!!!!" + Side.RIGHT);
        }
    }

    void getClosestFieldOnTheUp()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.z > 0.1)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        if (!object.Equals(bestTarget, null))
        {
            Debug.Log("NAME: " + bestTarget.name);
            cursor.transform.position = bestTarget.position; 
            if (checkIfPosEmpty(bestTarget.position))
            {
                setAsNoBets(cursor);
            }
            else
            {
                setAsHasBets(cursor);
            }
        }
        else
        {
            Debug.Log("WRONG WAY!!!!!!!!!!!!!!" + Side.UP);
        }
    }

    void getClosestFieldOnTheDown()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform field in gameFields.GetComponentsInChildren<Transform>())
        {
            if (!object.Equals(field.position, cursor.transform.position))
            {
                Vector3 directionToTarget = field.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr && directionToTarget.z < -0.1)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = field;
                }
            }
        }
        if (!object.Equals(bestTarget, null))
        {
            Debug.Log("NAME: " + bestTarget.name);
        cursor.transform.position = bestTarget.position;
            if(checkIfPosEmpty(bestTarget.position))
            {
                setAsNoBets(cursor);
            }
            else
            {
                setAsHasBets(cursor);
            }
        }
        else
        {
            Debug.Log("WRONG WAY!!!!!!!!!!!!!!" + Side.DOWN);
        }
    }

    private void setAsNoBets(GameObject field)
    {
        var fieldRenderer = cursor.GetComponent<SpriteRenderer>();
        var tempColor = fieldRenderer.color;
        tempColor = Color.green;
        tempColor.a = 0.4f;
        fieldRenderer.color = tempColor;
    }

    private void setAsHasBets(GameObject field)
    {
        var fieldRenderer = cursor.GetComponent<SpriteRenderer>();
        var tempColor = fieldRenderer.color;
        tempColor = Color.red;
        tempColor.a = 0.4f;
        fieldRenderer.color = tempColor;
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
        setAsHasBets(cursor);
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
                  //newBet.tag = "bet";
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
}

enum Side { LEFT, RIGHT, UP, DOWN };