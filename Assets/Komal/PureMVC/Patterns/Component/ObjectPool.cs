using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;

namespace komal.puremvc
{
    public enum AllocateState {
        InUse,
        Recycled 
    }

    //////////////////////////////////////////////////////////////
    //// gameObject with ComponentEx Pool
    //// All ComponentEx are IPoolableEx
    //////////////////////////////////////////////////////////////
    public interface IPoolableEx {
        AllocateState AllocateState { get; set; }
        void OnPoolableAllocated(bool isReuse);
        void OnPoolableReleased();
    }

    public sealed class GameObjectPool<U> where U : ComponentEx, IPoolableEx {
        internal Stack<GameObject> items; 
        private Func<GameObject> factory; 
        private int Count => items.Count;
        public int Capacity { get; set; } 

        internal GameObjectPool(Func<GameObject> factory,int capacity = 16) {
            this.factory = factory;
            this.Capacity = capacity;
            items = new Stack<GameObject>();
        }
        public void Clear() { items.Clear(); }
        public U Allocate() {
            GameObject gameObj = null;
            bool isReuse = true;
            if(items.Count == 0){
                gameObj = this.factory.Invoke();
                isReuse = false;
            }else{
                gameObj = items.Pop();
            }
            U item = gameObj.GetComponent<U>();
            item.AllocateState = AllocateState.InUse;
            item.OnPoolableAllocated(isReuse);
            return item;
        }
        public void Release(U target)
        {
            target.OnPoolableReleased();
            if (target.AllocateState.Equals(AllocateState.InUse) && items.Count < Capacity) 
            {
                items.Push(target.gameObject);
            }else{
                GameObject.DestroyImmediate(target.gameObject);
            }
        }
    }

    //////////////////////////////////////////////////////////////
    //// 普通对象池(一般用于数据)
    //////////////////////////////////////////////////////////////
    public interface IPoolable {
        AllocateState AllocateState { get; set; }
        void OnPoolableAllocated();
        void OnPoolableReleased();
    }

    public class Poolable : IPoolable
    {
        public AllocateState AllocateState { get; set; }

        public virtual void OnPoolableAllocated() { }

        public virtual void OnPoolableReleased() { }
    }

    public sealed class ObjectPool<U> where U : IPoolable {
        internal ConcurrentStack<U> items; 
        private Func<U> factory; 
        private int Count => items.Count;
        public int Capacity { get; set; } 

        internal ObjectPool(Func<U> factory,int capacity = 16) {
            this.factory = factory;
            this.Capacity = capacity;
            items = new ConcurrentStack<U>();
        }
        public void Clear() { items.Clear(); }
        public U Allocate() {
            U item = default(U);
            if (items.IsEmpty || !items.TryPop(out item)) {
                item = factory.Invoke();
            }
            item.AllocateState = AllocateState.InUse;
            item.OnPoolableAllocated();
            return item;
        }
        public void Release(U target)
        {
            target.OnPoolableReleased();
            if (target.AllocateState.Equals(AllocateState.InUse) && items.Count < Capacity) 
            {
                items.Push(target);
            }
        }
    }
}
