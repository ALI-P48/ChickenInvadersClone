using UnityEngine;
using System.Collections;
using System;


[CreateAssetMenu(fileName = "GameManager", menuName = "GameManager")]

public class GameManager : ScriptableObject
{

    [Header("General")]
    public bool friendlyFire = false;

    [Header("Player")] public float playerOpenFireDelay = 0f;
    public float playerInitHealth = 100f;
    public GameObject playerBulletPrefab;
    public float playerBulletSpeed = 10f;
    public float playerBulletDamage = 10f;
    public float playerBulletLifeTime = 3f;
    public int playerBulletWaveCount = 3;
    public float playerFireCoolDown = 0.1f;
    
    [Header("Enemy")]
    public float enemyInitHealth = 100f;
    public GameObject enemyBulletPrefab;
    public float enemyBulletSpeed = 10f;
    public float enemyBulletDamage = 10f;
    public float enemyBulletLifeTime = 3f;
    public float enemyFireCoolDown = 3f;
    public float enemyComeInSpeed = 7f;
    public Vector3[] enemyComeInPath;
    public float enemySpawnCoolDown;
    public float waveSpawnCoolDown;
    public float enemyWaveMoveSpeed = 5f;
    public GameObject enemyPrefab;
    public GameObject enemyWavePrefab;

}