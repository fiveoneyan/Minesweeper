using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISUccBuy : UIBase
 {
    public Button btnOK;
 
    #region LifeTime
     public override void OnEnter()
    {
        base.OnEnter();
        btnOK.onClick.AddListener(OnbtnOKClick);
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
       btnOK.onClick.RemoveListener(OnbtnOKClick);
    }
    #endregion

    void Init(params object[] param)
    {
      
    }
    void OnbtnOKClick() {
        UIManager.Instance.CloseUI(this);
    }
}
