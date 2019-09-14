using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    public float reach = 10;

    public LayerMask layerMask;

    public GameObject curMirror;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, reach, layerMask))
        {
            if(curMirror != null)
            {
                curMirror.GetComponent<MirrorRotation>().selected = false;
            }

            curMirror = hit.transform.parent.gameObject;
            curMirror.GetComponent<MirrorRotation>().selected = true;
        }
        else if(curMirror != null)
        {
            curMirror.GetComponent<MirrorRotation>().selected = false;
            curMirror = null;
        }
    }
}
