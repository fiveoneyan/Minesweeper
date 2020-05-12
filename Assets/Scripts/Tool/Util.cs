using UnityEngine;

public class Util
{
    private static Util _instance;
    public static Util Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Util();

            return _instance;
        }
    }

    public Vector3 UI2World(Vector3 pos2D, Transform target)
    {
        Vector3 dir = (target.position - Camera.main.transform.position);
        Vector3 norVec = Vector3.Project(dir, Camera.main.transform.forward);
        return Camera.main.ViewportToWorldPoint
            (
               new Vector3(
                   pos2D.x / Screen.width,
                   pos2D.y / Screen.height,
                   norVec.magnitude
               )
            );
    }

    public Vector3 World2UI(Vector3 pos3D)
    {
        Vector3 pos3 = Camera.main.WorldToScreenPoint(pos3D);
        Vector3 pos2 = UIManager.Instance.UICamera().ScreenToWorldPoint(pos3);
        pos2.z = 0;
        return pos2;
    }


    public DeviceModel GetDevice()
    {
        DeviceModel deviceModel = DeviceModel.IPhoneSE;
        float result = (Screen.width * 1.0f) / (Screen.height * 1.0f);
        if (Mathf.Approximately(result, 640.0f / 1136.0f))
            deviceModel = DeviceModel.IPhoneSE;
        else if (Mathf.Approximately(result, 852.0f / 1386.0f))
            deviceModel = DeviceModel.IPhoneSE;
        else if (Mathf.Approximately(result, 1125.0f / 2436.0f))
            deviceModel = DeviceModel.IPhoneX;
        else if (Mathf.Approximately(result, 828.0f / 1792.0f))
            deviceModel = DeviceModel.IPhoneX;
        else if (Mathf.Approximately(result, 1242.0f / 2688.0f))
            deviceModel = DeviceModel.IPhoneX;
        else if (Mathf.Approximately(result, 1536.0f / 2048.0f))
            deviceModel = DeviceModel.IPad;
        else if (Mathf.Approximately(result, 2048.0f / 2732.0f))
            deviceModel = DeviceModel.IPad;
        else if (Mathf.Approximately(result, 1668.0f / 2224.0f))
            deviceModel = DeviceModel.IPad;

        return deviceModel;
    }
}

public enum DeviceModel
{
    IPhoneSE,
    IPhoneX,
    IPad,
}
