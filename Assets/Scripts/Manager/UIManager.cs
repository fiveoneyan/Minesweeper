using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIRoot UIRoot;
    Dictionary<EUITYPE, UIBase> _dicUIObj;                   //所有有缓存的UI索引
    Dictionary<EUILEVELTYPE, List<UIBase>> _dicVisibleUIObj; //所有可见的UI

    public Camera UICamera()
    {
        Camera reCamera = null;
        if (UIRoot != null)
        {
            reCamera = UIRoot.GetUICamera();
        }
        return reCamera;
    }

    public UIBase CurrentUI { get; private set; }

    void Awake()
    {
        Instance = this;
        UIRoot.Init();

        _dicUIObj = new Dictionary<EUITYPE, UIBase>();

        _dicVisibleUIObj = new Dictionary<EUILEVELTYPE, List<UIBase>>();
        int levelCount = Enum.GetNames(typeof(EUILEVELTYPE)).Length;
        for (int i = 0; i < levelCount; i++)
        {
            EUILEVELTYPE addType = (EUILEVELTYPE)i;
            _dicVisibleUIObj.Add(addType, new List<UIBase>());

        }
    }

    public void OpenUI(EUITYPE uiType, Action<UIBase> callBack = null)
    {
        UIBase ui = null;

        _dicUIObj.TryGetValue(uiType, out ui);


        if (ui == null)
        {
            LoadUI(uiType, callBack);
        }
        else
        {
            OnGetUIObject(ui);
        }
    }

    public void OpenUI(EUITYPE uiType, params object[] param)
    {
        UIBase ui = null;

        _dicUIObj.TryGetValue(uiType, out ui);


        if (ui == null)
        {
            LoadUI(uiType, param);
        }
        else
        {
            OnGetUIObject(ui);
        }
    }

    public void CloseUI(EUITYPE uiType, Action callBack = null)
    {
        UIBase uiObj = null;
        _dicUIObj.TryGetValue(uiType, out uiObj);
        CloseUI(uiObj, callBack);
    }

    public void CloseUI(UIBase uiObj, Action callBack = null)
    {
        if (uiObj != null)
        {
            RemoveVisibleUIObj(uiObj);

            RefreshHandleAbleUI();

            if (uiObj.gameObject.activeSelf)
            {
                if (uiObj.UIOutType == EUIOUTTYPE.Destroy)
                {
                    _dicUIObj.Remove(uiObj.UIType);
                    Destroy(uiObj.gameObject);
                    uiObj.OnExit();
                }
                else if (uiObj.UIOutType == EUIOUTTYPE.Hide)
                {
                    UIRoot.HideUI(uiObj);
                    uiObj.OnExit();
                }
            }
        }
    }

    void LoadUI(EUITYPE uiType, Action<UIBase> callback)
    {
        GameObject uiObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/" + uiType.ToString()));
        uiObj.name = uiObj.name.Replace("(Clone)", "");
        UIBase ui = uiObj.GetComponent<UIBase>();
        ui.OnEnter();
        if (!_dicUIObj.ContainsKey(uiType))
        {
            _dicUIObj.Add(uiType, null);
        }
        _dicUIObj[uiType] = ui;
        OnGetUIObject(ui);
        if (callback != null)
            callback(ui);
    }

    void LoadUI(EUITYPE uiType, params object[] param)
    {
        GameObject uiObj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/" + uiType.ToString()));
        uiObj.name = uiObj.name.Replace("(Clone)", "");
        UIBase ui = uiObj.GetComponent<UIBase>();
        ui.OnEnter();
        if (!_dicUIObj.ContainsKey(uiType))
        {
            _dicUIObj.Add(uiType, null);
        }
        _dicUIObj[uiType] = ui;
        OnGetUIObject(ui);
        uiObj.SendMessage("Init", param, SendMessageOptions.DontRequireReceiver);
    }

    void OnGetUIObject(UIBase ui)
    {
        //已经存在于显示列表的UI不执行这些操作
        if (!UIHadInVisibleList(ui))
        {
            UIRoot.ShowUI(ui);
            AddVisibleUIObj(ui);
            RefreshHandleAbleUI();
        }
    }

    bool AddVisibleUIObj(UIBase uiObj)
    {
        List<UIBase> levelList = _dicVisibleUIObj[uiObj.UILevelType];

        if (!levelList.Contains(uiObj))
        {

            int insertIndex = 0;

            for (int i = 0; i < levelList.Count; i++)
            {
                if (levelList[i].SiblingIndex >= uiObj.SiblingIndex)
                {
                    insertIndex = i;
                    break;
                }
            }
            levelList.Insert(insertIndex, uiObj);
            return true;
        }
        else
        {
            return false;
        }
    }

    void RemoveVisibleUIObj(UIBase uiObj)
    {
        List<UIBase> levelList = _dicVisibleUIObj[uiObj.UILevelType];
        if (levelList.Contains(uiObj))
        {
            levelList.Remove(uiObj);
        }
        else
        {
            Debug.LogWarning("RemoveVisibleUIObj " + uiObj.name + " not in VisibleList");
        }
    }

    void RefreshHandleAbleUI()
    {
        UIBase topUI = GetHandleAbleUI();

        if (topUI != CurrentUI)
        {
            //在可操作界面与原来界面不同时做刷新事件
            if (CurrentUI != null)
            {
                CurrentUI.OnPause();
            }

            CurrentUI = topUI;

            if (CurrentUI != null)
            {
                CurrentUI.OnResume();
            }
        }
    }

    /// <summary>
    /// 获取当前在最上层的UI
    /// </summary>
    /// <returns></returns>
    public UIBase GetHandleAbleUI()
    {
        int levelCount = Enum.GetNames(typeof(EUILEVELTYPE)).Length;

        for (int i = levelCount - 1; i >= 0; i--)
        {
            List<UIBase> levelList = _dicVisibleUIObj[(EUILEVELTYPE)i];

            if (levelList.Count > 0)
            {
                return levelList[0];
            }
        }
        return null;
    }

    bool UIHadInVisibleList(UIBase uiObj)
    {
        List<UIBase> levelList = _dicVisibleUIObj[uiObj.UILevelType];

        return levelList.Contains(uiObj);

    }
}