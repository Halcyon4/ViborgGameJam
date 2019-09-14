using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public Transform target;
    public MirrorReflection mirrorReflection;

    public Vector3 hitPos;

    public LayerMask layerMask;

    public float maxRayLength = 20;

    public RayMeshGenerator meshGen;

    private List<Vector3> lastRayList = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //Note current mesh point set
        lastRayList = meshGen.GetRayPoints();

        //Clear mesh gen
        meshGen.clearPoints();

        //Add origin rayPoint
        meshGen.addPoint(transform.position);


        //Cast sunlight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, Mathf.Infinity, layerMask))
        {
            hitPos = hit.point;

            //Add hit to rayPoints
            meshGen.addPoint(hitPos);

            if(hit.transform.tag == "Mirror")
            {
                //Hit mirror, reflect
                mirrorReflection.ReflectRay(hit.point, target.position - transform.position, meshGen);
                mirrorReflection.hitByLigtRay = true;
            }
            else
            {
                mirrorReflection.hitByLigtRay = false;
            }
        }
        else
        {
            //Add rayPoint when no hit
            meshGen.addPoint(transform.position + (target.position - transform.position) * maxRayLength);
        }

        //Update Mesh if necessary
        //if(meshGen.GetRayPoints() != lastRayList)
        //{
            meshGen.UpdateMesh();
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, hitPos);
    }
}
