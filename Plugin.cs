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
    public static int BeltMultiplier;
    public static int beltSlider = 1;
    private Rect menuRect = new Rect(2200, 10, 250, 100);

    public static bool MechaModded = false;
    public static bool achievementToggle = true;

    public static bool passiveEnemy = false;
    public static bool SaveGameLoaded = false;


    public static bool DroneSpeedMod = true;
    public static bool BeltSpeedMod;
    public static bool ModdedMechMod = true;
    public static bool PassiveEnemyMod = true;
    public static bool getAchievmentMod = true;


    void Awake()
    {
        LoadConfig();
        VitaminPatch.MyPatcher.ApplyPatches();
    }
    public void Start()
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

        if(DroneSpeedMod)
        {
            GUILayout.Label("DroneSpeed");
            GUILayout.BeginHorizontal();
            DroneSlider = GUILayout.HorizontalSlider(DroneSlider, 0.1f, 10.0f);
            GUILayout.Label(DroneSlider.ToString("0.00" + "x Speed"));
            GUILayout.EndHorizontal();
        }        
        if(BeltSpeedMod)
        {
            GUILayout.Label("BeltMultiplier: " + BeltMultiplier.ToString());
            
        }
        if(ModdedMechMod)
        {
            MechaModded = GUILayout.Toggle(MechaModded, "Modded Mech");
        }
        if(PassiveEnemyMod)
        {
            passiveEnemy = GUILayout.Toggle(passiveEnemy, "Passive Enemy");
        }
        if(getAchievmentMod)
        {
            achievementToggle = GUILayout.Toggle(achievementToggle, "Get Achievements?");
        }           
        if (GUILayout.Button("Reset Speed"))
        {
            ResetSettings();
        }
    }
    void ResetSettings()
    {
        DroneSlider = 1.0f;
    }


    public static void LoadConfig()
    {
        string configPath = Path.Combine(Application.dataPath, "../BepInEx/plugins/VitaminMenu/config.txt");
        if (File.Exists(configPath))
        {
            string customPath = File.ReadAllText(configPath).Trim();

            // Teile den Inhalt in Zeilen
            string[] lines = customPath.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith("BeltMod"))
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        // Extrahiere den Wert nach dem Doppelpunkt und konvertiere ihn in einen Bool
                        VitaminsMieseMenu.BeltSpeedMod = parts[1].Trim() == "1";
                        
                    }
                }
                if (line.StartsWith("Beltmultiplier"))
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        // Extrahiere den Wert nach dem Doppelpunkt und konvertiere ihn in einen Bool
                        int.TryParse(parts[1].Trim(), out VitaminsMieseMenu.BeltMultiplier);

                    }
                }
            }
        }
        else
        {
            VitaminLogger.LogInfo("Could not find Config File!");
        }

    }
   
}


