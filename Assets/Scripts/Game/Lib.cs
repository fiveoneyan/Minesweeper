using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Lib :MonoBehaviour
{
    public static Lib instance;
    public  Dictionary<int, Sprite> dicnumber = new Dictionary<int, Sprite>();
    public  Dictionary<int, Sprite> FlagBlock = new Dictionary<int, Sprite>();
    public Sprite[] imgnumber;
    public Sprite[] FlagB;
    void Awake()
    {
        instance = this;
        for (int i = 0; i < imgnumber.Length; i++)
        {
            dicnumber.Add(i,imgnumber[i]);
        }
        for (int i = 0; i < FlagB.Length; i++)
        {
            FlagBlock.Add(i, FlagB[i]);
        }

    }
}
