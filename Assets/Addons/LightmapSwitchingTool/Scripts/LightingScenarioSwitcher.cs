﻿using UnityEngine;

public class LightingScenarioSwitcher : MonoBehaviour {
    [SerializeField]
    private LevelLightmapData LocalLevelLightmapData;
    private int LightingScenarioSelector;
    [SerializeField]
    private int lightingScenariosCount;
    [SerializeField]
    public int DefaultLightingScenario;

    // Use this for initialization
    void Start ()
    {
        //LocalLevelLightmapData = FindObjectOfType<LevelLightmapData>();
        LightingScenarioSelector = DefaultLightingScenario;
        lightingScenariosCount = LocalLevelLightmapData.lightingScenariosCount;
        LocalLevelLightmapData.LoadLightingScenario(DefaultLightingScenario);
        Debug.Log("Load default lighting scenario");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            if (LightingScenarioSelector == 1)
            {
                LightingScenarioSelector = 2;
            }
            else LightingScenarioSelector = 1;
            LocalLevelLightmapData.LoadLightingScenario(LightingScenarioSelector);
            Debug.Log("Lighting Scenario " + (LightingScenarioSelector));
        }
    }
}