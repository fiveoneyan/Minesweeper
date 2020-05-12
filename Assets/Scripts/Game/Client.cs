using komal.puremvc;
using komal.sdk;
using UnityEngine;
using System.Collections;

public class Client : SceneBase
{
    public static  Client Instance;
    public bool HaveAd=true;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        SDKManager.Instance.Login(); //登入玩家信息

        //显示Banner广告
        if (!SDKManager.Instance.IsAdsRemoved())
        {
            SDKManager.Instance.ShowBanner();
        }

        Application.targetFrameRate = 60;

        Invoke("InitGame", 0.01f);
    }

    void InitGame()
    {
        UIManager.Instance.OpenUI(EUITYPE.UIGame);
    }

    public void AdOver() {
        StartCoroutine("CD");
    }

    IEnumerator CD() {
        HaveAd = false;
        yield return new WaitForSeconds(50);
        HaveAd = true;
    }
}
