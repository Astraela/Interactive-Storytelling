using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : Singleton<Dragger>
{
    public Transform dragObject;
    public SpriteRenderer section;
    public GameObject FinalScreen;

    List<Transform> itemList = new List<Transform>();

    float max = -.48f;
    float step = .01f;

    void Start(){
        var items = Inventory.Instance.items;
        foreach(Item item in items){
            if(item == null || item.name == null) continue;
            var newItem = Instantiate(item.prefab);
            //newItem.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.5f,.5f,0));
            newItem.transform.position = section.bounds.center;
            newItem.GetComponent<InteractableItem>().Draggable = true;
            newItem.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            newItem.transform.localScale = new Vector3(3.736659f,3.736659f,3.736659f);
            itemList.Add(newItem.transform);
            newItem.transform.SetParent(GameObject.Find("Empty").transform);
        }
        UpdatePositions();
        Game.Instance.paintMenu.SetActive(true);
        Game.Instance.inventory.SetActive(false);
    }

    void Update(){
        if(dragObject == null)
            return;
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3( mousePos.x/Screen.width,mousePos.y/Screen.height,1);
        Vector3 newPos = Camera.main.ViewportToWorldPoint(mousePos);
        


        if(newPos.x < section.bounds.min.x || newPos.y < section.bounds.min.y ||
            newPos.x > section.bounds.max.x || newPos.y > section.bounds.max.y)
                return;
        dragObject.position = newPos;
        if(Input.GetAxis("Mouse ScrollWheel") != 0){
            float value = Input.GetAxis("Mouse ScrollWheel");
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
                value *= 32f;
                dragObject.localRotation *= Quaternion.Euler(0,0,value);
            }else{
                
                dragObject.localScale += new Vector3(value,value,value);
            }
        }
    }

    void UpdatePositions(){
        for(float i = 0; i < itemList.Count; i++){
            float pos = max + (step*i);
            itemList[(int)i].position = itemList[(int)i].position - new Vector3(0,0,itemList[(int)i].position.z) + new Vector3(0,0,pos);
        }
    }

    public void Replay(){
        foreach(Transform itemt in itemList){
            Destroy(itemt.gameObject);
        }
        Destroy(this);
    }

    public void PickUp(Transform obj){
        dragObject = obj;
    }

    public void Finish(){
        Game.Instance.paintMenu.SetActive(false);
        Camera.main.transform.position = new Vector3(-114,-32.5084f,-10);
        Camera.main.orthographicSize = 11.6f;
        Game.Instance.endMenu.SetActive(true);
        FinalScreen.SetActive(true);
        foreach(Transform transform in itemList){
            transform.GetComponent<InteractableItem>().enabled = false;
        }
        this.enabled = false;
    }

    public void Drop(){
        itemList.Remove(dragObject);
        itemList.Add(dragObject);
        UpdatePositions();
        dragObject = null;
    }
}
