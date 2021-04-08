using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//best wel basic scriptje om ervoor te zorgen dat de inventory slots goed mee scalen
public class GridLayoutScaler : MonoBehaviour
{
    public float divider = 11.97f; //de value die gebruikt wordt om de gridsize te berekenen
    public float spacerDivider = 65.8f; //de value die gebruikt wordt om de afstand tussen 2 grid items te berekenen
    public axis side = axis.Width; //welke axis het moet gebruiken

    //de Enum
    public enum axis
    {
        Width,
        Height
    }

    //laatste size zodat het niet te veel berekeningen doet
    float lastSize = 0;

    //benodigde components
    RectTransform rectTransform;
    GridLayoutGroup gridLayout;

    void Start(){
        //de componenten pakken
        rectTransform = GetComponent<RectTransform>();
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    //bool buiten de update zodat het niet elke keer geinitialiseerd word
    bool changed = false;
    void Update()
    {  
        
        //kijken of de size verandert is en zoja het veranderen, en changed naar true zetten.
        if(side == axis.Width && rectTransform.rect.width != lastSize){
            lastSize = rectTransform.rect.width;
            changed = true;
        }
        else if(side == axis.Height && rectTransform.rect.height != lastSize){
           lastSize = rectTransform.rect.height;
           changed = true;
        }
    
        if(Application.isEditor && !changed){
            bool b1 = gridLayout.spacing == new Vector2(lastSize/spacerDivider, lastSize/spacerDivider);
            bool b2 = gridLayout.cellSize == new Vector2(lastSize / divider, lastSize / divider);
            if(b1 && b2) return;
            changed = true;
        }
        //gridlayout veranderen en changed naar false zetten
        if(changed){
            gridLayout.cellSize = new Vector2(lastSize / divider, lastSize / divider);
            gridLayout.spacing = new Vector2(lastSize/spacerDivider, lastSize/spacerDivider);
            changed = false;
        }
    }
}
