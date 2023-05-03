using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage;

    public RectTransform clockParent;

    public Image seasonImage;

    public TextMeshProUGUI dataText;

    public TextMeshProUGUI timeText;

    public Sprite[] seasonSprites;

    public List<GameObject> clockBlocks = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }



    private void OnEnable()
    {
        EventHandler.GameDataEvent += OnGameDataEvent;
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameDataEvent -= OnGameDataEvent;
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
    }

    private void OnGameMinuteEvent(int minute, int hour)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    private void OnGameDataEvent(int hour, int day, int month, int year, Season season)
    {
        dataText.text = year + "Äê" + month.ToString("00") + "ÔÂ" + day.ToString("00") + "ÈÕ";
        seasonImage.sprite = seasonSprites[(int)season];

        SwitchHourTime(hour);

        DayNightImageRotate(hour);
    }

    private void SwitchHourTime(int hour)
    {
        int index = hour / 4;

        if(index == 0)
        {
            foreach (var item in clockBlocks)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                if (i < index + 1)
                    clockBlocks[i].gameObject.SetActive(true);
                else
                    clockBlocks[i].gameObject.SetActive(false);
            }
        }
    }

    private void DayNightImageRotate(int hour)
    {
        var target = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);
    }
}
