using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using komal.sdk;
using komal.puremvc;

namespace komal.test {
    public class TestObjectPool : ComponentEx
    {
        public GameObject Prefab;
        private GameObjectPool<TestObjectPool_PoolableObject> pool = null;
        // Start is called before the first frame update
        void Start()
        {
            pool = new GameObjectPool<TestObjectPool_PoolableObject>(()=>Instantiate(Prefab));
        }

        void Update(){
            if(Input.GetKeyDown(KeyCode.Space)){
                var item = pool.Allocate();
                item.PrintInfo();
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                var item = pool.Allocate();
                item.PrintInfo();

                pool.Release(item);
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)){
                var item = pool.Allocate();
                item.PrintInfo();

                item = pool.Allocate();
                item.PrintInfo();

                pool.Release(item);
            }
        }
    }
}