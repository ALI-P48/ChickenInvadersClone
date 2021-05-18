using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMoverEnemy : Enemy
{
    private bool movingRight;
    private float domain;
    private Vector3 startPos;
    private Vector3 right;
    private Vector3 left;
    private float delta;

    protected override void SetEnemy(Wave wave, float initHealth, float fireRate, float speed)
    {
        this.initHealth = initHealth;
        this.fireRate = fireRate;
        this.speed = speed;
        this.wave = wave;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        movingRight = true;
        domain = 6f;
        startPos = transform.localPosition;
    }

    public override bool CanMove()
    {
        return true;
    }

    public override void Move()
    {
        if (movingRight) {
            //transform.localPosition += Vector3.right * Time.deltaTime * speed;
            
        } else {
            //transform.localPosition += Vector3.left * Time.deltaTime * speed;
        }
    }
}
