using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : Singleton<Items> {
    public List<Item> itemList = new List<Item>();

}

[System.Serializable]
public class Item{
    public string name;
    public Sprite sprite;
    public GameObject prefab;
    public string description;
}