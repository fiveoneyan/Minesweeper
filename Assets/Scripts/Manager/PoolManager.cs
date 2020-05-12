using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private Dictionary<GType, List<GameObject>> dicPool = new Dictionary<GType, List<GameObject>>();

    void Awake()
    {
        instance = this;
    }

    public GameObject LoadObj(GType type, Transform parent)
    {
        GameObject obj = null;

        if (dicPool.ContainsKey(type))
        {
            if (dicPool[type].Count > 0)
            {
                obj = dicPool[type][0];
                dicPool[type].Remove(obj);
            }
            else
                Load(type, ref obj);
        }
        else
            Load(type, ref obj);

        obj.transform.SetParent(parent);
        obj.transform.localScale = Vector3.one;
        obj.SetActive(true);

        return obj;
    }

    void Load(GType type, ref GameObject obj)
    {
        obj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/" + type.ToString()), transform, true);
    }

    public void Delay2Pool(GType type, GameObject obj, float delay)
    {
        StartCoroutine(Add2Pool(type, obj, delay));
    }

    IEnumerator Add2Pool(GType type, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Add2Pool(type, obj);
    }

    public void Add2Pool(GType type, GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        if (dicPool.ContainsKey(type))
        {
            dicPool[type].Add(obj);
        }
        else
        {
            List<GameObject> objs = new List<GameObject>() { obj };
            dicPool.Add(type, objs);
        }
    }
}

public enum GType
{
    Block,
}
