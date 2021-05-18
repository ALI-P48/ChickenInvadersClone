using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wave : MonoBehaviour
{
    [HideInInspector] public List<Enemy> enemies;
    [HideInInspector] public Spawner spawner;
    //[HideInInspector] public int id;

    public virtual void StartWave()
    {
        MoveIn();
    }

    public abstract void MoveIn();

    public virtual bool IsWaveEdned()
    {
        return (enemies.Count == 0);
    }

    public virtual void EndWave()
    {
        foreach (Enemy enemy in enemies) {
            enemy.Die();
        }
        spawner.onWaveEnded(this);
    }

    public virtual void EnemyDied(Enemy enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (IsWaveEdned()) {
            EndWave();
        }
    }
}
