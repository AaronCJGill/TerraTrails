using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionScript : MonoBehaviour
{

    public float endBar = 115f;
    public GameObject instruction;

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

        if (transform.position.y <= endBar)
        {
            if (waitTime <= 0)
            {
                newPos.y += speed * Time.deltaTime;
            }

            transform.position = newPos;
        }

        if (transform.position.y >= endBar)
        {
            instruction.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }

    }
}
