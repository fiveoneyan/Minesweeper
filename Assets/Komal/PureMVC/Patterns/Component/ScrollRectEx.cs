using System;
using System.Collections.Generic;
using UnityEngine;

namespace komal.puremvc {
    public interface IScrollRectDelegate {
        float GetScrollRectCellWidthOrHeightByIndex(int index);
        int GetCellCounts();
        IScrollRectExCell CreateCell();
    }

    public interface IScrollRectExCell{
        GameObject gameObject{ get; }
        void BindCellIndex(int index);
        void UnBindCellIndex();
    }
    
    public sealed class ScrollRectEx : UnityEngine.UI.ScrollRect
    {
        //////////////////////////////////////////////////////////////
        //// 内部存储结构
        //////////////////////////////////////////////////////////////
        private class CellInfo {
            public int index;
            public float height;
            public float posToBound;
            public float topToBound;
        }
        //////////////////////////////////////////////////////////////
        //// 滚动界面的代理类
        //////////////////////////////////////////////////////////////
        [HideInInspector] public IScrollRectDelegate ScrollRectDelegate; 
        [SerializeField] private bool EnableObjectPool = false;

        private RectTransform _thisRectTransform;
        private RectTransform m_ThisRectTransform{ get{ if(_thisRectTransform == null){ _thisRectTransform = this.gameObject.GetComponent<RectTransform>(); } return _thisRectTransform; } }
        private List<CellInfo> m_CellInfoList = new List<CellInfo>();
        private Dictionary<int, IScrollRectExCell> m_CellDict = new Dictionary<int, IScrollRectExCell>();
        private int m_PreStartIndex = -1;
        private int m_PreEndIndex = -1;
        internal Stack<IScrollRectExCell> m_CellPool = new Stack<IScrollRectExCell>(); 
        private Vector2 m_Velocity = Vector2.zero;
        private Vector3 m_MoveTargetPosition = Vector3.zero;
        private bool m_Moving = false;
        private float m_MoveTime = 0.0f;

        public void FillCells(int offset = 0){
            if(ScrollRectDelegate == null){
                throw new SystemException("Please asign the ScrollRectDelegate before calling this method, e.g: scroll.ScrollRectDelegate = this;");
            }
            if(this.vertical && this.horizontal){
                throw new SystemException("only vertical or horizontal!");
            }
            // 回收所有的节点
            foreach(KeyValuePair<int, IScrollRectExCell> cell in m_CellDict){
                GameObject.DestroyImmediate(cell.Value.gameObject);
            }
            m_CellDict.Clear();
            m_CellInfoList.Clear();
            m_PreStartIndex = -1;
            m_PreEndIndex = -1;
            m_CellPool.Clear();
            m_Moving = false;

            int childCount = ScrollRectDelegate.GetCellCounts();
            float total_height = 0.0f;
            for(int index = 0; index < childCount; index++){
                float height = ScrollRectDelegate.GetScrollRectCellWidthOrHeightByIndex(index);
                CellInfo cellInfo = new CellInfo();
                cellInfo.index = index;
                cellInfo.height = height;
                cellInfo.topToBound = total_height;
                cellInfo.posToBound = total_height + height/2;
                m_CellInfoList.Add(cellInfo);
                total_height += height;
            }

            var contentTF = this.content.transform;
            if(this.vertical){
                if(total_height < this.m_ThisRectTransform.sizeDelta.y){
                    total_height = this.m_ThisRectTransform.sizeDelta.y;
                }
                // 设置 Content 的高度
                this.content.sizeDelta = new Vector2(this.content.sizeDelta.x, total_height);
                // 设置 Content 的起始位置
                contentTF.localPosition = new Vector3(contentTF.localPosition.x, 0, contentTF.localPosition.z);
            }else{
                if(total_height < this.m_ThisRectTransform.sizeDelta.x){
                    total_height = this.m_ThisRectTransform.sizeDelta.x;
                }
                // 设置 Content 的高度
                this.content.sizeDelta = new Vector2(total_height, this.content.sizeDelta.y);
                // 设置 Content 的起始位置
                contentTF.localPosition = new Vector3(0, contentTF.localPosition.y, contentTF.localPosition.z);
            }
            // 刷新
            this.RefreshCells();         
        }

        public void ScrollToIndex(int index, float time){
            if(index >=0 && index < m_CellInfoList.Count){
                float offset = this.m_CellInfoList[index].topToBound;
                this.ScrollByOffset(offset, time);
            }
        }

