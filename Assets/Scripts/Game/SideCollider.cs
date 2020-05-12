using UnityEngine;

public class SideCollider : MonoBehaviour
{
    [Header("border colliders --- ")]
    public Transform leftBorderCollider;
    public Transform rightBorderCollider;

    [Header("half collider width --- ")]
    public float halfColliderWidth = 0.5f;

    // half screen width size
    private float halfScreenWidth;

    public static SideCollider instance;
    private void Start()
    {
        instance = this;

        float halfSreenHeight = Camera.main.orthographicSize;
        halfScreenWidth = Screen.width / (float)Screen.height * halfSreenHeight;
        Debug.Log(halfScreenWidth);
        //
        leftBorderCollider.position = new Vector3(-halfScreenWidth - halfColliderWidth, leftBorderCollider.position.y, leftBorderCollider.position.z);
        rightBorderCollider.position = new Vector3(halfScreenWidth + halfColliderWidth, rightBorderCollider.position.y, rightBorderCollider.position.z);
    }
}
