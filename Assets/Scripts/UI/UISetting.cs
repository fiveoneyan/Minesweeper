using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using komal.sdk;

public class UISetting : UIBase
{
    public Transform transBg;
    public Button btnClose;
   /* public Button btnSfx;
    public Button btnBgm;
    public Button btnReplay;
    public Button btnHome;*/
  
    public Button btn_MENU01;
    public Button btn_MENU02;
    public Button btn_SETTINGS01;
    public Button btn_SETTINGS02;
    public Button btn_STATISTICS01;
    public Button btn_STATISTICS02;

    public Image line01;
    public Image line02;
    public Image line03;

    public Transform MenuPanel;
    public Transform SettingsPanel;
    public Transform StatisicsPanel;
    public StatisicsPanel aa;

    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();

        transBg.DOScale(1, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Time.timeScale = 0;
        });
        btnClose.onClick.AddListener(OnBtnCloseClick);

        /*btnSfx.GetComponentInChildren<Text>().text = AudioManager.Instance.GetSFXVolume() > 0 ? "音  效:开" : "音  效:关";
       btnBgm.GetComponentInChildren<Text>().text = AudioManager.Instance.GetBGMVolume() > 0 ? "背景音乐:开" : "背景音乐:关";

       btnSfx.onClick.AddListener(OnBtnSfxClick);
       btnBgm.onClick.AddListener(OnBtnBgmClick);
       btnReplay.onClick.AddListener(OnBtnReplayClick);
       btnHome.onClick.AddListener(OnHomeClick);*/

        Init();
        GameObject.Find("UISetting").GetComponent<SettingsPanel>().OnEnter();
        GameObject.Find("UISetting").GetComponent<MenuPanel>().OnEnter();
        aa.diaoyong();

    }

    public void Init()
    {
        btn_MENU01 = transform.Find("up/btn_MENU01").GetComponent<Button>();
        btn_MENU02 = transform.Find("up/btn_MENU02").GetComponent<Button>();
        btn_SETTINGS01 = transform.Find("up/btn_SETTINGS01").GetComponent<Button>();
        btn_SETTINGS02 = transform.Find("up/btn_SETTINGS02").GetComponent<Button>();
        btn_STATISTICS01 = transform.Find("up/btn_STATISTICS01").GetComponent<Button>();
        btn_STATISTICS02 = transform.Find("up/btn_STATISTICS02").GetComponent<Button>();
        MenuPanel= transform.Find("MenuPanel").GetComponent<Transform>();
        SettingsPanel = transform.Find("SettingsPanel").GetComponent<Transform>();
        StatisicsPanel = transform.Find("StatisicsPanel").GetComponent<Transform>();

        //up
        btn_MENU01.onClick.AddListener(OnMENU01Click);
        btn_MENU02.onClick.AddListener(OnMENU02Click);
        btn_SETTINGS01.onClick.AddListener(OnSETTINGS01Click);
        btn_SETTINGS02.onClick.AddListener(OnSETTINGS02Click);
        btn_STATISTICS01.onClick.AddListener(OnSTATISTICS01Click);
        btn_STATISTICS02.onClick.AddListener(OnSTATISTICS02Click);
        btn_SETTINGS02.gameObject.SetActive(false);
        btn_STATISTICS02.gameObject.SetActive(false);
        MenuPanel.gameObject.SetActive(true);
        SettingsPanel.gameObject.SetActive(false);
        StatisicsPanel.gameObject.SetActive(false);
        line01 = transform.Find("up/btn_MENU02/line").GetComponent<Image>();
        line02 = transform.Find("up/btn_SETTINGS02/line").GetComponent<Image>();
        line03 = transform.Find("up/btn_STATISTICS02/line").GetComponent<Image>();

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

        btnClose.onClick.RemoveListener(OnBtnCloseClick);
         /*btnSfx.onClick.RemoveListener(OnBtnSfxClick);
        btnBgm.onClick.RemoveListener(OnBtnBgmClick);
        btnReplay.onClick.RemoveListener(OnBtnReplayClick);
        btnHome.onClick.RemoveListener(OnHomeClick);*/

        btn_MENU01.onClick.RemoveListener(OnMENU01Click);
        btn_MENU02.onClick.RemoveListener(OnMENU02Click);

        btn_SETTINGS01.onClick.RemoveListener(OnSETTINGS01Click);
        btn_SETTINGS02.onClick.RemoveListener(OnSETTINGS02Click);
        btn_STATISTICS01.onClick.RemoveListener(OnSTATISTICS01Click);
        btn_STATISTICS02.onClick.RemoveListener(OnSTATISTICS02Click);

       
    }
    #endregion



    void Init(params object[] param)
    {

    }


   

    /// <summary>
    /// 音效
    /// </summary>
    void OnBtnSfxClick()
    {
        PlayClickAudio();
        if (AudioManager.Instance.GetSFXVolume() > 0)
        {
            AudioManager.Instance.SetSFXVolume(0);
          //  btnSfx.GetComponentInChildren<Text>().text = "音  效:关";
        }
        else
        {
            AudioManager.Instance.SetSFXVolume(1);
       //     btnSfx.GetComponentInChildren<Text>().text = "音  效:开";
        }
    }

    /// <summary>
    /// 背景音乐
    /// </summary>
    void OnBtnBgmClick()
    {
        PlayClickAudio();
        if (AudioManager.Instance.GetBGMVolume() > 0)
        {
            AudioManager.Instance.SetBGMVolume(0);
         //   btnBgm.GetComponentInChildren<Text>().text = "背景音乐:关";
        }
        else
        {
            AudioManager.Instance.SetBGMVolume(1);
        //    btnBgm.GetComponentInChildren<Text>().text = "背景音乐:开";
        }
    }

    /// <summary>
    /// 重玩
    /// </summary>
    void OnBtnReplayClick()
    {
        Close(() =>
        {
#if UNITY_EDITOR
            facade.SendNotification("MSG_Replay");
#else
            SDKManager.Instance.ShowInterstitial((result) =>
            {
                if (result == InterstitialResult.DISPLAY)
                {
                    AudioManager.Instance.PauseAllListener(true);
                }
                else if(result== InterstitialResult.DISMISS)
                {
                    AudioManager.Instance.PauseAllListener(false);
                    facade.SendNotification("MSG_Replay");
                }
            });
#endif
        });
    }

    /// <summary>
    /// 关闭
    /// </summary>
    void OnBtnCloseClick()
    {
        Close();
    }

    /// <summary>
    /// 主页
    /// </summary>
    void OnHomeClick()
    {
        Close(() =>
        {
            UIManager.Instance.OpenUI(EUITYPE.UILogin);
            UIManager.Instance.CloseUI(EUITYPE.UIGame);
        });
    }


    void OnMENU01Click()
    {
        btn_MENU02.gameObject.SetActive(true);      
        btn_STATISTICS01.gameObject.SetActive(true);
        btn_STATISTICS02.gameObject.SetActive(false);
        btn_SETTINGS01.gameObject.SetActive(true);
        btn_SETTINGS02.gameObject.SetActive(false);
        MenuPanel.gameObject.SetActive(true);
        SettingsPanel.gameObject.SetActive(false);
        StatisicsPanel.gameObject.SetActive(false);

        line01.gameObject.SetActive(true);
        line03.gameObject.SetActive(false);
        line02.gameObject.SetActive(false);

    }

    void OnMENU02Click()
    {
        btn_MENU01.gameObject.SetActive(true);
        btn_MENU02.gameObject.SetActive(false);
        
    }

    void OnSETTINGS01Click()
    {
        btn_SETTINGS02.gameObject.SetActive(true);     
        btn_MENU01.gameObject.SetActive(true);
        btn_MENU02.gameObject.SetActive(false);
        btn_STATISTICS01.gameObject.SetActive(true);
        btn_STATISTICS02.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
        StatisicsPanel.gameObject.SetActive(false);

        line02.gameObject.SetActive(true);
        line03.gameObject.SetActive(false);
        line01.gameObject.SetActive(false);

    }

    void OnSETTINGS02Click()
    {
        btn_SETTINGS01.gameObject.SetActive(true);
        btn_SETTINGS02.gameObject.SetActive(false);
    }

    void OnSTATISTICS01Click()
    {
        btn_STATISTICS02.gameObject.SetActive(true);   
        btn_MENU01.gameObject.SetActive(true);
        btn_MENU02.gameObject.SetActive(false);
        btn_SETTINGS01.gameObject.SetActive(true);
        btn_SETTINGS02.gameObject.SetActive(false);
        StatisicsPanel.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
        SettingsPanel.gameObject.SetActive(false);

        line03.gameObject.SetActive(true);
        line01.gameObject.SetActive(false);
        line02.gameObject.SetActive(false);
    }

    void OnSTATISTICS02Click()
    {
        btn_STATISTICS01.gameObject.SetActive(true);
        btn_STATISTICS02.gameObject.SetActive(false);
    }


 


    void Close(Action action = null)
    {
        if (!_handleAble)
            return;

        _handleAble = false;

        PlayClickAudio();
        Time.timeScale = 1;
        transBg.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            if (action != null)
                action();

            UIManager.Instance.CloseUI(this);
        });
    }
}
