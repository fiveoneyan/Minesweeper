using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private Camera _UICamera;
    private Dictionary<EUILEVELTYPE, Transform> _rootGroup;

    public void Init()
    {
        _UICamera = transform.Find("UICamera").GetComponent<Camera>();

        if (_UICamera == null)
        {
            Debug.LogError("找不到UI相机");
            return;
        }

        _rootGroup = new Dictionary<EUILEVELTYPE, Transform>();

        Transform canvasChild = null;

        canvasChild = transform.Find("HideRoot");
        if (canvasChild != null)
        {
            _rootGroup.Add(EUILEVELTYPE.Hide, canvasChild);
        }

        canvasChild = transform.Find("MainRoot");
        if (canvasChild != null)
        {
            _rootGroup.Add(EUILEVELTYPE.Main, canvasChild);
        }

        canvasChild = transform.Find("MiddleRoot");
        if (canvasChild != null)
        {
            _rootGroup.Add(EUILEVELTYPE.Middle, canvasChild);
        }

        canvasChild = transform.Find("PopRoot");
        if (canvasChild != null)
        {
            _rootGroup.Add(EUILEVELTYPE.Pop, canvasChild);
        }

        canvasChild = transform.Find("TipRoot");
        if (canvasChild != null)
        {
            _rootGroup.Add(EUILEVELTYPE.Tip, canvasChild);
        }
    }

    public Camera GetUICamera()
    {
        return _UICamera;
    }

    public void ShowUI(UIBase uiBase)
    {
        if (uiBase == null)
        {
            Debug.LogError("不存在的UI类型:" + uiBase.UILevelType.ToString());
            return;
        }
        Transform canvasChild = null;
        if (!_rootGroup.TryGetValue(uiBase.UILevelType, out canvasChild))
        {
            canvasChild = this.transform;
        }

        uiBase.transform.SetParent(canvasChild);
        uiBase.transform.SetSiblingIndex(uiBase.SiblingIndex - 1);
        uiBase.transform.localPosition = Vector3.zero;
        uiBase.transform.localRotation = Quaternion.identity;
        uiBase.transform.localScale = Vector3.one;
        uiBase.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        uiBase.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ShowUI(UIBase uiBase, int siblingIndex)
    {
        if (uiBase != null)
        {
            uiBase.SiblingIndex = siblingIndex;
            ShowUI(uiBase);
        }
    }

    public void HideUI(UIBase uibase)
    {
        if (uibase != null)
        {
            Transform rootChild = this.transform;

            _rootGroup.TryGetValue(EUILEVELTYPE.Hide, out rootChild);

            uibase.transform.SetParent(rootChild);
        }
    }
}