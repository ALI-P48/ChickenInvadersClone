using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagMoverEnemy : Enemy
{
    public Vector2 min;
    public Vector2 max;

    private Vector3 currentTraget;

    public void SetParameters(Wave wave, float initHealth, float speed, float fireRate, Vector2 min , Vector2 max)
    {
        SetEnemy(wave, initHealth, fireRate, speed);
        this.min = min;
        this.max = max;
    }

    protected override void SetEnemy(Wave wave, float initHealth, float fireRate, float speed)
    {
        this.wave = wave;
        this.initHealth = initHealth;
        this.fireRate = fireRate;
        this.speed = speed;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        currentTraget = NextTarget();
    }

    public override bool CanMove()
    {
        return true;
    }

    public override void Move()
    {
        Vector3 diff = currentTraget - transform.position;
        if (diff.magnitude < 0.1f) {
            currentTraget = NextTarget();
        } else {
            transform.position += diff.normalized * Time.deltaTime * speed;
        }
    }

    private Vector3 NextTarget()
    {
        Vector3 res = Vector3.zero;
        res.x = Random.Range(min.x, max.x);
        res.y = Random.Range(min.y, max.y);

        return res;
    }
}