using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    public GameObject earthObject;
    public GameObject newsPinPrefab;

    [NonReorderable]//because of unity editor bug to render nested array in inspector
    public List<newsRawData> AllNews;
    [NonReorderable]
    public List<NewsCategoryTexture> AllCategorytexture;

    public Dictionary<NewsCategory, Texture2D> NewsCategoryTexture;
    private void Awake()
    {
        if (instance != null && instance != this)
        {            
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        for (int i = 0; i < AllNews.Count; i++)      
        {
            createNewsObject(i);
        }

       
    }    


    void createNewsObject(int newsRawIndex)
    {
        var newsPin=Instantiate(newsPinPrefab);

        NewsObject newsObjectData = newsPin.GetComponent<NewsObject>();
        newsPin.transform.parent = earthObject.transform;
        newsPin.transform.localScale = Vector3.one;

        newsObjectData.Init(newsRawIndex);
    }
    

    public void hideAllNewsPanel()
    {
        NewsPanelData[] allNewsPanel = earthObject.GetComponentsInChildren<NewsPanelData>();
        foreach(var i in allNewsPanel)
            if (!i.isHiden)
            {
                i.hide();
            }
    }
}


public enum NewsCategory { Economic,Sport,Nature,Science, Politics, Crime,Public};
[System.Serializable]
public class newsRawData
{
    public float latitude;
    public float lonitude;
    public string title;
    public string mainText;
    public string date;
    public string time;
    public Texture2D image;
    public NewsCategory category;
    public newsRawData(float latitude, float lonitude, string title, string mainText, string date, string time, Texture2D image,NewsCategory category)
    {
        this.latitude = latitude;
        this.lonitude = lonitude;
        this.title = title;
        this.mainText = mainText;
        this.date = date;
        this.time = time;
        this.image = image;
        this.category = category;
    }
}

[System.Serializable]
public class NewsCategoryTexture
{
    public NewsCategory category;
    public Texture2D texture;
    public Color ringColor;

}