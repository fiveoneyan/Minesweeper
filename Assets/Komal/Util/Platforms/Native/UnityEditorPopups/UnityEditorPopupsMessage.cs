using UnityEngine;
using UnityEngine.UI;
using komal.puremvc;

public class UnityEditorPopupsMessage : ComponentEx
{
    public GameObject titleText;
    public GameObject messageText;
    public GameObject okButtonText;

    private string title;
    private string message;
    private string ok;
    private System.Action<string> callback;

    public void Init( string title, string message, string ok = "OK", System.Action<string> callback = null ){
        this.title = title;
        this.message = message;
        this.ok = ok;
        this.callback = callback;
        this.titleText.GetComponent<Text>().text = this.title;
        this.messageText.GetComponent<Text>().text = this.message;
        this.okButtonText.GetComponent<Text>().text = this.ok;
    }

    public void OnOKButtonClick(){
        if(callback != null){
            callback("0");
        }
        Destroy(this.gameObject);
    }
}
