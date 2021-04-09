using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : Singleton<Game>
{
    Item selectedItem;

    string selectedDimensionName;
    Vector3 portalPosition;

    public bool interactable = false;
    public GameObject inventory;
    public GameObject menu;
    public Animator bg;
    public GameObject section;
    public GameObject endMenu;
    public GameObject paintMenu;

    public GameObject screenshotObj;

    bool Started = false;

    void ItemYes(){
        Inventory.Instance.Equip(selectedItem);
        Dialogue.Instance.buttons[0].onClick.RemoveListener(ItemYes);
        Dialogue.Instance.buttons[1].onClick.RemoveListener(ItemNo);
            interactable = true;
    }

    void ItemNo(){
        Dialogue.Instance.buttons[0].onClick.RemoveListener(ItemYes);
        Dialogue.Instance.buttons[1].onClick.RemoveListener(ItemNo);
            interactable = true;
    }

    public void ItemClicked(string name){
        if(!interactable || Inventory.Instance.hasItem(name))
            return;
            interactable = false;
        selectedItem = Items.Instance.itemList.Find(x => x.name == name);
        Dialogue.Instance.SetDialogue(selectedItem.description + " Do you want to pick up this item?", new List<string>(){"Yes","No"});
        Dialogue.Instance.buttons[0].onClick.AddListener(ItemYes);
        Dialogue.Instance.buttons[1].onClick.AddListener(ItemNo);
    }

    void PortalYes(){
        Dimensions.Instance.transition(selectedDimensionName,portalPosition);
        Dialogue.Instance.buttons[0].onClick.RemoveListener(PortalYes);
        Dialogue.Instance.buttons[1].onClick.RemoveListener(PortalNo);
    }

    void PortalNo(){
        Dialogue.Instance.buttons[0].onClick.RemoveListener(PortalYes);
        Dialogue.Instance.buttons[1].onClick.RemoveListener(PortalNo);
        interactable = true;
    }

    public void PortalClicked(string name,Vector3 position){
        print(interactable);
        if(!interactable)
            return;
        interactable = false;
        selectedDimensionName = name;
        portalPosition = position;
        var dimension = Dimensions.Instance.dimensionsList.Find(x => x.name == name);
            Dialogue.Instance.SetDialogue(dimension.description + " Do you want to go through this portal? You won't be able to go back.", new List<string>(){"Yes","No"});
        Dialogue.Instance.buttons[0].onClick.AddListener(PortalYes);
        Dialogue.Instance.buttons[1].onClick.AddListener(PortalNo);
    }

    void Continue(){
        interactable = true;
        Dialogue.Instance.buttons[0].onClick.RemoveListener(Continue);
    }

    public void Play(){
        if(!Started){
            bg.SetBool("Open",true);
            section.SetActive(false);
            Dialogue.Instance.SetDialogue("Looking for inspiration for your next painting you start dreaming. Go through different dimensions and find objects of inspiration for your next masterpiece.", new List<string>(){"Continue"});
            Dialogue.Instance.buttons[0].onClick.AddListener(Continue);
            Started = true;
            StartCoroutine(EyeOpen());
        }else{
            interactable = true;
            menu.SetActive(false);
        }
    }

    IEnumerator EyeOpen(){
        yield return new WaitForSeconds(.75f);
        float max = 200;
        for(float i = max; i > 0; i--){
            bg.GetComponent<Image>().color = new Color(1,1,1,i/max);
            yield return new WaitForEndOfFrame();
        }
        menu.SetActive(false);
        print(interactable);
    }

    public void Quit(){
        Application.Quit();
    }

    public void Replay(){
        if(interactable){
            inventory.SetActive(true);
            Inventory.Instance.Empty();
            Camera.main.transform.position = new Vector3(-114,-38,-10);
            Camera.main.orthographicSize = 20.1049f;
            endMenu.SetActive(false);
            Dimensions.Instance.transition("Normal",portalPosition);
        }
    }

    public void Save(){
        if(Application.platform == RuntimePlatform.WebGLPlayer){
            Dialogue.Instance.SetDialogue("I'm afraid saving doesn't work on WebGL :(", new List<string>(){"Thanks!",":(","UwU","OwO"});
            return;
        }
        var obj = Instantiate(screenshotObj);
        obj.GetComponent<ScreenshotMaker>().target = GameObject.Find("Empty");
        obj.GetComponent<ScreenshotMaker>().Activate();
        Dialogue.Instance.SetDialogue("File has been saved to: "+ Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), new List<string>(){"Thanks!","Ok","UwU","OwO"});
    }

    public void Finish(){
        Dragger.Instance.Finish();
    }

    void Update(){
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(menu.activeSelf){
                menu.SetActive(false);
                interactable = true;
            }else if(interactable){
                section.SetActive(true);
                bg.GetComponent<Image>().color = new Color(1,1,1,1);
                bg.SetBool("Open",false);
                section.SetActive(true);
                menu.SetActive(true);
                interactable = false;
            }
        }
    }

}