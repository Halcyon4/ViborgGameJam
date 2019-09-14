using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotation : MonoBehaviour
{

    public float speed = 0.1f;

    public Transform targetTransform;

    public bool selected;
    bool rotating;
    float rotationProgress;

    cakeslice.Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.GetComponent<cakeslice.Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !rotating && selected)
        {
            targetTransform.Rotate(transform.up, 45f);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !rotating && selected)
        {
            targetTransform.Rotate(transform.up, -45f);
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
        if(outline != null)
        {
            outline.enabled = selected;
        }
    }
}
