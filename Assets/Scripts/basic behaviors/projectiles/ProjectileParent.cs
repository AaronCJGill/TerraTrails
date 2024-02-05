using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParent : MonoBehaviour
{
    [SerializeField]
    Projectile[] children = new Projectile[3];

    void Start()
    {
        //Vector2.SignedAngle(Vector2.right, direction) - 90f
        //float angle = Mathf.Atan2(PlayerMovement.instance.transform.position.y, PlayerMovement.instance.transform.position.x) * Mathf.Rad2Deg;
        
        //var angle = Vector2.SignedAngle(Vector2.right, PlayerMovement.instance.transform.position) - 90f;
        //var targetRotation = new Vector3(0, 0, angle);
        //var lookTo = Quaternion.Euler(targetRotation);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, 360 * Time.deltaTime);

        transform.right = PlayerMovement.instance.transform.position - transform.position;
    }

    void Update()
    {

        if (children[0] == null && children[1] == null&& children[2] == null)
        {
            //if children dont exist, then destroy this
            Destroy(gameObject);
        }
    }
}
