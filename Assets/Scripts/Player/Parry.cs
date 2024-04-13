using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField]
    float reuseTime = 2;
    float timer = 0;
    [SerializeField]
    float destroyRadius = 3;
    [SerializeField]
    float parryActiveTime = 0.5f;

    bool canParry = true;
    [SerializeField]
    GameObject parrycircle;
    private void Awake()
    {
        parrycircle = Resources.Load("Parry Circle") as GameObject;
        if (parrycircle != null)
        {
            Debug.Log("Parry");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Health.isDead)
        {
            if (timer <= reuseTime)
            {
                timer += Time.deltaTime;
                canParry = false;
                //Debug.Log("Parrytime " + timer);
            }
            else
            {
                canParry = true;
                //Debug.Log("canParry");

            }
            if (Input.GetAxisRaw("Jump") == 1 && canParry)
            {
                canParry = false;

                //Debug.Log("Parried");
                //lazy method, find each gameobject projectile in scene and just delete whichever ones are closest
                GameObject pc = Instantiate(parrycircle, transform.position, Quaternion.identity);
                pc.GetComponent<ParryCircle>().init(destroyRadius, parryActiveTime);
                Invoke("parryAction", 0.1f);

                timer = 0;
            }
        }

    }

    void parryAction()
    {
        Projectile[] projList = FindObjectsOfType<Projectile>();
        //Debug.Log("Parry Action pressed");
        /*
        foreach (Projectile proj in projList)
        {
            if (Vector2.Distance(transform.position, proj.transform.position) <= destroyRadius)
                Destroy(proj.gameObject);
        }
        */
    }

    private void OnDrawGizmos()
    {
        
    }

}
