using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    private static int ScreenWidth { get; set; }
    private static int ScreenHeight { get; set; }
    internal static float CameraHeight { get; set; }
    internal static float CameraWidth { get; set; }


    private void Awake()
    {
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        CameraHeight = Camera.main.orthographicSize;
        CameraWidth = CameraHeight* ScreenWidth / ScreenHeight;
    }
}
