using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class NewsObject : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityFocusHandler
{
    public float Latitude;
    public float Longitude;
    float altitude = 0.502f;

    public GameObject newsPanel;
    public NewsPanelData newsPanelData;

    public Material iconMaterial;
    public Material bodyMaterial;
    public Material ringMaterial;
    public MeshRenderer meshRenderer;
    public MeshRenderer ringMeshRenderer;
    public float ringAnimationSpeedVariation;

    public GameObject ringObj;
    public GameObject bodyObj;


    int newsRawListIndex;

    private void Awake()
    {
        bodyObj = transform.GetChild(0).gameObject;
        ringObj = transform.GetChild(1).gameObject;

        meshRenderer = bodyObj.GetComponent<MeshRenderer>();
        iconMaterial = meshRenderer.materials[0];
        bodyMaterial = meshRenderer.materials[1];


        ringMeshRenderer = ringObj.GetComponent<MeshRenderer>();
        Material ringmat = Instantiate(ringMaterial);
        ringMeshRenderer.material = ringmat;
        ringMaterial = ringmat;
        ringAnimationSpeedVariation = Random.Range(0.03f, 0.2f);
    }

    public void Init(int _newsRawListIndex)
    {
        newsRawListIndex = _newsRawListIndex;
        var newsRawData = AppManager.instance.AllNews[newsRawListIndex];

        Latitude = newsRawData.latitude;
        Longitude = newsRawData.lonitude;

        newsPanelData.setData(newsRawData.title, newsRawData.mainText, newsRawData.date, newsRawData.time, newsRawData.image);

        newsPanel.SetActive(false);

        updatePos();

        setTexture(_newsRawListIndex);
    }


    void setTexture(int _newsRawListIndex)
    {
        newsRawData nr = AppManager.instance.AllNews[_newsRawListIndex];
        Texture2D tex = null;
        Color c = Color.white;
        foreach (var cat in AppManager.instance.AllCategorytexture)
        {
            if (nr.category == cat.category)
            {
                tex = cat.texture;
                c = cat.ringColor;
            }
        }

        if (tex != null)
        {
            iconMaterial.mainTexture = tex;
            iconMaterial.SetTexture("_EmissionMap", tex);

            ringMaterial.color = c;
            ringMaterial.SetColor("_EmissionColor", c);

        }

    }

    Vector3 getPositionFrom_Lat_Lon(float lat, float lon)
    {
        Vector3 pos = Vector3.one;
        float lat_ = (-lat + 90) * Mathf.Deg2Rad;
        float lon_ = (lon) * Mathf.Deg2Rad;
        float alt_ = altitude;

        pos.x = Mathf.Sin(lat_) * Mathf.Cos(lon_);
        pos.y = Mathf.Cos(lat_);
        pos.z = Mathf.Sin(lat_) * Mathf.Sin(lon_);
        return (pos * alt_);
    }

    float currentLat, currentLon;
    public void updatePos()
    {
        Vector3 pos = getPositionFrom_Lat_Lon(Latitude, Longitude);
        transform.position = AppManager.instance.earthObject.transform.position + pos;
        currentLat = Latitude;
        currentLon = Longitude;

        Vector3 upDir = transform.position - AppManager.instance.earthObject.transform.position;
        Debug.DrawRay(transform.position, upDir, Color.green, 10);
        transform.up = upDir;

    }

    Vector3 newScale;
    float ringAnimationSpeed;
    float BodyAnimationSpeed = 1f;

    bool focusedEnterTriger;
    bool focusedExitTriger;
    private void Update()
    {
        if (currentLat != Latitude || currentLon != Longitude)
            updatePos();

        ringAnimation();
        bodyAnimation();

        if (focusedEnterTriger)
        {
            Vector3 newScale = Vector3.one * 0.7f;
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, 15 * Time.deltaTime);
        }

        if (focusedExitTriger)
        {
            Vector3 newScale = Vector3.one*0.5f;
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, 15 * Time.deltaTime);
        }



    }

    private void ringAnimation()
    {
        ringObj.transform.localScale = Vector3.Lerp(ringObj.transform.localScale, newScale, Time.deltaTime * 0.5f);
        newScale += Vector3.one * ringAnimationSpeed;

        if (ringObj.transform.localScale.x >= 1f)
        {
            ringAnimationSpeed = -1 * ringAnimationSpeedVariation;
        }
        if (ringObj.transform.localScale.x <= 0.25f)
        {
            ringAnimationSpeed = 1f * ringAnimationSpeedVariation;
        }
    }

    private void bodyAnimation()
    {
        bodyObj.transform.rotation *= Quaternion.Euler(0, BodyAnimationSpeed * Mathf.Rad2Deg * Time.deltaTime, 0);
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        focusedEnterTriger = true;
        focusedExitTriger = false;
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        // transform.localScale = Vector3.one;
        focusedEnterTriger = false;
        focusedExitTriger = true;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (newsPanelData.gameObject.activeSelf == false)
        {
            AppManager.instance.hideAllNewsPanel();
            newsPanelData.show();
        }
        else
            newsPanelData.hide();



    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {

    }


}
