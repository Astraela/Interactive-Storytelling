using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    public List<Button> slots;
    public Item[] items;
    
    int selectedItem;
    Item selectedItemObj;

    bool cantEquipState = false;
    void Start()
    {
        items = new Item[slots.Count];
        for(int i = 0; i < slots.Count; i++){
            int nr = i;
            slots[i].onClick.AddListener(() =>buttonClick(nr));
        }
    }
    
    public bool hasItem(string item){
        for(int i = 0; i < items.Length;i++){
            if(items[i] != null){
                if(items[i].name == item){
                    return true;
                }
            }
        }
        return false;
    }

    void cancel(){
        Game.Instance.interactable = true;
        Dialogue.Instance.buttons[0].onClick.RemoveListener(cancel);
    }

    public void CantEquip(){
        cantEquipState = true;
        Game.Instance.interactable = false;
        Dialogue.Instance.SetDialogue("All items are taken, Select an item to replace.", new List<string>(){"Cancel"});
        Dialogue.Instance.buttons[0].onClick.AddListener(cancel);
    }

    public void Equip(Item item){
        for(int i = 0; i < items.Length;i++){
            if(items[i] == null || items[i].name == null){
                items[i] = item;
                slots[i].transform.GetChild(1).GetComponent<Image>().sprite = item.sprite;
                slots[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,1);
                slots[i].transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
                return;
            }
        }
        selectedItemObj = item;
        CantEquip();
    }

    public void Empty(){
        for(int i = 0; i < items.Length; i++){
            items[i] = null;
            slots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
            slots[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0);
            slots[i].transform.GetChild(1).GetComponent<Image>().preserveAspect = false;
        }
    }

    void yes(){
        items[selectedItem] = selectedItemObj;
        slots[selectedItem].transform.GetChild(1).GetComponent<Image>().sprite = selectedItemObj.sprite;
        slots[selectedItem].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,1);
        slots[selectedItem].transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
        Dialogue.Instance.buttons[0].onClick.RemoveListener(yes);
        Dialogue.Instance.buttons[1].onClick.RemoveListener(no);
        Game.Instance.interactable = true;
    }

    void no(){
        CantEquip();
        Dialogue.Instance.buttons[0].onClick.RemoveListener(yes);
        Dialogue.Instance.buttons[1].onClick.RemoveListener(no);
    }

    public void buttonClick(int btn){
        if(!cantEquipState)
            return;
        cantEquipState = false;
        selectedItem = btn;
        Dialogue.Instance.SetDialogue("Are you sure you want to replace item " + (btn+1) + "?", new List<string>(){"Yes","No"});
        Dialogue.Instance.buttons[0].onClick.AddListener(yes);
        Dialogue.Instance.buttons[1].onClick.AddListener(no);
    }

}
