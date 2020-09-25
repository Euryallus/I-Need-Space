using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalRotate : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -1), Space.Self); //rotates game object around the LOCAL x axis once per frame (only applies to the portal object)
    }
}
