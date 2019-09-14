using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotation : MonoBehaviour
{

    public float speed = 0.1f;
    [SerializeField] float roationAmount = 22.5f;

    public Transform targetTransform;

    public bool selected;
    bool rotating;
    float rotationProgress;

    cakeslice.Outline[] outlines;

    // Start is called before the first frame update
    void Start()
    {
        //Find all outline child objects

        outlines = gameObject.GetComponentsInChildren<cakeslice.Outline>() ;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !rotating && selected)
        {
            targetTransform.Rotate(transform.up, roationAmount);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !rotating && selected)
        {
            targetTransform.Rotate(transform.up, -roationAmount);
        }

        if (transform.rotation != targetTransform.rotation)
        {
            rotating = true;

            rotationProgress += speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, rotationProgress);
        }
        else
        {
            rotating = false;
            rotationProgress = 0;
        }

        //Update outline
        for(int i = 0; i < outlines.Length; i++)
        {
            outlines[i].enabled = selected;
        }
    }
}
