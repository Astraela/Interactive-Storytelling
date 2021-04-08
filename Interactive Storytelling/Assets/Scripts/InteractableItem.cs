using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public string itemName;
    public bool Draggable = false;

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
        if(!Draggable)
            transform.localScale = transform.localScale + (goal - transform.localScale)/10;
    }

    void OnMouseDown(){
        if(!Draggable){
            Game.Instance.ItemClicked(itemName);
        }else{
            Dragger.Instance.PickUp(transform);
        }
    }

    void OnMouseUp(){
        if(Draggable)
            Dragger.Instance.Drop();
    }

    void OnMouseEnter(){
        goal = size*1.2f;
    }

    void OnMouseExit(){
        goal = size;
    }
}
