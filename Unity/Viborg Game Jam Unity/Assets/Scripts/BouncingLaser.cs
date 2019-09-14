using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingLaser : MonoBehaviour
{

    [SerializeField] private int m_maxBounce = 1;
    [SerializeField] private List<Vector3> m_bouncePoints = new List<Vector3>();
    private Ray m_ray;
    private RaycastHit hit;
    private Vector3 m_rayStartPos;
    private Vector3 m_rayRotation;

    private void Start()
    {
        m_rayStartPos = transform.position;
        m_rayRotation = Vector3.forward;
        m_bouncePoints.Add(m_rayStartPos);
    }
    private void Update()
    {
        DrawLaser();
    }

    private void DrawLaser ()
    {
        for (int i = 0; i < m_maxBounce; i++)
        {
                int layerMask = 1 << 8;
                layerMask = ~layerMask;

                RaycastHit hit;
                if (Physics.Raycast(m_rayStartPos, transform.TransformDirection(m_rayRotation), out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(m_rayRotation) * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");
               
                    m_rayStartPos = hit.point;
                    m_rayRotation = new Vector3(70,0);
                    m_bouncePoints.Add(m_rayStartPos);
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    Debug.Log("Did not Hit");

                }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, hit.point);
    }
}

