using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;

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
        errorTextMessageHandle.GetComponent<Text>().text = errorMessagesConfig.Get(errorMessage.code);
        errorToastHandle.SetActive(true);
    }

    public void DisplayErrorMessage(ErrorView errorMessage) {
        errorTextMessageHandle.GetComponent<Text>().text = errorMessagesConfig.Get(errorMessage.code);
        errorToastHandle.SetActive(true);
    }
}