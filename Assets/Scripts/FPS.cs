﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class FPS : MonoBehaviour
{

    public float FramesPerSec;
    private float frequency = 1.0f;
    private string fps;
    private TMPro.TextMeshProUGUI Text;

    void Start()
    {
        Text = GetComponent<TMPro.TextMeshProUGUI>();
        fps = Text.text;
        StartCoroutine(Worker());
    }

    private IEnumerator Worker()
    {
        for (; ;)
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            FramesPerSec = frameCount / timeSpan;
            // Display it
            fps = string.Format("FPS: {0}", Mathf.RoundToInt(FramesPerSec));
        }
    }

    void OnGUI()
    {
        Text.text = fps;
    }
}
