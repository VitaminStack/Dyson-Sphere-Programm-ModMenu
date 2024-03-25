using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using System.Reflection;
using JetBrains.Annotations;
using System.IO;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using ABN;
using UnityEngine.SocialPlatforms.Impl;

[BepInPlugin("com.example.myplugin", "VitaminsMieseMenu", "1.0.0")]
public class VitaminsMieseMenu : BaseUnityPlugin
{
    
    string PluginName = "VitaminsMieseMenu";
    string PluginVersion = "1.0.0";

    private bool isMenuVisible;
    public static float DroneSlider = 1.0f;
    public static float MenuBeltSlider = 1.0f;
    public static int beltSlider = 1;
    private Rect menuRect = new Rect(2200, 10, 250, 100);

    public static bool MechaModded = false;
    public static bool achievementToggle = true;

    void Awake()
    {
        VitaminPatch.MyPatcher.ApplyPatches();
    }

    private void Start()
    {
        VitaminLogger.LogInfo($"{PluginName} wurde gestartet!");
        isMenuVisible = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            isMenuVisible = !isMenuVisible;
        }
    }
    void OnGUI()
    {
        if (isMenuVisible)
        {
            menuRect = GUILayout.Window(0, menuRect, MenuWindowIngame, "Menu");
        }
    }



    void MenuWindowIngame(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        GUILayout.Label("DroneSpeed");
        GUILayout.BeginHorizontal();
        DroneSlider = GUILayout.HorizontalSlider(DroneSlider, 0.1f, 10.0f);
        GUILayout.Label(DroneSlider.ToString("0.00" + "x Speed"));
        GUILayout.EndHorizontal();

        GUILayout.Label("BeltSpeed - setup before loading Savegame");
        GUILayout.BeginHorizontal();
        MenuBeltSlider = GUILayout.HorizontalSlider(MenuBeltSlider, 1f, 10f);
        beltSlider = Mathf.RoundToInt(MenuBeltSlider);
        GUILayout.Label(beltSlider.ToString() + "x Speed");
        GUILayout.EndHorizontal();

        MechaModded = GUILayout.Toggle(MechaModded, "Modded Mech");
        achievementToggle = GUILayout.Toggle(achievementToggle, "Get Achievements?");

        if (GUILayout.Button("Reset Speed"))
        {
            ResetSettings();
        }
    }
    void ResetSettings()
    {
        DroneSlider = 1.0f;
        MenuBeltSlider = 1.0f;
    }
}



