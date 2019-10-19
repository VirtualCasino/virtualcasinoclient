using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ErrorMessageDisplayer {

    public ErrorMessagesConfig errorMessagesConfig = new ErrorMessagesConfig();
    public ErrorMessageDisplayer(GameObject errorToastHandle, GameObject errorTextMessageHandle) {
        this.errorToastHandle = errorToastHandle;
        this.errorTextMessageHandle = errorTextMessageHandle;
    }

    public GameObject errorToastHandle;
    public GameObject errorTextMessageHandle;


    public void DisplayErrorMessage(string jsonMessage) {
        ErrorView errorMessage = JsonUtility.FromJson<ErrorView>(jsonMessage);
        if(errorMessage == null || errorMessage.code == null) {
            errorTextMessageHandle.GetComponent<Text>().text = errorMessagesConfig.defaultErrorMessage;
            errorToastHandle.SetActive(true);
            return;
        }
        errorTextMessageHandle.GetComponent<Text>().text = errorMessagesConfig.Get(errorMessage.code);
        errorToastHandle.SetActive(true);
    }

    public void DisplayErrorMessage(ErrorView errorMessage) {
        errorTextMessageHandle.GetComponent<Text>().text = errorMessagesConfig.Get(errorMessage.code);
        errorToastHandle.SetActive(true);
    }

    public IEnumerator hideErrorMessageAfterTime(int time) {
        yield return new WaitForSeconds(time);
        hideErrorMessage();
    }

    private void hideErrorMessage() {
        try {
            Debug.Log("Hiding error message");
            errorTextMessageHandle.GetComponent<Text>().text = null;
            errorToastHandle.SetActive(false);
            Debug.Log("Error message hidden");
        }
        catch(Exception a) {
            Debug.Log("Exception: " + a.Message);
        }
    }
}