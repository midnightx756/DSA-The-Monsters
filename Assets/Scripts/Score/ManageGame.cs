using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Image Clock;

    [Header("Slabs - Keep it sorted")]
    public List<Scores> rangeList;
    float n;

    bool end = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sc = FindAnyObjectByType<ScoreKeeper>();
        n = Time.time;
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

        txt.text =  "" +  (TimeAvailible -  (Time.time - n));
       //Clock.fillAmount = 1;
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
            if(sc.GetScore() >= rangeList[mid].lower && sc.GetScore() <=  rangeList[mid].upper)
            {
                f = true;
                break;
            }
            else if(sc.GetScore() < rangeList[mid].lower)
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
