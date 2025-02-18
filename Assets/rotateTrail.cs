using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateTrail : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private float f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * f * Time.fixedDeltaTime);
    }
}
