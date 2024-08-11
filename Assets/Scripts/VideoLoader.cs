using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class VideoLoader : MonoBehaviour
{
    [SerializeField] private ScObjVideo[] scObjVideo;
    [SerializeField] private Image[] intros;
    [SerializeField] private string pathToScObjVideo;
    void Awake()
    {
        GetVideoReference();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetVideoReference()
    {
        scObjVideo = Resources.LoadAll<ScObjVideo>(pathToScObjVideo);
    }
    
    /*private IEnumerator LoadIntros()
    {
        if (Resources.LoadAll<Image>("Intros").Length == 0)
        {
            
        }
        else
        {
            intros = Resources.LoadAll<Image>("Intros");
        }
        return
    }*/
}
