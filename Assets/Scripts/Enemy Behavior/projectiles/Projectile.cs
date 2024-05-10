using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 12f;
    ParticleSystem particleEffect;
    // Start is called before the first frame update
    private void Start()
    {
        particleEffect = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime * -1);
        if (Mathf.Abs( transform.position.x )> 40 || Mathf.Abs(transform.position.y) > 40)
        {
            
            Destroy(gameObject);
        }

        if (transform.position.x > LevelBoundary.BottomRightBound.X || transform.position.x < LevelBoundary.leftTopBound.X ||
            transform.position.y < LevelBoundary.BottomRightBound.Y || transform.position.y > LevelBoundary.leftTopBound.Y
            )
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        particleEffect.transform.parent = null;
        particleEffect.Emit(5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
