using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NewsPanelData : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI mainText;
    public RawImage image;
    public Button closeBTN;
    public TextMeshProUGUI date;
    public TextMeshProUGUI time;


    public bool isHiden;
    bool updateScrollViewContentHeight;

    private void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        transform.position = transform.parent.position + transform.parent.up * .1f;
        isHiden = true;
    }
    private void Update()
    {
         if(isHiden)
         {
             transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 15);
             if (transform.localScale.x < 0.2f) 
             {                
                 gameObject.SetActive(false);
             }
         }
         else
         {
             transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one*.8f, Time.deltaTime * 15);            
         }

        if (gameObject.activeSelf)
        {
            transform.right = Vector3.Cross(Vector3.up, transform.position - Camera.main.transform.position);
        }

        if (updateScrollViewContentHeight)
        {
            mainText.rectTransform.sizeDelta = new Vector2(mainText.rectTransform.sizeDelta.x, mainText.preferredHeight);
            mainText.transform.parent.GetComponent<RectTransform>().sizeDelta = mainText.rectTransform.sizeDelta;
            updateScrollViewContentHeight = false;
        }


    }


    public void show()
    {
        //AppManager.instance.hideAllNewsPanel();
        isHiden = false;

        gameObject.SetActive(true);
       // transform.localScale = Vector3.one * .8f;
        transform.position = transform.parent.position + transform.parent.up * .1f;
        
        Debug.LogError("show");
    }

    public void hide()
    {
        isHiden = true;
       // gameObject.SetActive(false);
        Debug.LogError("Hide");

    }

    public void setData(string title, string mainText, string date, string time, Texture2D texture)
    {
        titleText.text = title;
        this.mainText.text = mainText;
        this.date.text = date;
        this.time.text = time;
        image.texture = texture;

        updateScrollViewContentHeight = true;
    }

}
