using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomCard : MonoBehaviour
{
    GameObject canvas;
    GameObject zoomCard;
    public bool isCardZoomed;
    private void Awake() 
    {
        canvas = GameObject.Find("Main Canvas");
    }
    public void OnHoverEnter()
    {
        zoomCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y), Quaternion.identity);
        zoomCard.transform.SetParent(canvas.transform, false);

        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240, 360);

        zoomCard.GetComponent<Image>().raycastTarget = false;
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}
