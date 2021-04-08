using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePortal : MonoBehaviour
{
    public string dimensionName;
    Vector3 size;

    Vector3 goal;
    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        goal = size;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transform.localScale + (goal - transform.localScale)/10;
    }

     void OnMouseDown(){
        Game.Instance.PortalClicked(dimensionName,transform.position);
    }

    void OnMouseEnter(){
        goal = size*1.2f;
    }

    void OnMouseExit(){
        goal = size;
    }
}
