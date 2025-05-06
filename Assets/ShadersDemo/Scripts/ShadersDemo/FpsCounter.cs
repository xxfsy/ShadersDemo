using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    TextMeshProUGUI _text;
    float _fps;
    float min = float.MaxValue, max = float.MinValue;
    float avg;
    float sum = 0;
    int recordsCounter = 1;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("UpdateFps", 1f);
        Invoke("UpdateAvg", 5f);
    }

    void UpdateFps()
    {
        _fps = 1.0f / Time.deltaTime;
        sum += _fps;
        if (min > _fps)
            min = _fps;
        if (max < _fps)
            max = _fps;
        _text.SetText($"FPS: {(int)_fps} \nMax = {(int)max} \nMin = {(int)min}");
        recordsCounter++;
    }

    void UpdateAvg()
    {
        //int tempValue = (int)(sum / recordsCounter);
        //sum += _fps;
        _text.SetText($"FPS: {(int)_fps} \nMax = {(int)max} \nMin = {(int)min} \nAvg = {(int)(sum / recordsCounter)}");
        //recordsCounter++;
    }
}
