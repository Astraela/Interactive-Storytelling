using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    public float down = .2f;
    public float stopRange = .05f;
    Vector3 topSize;
    Vector3 bottomSize;

    bool goingDown = true;
    // Start is called before the first frame update
    void Start()
    {
        topSize = transform.localPosition;
        bottomSize = topSize - new Vector3(0,down,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(goingDown){
            transform.localPosition = transform.localPosition + (bottomSize-transform.localPosition)/200f;
            
            if(Vector3.Distance(transform.localPosition,bottomSize) < stopRange)
                goingDown = false;
        }else{
            transform.localPosition = transform.localPosition + (topSize-transform.localPosition)/200f;
            if(Vector3.Distance(transform.localPosition,topSize) < stopRange){
                goingDown = true;
                bottomSize = topSize - new Vector3(0,down,0);
            }
        }
    }
}
