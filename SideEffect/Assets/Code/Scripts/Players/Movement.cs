using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 7f, 0f, 0f);
        
        if (Input.GetButtonDown("Jump") && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 0.001f)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
        }
    }
}
