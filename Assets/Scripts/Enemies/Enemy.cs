using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float initHealth;
    public float fireRate;
    public float speed;
    private bool initPathEnded;
    [HideInInspector] public Wave wave;

    private float scale = 1f;
    private Vector3[] initPath;
    private int currentInitPathTarget;

    public float health {
        get {
            return _health;
        }
        set {
            _health = value;
            OnHealthChanged(value);
        }
    }
    private float _health;
    [HideInInspector] public float lastTimeFired;

    protected abstract void SetEnemy(Wave wave, float initHealth, float fireRate, float speed);

    public void SetScale(float x)
    {
        this.scale = x;
        transform.localScale = new Vector3(x, x, x);
    }

    public void SetInitPath(Vector3[] path)
    {
        initPathEnded = false; 
        initPath = path;
        currentInitPathTarget = 0;
        transform.position = initPath[0];
        currentInitPathTarget++;
        StartCoroutine(MoveFromTo(transform.position, initPath[currentInitPathTarget])); 
    }
    
    public virtual void OnSpawn()
    {
        lastTimeFired = Time.timeSinceLevelLoad;
        health = initHealth;
        initPathEnded = true;
    }

    public virtual void OnInitPathEnded()
    {
        initPathEnded = true;
    }
    
    void Update() {
        if (CanFire()) {
            Fire();
        }

        if (CanMove()) {
            Move();
        }
    }

    public virtual bool CanFire()
    {
        return (initPathEnded && Time.timeSinceLevelLoad - lastTimeFired > fireRate);
    }

    public abstract bool CanMove();
    
    private void Fire()
    {
        Bullet bullet = Instantiate(wave.spawner.config.enemyBulletPrefab, transform.position + 0.3f*Vector3.down, Quaternion.identity).GetComponent<Bullet>();
        bullet.Set(false, -1*wave.spawner.config.enemyBulletSpeed, wave.spawner.config.enemyBulletDamage, wave.spawner.config.enemyBulletLifeTime, scale);
        lastTimeFired = Time.timeSinceLevelLoad;
    }

    public abstract void Move();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet") {
            Bullet bullet = other.GetComponent<Bullet>();
            OnBulletHit(bullet);
        }
    }

    public virtual void OnBulletHit(Bullet bullet)
    {
        if (bullet.friendly) {
            health = health - bullet.damage;
            Destroy(bullet.gameObject);
        }
    }

    public virtual void OnHealthChanged(float value)
    {
        if (value <= 0) {
            Die();
        }
    }

    public virtual void Die()
    {
        wave.EnemyDied(this);
    }
    
    private IEnumerator MoveFromTo(Vector3 a, Vector3 b) {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            transform.position = Vector3.Lerp(a, b, t); 
            yield return new WaitForFixedUpdate();
        }
        
        transform.position = b;

        if (currentInitPathTarget < initPath.Length - 1) {
            StartCoroutine(MoveFromTo(initPath[currentInitPathTarget], initPath[currentInitPathTarget+1]));
            currentInitPathTarget++;
        } else {
            OnInitPathEnded();
        }
    }
    
    
}
