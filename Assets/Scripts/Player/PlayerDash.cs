using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField]
    float dashCooldown = 10;
    float cooldownTimer = 0;
    PlayerMovement pm;
    [SerializeField]
    float dashTime = 0.35f;
    float dashTimer = 0;
    [SerializeField]
    float speedMultiplier = 1.5f;
    [SerializeField]
    bool dashActive = false;
    [SerializeField]
    bool canDash = false;
    AudioInstance audioinstance;//second sound

    private void Start()
    {
        pm = PlayerMovement.instance;
        audioinstance = GetComponent<AudioInstance>();
    }


    void Update()
    {
        if (!Health.isDead)
        {
            //cooldown timer needs to go up when we cant dash
            if (dashCooldown > cooldownTimer && !canDash)
            {
                cooldownTimer += Time.deltaTime;
            }
            else if (dashCooldown <= cooldownTimer)
            {
                //when we reach over that time, we set candash to true
                canDash = true;
            }
            //handle turning on the dash action
            if (canDash)
            {
                if (Input.GetAxisRaw("Jump") == 1)
                {
                    audioinstance.playThirdSound();
                    Debug.Log("Dashed");
                    pm.playerDash(speedMultiplier);
                    dashTimer = 0;
                    cooldownTimer = 0;
                    canDash = false;
                    dashActive = true;
                }
            }

            //turn dash off 
            if (dashActive)
            {
                dashTimer += Time.deltaTime;
                if (dashTimer >= dashTime)
                {
                    pm.playerDash();
                    dashActive = false;
                }
            }
        }
    }
}
