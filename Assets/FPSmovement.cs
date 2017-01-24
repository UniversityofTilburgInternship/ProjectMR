﻿﻿﻿﻿﻿﻿﻿using UnityEngine;

public class FPSmovement : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float movementSpeed = 8;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed *= 2;
        }
        if (Input.GetKey("e"))
        {
            transform.position += transform.up * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("q"))
        {
            transform.position -= transform.up * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 