using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI fpsTextAverage;
    [SerializeField] private TextMeshProUGUI fpsTextLow;
    [SerializeField] private TextMeshProUGUI fpsTextLower;

    [SerializeField] private float hudRefreshRate = 0.5f;
    [SerializeField] List<int> allFps = new List<int>();

    private float timer;

    void Update()
    {
        if (Time.unscaledDeltaTime == 0) return;

        timer += Time.unscaledDeltaTime;
        if (timer >= hudRefreshRate)
        {
            int fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            allFps.Add(fps);

            if (allFps != null)
            {
                fpsText.text = $"FPS: {fps}";
                fpsTextAverage.text = $"FPS Average : {(int)allFps.Average()}";
                fpsTextLow.text = $"FPS 1% : {(int)allFps.OrderBy(x => x).Skip(3).Take(10).Average()}";
                fpsTextLower.text = $"FPS 0.05% : {(int)allFps.OrderBy(x => x).Skip(3).Take(5).Average()}";
            }

            timer = 0f;

            if (allFps.Count >= 10000)
            {
                allFps.RemoveAt(0);
            }
        }
    }

}
