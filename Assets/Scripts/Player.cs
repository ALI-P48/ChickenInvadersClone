using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager config;
    
    private Vector2 spriteSize;
    private Vector3 screenBounds;
    private float lastTimeFired;
    private List<Transform> bulletSpawnPoints;
    private ParticleSystem destructionEffect;
    private Animator animator;
    private ParticleSystem[] motors;

    [HideInInspector] public GameController gameController;
    private float mouseDownStartTime;

    private bool holding  {
        get {
            return _holding;
        }
        set
        {
            if (_holding == value)
                return;
            
            _holding = value;
            if (_holding) {
                TurnMotorsOn();
                mouseDownStartTime = Time.timeSinceLevelLoad;
            } else {
                TurnMotorsOff();
            }
        }
    }

    private bool _holding = false;
    
    public float health {
        get {
            return _health;
        }
        set {
            _health = value;
            gameController.uiController.UpdateHearts(value);
            if (health <= 0) {
                Die();
            }
        }
    }
    private float _health;
    
    // Start is called before the first frame update
    void Start()
    {
        holding = false;
        Transform motorsParent = transform.Find("Motors");
        motors = new ParticleSystem[motorsParent.childCount];
        int x = 0;
        foreach (Transform child in motorsParent) {
            motors[x] = child.GetComponent<ParticleSystem>();
            x++;
        }
        destructionEffect = transform.Find("Destruction").GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        health = config.playerInitHealth;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        spriteSize = new Vector2(transform.Find("Sprite").GetComponent<SpriteRenderer>().bounds.size.x / 2, transform.Find("Sprite").GetComponent<SpriteRenderer>().bounds.size.y / 2);
        bulletSpawnPoints = new List<Transform>(config.playerBulletWaveCount);
        float offset = 2*spriteSize.x / (config.playerBulletWaveCount + 1);
        
        GameObject bulletSpawnPointParent = transform.Find("BulletSpawnPoints").gameObject;
        // bulletSpawnPointParent.transform.parent = transform;
        // bulletSpawnPointParent.transform.localPosition = Vector3.zero;
        // bulletSpawnPointParent.name = "BulletSpawnPoints";
        
        for (int i = 1 ; i <= config.playerBulletWaveCount; i++) {
            GameObject newSpawnPoint = new GameObject();
            newSpawnPoint.transform.parent = bulletSpawnPointParent.transform;
            newSpawnPoint.name = i.ToString();
            newSpawnPoint.transform.position = new Vector3((-1*spriteSize.x) + (offset * i),
                transform.position.y, transform.position.z);
            bulletSpawnPoints.Add(newSpawnPoint.transform);
        }
    }
    
    void Update()
    {
        Vector3 pos = Vector3.zero;
        #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
            holding = Input.GetMouseButton(0);
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        #else
            holding = Input.GetMouseButton(0);
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        #endif
        pos.x = Mathf.Clamp(pos.x, screenBounds.x * -1 + spriteSize.x, screenBounds.x - spriteSize.x);
        pos.y = Mathf.Clamp(pos.y, screenBounds.y * -1 + spriteSize.y + (screenBounds.y*0.1f), screenBounds.y - spriteSize.y - (screenBounds.y*0.3f));
        pos.z = transform.position.z;
        transform.position = pos;
        
        if (holding && Time.timeSinceLevelLoad-mouseDownStartTime>config.playerOpenFireDelay) {
            Fire();
        }
    }

    private void Fire()
    {
        if (Time.timeSinceLevelLoad - lastTimeFired > config.playerFireCoolDown) {
            for (int i = 0; i < config.playerBulletWaveCount; i++) {
                Bullet bullet = Instantiate(config.playerBulletPrefab, bulletSpawnPoints[i].position, Quaternion.identity).GetComponent<Bullet>();
                //bullet.transform.parent = transform;
                bullet.Set(true, config.playerBulletSpeed, config.playerBulletDamage, config.playerBulletLifeTime, 1f);
                lastTimeFired = Time.timeSinceLevelLoad;
            }
        }
    }

    private void Die()
    {
        //Debug.LogError("You Died!");
        destructionEffect.Play();
        animator.SetTrigger("Die");
        gameController.EndGame();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet") {
            Bullet bullet = other.GetComponent<Bullet>();
            if (config.friendlyFire || (!config.friendlyFire && !bullet.friendly)) {
                health = health - bullet.damage;
                Destroy(bullet.gameObject);
            }
        }
    }

    private void TurnMotorsOn()
    {
        foreach (ParticleSystem ps in motors) {
            ps.Play();
        }
        animator.SetTrigger("TurnOn");
    }
    
    private void TurnMotorsOff()
    {
        foreach (ParticleSystem ps in motors) {
            ps.Stop();
        }
        animator.SetTrigger("TurnOff");
    }
}
