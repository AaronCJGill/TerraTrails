using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionScript : MonoBehaviour
{
    public float speed = 2f;
    public float barForStopx = -1f;
    public float barForStopy = 4f;
    public float deacceleration = 2f;

    public bool vertical = false;
    public bool diagonal = false;

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

        if (!vertical && !diagonal)
        {
            if (transform.position.x <= barForStopx)
            {
                newPos.x += speed * deacceleration;
            }
        }

        if(vertical)
        {
            if (transform.position.y >= barForStopy)
            {
                newPos.y -= speed * Time.deltaTime;
            }
        }

        if (diagonal)
        {
            if (transform.position.x <= barForStopy && transform.position.y >= barForStopy)
            {
                newPos.x += speed * Time.deltaTime;
                newPos.y -= speed * Time.deltaTime;
            }
        }

        transform.position = newPos;

    }
}
