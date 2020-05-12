using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using komal;
using komal.puremvc;

// 此类继承自 ScrollRectEx, 同时实现代理接口
public class TestScrollRect : ComponentEx, IScrollRectDelegate
{
    //////////////////////////////////////////////////////////////
    //// 挂载的预制对象（列表中的每一项对应的预制体）
    //////////////////////////////////////////////////////////////
    public GameObject CellPrefab;
    public ScrollRectEx m_Scroll;

    void Start() {
        // 初始化调用的代码示例
        m_Scroll.ScrollRectDelegate = this;
        m_Scroll.FillCells();
    }
    
    //////////////////////////////////////////////////////////////
    //// IScrollRectDelegate
    //////////////////////////////////////////////////////////////
    public float GetScrollRectCellWidthOrHeightByIndex(int index){
        return 20*(1+index%3);
    }

    public int GetCellCounts(){
        return 100;
    }

    // 开启对象池的时候，ScrollRectEx 底层会自动缓存对象
    public IScrollRectExCell CreateCell(){
        var go = Instantiate(this.CellPrefab);
        IScrollRectExCell cell = go.GetComponent<IScrollRectExCell>();
        return cell;
    }
}
