using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public float leftBorder;
    public float rightBorder;
    public float topBorder;
    public float downBorder;
    private float width;
    private float height;
    private int count;
    public float speed;
    bool fistTan = false;
    void Start()
    {
        //世界坐标的右上角  因为视口坐标右上角是1,1,点
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
         Mathf.Abs(-Camera.main.transform.position.z)));
        //世界坐标左边界
        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        //世界坐标右边界

        rightBorder = cornerPos.x;
        //世界坐标上边界
        topBorder = cornerPos.y;
        //世界坐标下边界
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;

    }
    void Update()
    {

        transform.Translate(new Vector3(0, Time.deltaTime * speed, 0f));
        //上 如果物体的Y轴和屏幕Y轴相等那么就是证明到达边界
        if (transform.localPosition.y >= topBorder)
        {
            transform.position = new Vector3(transform.localPosition.x, topBorder, 0);
            //计算物体到达边界之后又按原来角度弹回去
            if (transform.eulerAngles.z > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 180 - transform.eulerAngles.z);
                count++;
            }
        }
        //下
        if (transform.localPosition.y <= downBorder)
        {
            transform.position = new Vector3(transform.localPosition.x, downBorder, 0);

            if (transform.eulerAngles.z > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 180 - transform.eulerAngles.z);
                count++;
            }
        }
        //左
        if (transform.localPosition.x <= leftBorder)
        {
            transform.position = new Vector3(leftBorder, transform.localPosition.y, 0);
            if (transform.eulerAngles.z > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
                count++;
            }
        }
        //右
        if (transform.localPosition.x >= rightBorder)
        {
            transform.position = new Vector3(rightBorder, transform.localPosition.y, 0);
            if (transform.eulerAngles.z > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
                count++;
            }
        }

    }
}