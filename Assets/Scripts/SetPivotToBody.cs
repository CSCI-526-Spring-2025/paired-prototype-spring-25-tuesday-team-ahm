using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPivotToBody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform body = transform.Find("Body");

        if (body != null)
        {
            Vector3 bodyPosition = body.position;

            Vector3 oriPosition = transform.position;
            transform.position = bodyPosition;

            Vector3 offset = bodyPosition - oriPosition;

            transform.position = bodyPosition;
            foreach (Transform child in transform)
            {
                child.position -= offset;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
