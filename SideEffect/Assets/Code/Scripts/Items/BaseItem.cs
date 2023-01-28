using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem: MonoBehaviour {
  public abstract string Name { get; set; }
  public abstract int Damage { get; set; }
  public abstract int Health { get; set; }

  public void ThrowItem() {
    gameObject.layer = LayerMask.NameToLayer("ThrownItem");
    gameObject.tag = "ThrownItem";
  }

  public void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.tag == "Player" && gameObject.tag == "ThrownItem") {
      Debug.Log("Player hit!");

      var playerHealth = collision.gameObject.GetComponent<Player>().Health;
      var playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
      var playerVelocity = playerRigidbody.velocity;
      var force = playerVelocity * Damage * playerHealth;
      playerRigidbody.AddForce(force, ForceMode2D.Impulse);
      
      var itemRigidbody = gameObject.GetComponent<Rigidbody2D>();
      var itemVelocity = itemRigidbody.velocity;
      var itemForce = itemVelocity * -1;
      itemRigidbody.AddForce(itemForce, ForceMode2D.Impulse);

      Health -= 1;

      collision.gameObject.GetComponent<Player>().Health += Damage;
      if (Health <= 0) {
        Object.Destroy(gameObject);
      }

      ApplyEffect();
    }
  }

  public abstract void ApplyEffect();
}