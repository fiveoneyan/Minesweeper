using komal;
using komal.sdk;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using komal.puremvc;

public class UITips : UIBase
{
    public Transform transBg;
    public Text txtFastTime;
    public Text txtTime;
    public Button btnReplay;
    public Image imgWellDone;
    public Image imgNewRecord;
    public Button btnRank;
    public Button btnRemoveAd;
    public AudioClip AudPass;

    private difficulty dd01;

    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        if (SDKManager.Instance.IsAdsRemoved()||!KomalUtil.Instance.IsNetworkReachability()) {
            btnRemoveAd.interactable = false;

        }
        facade.SendNotification("MSG_Move",false);

        AudioManager.Instance.PlaySFX(AudPass);
        transBg.DOScale(1, 0.25f).SetEase(Ease.OutBack);
        btnReplay.onClick.AddListener(OnReplayClick);
        btnRank.onClick.AddListener(OnRankClick);
        btnRemoveAd.onClick.AddListener(OnRemoveAdClick);

    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnExit()
    {
        base.OnExit();

        btnReplay.onClick.RemoveListener(OnReplayClick);
        btnRank.onClick.RemoveListener(OnRankClick);
        btnRemoveAd.onClick.RemoveListener(OnRemoveAdClick);
        facade.SendNotification("MSG_Move", true);
    }
    #endregion

    void Init(params object[] param)
    {
        dd01 = (difficulty)param[2];
        int fast = PlayerPrefs.GetInt(param[2].ToString(),99999);
        PlayerPrefs.SetInt(param[2].ToString() + "win", PlayerPrefs.GetInt(param[2].ToString() + "win", 0) + 1);
        if (fast > (int)param[1])
        {
            fast = (int)param[1];
            PlayerPrefs.SetInt(param[2].ToString(), fast);
            imgWellDone.gameObject.SetActive(false);
            imgNewRecord.gameObject.SetActive(true);

            Debug.Log(param[2].ToString() + "win");
        }
        //txtResult.text = param[0].ToString();
        txtTime.text = param[1].ToString();
        txtFastTime.text = fast.ToString();
        //上传排行榜
        if (PlayerPrefs.HasKey("easy")) {
            SDKManager.Instance.RecordRank(Config.ID.GetValue("TimeEasy"), PlayerPrefs.GetInt("easy"));
        }
        if (PlayerPrefs.HasKey("normal"))
        {
            SDKManager.Instance.RecordRank(Config.ID.GetValue("TimeNormal"), PlayerPrefs.GetInt("normal"));
        }
        if (PlayerPrefs.HasKey("hard"))
        {
            SDKManager.Instance.RecordRank(Config.ID.GetValue("TimeHard"), PlayerPrefs.GetInt("hard"));
        }
    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {
                IAP_MSG.PURCHASING,
                IAP_MSG.PURCHASE_FAILURE,
                IAP_MSG.PURCHASE_SUCCESS,

            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case IAP_MSG.PURCHASING:
                //点击去广告时执行
                break;
            case IAP_MSG.PURCHASE_FAILURE:
                //购买失败执行
                break;
            case IAP_MSG.PURCHASE_SUCCESS:
                //购买成功
                UIManager.Instance.OpenUI(EUITYPE.UISuccBuy);
                btnRemoveAd.interactable = false;
                break;

        }
    }

    void OnRankClick()
    {
        PlayClickAudio();
        SDKManager.Instance.OpenGameCenter();

    }
    void OnRemoveAdClick()
    {
        PlayClickAudio();
        SDKManager.Instance.RemoveAds();

    }

    void OnReplayClick()
    {
        if (!_handleAble)
            return;

        _handleAble = false;

        
//        PlayClickAudio();
        transBg.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {

            if (dd01 == difficulty.easy)
            {
                if (PlayerPrefs.GetInt("easywin", 0) % 2 == 0)
                {
                 
                    ShowAd();
                }
                else { facade.SendNotification("MSG_Replay"); }
            }
            else {
              
                ShowAd();
            }
          

            UIManager.Instance.CloseUI(this);
        });
    }
    void ShowAd() {
#if UNITY_EDITOR
        facade.SendNotification("MSG_Replay");
#else
        if (KomalUtil.Instance.IsNetworkReachability()&&Client.Instance.HaveAd)
        {
            SDKManager.Instance.ShowInterstitial((result) =>
            {
                if (result == InterstitialResult.DISMISS)
                {
                    Client.Instance.AdOver();
                    facade.SendNotification("MSG_Replay");
                    AudioManager.Instance.PauseAllListener(false);
                    
                }
                else if (result == InterstitialResult.DISPLAY)
                {
                   
                    AudioManager.Instance.PauseAllListener(true);
                }

            });
        }
        else { facade.SendNotification("MSG_Replay");
        }
       
#endif

    }
}
