using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using komal.puremvc;

// 继承：按下，抬起和离开的三个接口
public class ButtonExtension : ComponentEx, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	// 延迟时间
	private float delay = 0.3f;

	// 按钮是否是按下状态
	public  bool isDown = false;

	
	// 按钮最后一次是被按住状态时候的时间
	private float lastIsDownTime;
	public override string[] ListNotificationInterests()
	{
		return new string[] {


			};
	}

	public override void HandleNotification(INotification notification)
	{

	}

	

	void Update()
	{

		// 如果按钮是被按下状态
		if (isDown)
		{

			// 当前时间 -  按钮最后一次被按下的时间 > 延迟时间0.2秒
			if (Time.time - lastIsDownTime > delay)
			{
				// 触发长按方法
				Debug.Log("长按");
				// 记录按钮最后一次被按下的时间
				lastIsDownTime = Time.time;
				facade.SendNotification("MSG_Press", GetComponent<Block>());
			}
			
		}

		


	}

	// 当按钮被按下后系统自动调用此方法
	public void OnPointerDown(PointerEventData eventData)
	{
		if (GetComponent<Block>()._hasChecked) {
			isDown = true;

			lastIsDownTime = Time.time;
		}
		
	}

	// 当按钮抬起的时候自动调用此方法
	public void OnPointerUp(PointerEventData eventData)
	{
		facade.SendNotification("MSG_Up", GetComponent<Block>());
		isDown = false;
	}

	// 当鼠标从按钮上离开的时候自动调用此方法
	public void OnPointerExit(PointerEventData eventData)
	{
		facade.SendNotification("MSG_Up", GetComponent<Block>());
		isDown = false;
	}
}
