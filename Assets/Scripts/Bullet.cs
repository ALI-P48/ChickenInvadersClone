using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public bool friendly = true;
    [HideInInspector] public float speed = 10f;
    [HideInInspector] public float damage = 10f;
    [HideInInspector] public float lifeTime = 5f;

    private float birthTime;
    
    public void Set(bool friendly, float speed, float damage, float lifeTime, float scale)
    {
        this.friendly = friendly;
        this.speed = speed;
        this.damage = damage;
        this.lifeTime = lifeTime;
        transform.localScale *= scale;
    }
    
    void Start()
    {
        birthTime = Time.timeSinceLevelLoad;
    }
    
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if (Time.timeSinceLevelLoad - birthTime > lifeTime) {
            Destroy(gameObject);
        }
    }
}
