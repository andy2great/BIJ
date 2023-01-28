using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem: MonoBehaviour {
  public abstract string Name { get; set; }
  public abstract int Damage { get; set; }
  public abstract int Health { get; set; }
  public abstract int XSpeedFactor { get; set; }
  public abstract int YSpeedFactor { get; set; }
  public abstract float Spin { get; set; }
  public abstract BaseEffect Effect { get; set; }

  public void Start() {
    StartCoroutine(Destroy());
  }

  public void Update() {
    if (gameObject.tag == "ThrownItem") {
      if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < 0.001f && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 0.001f) {
        gameObject.tag = "Item";
      }
    }
  }

  public void PickUpItem() {
    gameObject.SetActive(false);
  }

  public void ThrowItem(Vector2 vector) {
    gameObject.SetActive(true);
    gameObject.layer = LayerMask.NameToLayer("Default");
    gameObject.tag = "Untagged";
    vector.x *= XSpeedFactor;
    vector.y *= YSpeedFactor;
    gameObject.GetComponent<Rigidbody2D>().AddForce(vector, ForceMode2D.Impulse);
    gameObject.GetComponent<Rigidbody2D>().AddTorque(Spin, ForceMode2D.Impulse);

    StartCoroutine(EnableCollider());
  }


  // on trigger enter 2d is called when the collider of the object enters the collider of another object
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player" && gameObject.tag == "ThrownItem") {
      ApplyPlayerGettingHit(collision.gameObject);
      ApplyItemGettingHit(gameObject);
    }
  }

  private void ApplyPlayerGettingHit(GameObject player) {
      var directionX = (player.transform.position - gameObject.transform.position).x < 0 ? -1 : 1;
      var directionY = (player.transform.position - gameObject.transform.position).y < 0 ? -1 : 1;
      
      // make force vector base on x direction, player health and item damage
      var playerHealth = player.GetComponent<Player>().Health;
      var playerRigidbody = player.GetComponent<Rigidbody2D>();
      var force = new Vector2(directionX * Damage * playerHealth, 5f * directionY);
      playerRigidbody.AddForce(force, ForceMode2D.Impulse);

      player.GetComponent<Player>().Health += Damage;
      // add effect to player
      player.GetComponent<Player>().AddEffect(Effect);
  }

  private void ApplyItemGettingHit(GameObject item) {
    var itemRigidbody = item.GetComponent<Rigidbody2D>();
    var itemVelocity = itemRigidbody.velocity;
    var itemForce = itemVelocity * -2;
    itemRigidbody.AddForce(itemForce, ForceMode2D.Impulse);

    item.GetComponent<BaseItem>().Health -= 1;

    if (item.GetComponent<BaseItem>().Health <= 0) {
      Object.Destroy(item);
    }
  }

  
  private IEnumerator EnableCollider() {
    yield return new WaitForSeconds(0.5f);
    gameObject.tag = "ThrownItem";
  }

  private IEnumerator Destroy() {
    while (true) {
      if (gameObject.tag == "Item") {
        yield return new WaitForSeconds(10f);
        if (gameObject.tag != "Item") continue;

        Object.Destroy(gameObject);
      }
      yield return new WaitForSeconds(1f);
    }
  }
}