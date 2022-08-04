using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public GameObject startReference, endReference;
    public BoxCollider hiddenPlatform;


    private void Start()
    {
        Vector3 direction = endReference.transform.position - startReference.transform.position;
        float distance = direction.magnitude;
        direction = direction.normalized;
        hiddenPlatform.transform.forward = direction;
        hiddenPlatform.size = new Vector3(hiddenPlatform.size.x, hiddenPlatform.size.y,distance);

        hiddenPlatform.transform.position = startReference.transform.position + (direction * distance / 2) + (new Vector3(0,-direction.z,direction.y) * hiddenPlatform.size.y / 2);
    }

}
