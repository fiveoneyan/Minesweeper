using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisicsPanel : UIBase
 {
    public Text txtEasy01;
    public Text txtEasy02;
    public Text txtEasy03;

    public Text txtNormal01;
    public Text txtNormal02;
    public Text txtNormal03;

    public Text txtHard01;
    public Text txtHard02;
    public Text txtHard03;

    #region LifeTime
    public override void OnEnter()
    {
        base.OnEnter();

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
    }
    #endregion

    void Init(params object[] param)
    {
      
    }
    public void diaoyong() {


        txtEasy01.text = PlayerPrefs.GetInt("easy", 0).ToString();
        if (PlayerPrefs.GetInt("easywin", 0) != 0)
        {
            txtEasy02.text = (PlayerPrefs.GetInt("easywin", 0) * 100.0f / PlayerPrefs.GetInt("play_easy", 0) * 1.0f).ToString("F2") + "%";
        }
        else
        {
            txtEasy02.text = "0.0%";
        }

        txtEasy03.text = PlayerPrefs.GetInt("easywin", 0).ToString();

        txtNormal01.text = PlayerPrefs.GetInt("normal", 0).ToString();
        if (PlayerPrefs.GetInt("normalwin", 0) != 0)
        {
            txtNormal02.text = (PlayerPrefs.GetInt("normalwin", 0) * 100.0f / PlayerPrefs.GetInt("play_normal", 0) * 1.0f).ToString("F2") + "%";
        }
        else {
            txtNormal02.text = "0.0%";
        }
        txtNormal03.text = PlayerPrefs.GetInt("normalwin", 0).ToString();
        
        txtHard01.text = PlayerPrefs.GetInt("hard", 0).ToString();
        if (PlayerPrefs.GetInt("hardwin", 0) != 0)
        {
            txtHard02.text = (PlayerPrefs.GetInt("hardwin", 0) * 100.0f / PlayerPrefs.GetInt("play_hard", 0) * 1.0f).ToString("F2") + "%";
        }
        else
        {
            txtHard02.text = "0.0%";
        }

        txtHard03.text = PlayerPrefs.GetInt("hardwin", 0).ToString();

    }
    public void Reset() {
        PlayerPrefs.DeleteAll();
        diaoyong();
    }

}
