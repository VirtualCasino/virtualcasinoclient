using System.Collections;
using System.Collections.Generic;

public class ErrorMessagesConfig {

    public string defaultErrorMessage = "Fatal server error. Please try again later or contact with administrator";
    public Dictionary<string, string> param = new Dictionary<string, string>();

    public ErrorMessagesConfig() {
        param.Add("casinoServices.client.clientNickNotValid","Your nick should contains between 3 and 10 letters or numbers");
        param.Add("internalServerError","Invalid server response. Please contact with administrators or try again later");
    }

    public string Get(string key) {
        return param[key] ?? defaultErrorMessage;
    }

}