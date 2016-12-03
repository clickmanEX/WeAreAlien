using UnityEngine;
using System.Collections;

public class GameSpeed : MonoBehaviour
{

    // スマホ用に描画を安定させてFPSを60にする.
    public bool speed_ON;

    // Use this for initialization
    void Start()
    {
        if (speed_ON)
        {
            QualitySettings.vSyncCount = 1; // VSync（垂直同期）をONにする
            Application.targetFrameRate = 60; // ターゲットフレームレートを60に設定
        }
        else
        {
            QualitySettings.vSyncCount = 0; // VSyncをOFFにする
            Application.targetFrameRate = 30; // ターゲットフレームレートを30に設定
        }

    }
}