using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button Level1;
    public Button Level2;
    public Button Level3;

    public Transform Begin;
    public Transform Play;
    public Transform GameOver;

    void Start()
    {
        Level1.onClick.AddListener(() =>
        {
            Begin.gameObject.SetActive(false);
            Play.gameObject.SetActive(true);
            Show_card(3, 2);
        });
        Level2.onClick.AddListener(() =>
        {
            Begin.gameObject.SetActive(false);
            Play.gameObject.SetActive(true);
            Show_card(4, 2);
        });
        Level3.onClick.AddListener(() =>
        {
            Begin.gameObject.SetActive(false);
            Play.gameObject.SetActive(true);
            Show_card(5, 2);
        });

        Button btnToStart = GameOver.Find("Button_to_start").GetComponent<Button>();
        btnToStart.onClick.RemoveAllListeners();
        btnToStart.onClick.AddListener(restart);
    }
    //private void Onclick(int width, int height)
    //{
    //    Begin.gameObject.SetActive(false);
    //    Play.gameObject.SetActive(true);
    //    Show_card(width, height);
    //}
    

    void Show_card(int width, int height)//set the number of cards gievn the level number
    {

        Sprite[] Allsprite = Resources.LoadAll<Sprite>("");//get all card images
        int totalCount = width * height / 2;
        List<Sprite> Img_List = new List<Sprite>();
        for (int i = 0; i < Allsprite.Length; i++)
        {
            Img_List.Add(Allsprite[i]);
        }
        List<Sprite> needShowCardList = new List<Sprite>();
        while (totalCount > 0)
        {
            int randomIndex = Random.Range(0, Img_List.Count);
            needShowCardList.Add(Img_List[randomIndex]);
            needShowCardList.Add(Img_List[randomIndex]);
            Img_List.RemoveAt(randomIndex);
            totalCount--;
        }
        shuffle(needShowCardList);// shuffle the needshow List to randomly display the cards 

        Transform Card_Parent = Play.Find("Panel");
        //After passing the higher level,destory the components
        GameObject itemTemplate = Card_Parent.GetChild(0).gameObject;
        for (int i = 1; i < Card_Parent.childCount; i++)
        {
            GameObject itemTemp = Card_Parent.GetChild(i).gameObject;
            Sprite ss = itemTemp.transform.Find("front").GetComponent<Image>().sprite;
            itemTemp.transform.SetParent(null);
            Destroy(itemTemp);
        }

        //generate cards
        int maxCount = Mathf.Max(Card_Parent.childCount, needShowCardList.Count);
        GameObject Prefab_card = Card_Parent.GetChild(0).gameObject;
        for (int i = 0; i < maxCount; i++)
        {
            GameObject itemObject = null;
            if (i < Card_Parent.childCount)
            {
                itemObject = Card_Parent.GetChild(i).gameObject;
            }
            else
            {
                itemObject = GameObject.Instantiate<GameObject>(Prefab_card);// create a new child card
                itemObject.transform.SetParent(Card_Parent, false);
            }
            itemObject.transform.Find("front").GetComponent<Image>().sprite= needShowCardList[i];//change the front img
            FlipCard cardAniCtrl = itemObject.GetComponent<FlipCard>();
            cardAniCtrl.Reset();//set card to default state
        }
        //adjust panel size
        GridLayoutGroup grid_comp = Card_Parent.GetComponent<GridLayoutGroup>();
        float panelWidth = width * grid_comp.cellSize.x + (width - 1) * grid_comp.spacing.x;
        float panelHeight = height * grid_comp.cellSize.y + (width - 1) * grid_comp.spacing.y;
        Card_Parent.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);

    }

    public void AllcardsRemoved()//check whether the game is over
    {
        FlipCard[] allCards = GameObject.FindObjectsOfType<FlipCard>();
        if (allCards != null && allCards.Length > 0)
        {
            List<FlipCard> Frontcards = new List<FlipCard>();
            for (int i = 0; i < allCards.Length; i++)
            {
                FlipCard Temp_card = allCards[i];
                if (Temp_card.isInFront && !Temp_card.isOver)//
                {
                    Frontcards.Add(Temp_card);
                }

                if (Frontcards.Count >= 2)//each turn,a player flips two card and compare the pattern, if the patterns are same, remove the two cards;Otherwise flip cards back
                {
                    string card_Name1 = Frontcards[0].GetCardName();
                    string card_Name2 = Frontcards[1].GetCardName();

                    if (card_Name1 == card_Name2)
                    {
                        Frontcards[0].Matched();// if matched, hide the two cards
                        Frontcards[1].Matched();
                    }
                    else
                    {
                        Frontcards[0].Notmatch();// turn it back if not matched
                        Frontcards[1].Notmatch();
                    }
                    allCards = GameObject.FindObjectsOfType<FlipCard>();// To check if all cards have been matched.If so , end the game.
                    bool isAllOver = true;
                    for (int o = 0; o < allCards.Length; o++)
                    {
                        isAllOver &= allCards[o].isOver;
                    }
                    if (isAllOver)
                    {
                        Victory();
                    }
                    break;
                }
            }
        }
    }
    public void shuffle<T>(List<T> list)
    {
        int currentIndex;
        T tempvalue;
        for(int i = 0; i < list.Count; i++)
        {
            currentIndex = Random.Range(0, list.Count - 1);
            tempvalue = list[currentIndex];
            list[currentIndex] = list[list.Count - 1 - i];
            list[list.Count - 1 - i] = tempvalue;
        }
    }

    public void Victory()
    {
        Begin.gameObject.SetActive(false);
        Play.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(true);
    }

    public void restart()
    {
        Begin.gameObject.SetActive(true);
        Play.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);
    }
}
