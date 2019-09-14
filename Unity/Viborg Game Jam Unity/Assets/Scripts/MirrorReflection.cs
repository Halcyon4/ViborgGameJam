using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflection : MonoBehaviour
{

    Vector3 reflectionDirection;

    Vector3 hitPoint;
    Vector3? otherHitPoint;

    MirrorReflection mirrorReflection;
    SolarPanel solarPanel;

    public LayerMask layerMask;

    public bool hitByLigtRay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReflectRay(Vector3 hitPos, Vector3 direction)
    {
        RaycastHit hit;

        hitPoint = hitPos;

        reflectionDirection = Vector3.Reflect(direction, transform.forward);

        if(Physics.Raycast(hitPos, reflectionDirection, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag == "Mirror")
            {
                otherHitPoint = hit.point;

                mirrorReflection = hit.transform.parent.GetComponent<MirrorReflection>();

                mirrorReflection.hitByLigtRay = true;
                mirrorReflection.ReflectRay(hit.point, hit.point - hitPoint);
            }
            else if(mirrorReflection != null)
            {
                mirrorReflection.hitByLigtRay = false;
                mirrorReflection = null;
                otherHitPoint = null;
            }

            if(hit.transform.tag == "SolarPanel")
            {
                solarPanel = hit.transform.gameObject.GetComponent<SolarPanel>();
                solarPanel.powered = true;
            }
            else if(solarPanel != null)
            {
                solarPanel.powered = false;
                solarPanel = null;
            }
        }
        else
        {
            if (mirrorReflection != null)
            {
                mirrorReflection.hitByLigtRay = false;
                mirrorReflection = null;
                otherHitPoint = null;
            }

            if (solarPanel != null)
            {
                solarPanel.powered = false;
                solarPanel = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(hitByLigtRay)
        {
            if(otherHitPoint != null)
            {
                Gizmos.DrawLine(hitPoint, (Vector3)otherHitPoint);
            }
            else
            {
                Gizmos.DrawLine(hitPoint, hitPoint + reflectionDirection * 10f);
            }
        }
    }
}
