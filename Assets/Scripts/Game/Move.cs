using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;
using komal.puremvc;
/// <summary>
/// 点击屏幕实现缩放与旋转，移动
/// </summary>
public class Move : ComponentEx
{
    float x, y;

    float Speed = 5;
    public Vector3 OrinaScale;
    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)
    float minX, maxX, minY, maxY;
    public Vector3 scale;
    public RectTransform container;
    public bool isHem=true;
  

    // 位置偏移量
    Vector3 offset = Vector3.zero;

    private void Start()
    {
        scale = transform.localScale;
        transform.localScale = Vector3.one*1f;

        Magnet();
        SetDragRange();
        Aim();
        isHem = true;
    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {

            "MSG_SetDragRange",
            "MSG_Magnet",
            "MSG_Aim",
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case "MSG_SetDragRange":
                SetDragRange();
                break;
            case "MSG_Magnet":
                SetDragRange();
                Magnet();
                break;
            case "MSG_Aim":
                SetDragRange();
                Start();
                break;
           
        }
    }

    void Aim() {
        if (container.rect.height >= GetComponent<RectTransform>().rect.height)
        { transform.localPosition = new Vector3(maxX + 25, maxY - 25, 0); }
        else {
            transform.localPosition = new Vector3(maxX + 25, -maxY - 25, 0);
        }


    }

    // 设置最大、最小坐标
    void SetDragRange()
    {
 
        minX = -((GetComponent<RectTransform>().rect.width / 2) * transform.localScale.x - container.rect.width / 2);
        //Debug.Log(minX);

        maxX =Mathf.Abs( (GetComponent<RectTransform>().rect.width / 2) * transform.localScale.x - container.rect.width / 2);
      //  Debug.Log(maxX);



        maxY = Mathf.Abs(container.rect.height / 2 - ((GetComponent<RectTransform>().rect.height / 2) * transform.localScale.x));
       // Debug.Log(maxY);
    }

    void Magnet() {

        
            if (transform.localPosition.y <= -maxY)
            {

                transform.localPosition = new Vector3(transform.localPosition.x, -maxY- 25, 0);
               
               // Debug.Log("a上");

            }
            //下
             if (transform.localPosition.y >= maxY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, maxY + 25, 0);
                

             //   Debug.Log("下");
                
            }
            // 左
           if (transform.localPosition.x >= maxX)
            {
                transform.localPosition = new Vector3(maxX + 25, transform.localPosition.y, 0);
              //  Debug.Log("左");
            }
            //右
            if (transform.localPosition.x <= minX)
            {
                transform.localPosition = new Vector3(minX - 25, transform.localPosition.y, 0);
              //  Debug.Log("右");
            }
        


    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(2, 2, 2);
            SetDragRange();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.localScale = Vector3.one;
            SetDragRange();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(0.56f, 0.56f, 0.56f);
            SetDragRange();
        }
        //没有触摸  
        //if (Input.touchCount <= 0)
        //{
        //    return;
        //}

        int diff = PlayerPrefs.GetInt("diff");

        if (diff == 2 || diff == 3)
        {


            //#if UNITY_EDITOR
            //            if (Input.GetMouseButton(0))
            //#else


            //#endif

            if (Input.touchCount <= 0)
            {

                Magnet();


            }
            //单点触摸， 水平上下移动

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //获取x轴
                x = Input.GetAxis("Mouse X") * Speed;
                //获取y轴
                y = Input.GetAxis("Mouse Y") * Speed;
                 this.transform.Translate(x, y, 0);

                //if (container.rect.height >= GetComponent<RectTransform>().rect.height * transform.localScale.y)
                //{
                //    Debug.Log("aa");
                    
                //    this.transform.Translate(x, 0, 0);
                   
                //    isHem = false;
                //}
                //else
                //{
                //    isHem = false;
                //    this.transform.Translate(x, y, 0);
                //}
                if ((scale.x <= 0.56f && scale.y <= 0.56f && scale.z <= 0.56f && diff == 2) || (scale.x <= 0.3f && scale.y <= 0.3f && scale.z <= 0.3f && diff == 3))
                {
                   
                    transform.position = new Vector3(0f, 0f, 0f);

                }
                Debug.Log(container.rect.height + ">>>>>" + GetComponent<RectTransform>().rect.height * transform.localScale.y);
                
            }
    
            if ((scale.x <= 0.56f && scale.y <= 0.56f && scale.z <= 0.56f && diff == 2) || (scale.x <= 0.3f && scale.y <= 0.3f && scale.z <= 0.3f && diff == 3))
            {

                transform.position = new Vector3(0f, 0f, 0f);

            }

            //if (Input.GetMouseButtonUp(0))
            //{

            //    Magnet();
            //}
            //单点触摸， 水平上下旋转  
            /*if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;
                transform.Rotate(Vector3.down * deltaPos.x, Space.World);
                transform.Rotate(Vector3.right * deltaPos.y, Space.World);
            }*/

            //多点触摸, 放大缩小
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            //第2点刚开始接触屏幕, 只记录，不做处理  
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }

            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

            //两个距离之差，为正表示放大手势， 为负表示缩小手势  
            float offset = newDistance - oldDistance;

            //放大因子， 一个像素按 0.01倍来算(100可调整)  
            float scaleFactor = offset / 100f;
            Vector3 localScale = transform.localScale;
             scale = new Vector3(localScale.x + scaleFactor,
                                        localScale.y + scaleFactor,
                                        localScale.z + scaleFactor);
            if (scale.x <= 1f && scale.y <= 1f && scale.z <= 1f)
            {
                if (scale.x > 0.56f && scale.y > 0.56f && scale.z > 0.56f && diff == 2)
                {
                    transform.localScale = scale;
                    SetDragRange();

                }

                else if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f && diff == 3)
                {
                    transform.localScale = scale;
                    SetDragRange();
                }
            }

           
                //记住最新的触摸点，下次使用  
               oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;
        }
        else {

            transform.localScale = new Vector3(1f, 1f, 1f);
        
            transform.position=new Vector3(0f,0f,0f);
        }
    }
}