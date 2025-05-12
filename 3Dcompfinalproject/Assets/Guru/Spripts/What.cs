using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class What : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, -1, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
