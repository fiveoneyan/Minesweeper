using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class UIDifficuty : UIBase
 {
    public Transform transBg;
    public Button btnEasy;
    public Button btnNormal;
    public Button btnHard;
    public Button btnClose;
    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        btnEasy.onClick.AddListener(OnbtnEasyClick);
        btnNormal.onClick.AddListener(OnbtnNormalClick);
        btnHard.onClick.AddListener(OnbtnHardClick);
        btnClose.onClick.AddListener(OnbtnCloseClick);
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
        btnEasy.onClick.RemoveListener(OnbtnEasyClick);
        btnNormal.onClick.RemoveListener(OnbtnNormalClick);
        btnHard.onClick.RemoveListener(OnbtnHardClick);
        btnClose.onClick.RemoveListener(OnbtnCloseClick);
    }
    #endregion

    void Init(params object[] param)
    {
      
    }


    void OnbtnEasyClick()
    {
       
    }

    void OnbtnNormalClick()
    {

    }

    void OnbtnHardClick()
    {

    }

    void OnbtnCloseClick()
    {
        Close(() =>
        {
            UIManager.Instance.OpenUI(EUITYPE.UIDifficuty);
            UIManager.Instance.CloseUI(EUITYPE.UIGame);
        });

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
