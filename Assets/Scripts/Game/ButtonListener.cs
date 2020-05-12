using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using komal.puremvc;

public class ButtonListener : ComponentEx, IPointerDownHandler, IPointerUpHandler
{
   public  bool  isPress=false;
    public static ButtonListener instance;
    public override string[] ListNotificationInterests()
    {
        return new string[] {

            
            };
    }

    public override void HandleNotification(INotification notification)
    {
       
    }

    protected  void Start()
    {
       
        instance = this;
       

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("按下！！！！");
        isPress = true ;
        facade.SendNotification("MSG_Press", this);

    }


    public void OnPointerUp(PointerEventData eventData)
    {
        print("抬起！！！！");
        isPress = false;
        facade.SendNotification("MSG_Up", this);
    }

}
