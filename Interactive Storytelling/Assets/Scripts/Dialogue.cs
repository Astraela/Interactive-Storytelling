using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : Singleton<Dialogue>
{
    public GameObject DialogueBox;
    public List<Button> buttons = new List<Button>();


    void Start(){
        for(int i = 0; i < buttons.Count; i++){
            int nr = i;
            buttons[i].onClick.AddListener(() =>buttonClick(nr));
        }
    }

    public void buttonClick(int btn){
        DialogueBox.SetActive(false);
        for(int i = 0; i < buttons.Count; i++){
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void SetDialogue(string dialogueText, List<string> options = null){
        DialogueBox.GetComponentInChildren<Text>().text = dialogueText;
        DialogueBox.SetActive(true);
        if(options != null){
            for(int i = 0; i < options.Count; i++){
                buttons[i].GetComponentInChildren<Text>().text = options[i];
                buttons[i].gameObject.SetActive(true);
            }
        }
    }
}
