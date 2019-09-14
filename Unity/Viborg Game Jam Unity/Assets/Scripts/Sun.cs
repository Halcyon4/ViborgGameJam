using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public Transform target;
    public MirrorReflection mirrorReflection;

    [HideInInspector] public Vector3 hitPos;

    public LayerMask layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, Mathf.Infinity, layerMask))
        {
            hitPos = hit.point;

            if(hit.transform.tag == "Mirror")
            {
                mirrorReflection.ReflectRay(hit.point, target.position - transform.position);
                mirrorReflection.hitByLigtRay = true;
            }
            else
            {
                mirrorReflection.hitByLigtRay = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, hitPos);
    }
}
