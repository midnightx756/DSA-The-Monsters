using System;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Scores
{
    public int upper;
    public int lower;
}
public class ManageGame : MonoBehaviour
{

    ScoreKeeper sc;

    [SerializeField] float TimeAvailible;

    [SerializeField] Canvas ClockCanvas;
    TextMeshProUGUI txt;
    Image Clock;

    [Header("Slabs - Keep it sorted")]
    public List<Scores> rangeList;
    float n;

    int initScore;
    bool end = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sc = FindAnyObjectByType<ScoreKeeper>();
        initScore = sc.GetScore();
        n = Time.time;

        txt = ClockCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        Clock = ClockCanvas.transform.GetChild(0).gameObject.GetComponent<Image>();
        if (!ScoreKeeper.phaseHOD)
        {
            ClockCanvas.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!ScoreKeeper.phaseHOD) return;
        if(!end && Time.time - n >= TimeAvailible)
        {
            StartCoroutine(Finish());
            return;
        }

        txt.text =  "" +  (int)(TimeAvailible -  (Time.time - n));
       Clock.fillAmount = 1 - (Time.time - n)/TimeAvailible;
    }

    IEnumerator Finish()
    {
        end = true;
        int l = 0;
        int h = rangeList.Count - 1;
        int mid = 0;
        bool f = false;
        while(l <= h)
        {
            mid = l +  (h-l)/2;
            if(sc.GetScore() >= rangeList[mid].lower + initScore && sc.GetScore() <=  rangeList[mid].upper + initScore)
            {
                f = true;
                break;
            }
            else if(sc.GetScore() < rangeList[mid].lower + initScore)
                h = mid - 1;
            else
                l = mid + 1;
        }

        Debug.Log("Your fate has been decided");
        yield return new WaitForSecondsRealtime(5f);
        if (f)
        {
            Debug.Log("You fool HAHAHAHAHA");
            sc.LoadOnDemand("ShitassAXEA");
        }
        else
        {
            Debug.Log("You saved your butt");
            sc.LoadOnDemand("EndGameMenu");
        }
    }
}
