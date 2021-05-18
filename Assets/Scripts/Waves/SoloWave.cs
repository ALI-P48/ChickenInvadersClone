using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloWave : Wave
{

    public float speed;
    public float initHealth;
    public float fireRate;
    public float scale;

    public override void MoveIn()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.SetInitPath(new Vector3[] {new Vector3(-10f, 1.5f, 0f), new Vector3(0f, 1.5f, 0f)});
        }
    }

    public override void StartWave()
    {
        enemies = new List<Enemy>(1);
        ZigZagMoverEnemy enemy = Instantiate(spawner.config.enemyPrefab, spawner.config.enemyComeInPath[0], Quaternion.identity)
            .AddComponent<ZigZagMoverEnemy>();
        enemy.SetParameters(this, initHealth, speed, fireRate, new Vector2(-6f, -1f), new Vector2(6f, 2.5f));
        enemy.SetScale(scale);
        enemy.transform.SetParent(transform);
        enemy.transform.position = new Vector3(0f, 1.5f, 0f);
        enemy.OnSpawn();
        enemies.Add(enemy);
        base.StartWave();
    }
}
