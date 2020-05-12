
/* Brief: Small script for easily destroying an object after a while
 * Author: Komal
 * Date: "2019-07-16"
 */

using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float Delay = 3f; //Delay in seconds before destroying the gameobject

    void Start ()
    {
        Destroy (gameObject, Delay);
    }
}
