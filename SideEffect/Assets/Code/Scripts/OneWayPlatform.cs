using System.Collections;
using System.Linq;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
  private GameObject currentPlatform;
  private Collider2D playerCollider;

  void Start() {
    playerCollider = GetComponent<Collider2D>();
  }

    private void Update()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            if (currentPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            currentPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        var platformColliders = currentPlatform.GetComponents<Collider2D>();

        foreach (var col in platformColliders) {
          Physics2D.IgnoreCollision(playerCollider, col);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (var col in platformColliders) {
          Physics2D.IgnoreCollision(playerCollider, col, false);
        }
    }
}
