using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health { get; set; }
    public BaseItem Item { get; set; }

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

        // stop sliding when not moving
        if (Input.GetAxis("Horizontal") == 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }

        // prevent character from tilting
        transform.rotation = Quaternion.identity;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // pick only one item at a time and add to it's item
        if (collision.gameObject.tag == "Item" && Item == null)
        {
            Item = collision.gameObject.GetComponent<BaseItem>();
            Destroy(collision.gameObject);
        }
    }
}
