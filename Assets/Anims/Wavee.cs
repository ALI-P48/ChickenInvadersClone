using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavee : MonoBehaviour
{
    [HideInInspector] public int cols;
    [HideInInspector] public int rows;
    [HideInInspector] public Dictionary<int, Enemyy> spawnedEnemies;
    [HideInInspector] public Spawner spawner;
    public GameManager config;

    private int count;
    private int spawned;

    public void Set(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        count = rows * cols;
        spawned = 0;

        spawnedEnemies = new Dictionary<int, Enemyy>(count);
        StartCoroutine(Spawn());
    }

    public void onEnemyInPosition(int id)
    {
        if (id == count - 1) {
            StartCoroutine(Move(true));
            foreach (Enemyy enemy in spawnedEnemies.Values) {
                enemy.Fire();
            }
        }
    }

    public void onEnemyDied(int id)
    {
        spawnedEnemies.Remove(id);
        if (spawnedEnemies.Count == 0) {
            //spawner.onWaveDied();
            Destroy(gameObject);
        }
    }
    
    IEnumerator Spawn()
    {
        int id = spawned;
        Enemyy enemy = Instantiate(config.enemyPrefab, config.enemyComeInPath[0], Quaternion.identity)
            .GetComponent<Enemyy>();
        enemy.transform.parent = transform;
        enemy.localPos = new Vector3((spawnedEnemies.Count / cols)+0.5f, -(spawnedEnemies.Count % cols)-0.5f, 0f);
        enemy.id = id;
        enemy.wave = this;
        enemy.canShoot = false;
        spawnedEnemies.Add(id, enemy);
        spawned++;
        yield return new WaitForSeconds(config.enemySpawnCoolDown);
        if (spawned < count)
        {
            StartCoroutine(Spawn());
        }
    }
    
    IEnumerator Move(bool goToRight)
    {
        Vector3 strt = transform.localPosition;
        Vector3 dest = transform.localPosition;
        if (goToRight)
            dest.x = 6;
        else
            dest.x = 0;
        float step = (config.enemyWaveMoveSpeed / (transform.localPosition - dest).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            transform.localPosition = Vector3.Lerp(strt, dest, t); 
            yield return new WaitForFixedUpdate(); 
        }
        
        transform.localPosition = dest;
        
        StartCoroutine(Move(!goToRight));
    }
}
