using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryScreen : MonoBehaviour
{
    public TreasureBook treasureBook;
    public GameObject leftPanel;
    public GameObject leftPanelObject;

    public Text title;
    public Text desc;
    public Text flavorText;
    public Text quantity;
    public Image previewImage;
    public GameObject flavorPanel;
    public Button flavorBtn;

    
    private bool saveFileExists = false;
    private string unknownName = "???";
    public TreasureBook.FossilType selectedFossil;

    private PlayerInfo info;
    void Start()
    {

        info = PlayerInfo.GetInstance();
        saveFileExists = info.LoadGame();

        bool startingBtn = true;
        foreach(TreasureBook.FossilType fossil in TreasureBook.FossilType.GetValues(typeof(TreasureBook.FossilType)))
        {
            if (fossil == TreasureBook.FossilType.DEFAULT)
            {
                //nothing
            } else
            {
                int numCollected = -1;
                if (saveFileExists)
                {
                    foreach (TreasureBook.MinedFossil collected in info.collectedFossils)
                    {
                        if (collected.type == fossil)
                        {
                            numCollected = collected.quantity;
                        }
                    }
                }

                if (startingBtn)
                {
                    if (numCollected >= 0)
                    {
                        leftPanelObject.GetComponentInChildren<Text>().text = treasureBook.GetFossilInfo(fossil).name;
                    } else
                    {
                        leftPanelObject.GetComponentInChildren<Text>().text = unknownName;
                    }
                    leftPanelObject.GetComponent<Button>().onClick.AddListener(() => LeftButtonClicked((int)fossil));
                    startingBtn = false;
                }
                else
                {
                    GameObject newBtn = Instantiate(leftPanelObject, leftPanel.transform);
                    if (numCollected >= 0) {
                        newBtn.GetComponentInChildren<Text>().text = treasureBook.GetFossilInfo(fossil).name;
                    } else {
                        newBtn.GetComponentInChildren<Text>().text = unknownName;
                    }
                    newBtn.GetComponent<Button>().onClick.AddListener(() => LeftButtonClicked((int)fossil));
                }
            }

            
           
        }
        LeftButtonClicked(1);
    }

    public void BackButtonPressed()
    {
        SceneManager.LoadScene("Arcade");
    }

    void LeftButtonClicked(int fossilNum)
    {

        int numCollected = -1;
        if (saveFileExists)
        {
            foreach (TreasureBook.MinedFossil collected in info.collectedFossils)
            {
                if (collected.type == (TreasureBook.FossilType)fossilNum)
                {
                    numCollected = collected.quantity;
                }
            }
        }

        previewImage.sprite = treasureBook.GetFossilSprite((TreasureBook.FossilType)fossilNum);
        if (numCollected >= 0)
        {
            title.text = treasureBook.GetFossilInfo((TreasureBook.FossilType)fossilNum).name;
            desc.text = treasureBook.GetFossilInfo((TreasureBook.FossilType)fossilNum).desc;
            flavorText.text = treasureBook.GetFossilInfo((TreasureBook.FossilType)fossilNum).flavor;
            previewImage.color = Color.white;
            quantity.text = "x" + numCollected.ToString();
        } else
        {
            title.text = unknownName;
            desc.text = unknownName;
            flavorText.text = unknownName;
            previewImage.color = Color.black;
            quantity.text = "x0";
        }
        flavorBtn.gameObject.SetActive(numCollected > 0);
        selectedFossil = (TreasureBook.FossilType)fossilNum;
    }

    public void EatAFossil()
    {
        flavorPanel.GetComponent<Animator>().SetTrigger("Show");
        foreach (TreasureBook.MinedFossil collected in info.collectedFossils)
        {
            if (collected.type == selectedFossil){
                collected.quantity = collected.quantity - 1 ;
                quantity.text = "x" + collected.quantity;
            }
        }
        info.SaveGame();
    }

    public void WhyDidYouDoThis()
    {
        flavorPanel.GetComponent<Animator>().SetTrigger("Hide");
    }
    
}
