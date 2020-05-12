using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using komal.puremvc;

public class TestObjectPool_PoolableObject : ComponentEx 
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start ID >> " + m_ID);
    }

    public void PrintInfo(){
        Debug.Log($">> m_ID {m_ID} mediatorName {mediatorName}");
    }
}
