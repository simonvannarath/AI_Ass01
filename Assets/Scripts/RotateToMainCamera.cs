using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMainCamera : MonoBehaviour {

    public Camera mainCamera;
    
	// Update is called once per frame
	void Update () {
        Vector3 dir = mainCamera.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);	
	}
}
