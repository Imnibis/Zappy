using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    Transform camera;
    Quaternion originalRotation;

    void Start()
    {
        camera = GetComponent<Canvas>().worldCamera.transform;
        originalRotation = Map.OrientationToQuaternion(Orientation.SOUTH);
    }

    void Update()
    {
        transform.rotation = camera.rotation * originalRotation;
    }
}
