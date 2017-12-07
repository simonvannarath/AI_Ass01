using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{

    public GameObject objectToFollow;

    public float speed = 2.0f;

    void Update()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        //position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y, interpolation);
        //position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
        position.z = Mathf.Lerp(this.transform.position.z, (objectToFollow.transform.position.z - 5), interpolation);
        // transform.position = new Vector3(transform.position.x, target.position.y, -10);

        this.transform.position = position;
    }
}