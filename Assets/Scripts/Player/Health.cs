using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _health = 5;
    public int health { get { return _health; }}
    private int maxHealth = 5;
    public static bool isDead = false;
    SpriteRenderer sr;

    public static Health instance;

    bool canBeDamaged = true;
    [SerializeField]
    [Tooltip("Amount of time in seconds, that character has invincibility for ")]
    float noDamageTimer = 5;
    float currentTimer= 0;

    bool oneHitKill = true;
    public static bool OneHitKill;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        isDead = false;// stop player from being dead on awake
        sr = GetComponent<SpriteRenderer>();
        if (oneHitKill)
            maxHealth = 1;
        OneHitKill = oneHitKill;
    }
    private void Start()
    {
        GameManager.instance.updateHealthUI();
    }
    public void resetHealth()
    {
        _health = maxHealth;
        GameManager.instance.updateHealthUI();
        canBeDamaged = false;
    }
    public void takeDamage(int dmg)
    {
        if (canBeDamaged)
        {
            _health -= dmg;
            healthCheck();
            
            GameManager.instance.updateHealthUI();
            canBeDamaged = false;
        }
    }
    public void addHealth(int heal)
    {
        if (!OneHitKill)
        {
            _health += heal;
            GameManager.instance.updateHealthUI();
        }
    }
    void healthCheck()
    {
        if (OneHitKill)
        {
            _health = 0;
            isDead = true;
        }
        else
        {
            _health = Mathf.Clamp(health, 0, maxHealth);
            if (health == 0)
            {
                isDead = true;
            }
        }

    }

    private void Update()
    {
        if (!canBeDamaged)
        {
            if (currentTimer < noDamageTimer)
            {
                currentTimer += Time.deltaTime;
            }
            else
            {
                //when the timer is up
                currentTimer = 0;
                canBeDamaged = true;
            }
        }
    }

    public void GameOverSequence()
    {
        canBeDamaged = false;
    }
    IEnumerator doHealtFlash()
    {
        //off
        sr.enabled = false;
        yield return new WaitForSeconds(1f);
        sr.enabled = true;
        //on
        yield return new WaitForSeconds(1f);
        sr.enabled = false;
        //off
        yield return new WaitForSeconds(1f);
        sr.enabled = true;
        //on
    }

}
