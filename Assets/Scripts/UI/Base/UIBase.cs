using komal.puremvc;
using UnityEngine;

public abstract class UIBase : ComponentEx
{
    public EUITYPE UIType;
    public EUILEVELTYPE UILevelType;
    public EUIOUTTYPE UIOutType;
    public int SiblingIndex;
    protected bool _handleAble;
    protected AudioClip _btnClip;



    public virtual void OnEnter()
    {
        this.gameObject.SetActive(true);
        _handleAble = true;

        _btnClip = Resources.Load<AudioClip>("music/button");//按钮

    }

    public virtual void OnResume()
    {
        _handleAble = true;
    }

    public virtual void OnPause()
    {
        _handleAble = false;
    }

    public virtual void OnExit()
    {
        _handleAble = false;
    }

    /// <summary>
    /// 播放按钮音效
    /// </summary>
    protected void PlayClickAudio()
    {
        if (_btnClip == null)
            return;

        AudioManager.Instance.PlaySFX(_btnClip);
    }


}
