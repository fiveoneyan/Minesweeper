using UnityEngine;
using UnityEngine.UI;
using komal.sdk;
using komal;
using komal.puremvc;

public class SettingsPanel : UIBase
{
    public Button btnSound_O;
    public Button btnSound_P;
    public Button btnShake_O;
    public Button btnShake_P;
    public Button btnRATE;
    public Button btnRESTORS;

    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();
        btnSound_O = transform.Find("SettingsPanel/btnSOUND/btnOPEN").GetComponent<Button>();
        btnSound_P = transform.Find("SettingsPanel/btnSOUND/btnCLOSE").GetComponent<Button>();
        btnShake_O = transform.Find("SettingsPanel/btnSHAKE/btnOPEN").GetComponent<Button>();
        btnShake_P = transform.Find("SettingsPanel/btnSHAKE/btnCLOSE").GetComponent<Button>();
        if (AudioManager.Instance.GetSFXVolume() <= 0)
        {
            btnSound_P.gameObject.SetActive(true);
            btnSound_O.gameObject.SetActive(false);
        }
        else {
            btnSound_P.gameObject.SetActive(false);
            btnSound_O.gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Shake", 1) == 1)
        {
            btnShake_P.gameObject.SetActive(false);
            btnShake_O.gameObject.SetActive(true);

        }
        else {
            btnShake_P.gameObject.SetActive(true);
            btnShake_O.gameObject.SetActive(false);
        }
        btnSound_O.onClick.AddListener(OnSound_OClick);
        btnSound_P.onClick.AddListener(OnSound_PClick);
        btnShake_O.onClick.AddListener(OnShake_OClick);
        btnShake_P.onClick.AddListener(OnShake_PClick);
        btnRATE.onClick.AddListener(OnRATEClick);
        btnRESTORS.onClick.AddListener(OnRankClick);
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
        btnSound_O.onClick.RemoveListener(OnSound_OClick);
        btnSound_P.onClick.RemoveListener(OnSound_PClick);
        btnShake_O.onClick.RemoveListener(OnShake_OClick);
        btnShake_P.onClick.RemoveListener(OnShake_PClick);
        btnRATE.onClick.RemoveListener(OnRATEClick);
        btnRESTORS.onClick.RemoveListener(OnRankClick);
    }
    #endregion

    public void AllFunction()
    {
        OnEnter();
        OnResume();
        OnPause();
        OnExit();
    }


    void Init(params object[] param)
    {

    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {

                IAP_MSG.RESTORING,
                IAP_MSG.RESTORE_FAILURE,
                IAP_MSG.RESTORE_SUCCESS
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {


            case IAP_MSG.RESTORING:
                //点击恢复购买时执行

                break;
            case IAP_MSG.RESTORE_FAILURE:
                //恢复失败执行
                break;
            case IAP_MSG.RESTORE_SUCCESS:
                //恢复成功执行

                UIManager.Instance.OpenUI(EUITYPE.UISuccBuy);
                break;
        }
    }

    void OnRATEClick()
    {
        PlayClickAudio();
        Application.OpenURL(Config.ID.GetValue("AppUrl"));

    }

    void OnRankClick()
    {
        PlayClickAudio();
        SDKManager.Instance.OpenGameCenter();


    }

    void OnSound_OClick()
    {
        btnSound_O.gameObject.SetActive(false);
        btnSound_P.gameObject.SetActive(true);
        PlayClickAudio();
        AudioManager.Instance.SetSFXVolume(0);
    }

    void OnSound_PClick()
    {
        btnSound_P.gameObject.SetActive(false);
        btnSound_O.gameObject.SetActive(true);
            AudioManager.Instance.SetSFXVolume(1);
    }

    void OnShake_OClick()
    {
        btnShake_O.gameObject.SetActive(false);
        btnShake_P.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Shake",0);
    }

    void OnShake_PClick()
    {
        btnShake_P.gameObject.SetActive(false);
        btnShake_O.gameObject.SetActive(true);
        PlayerPrefs.SetInt("Shake", 1);
    }
}
