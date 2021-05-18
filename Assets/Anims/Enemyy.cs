using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyy : MonoBehaviour
{
    [SerializeField] private GameManager config;

    [HideInInspector] public int id;
    [HideInInspector] public Wavee wave;
    [HideInInspector] public bool canShoot;
    
    public float health {
        get {
            return _health;
        }
        set {
            _health = value;
            if (health <= 0) {
                Die();
            }
        }
    }
    private float _health;
    private float lastTimeFired;
    private int nextNode = 0;

    public Vector3 localPos;
    
    void Start()
    {
        lastTimeFired = Time.timeSinceLevelLoad;
        health = config.enemyInitHealth;
        nextNode = 1;
        StartCoroutine(MoveFromTo(config.enemyComeInPath[0], config.enemyComeInPath[1], config.enemyComeInSpeed));
    }

    void Update()
    {
        if (canShoot && Time.timeSinceLevelLoad - lastTimeFired > config.enemyFireCoolDown) {
            Fire();
        }
    }
    
    public void Fire()
    {
        Bullet bullet = Instantiate(config.enemyBulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        //bullet.transform.parent = transform;
        bullet.Set(false, -1*config.enemyBulletSpeed, config.enemyBulletDamage, config.enemyBulletLifeTime, 1f);
        lastTimeFired = Time.timeSinceLevelLoad;
        canShoot = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet") {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet.friendly) {
                health = health - bullet.damage;
                Destroy(bullet.gameObject);
            }
        }
    }

    public void OnCameIn()
    {
        Vector3 firstStep = localPos;
        firstStep.x = transform.localPosition.x;
        StartCoroutine(MoveFromToLocal(transform.localPosition, firstStep, config.enemyComeInSpeed, () =>
        {
            StartCoroutine(MoveFromToLocal(transform.localPosition, localPos, config.enemyComeInSpeed, () =>
            {
                wave.onEnemyInPosition(id);
            }));
        }));
        
    }
    
    private void Die() {
        wave.onEnemyDied(id);
        Destroy(gameObject);
    }
    
    IEnumerator MoveFromTo(Vector3 a, Vector3 b, float speed) {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            transform.position = Vector3.Lerp(a, b, t); 
            yield return new WaitForFixedUpdate();
        }
        
        transform.position = b;

        if (nextNode < config.enemyComeInPath.Length - 1) {
            StartCoroutine(MoveFromTo(config.enemyComeInPath[nextNode], config.enemyComeInPath[nextNode+1], config.enemyComeInSpeed));
            nextNode++;
        } else {
            OnCameIn();
        }
    }
    
    IEnumerator MoveFromToLocal(Vector3 a, Vector3 b, float speed, System.Action act = null) {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            transform.localPosition = Vector3.Lerp(a, b, t);
            yield return new WaitForFixedUpdate();
        }
        
        transform.localPosition = b;

        if (act != null) {
            act.DynamicInvoke();
        }
    }
}