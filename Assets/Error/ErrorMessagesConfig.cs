using System.Collections;
using System.Collections.Generic;
using System;

public class ErrorMessagesConfig {

    public string defaultErrorMessage = "Invalid server response. Please contact with administrators or try again later";
    public Dictionary<string, string> param = new Dictionary<string, string>();

    public ErrorMessagesConfig() {
        param.Add("internalServerError","Invalid server response. Please contact with administrators or try again later");


        param.Add("casinoServices.client.clientNotExist","Client doesn't exist");
        param.Add("casinoServices.client.clientBusy","Client is busy");
        param.Add("casinoServices.client.clientNotRegistered","Client is not registered");
        param.Add("casinoServices.client.tokensCountMustBePositive","Tokens value must be positive");
        param.Add("casinoServices.client.clientNickNotValid","Your nick should contains between 3 and 10 letters or numbers");

        param.Add("casinoServices.table.tableNotExist","Table doesn't exist");
        param.Add("casinoServices.table.clientAlreadyParticipated","Client already participated to this table");
        param.Add("casinoServices.table.tableAlreadyReserved","Table is already reserved");
        param.Add("casinoServices.table.tableClosed","Table is already closed");
        param.Add("casinoServices.table.tableFull","Table is already full");
        param.Add("casinoServices.table.tableNotReserved","Table is not reserved");


        param.Add("rouletteGame.rouletteGame.betNotExist","Bet doesn't exist");
        param.Add("rouletteGame.rouletteGame.bettingTimeEndMustBeFuture","Betting time must be in future");
        param.Add("rouletteGame.rouletteGame.bettingTimeExceeded","Beting time is ended");
        param.Add("rouletteGame.rouletteGame.bettingTimeNotEndedYet","Beting time is not ended yet");
        param.Add("rouletteGame.rouletteGame.betValueMustBePositive","Bet value must be positive");
        param.Add("rouletteGame.rouletteGame.currentSpinNotFinished","Current spin is not finished yet");
        param.Add("rouletteGame.rouletteGame.placedBetsExceedPlayerTokens","You don't have enough count of tokens");
        param.Add("rouletteGame.rouletteGame.rouletteGameForTableNotExists","Roulette game doesn't exist");
        param.Add("rouletteGame.rouletteGame.rouletteGameNotExist","Roulette game doesn't exist");
        param.Add("rouletteGame.rouletteGame.roulettePlayerNotExist","Roulette player doesn't exist");
        param.Add("rouletteGame.rouletteGame.spinAlreadyFinished","Spin is already finished");
        param.Add("rouletteGame.rouletteGame.spinNotStartedYet","Spin not started yet");
    }

    public string Get(string key) {
        try {
            return param[key] ?? defaultErrorMessage;
        }
        catch(Exception e) {
            return defaultErrorMessage;
        }
    }

}