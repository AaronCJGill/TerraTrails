using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollingScript : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime >= 0)
        {
            waitTime -= Time.deltaTime;
        }

        Vector3 newPos = transform.position;

        if (waitTime <= 0)
        {
            newPos.y += speed * Time.deltaTime;
        }

        transform.position = newPos;

    }
}
