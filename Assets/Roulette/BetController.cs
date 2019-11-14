using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BetController : MonoBehaviour
{
    public GameObject cursor;
    public GameObject coins;
    private Dictionary<string, List<PlayerBet>> allBets = new Dictionary<string, List<PlayerBet>>();
    private List<GameObject> betsCoins = new List<GameObject>();
    private HudController hudController;
    public FieldChooser fieldChooser;
    HttpClient httpClient;

    void Start()
    {
        httpClient = new HttpClient();
        initHudView();
    }

    public void init(List<PlayerView> playersViews)
    {
        supplyTableBets(playersViews);
    }

    private void initHudView()
    {
        GameObject gameObject = GameObject.Find("HUD");
        hudController = gameObject.GetComponent<HudController>();
    }

    private void supplyTableBets(List<PlayerView> playersViews)
    {

    }

    public void bet()
    {
        if (isMyBetOn())
        {
            if (anyBetsOnCurrentFieldExists())
            {
                PlayerBet myBet = allBets[fieldChooser.getFieldName()].Find(x => x.playerId.Contains(PlayerPrefs.GetString("Id")));
                hudController.setBetValueToPlacedBetValue(myBet.GetValue());
                cancelBet();
            }
        }
        else
        {
            doBet();
        }
    }

    public void doBet()
    {
        StartCoroutine(placeRouletteBetCorutine(getBetRequestParams()));
        CursorView.setAsHasYourBet(cursor);

        List<PlayerBet> tempPlayersBets = null;
        var fieldName = fieldChooser.getFieldName();
        if (anyBetsOnCurrentFieldExists())
        {
            allBets[fieldName].Add(new PlayerBet(PlayerPrefs.GetString("Id"), hudController.getBetValue()));
        }
        else
        {
            var playersBet = new List<PlayerBet>();
            playersBet.Add(new PlayerBet(PlayerPrefs.GetString("Id"), hudController.getBetValue()));
            allBets.Add(fieldName, playersBet);
            var field = GameObject.Find(fieldName);
            var newBetCoins = Instantiate(coins, field.transform.position, coins.transform.rotation);
            newBetCoins.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            betsCoins.Add(newBetCoins);
            refreshBetsView();

            Debug.Log("NewBetAddedToDictionary: " + allBets[fieldName][0].value.ToString());
        }
        hudController.actualizeHudAfterBetPlaced();
    }

    private string getBetRequestParams()
    {
        int tokensCount = hudController.getBetValue();
        string clientId = PlayerPrefs.GetString("Id");
        RoulettePlayerId roulettePlayerId = new RoulettePlayerId(clientId);
        string tableId = PlayerPrefs.GetString("TableId");
        RouletteGameId rouletteGameId = new RouletteGameId(tableId);

        PlaceRouletteBet placeRouletteBet = new PlaceRouletteBet(rouletteGameId, roulettePlayerId, cursor.transform.name, tokensCount);
        return JsonUtility.ToJson(placeRouletteBet) ?? "";
    }

    private IEnumerator placeRouletteBetCorutine(string placeRouletteBetJson)
    {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Post("/virtual-casino/roulette-game/games/bets", placeRouletteBetJson), (output) => result = output);
    }

    public void cancelBet()
    {
        var fieldName = fieldChooser.getFieldName();
        var tempPlayersBets = new List<PlayerBet>();
        if (anyBetsOnCurrentFieldExists())
        {
            Debug.Log("Bets size " + allBets[fieldName].Count);
            PlayerBet myBet = allBets[fieldName].Find(x => x.playerId.Contains(PlayerPrefs.GetString("Id")));
            StartCoroutine(cancelRouletteBetCorutine(getCancelBetRequestParams()));
            allBets[fieldName].Remove(myBet);
            Debug.Log("Bets size after rm " + allBets[fieldName].Count);
            Debug.Log("REMOVED!!!!");
            CursorView.setAsHasntYourBet(cursor);
            if (allBets[fieldName].Count == 0)
            {
                var betCoins = betsCoins.Find(x => object.Equals(x.transform.position, GameObject.Find(fieldName).transform.position));
                betsCoins.Remove(betCoins);
                allBets.Remove(fieldName);
                Destroy(betCoins);
            }
            hudController.actualizeHudAfterBetCanceled(myBet.GetValue());
        }
      
    }

    private string getCancelBetRequestParams()
    {
        string clientId = PlayerPrefs.GetString("Id");
        string tableId = PlayerPrefs.GetString("TableId");
        CancelRouletteBet cancelRouletteBet = new CancelRouletteBet(new RouletteGameId(tableId), new RoulettePlayerId(clientId), cursor.transform.name);
        return JsonUtility.ToJson(cancelRouletteBet) ?? "";
    }

    private IEnumerator cancelRouletteBetCorutine(string cancelRouletteBetJson)
    {
        HttpResponse result = null;
        yield return Run<HttpResponse>(httpClient.Delete("/virtual-casino/roulette-game/games/bets", cancelRouletteBetJson), (output) => result = output);
    }

    private void refreshBetsView()
    {
        //showBets();
    }

    private void showBets()
    {
        betsCoins.ForEach(bet =>
        {
            if (positionHasntCoins(bet.transform.position))
            {
                var newBet = Instantiate(coins, bet.transform.position, bet.transform.rotation, bet.transform);
            }
        });
    }

    public bool positionHasntCoins(Vector3 currentPosition)
    {
        var betCoins = betsCoins.Find(x => object.Equals(x.transform.position, currentPosition)); 
        return object.Equals(betCoins, null);
    }

    public void setCurrentBet()
    {
        if (anyBetsOnCurrentFieldExists())
        {
            PlayerBet myBet = allBets[fieldChooser.getFieldName()].Find(x => x.playerId.Contains(PlayerPrefs.GetString("Id")));
            if (object.Equals(myBet, null))
            {
                hudController.resetBetValueToCurrent();
            }
            else
            {
                hudController.setBetValueToPlacedBetValue(myBet.GetValue());
            }
        }
    }

    public List<PlayerBet> getCurrentFieldBets()
    {
        if (anyBetsOnCurrentFieldExists())
        {
            return allBets[fieldChooser.getFieldName()];
            }
        return new List<PlayerBet>();
    }

    private bool isMyBetOn()
    {
        if (anyBetsOnCurrentFieldExists())
        {
            PlayerBet myBet = allBets[fieldChooser.getFieldName()].Find(x => x.playerId.Contains(PlayerPrefs.GetString("Id")));
            return !object.Equals(myBet, null);
        }
        return false;
    }

    private bool anyBetsOnCurrentFieldExists()
    {
        return allBets.ContainsKey(fieldChooser.getFieldName());
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
