using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotation : MonoBehaviour
{

    public float speed = 0.1f;
    [SerializeField] float roationAmount = 22.5f;

    public Transform targetTransform;

    public float rotationLength = 45;

    private enum rotationType { up, right, forward };

    public bool selected;
    [SerializeField]
    private rotationType rotationAxis = rotationType.up;
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
            switch (rotationAxis)
            {
                case rotationType.up:
                    targetTransform.Rotate(transform.up, rotationLength);
                    break;
                case rotationType.right:
                    targetTransform.Rotate(transform.right, rotationLength);
                    break;
                case rotationType.forward:
                    targetTransform.Rotate(transform.forward, rotationLength);
                    break;
                default:
                    targetTransform.Rotate(transform.up, rotationLength);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !rotating && selected)
        {

            switch (rotationAxis)
            {
                case rotationType.up:
                    targetTransform.Rotate(transform.up, -rotationLength);
                    break;
                case rotationType.right:
                    targetTransform.Rotate(transform.right, -rotationLength);
                    break;
                case rotationType.forward:
                    targetTransform.Rotate(transform.forward, -rotationLength);
                    break;
                default:
                    targetTransform.Rotate(transform.up, -rotationLength);
                    break;
            }
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
