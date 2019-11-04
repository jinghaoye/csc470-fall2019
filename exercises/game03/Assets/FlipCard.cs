using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlipCard : MonoBehaviour,IPointerClickHandler
{
    Transform Front_img;
    Transform Back_img;
    float Duaration = 0.3f;

    public bool isInFront = false;
    public bool isOver = false;

    void Start()
    {
        Front_img = transform.Find("front");
        Back_img = transform.Find("back");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInFront)
        {
            StartCoroutine(FliptoFront());//
        }
        //else
        //{
        //    StartCoroutine(FlipCardTo());
        //}
        //Debug.Log("click");
    }

    IEnumerator FliptoFront() //Using coroutine to simulate flip animation
    {
        //Make sure the back is in correct state
        Front_img.gameObject.SetActive(false);
        Back_img.gameObject.SetActive(true);
        Front_img.rotation = Quaternion.identity;
        while (Back_img.rotation.eulerAngles.y < 90)//fliping
        {
            Back_img.rotation *= Quaternion.Euler(0, Time.deltaTime * 90f * (1f / Duaration), 0);
            if (Back_img.rotation.eulerAngles.y > 90)
            {
                Back_img.rotation = Quaternion.Euler(0, 90, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        //After the back rotation finish, the front start to rotate and hide the back
        Front_img.gameObject.SetActive(true);
        Back_img.gameObject.SetActive(false);
        Front_img.rotation = Quaternion.Euler(0, 90, 0);
        while (Front_img.rotation.eulerAngles.y > 0)
        {
            Front_img.rotation *= Quaternion.Euler(0, -Time.deltaTime * 90f * (1f / Duaration), 0);
            if (Front_img.rotation.eulerAngles.y >90)
            {
                Front_img.rotation = Quaternion.Euler(0, 0, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        isInFront = true;
        Camera.main.gameObject.GetComponent<GameManager>().AllcardsRemoved();//check if all card have been matched in each flip

    }

    IEnumerator FliptoBack()
    {
        Front_img.gameObject.SetActive(true);
        Back_img.gameObject.SetActive(false);
        Front_img.rotation = Quaternion.identity;
        while (Front_img.rotation.eulerAngles.y < 90)
        {
            Front_img.rotation *= Quaternion.Euler(0, Time.deltaTime * 90f * (1f / Duaration), 0);
            if (Front_img.rotation.eulerAngles.y > 90)
            {
                Front_img.rotation = Quaternion.Euler(0, 90, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        Front_img.gameObject.SetActive(false);
        Back_img.gameObject.SetActive(true);
        Back_img.rotation = Quaternion.Euler(0, 90, 0);
        while (Back_img.rotation.eulerAngles.y > 0)
        {
            Back_img.rotation *= Quaternion.Euler(0, -Time.deltaTime * 90f * (1f / Duaration), 0);
            if (Back_img.rotation.eulerAngles.y > 90)
            {
                Back_img.rotation = Quaternion.Euler(0, 0, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        isInFront = false;

    }

    public void Reset()//reset card state.
    {
        Front_img = transform.Find("front");
        Back_img = transform.Find("back");
        Front_img.gameObject.SetActive(false);
        Back_img.gameObject.SetActive(true);
        this.isOver = false;
        this.isInFront = false;
        Front_img.rotation = Quaternion.identity;
        Back_img.rotation = Quaternion.identity;
    }

    public string GetCardName()
    {
        return Front_img.GetComponent<Image>().sprite.name;
    }

    public void Matched()
    {
        isOver = true;
        Front_img.gameObject.SetActive(false);
        Back_img.gameObject.SetActive(false);
    }

    public void Notmatch()
    {
        StartCoroutine(FliptoBack());
    }
}
