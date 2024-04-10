using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionScript : MonoBehaviour
{
    public float speed = 2f;

    public float barForStop = 4f;

    public float deacceleration = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (deacceleration >= 0)
        {
            deacceleration -= Time.deltaTime;
        }

        Vector3 newPos = transform.position;

        if(transform.position.x <= barForStop)
        {
            newPos.x += speed * deacceleration;
        }

        transform.position = newPos;

    }
}
