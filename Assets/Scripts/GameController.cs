using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameManager config;
    public GameObject playerPrefab;
    public Spawner spawner;
    public UIController uiController;
    public ParticleSystem bgParticle;

    private Player player;
    private float difficulty = 0.5f;

    void Start()
    {
        player = null;
    }
    
    public void StartGame()
    {
        uiController.ChangeState("InGame");
        if (spawner.gameObject.activeSelf) {
            spawner.SpawnNextWave();
        }
        player = Instantiate(playerPrefab).GetComponent<Player>();
        player.gameController = this;
        player.transform.position = new Vector3(0f, -2f, 0f);
    }

    public void EndGame()
    {
        uiController.ChangeState("Menu");
        Destroy(player.gameObject);
        foreach (Transform child in spawner.transform) {
            GameObject.Destroy(child.gameObject);
        }
        player = null;
    }
}
