using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuPanel : UIBase
{
    public Button btnEASY_W;
    public Button btnEASY_B;
    public Button btnNORMAL_W;
    public Button btnNORMAL_B;
    public Button btnHARD_W;
    public Button btnHARD_B;
    public Button btnPlay;
    public Transform transBg;
    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();

        btnEASY_W = transform.Find("MenuPanel/btnEASY_W").GetComponent<Button>();
        btnEASY_B = transform.Find("MenuPanel/btnEASY_B").GetComponent<Button>();
        btnNORMAL_W = transform.Find("MenuPanel/btnNORMAL_W").GetComponent<Button>();
        btnNORMAL_B = transform.Find("MenuPanel/btnNORMAL_B").GetComponent<Button>();
        btnHARD_W = transform.Find("MenuPanel/btnHARD_W").GetComponent<Button>();
        btnHARD_B = transform.Find("MenuPanel/btnHARD_B").GetComponent<Button>();

        btnEASY_W.onClick.AddListener(OnEASY_WClick);
        btnEASY_B.onClick.AddListener(OnEASY_BClick);
        btnNORMAL_W.onClick.AddListener(OnNORMAL_WClick);
        btnNORMAL_B.onClick.AddListener(OnNORMAL_BClick);
        btnHARD_W.onClick.AddListener(OnHARD_WClick);
        btnHARD_B.onClick.AddListener(OnHARD_BClick);

        btnPlay.onClick.AddListener(OnBtnPlay);
        // btnHARD_B.gameObject.SetActive(false);


        int diff = PlayerPrefs.GetInt("diff", 1);
        if (diff == 1)
        {
            btnEASY_B.gameObject.SetActive(true);
            btnNORMAL_B.gameObject.SetActive(false);
            btnNORMAL_W.gameObject.SetActive(true);
            btnHARD_B.gameObject.SetActive(false);
            btnHARD_W.gameObject.SetActive(true);
        }
        else if (diff == 2)
        {
            btnEASY_B.gameObject.SetActive(false);
            btnNORMAL_B.gameObject.SetActive(true);
            btnNORMAL_W.gameObject.SetActive(false);
            btnHARD_B.gameObject.SetActive(false);
            btnHARD_W.gameObject.SetActive(true);
        } else if (diff == 3)
        {
            btnEASY_B.gameObject.SetActive(false);
            btnNORMAL_B.gameObject.SetActive(false);
            btnNORMAL_W.gameObject.SetActive(true);
            btnHARD_B.gameObject.SetActive(true);
            btnHARD_W.gameObject.SetActive(false);
        }


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
        btnEASY_W.onClick.RemoveListener(OnEASY_WClick);
        btnEASY_B.onClick.RemoveListener(OnEASY_BClick);
        btnNORMAL_W.onClick.RemoveListener(OnNORMAL_WClick);
        btnNORMAL_B.onClick.RemoveListener(OnNORMAL_BClick);
        btnHARD_W.onClick.RemoveListener(OnHARD_WClick);
        btnHARD_B.onClick.RemoveListener(OnHARD_BClick);
    }
    #endregion

    void Init(params object[] param)
    {

    }


    void OnEASY_WClick()
    {
        btnEASY_B.gameObject.SetActive(true);
        btnNORMAL_B.gameObject.SetActive(false);
        btnNORMAL_W.gameObject.SetActive(true);
        btnHARD_B.gameObject.SetActive(false);
        btnHARD_W.gameObject.SetActive(true);
        PlayerPrefs.SetInt("diff", 1);
        facade.SendNotification("MSG_Replay");

    }

    void OnEASY_BClick()
    {
        btnEASY_W.gameObject.SetActive(true);


    }

    void OnNORMAL_WClick()
    {
        btnNORMAL_B.gameObject.SetActive(true);
        btnEASY_B.gameObject.SetActive(false);
        btnEASY_W.gameObject.SetActive(true);
        btnHARD_B.gameObject.SetActive(false);
        btnHARD_W.gameObject.SetActive(true);
        PlayerPrefs.SetInt("diff", 2);
        facade.SendNotification("MSG_Replay");
        facade.SendNotification("MSG_Aim");
    }

    void OnNORMAL_BClick()
    {
        btnNORMAL_W.gameObject.SetActive(true);


    }

    void OnHARD_WClick()
    {
        btnHARD_B.gameObject.SetActive(true);
        btnEASY_B.gameObject.SetActive(false);
        btnEASY_W.gameObject.SetActive(true);
        btnNORMAL_B.gameObject.SetActive(false);
        btnNORMAL_W.gameObject.SetActive(true);
        PlayerPrefs.SetInt("diff", 3);
        facade.SendNotification("MSG_Replay");
        facade.SendNotification("MSG_Aim");
    }

    void OnHARD_BClick()
    {
        btnHARD_W.gameObject.SetActive(true);


    }
    void OnBtnPlay(){

        Close();
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
