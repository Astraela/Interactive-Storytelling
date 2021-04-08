using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimensions : Singleton<Dimensions>
{
    public string StartDimension;

    public List<Dimension> dimensionsList = new List<Dimension>();
    public Transform sprite;

    GameObject current;
    GameObject newCurrent;
    float spriteSize = 38.73453f;
    void Start()
    {
        current = Instantiate(dimensionsList.Find(x => x.name == StartDimension).dimension);
    }

    public void transition(string dimension, Vector3 position){
        sprite.localScale = new Vector3(0,0,1);
        sprite.position = position;
        Dimension nextDimension = dimensionsList.Find(x => x.name == dimension);

        foreach(SpriteRenderer renderer in current.GetComponentsInChildren<SpriteRenderer>()){
            renderer.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        newCurrent = Instantiate(nextDimension.dimension); 

        foreach(SpriteRenderer renderer in newCurrent.GetComponentsInChildren<SpriteRenderer>()){
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        StartCoroutine(MaskIncrease());
    }

    public void instantTransition(string dimension){
        Dimension nextDimension = dimensionsList.Find(x => x.name == dimension);
        newCurrent = Instantiate(nextDimension.dimension); 
        Destroy(current);
        current = newCurrent;
        Game.Instance.interactable = true;
    }

    IEnumerator MaskIncrease(){
        float max = 200;
        for(float i = 0; i < max; i++){
            sprite.localScale = new Vector3(i/max*spriteSize,i/max*spriteSize,1);
            sprite.rotation *= Quaternion.Euler(0,0,-2f);
            yield return new WaitForEndOfFrame();
        }
        Destroy(current);
        current = newCurrent;
        current.transform.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        foreach(SpriteRenderer renderer in newCurrent.GetComponentsInChildren<SpriteRenderer>()){
            renderer.maskInteraction = SpriteMaskInteraction.None;
        }
        sprite.localScale = Vector3.zero;
        Game.Instance.interactable = true;
        
    }
}

[System.Serializable]
public class Dimension{
    public string name;
    public GameObject dimension;
    public string description;
}