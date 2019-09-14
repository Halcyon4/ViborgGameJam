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

        //Clear current selected item
        MirrorRotation curRotation;
        if (curMirror != null)
        {
            curRotation = curMirror.GetComponent<MirrorRotation>();
            if (curRotation != null)
            {
                curRotation.selected = false;
            }
        }


        if (Physics.Raycast(transform.position, transform.forward, out hit, reach, layerMask))
        {
            curMirror = hit.transform.gameObject;

            curRotation = curMirror.GetComponent<MirrorRotation>();
            if (curRotation != null)
            {
                curRotation.selected = true;
            }
        }
        else if(curMirror != null)
        {
            //Dont do anything since nothing is in view
        }
    }
}
