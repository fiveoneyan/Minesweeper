using UnityEngine;
using UnityEngine.UI;
using komal.puremvc;

public class UnityEditorPopupsDialog : ComponentEx
{
    public GameObject titleText;
    public GameObject messageText;
    public GameObject yesButtonText;
    public GameObject noButtonText;

    private string title;
    private string message;
    private string yes;
    private string no;
    private System.Action<string> yesCallback;
    private System.Action<string> noCallback;

    public void Init( string title, string message, string yes = "Yes", string no = "No", System.Action<string> yesCallback = null, System.Action<string> noCallback = null ){
        this.title = title;
        this.message = message;
        this.yes = yes;
        this.no = no;
        this.yesCallback = yesCallback;
        this.noCallback = noCallback;
        this.titleText.GetComponent<Text>().text = this.title;
        this.messageText.GetComponent<Text>().text = this.message;
        this.yesButtonText.GetComponent<Text>().text = this.yes;
        this.noButtonText.GetComponent<Text>().text = this.no;
    }

    public void OnYesButtonClick(){
        if(yesCallback != null){
            yesCallback("0");
        }
        Destroy(this.gameObject);
    }

    public void OnNoButtonClick(){
        if(noCallback != null){
            noCallback("1");
        }
        Destroy(this.gameObject);
    }
}
