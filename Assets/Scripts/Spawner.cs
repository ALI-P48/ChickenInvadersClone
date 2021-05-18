using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public WaveType[] ww;
    
    public GameManager config;
    public GameController gameController;
    public List<Wave> waves;
    public int currentWaveID;
    public Wave currentWave;

    void Start()
    {
        if(waves.Count>0)
            currentWaveID = 0;
    }

    void Update()
    {
        if (currentWave!=null && currentWave.IsWaveEdned()) {
            currentWave.EndWave();
        }
    }

    public void onWaveEnded(Wave wave)
    {
        Debug.LogError("a");
        Destroy(wave.gameObject);
        currentWaveID++;
        SpawnNextWave();
    }

    public void SpawnNextWave()
    {
        if (currentWaveID < waves.Count) {
            Wave wave = Instantiate(waves[currentWaveID].gameObject).GetComponent<Wave>();
            wave.transform.parent = transform;
            wave.transform.localPosition = new Vector3(0, 3f, 0f);
            wave.spawner = this;
            wave.StartWave(); 
        }
    }
}

[System.Serializable]
public class WaveType : Editor
{
    public enum myEnum // your custom enumeration
    {
        SoloWave, RectWave
    };
    public myEnum dropDown = myEnum.SoloWave;

    // The function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        dropDown = (myEnum) EditorGUILayout.EnumPopup("Display", dropDown);

        // Create a space to separate this enum popup from the other variables 
        EditorGUILayout.Space(); 

        // Check the value of the enum and display variables based on it

        // Save all changes made on the Inspector
        serializedObject.ApplyModifiedProperties();
    }
}