        public void ScrollByOffset(float offset, float time){
            var contentTF = this.content.transform;
            if(this.vertical){
                if(time < 0.001f){
                    this.content.localPosition = new Vector3(contentTF.localPosition.x, offset, contentTF.localPosition.z);
                    return;
                }
                float ds = offset - contentTF.localPosition.y;
                if(Math.Abs(ds) < 0.001f){
                    m_Moving = false;
                }else {
                    var speed = Math.Abs(ds/time);
                    m_Moving = true;
                    m_Velocity.y = speed;
                    m_MoveTargetPosition = new Vector3(contentTF.localPosition.x, offset, contentTF.localPosition.z);
                    m_MoveTime = time;
                }
            }else{
                if(time < 0.001f){
                    this.content.localPosition = new Vector3(offset, contentTF.localPosition.y, contentTF.localPosition.z);
                    return;
                }
                float ds = offset + contentTF.localPosition.x;
                if(Math.Abs(ds) < 0.001f){
                    m_Moving = false;
                }else {
                    var speed = Math.Abs(ds/time);
                    m_Moving = true;
                    m_Velocity.x = speed;
                    m_MoveTargetPosition = new Vector3(-offset, contentTF.localPosition.y, contentTF.localPosition.z);
                    m_MoveTime = time;
                }
            }
        }

        void Update(){
            if(m_Moving){
                float speed = this.vertical ? m_Velocity.y : m_Velocity.x;
                // Move our position a step closer to the target.
                float step =  speed * Time.deltaTime; // calculate distance to move
                this.content.localPosition = Vector3.MoveTowards(this.content.localPosition, m_MoveTargetPosition, step);
                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(this.content.localPosition, m_MoveTargetPosition) < 0.001f) {
                    m_Moving = false;
                }
                m_MoveTime = m_MoveTime - Time.deltaTime;
                if(m_MoveTime <= 0.0f){
                    m_Moving = false;
                }
            }
        }

        void AddCellRange(int from, int to){
            if(to < from){ int tmp = to; to = from; from = tmp; }
            for(int i=from; i<=to; ++i){
                this.AddCell(i);
            }
        }

        void RemoveCellRange(int from, int to){
            if(to < from){ int tmp = to; to = from; from = tmp; }
            for(int i=to; i>=from; --i){
                this.RemoveCell(i);
            }
        }
        
        void AddCell(int index){
            // Debug.Log($"AddCell {index}");
            CellInfo cellInfo = m_CellInfoList[index];
            IScrollRectExCell cell = this.AllocateCell();
            cell.gameObject.transform.SetParent(this.content.transform);
            RectTransform tf = cell.gameObject.GetComponent<RectTransform>();
            tf.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if(this.vertical){
                tf.sizeDelta = new Vector2(tf.sizeDelta.x, cellInfo.height);
                tf.localPosition = new Vector3(0, -cellInfo.posToBound, 0);
            }else{
                tf.sizeDelta = new Vector2(cellInfo.height, tf.sizeDelta.y);
                tf.localPosition = new Vector3(cellInfo.posToBound, 0, 0);
            }
            cell.BindCellIndex(index);
            // 数据更新
            if(m_CellDict.ContainsKey(index)){
                // Error
                Debug.LogError($"不应该包含该数据的");
            }else{
                m_CellDict.Add(index, cell);
            }
        }

        void RemoveCell(int index){
            // Debug.Log($"RemoveCell {index}");
            if(m_CellDict.ContainsKey(index)){
                IScrollRectExCell cell = m_CellDict[index];
                m_CellDict.Remove(index);
                cell.UnBindCellIndex();
                cell.gameObject.transform.SetParent(this.m_ThisRectTransform, false);
                this.ReleaseCell(cell);
            }else{
                Debug.LogError($"应该包含该数据的");
            }
        }

        IScrollRectExCell AllocateCell(){
            if(this.EnableObjectPool){
                if(m_CellPool.Count == 0){
                    return ScrollRectDelegate.CreateCell();
                }else{
                    var cell = m_CellPool.Pop();
                    cell.gameObject.SetActive(true);
                    return cell;
                }
            }else{
                return ScrollRectDelegate.CreateCell();
            }
        }

        public void ReleaseCell(IScrollRectExCell cell){
            if(this.EnableObjectPool){
                cell.gameObject.SetActive(false);
                m_CellPool.Push(cell);
            }else{
                GameObject.DestroyImmediate(cell.gameObject);
            }
        }

