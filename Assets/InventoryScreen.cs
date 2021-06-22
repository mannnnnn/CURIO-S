using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
    public TreasureBook treasureBook;
    public GameObject leftPanel;
    public GameObject leftPanelObject;

    public Text title;
    public Text desc;
    public Text flavorText;
    public Image previewImage;
    public GameObject flavorPanel;
    
    void Start()
    {
        bool startingBtn = true;
        foreach(TreasureBook.FossilType fossil in TreasureBook.FossilType.GetValues(typeof(TreasureBook.FossilType)))
        {
            if (fossil == TreasureBook.FossilType.DEFAULT)
            {
                //nothing
            }
             else if(startingBtn)
            {
                leftPanelObject.GetComponentInChildren<Text>().text = treasureBook.GetFossilInfo(fossil).name;
                leftPanelObject.GetComponent<Button>().onClick.AddListener(() => LeftButtonClicked((int)fossil));
                startingBtn = false;
            } else
            {
               GameObject newBtn =  Instantiate(leftPanelObject, leftPanel.transform);
                newBtn.GetComponentInChildren<Text>().text = treasureBook.GetFossilInfo(fossil).name;
                newBtn.GetComponent<Button>().onClick.AddListener(() => LeftButtonClicked((int)fossil));
            }
           
        }
        LeftButtonClicked(1);
    }

    void LeftButtonClicked(int fossilNum)
    {
        title.text = treasureBook.GetFossilInfo((TreasureBook.FossilType)fossilNum).name;
        desc.text = treasureBook.GetFossilInfo((TreasureBook.FossilType)fossilNum).desc;
        flavorText.text = treasureBook.GetFossilInfo((TreasureBook.FossilType)fossilNum).flavor;
        previewImage.sprite = treasureBook.GetFossilSprite((TreasureBook.FossilType)fossilNum);
    }

    public void EatAFossil()
    {
        flavorPanel.GetComponent<Animator>().SetTrigger("Show");
    }

    public void WhyDidYouDoThis()
    {
        flavorPanel.GetComponent<Animator>().SetTrigger("Hide");
    }
    
}
