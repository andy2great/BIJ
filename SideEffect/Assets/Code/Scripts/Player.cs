using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health { get; set; } = 0;
    public BaseItem Item { get; set; }
    public bool FacingRight { get; set; }
    public List<BaseEffect> Effects { get; set; } = new List<BaseEffect>();
    
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    void Update()
    {
        Move();
        Act();
    }

    public void AddEffect(BaseEffect effect) {
        _animator.SetTrigger("TrHit");   
        var existingEffect = Effects.FirstOrDefault(e => e.GetType() == effect.GetType());
        if (existingEffect != null) {
            existingEffect.Stage++;
        } else {
            Effects.Add(effect);
            StartCoroutine(effect.ApplyEffect());
        }
    }

    private void Move() {
        var moving = Input.GetAxis("Horizontal") != 0;
        _animator.SetBool("BlWalk", moving);
        if (moving) {
            FacingRight = Input.GetAxis("Horizontal") > 0;
            GetComponent<SpriteRenderer>().flipX = !FacingRight;
        }

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 7f, 0f, 0f);
        
        if (Input.GetButtonDown("Jump") && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 0.001f && !(Input.GetAxis("Vertical") < 0))
        {
            _animator.SetTrigger("TrJump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
        }

        transform.rotation = Quaternion.identity;
    }

    private void Act() {
        if (Input.GetButtonDown("Fire1")) {
            var throwVector = new Vector2(FacingRight ? 10f : -10f, 5f);
            throwVector.x += GetComponent<Rigidbody2D>().velocity.x*2;

            Throw(throwVector);
        }
    }

    private void Throw(Vector2 vector) {
        if (Item == null) return;
        _animator.SetTrigger("TrAttack");
        
        Item.transform.position = transform.position;
        Item.ThrowItem(vector);
        Item = null;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.E)) && collision.gameObject.tag == "Item")
        {
            Throw(new Vector2(0, 7f));

            Item = collision.gameObject.GetComponent<BaseItem>();
            Item.PickUpItem();
        }
        
        if (collision.gameObject.tag == "Platform" && Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") < 0) {
            StartCoroutine(FallThroughFloor());
        }
    }

    IEnumerator FallThroughFloor() {
        Debug.Log("Should fall");
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.25f);
        GetComponent<Collider2D>().enabled = true;
    }
}