        private void RefreshCells(){
            if(m_CellInfoList.Count == 0){
                return;
            }
            float contentD;
            if(this.vertical){
                contentD = this.content.transform.localPosition.y;
            }else{
                contentD = -this.content.transform.localPosition.x;
            }
            if(contentD < 0.0f){
                contentD = 0.0f;
            }
            // Debug.Log($"contentD: {contentD}");
            // 其他情况，一律进入逻辑判断;
            int startIndex = 0;
            int endIndex = 0;
            for(int i = 0; i<m_CellInfoList.Count; i++){
                CellInfo cellInfo = m_CellInfoList[i];
                if(cellInfo.topToBound <= contentD && cellInfo.topToBound+cellInfo.height > contentD){
                    startIndex = cellInfo.index;
                    endIndex = startIndex;
                    // 直接向下找到后面的一个
                    var CONST_VALUE = contentD + (this.vertical ? m_ThisRectTransform.sizeDelta.y : m_ThisRectTransform.sizeDelta.x);
                    for(int j = i; j<m_CellInfoList.Count; j++){
                        CellInfo cellInfoJ = m_CellInfoList[j];
                        endIndex = cellInfoJ.index;
                        if(cellInfoJ.topToBound+cellInfoJ.height > CONST_VALUE){
                            break;
                        }
                    }
                    break;
                }
            }
            
            // 上下各多加多一项，以优化加载的体验
            if(startIndex > 0){ --startIndex; }
            if(endIndex < m_CellInfoList.Count-1){ ++endIndex; }

            // 根据 startIndex 和 endIndex 来加载 Cell
            int preStartIndex = m_PreStartIndex;
            int preEndIndex = m_PreEndIndex;
            if(preStartIndex == -1 || preEndIndex == -1){
                // Debug.Log($"preStartIndex {preStartIndex}  preEndIndex {preEndIndex}  startIndex {startIndex}  endIndex {endIndex}");
                // 首次加载
                m_PreStartIndex = startIndex;
                m_PreEndIndex = endIndex;
                for(int i=startIndex; i<=endIndex; i++){
                    this.AddCell(i);
                }
                return;
            }
            bool isChange = true;
            if(startIndex == preStartIndex){
                if(endIndex == preEndIndex){
                    // do nothing
                    isChange = false;
                }else if(endIndex > preEndIndex){
                    this.AddCellRange(preEndIndex+1, endIndex);
                }else{
                    this.RemoveCellRange(endIndex+1, preEndIndex);
                }
            }else if(startIndex > preStartIndex){
                if(endIndex == preEndIndex){
                    this.RemoveCellRange(preStartIndex, startIndex-1);
                }else if(endIndex > preEndIndex){
                    if(preEndIndex>=startIndex){
                        this.RemoveCellRange(preStartIndex, startIndex-1);
                        this.AddCellRange(preEndIndex+1, endIndex);
                    }else{
                        this.RemoveCellRange(preStartIndex, preEndIndex);
                        this.AddCellRange(startIndex, endIndex);
                    }
                }else{
                    this.RemoveCellRange(preStartIndex, startIndex-1);
                    this.RemoveCellRange(endIndex+1, preEndIndex);
                }
            }else{
                if(endIndex == preEndIndex){
                    this.AddCellRange(startIndex, preStartIndex-1);
                }else if(endIndex > preEndIndex){
                    this.AddCellRange(startIndex, preStartIndex-1);
                    this.AddCellRange(preEndIndex+1, endIndex);
                }else{
                    if(endIndex >= preStartIndex){
                        this.RemoveCellRange(endIndex+1, preEndIndex);
                        this.AddCellRange(startIndex, preStartIndex-1);
                    }else{
                        this.RemoveCellRange(preStartIndex, preEndIndex);
                        this.AddCellRange(startIndex, endIndex);
                    }
                }
            }

            if(isChange){
                // Debug.Log($"preStartIndex {preStartIndex}  preEndIndex {preEndIndex}  startIndex {startIndex}  endIndex {endIndex}");
                m_PreStartIndex = startIndex;
                m_PreEndIndex = endIndex;
            }
        }

        //////////////////////////////////////////////////////////////
        //// 注册滚动事件的监听
        //////////////////////////////////////////////////////////////
        protected override void OnEnable()
        {
            base.OnEnable();
            //Subscribe to the ScrollRect event
            this.onValueChanged.AddListener(scrollRectCallBack);
        }

        //Will be called when ScrollRect changes
        void scrollRectCallBack(Vector2 value)
        {
            this.RefreshCells();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //Un-Subscribe To ScrollRect Event
            this.onValueChanged.RemoveListener(scrollRectCallBack);
            m_Velocity = Vector2.zero;
        }
    }
}
