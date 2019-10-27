using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnLevel1;
    public Button btnLevel2;
    public Button btnLevel3;

    public Transform panelStart;
    public Transform panelCard;
    public Transform panelOver;
    void Start()
    {
        btnLevel1.onClick.AddListener(Onclick);
        btnLevel2.onClick.AddListener(Onclick);
        btnLevel3.onClick.AddListener(Onclick);
    }
    private void Onclick()
    {
        panelStart.gameObject.SetActive(false);
        panelCard.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {


    }
}
