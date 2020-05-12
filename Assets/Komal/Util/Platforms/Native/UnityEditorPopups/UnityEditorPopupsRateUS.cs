using UnityEngine;
using UnityEngine.UI;
using komal.puremvc;

public class UnityEditorPopupsRateUS : ComponentEx
{
    public GameObject titleText;
    public GameObject messageText;
    public GameObject rateButtonText;
    public GameObject remindButtonText;
    public GameObject declinedButtonText;

    private string title;
    private string message;
    private string rate;
    private string remind;
    private string declined;
    private System.Action<string> rateCallback;
    private System.Action<string> remindCallback;
    private System.Action<string> declinedCallback;

    public void Init( string title, string message, string rate = "Rate", string remind = "Remind", string declined = "Declined", System.Action<string> rateCallback = null, System.Action<string> remindCallback = null, System.Action<string> declinedCallback = null ){
        this.title = title;
        this.message = message;
        this.rate = rate;
        this.remind = remind;
        this.declined = declined;
        this.rateCallback = rateCallback;
        this.remindCallback = remindCallback;
        this.declinedCallback = declinedCallback; 
        this.titleText.GetComponent<Text>().text = this.title;
        this.messageText.GetComponent<Text>().text = this.message;
        this.rateButtonText.GetComponent<Text>().text = this.rate;
        this.remindButtonText.GetComponent<Text>().text = this.remind;
        this.declinedButtonText.GetComponent<Text>().text = this.declined;
    }

    public void OnRateButtonClick(){
        if(rateCallback != null){
            rateCallback("0");
        }
        Destroy(this.gameObject);
    }

    public void OnRemindButtonClick(){
        if(remindCallback != null){
            remindCallback("1");
        }
        Destroy(this.gameObject);
    }

    public void OnDeclinedButtonClick(){
        if(declinedCallback != null){
            declinedCallback("2");
        }
        Destroy(this.gameObject);
    }
}
