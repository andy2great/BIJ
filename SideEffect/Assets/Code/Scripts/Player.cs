using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
  public int Health { get; set; }
  public BaseItem Item { get; set; }
  public bool FacingRight { get; set; }
  public List<BaseEffect> Effects { get; set; } = new List<BaseEffect>();
  public PauseMenu PauseScreen;
  private bool hasReceivedEffect = false;

  void Start()
  {
  }

  void Update()
  {
    VisualizeEffects();
    if (!IsOwner) return;

    Move();
    Act();
  }

  public void VisualizeEffects()
  {
    var walking = Input.GetAxis("Horizontal") != 0;
    if (walking)
    {
      FacingRight = Input.GetAxis("Horizontal") > 0;
      GetComponent<SpriteRenderer>().flipX = !FacingRight;
    }
  }

  public void AddEffect(BaseEffect effect)
  {
    if (effect.Name == "Cure") 
    {
      var currentEffects = new List<BaseEffect>(Effects);

      foreach(var currentEffect in currentEffects)
      {
        StartCoroutine(currentEffect.RemoveEffect());
        Effects.Remove(currentEffect);
      }

      return;
    }

    // Activate trigger animation
    GetComponent<Animator>().SetTrigger("BlHit");

    if (!hasReceivedEffect)
    {
      hasReceivedEffect = true;
      PauseGame();
      StartCoroutine(ShowWarning());
      ResumeGame();
    }

    var existingEffect = Effects.FirstOrDefault(e => e.GetType() == effect.GetType());
    if (existingEffect != null)
    {
      existingEffect.Stage++;
    }
    else
    {
      Effects.Add(effect);
      StartCoroutine(effect.ApplyEffect());
    }
  }

  private IEnumerator ShowWarning()
  {
    PauseScreen.gameObject.SetActive(true);
    StartCoroutine(PauseScreen.CountDown());
    yield return new WaitForSecondsRealtime(3.5f);
    PauseScreen.gameObject.SetActive(false);
  }

  private void ResumeGame()
  {
    Time.timeScale = 1;
  }

  private void PauseGame()
  {
    Time.timeScale = 0;
  }

  private void Move()
  {
    var walking = Input.GetAxis("Horizontal") != 0;
    GetComponent<Animator>().SetBool("BlWalk", walking);
    transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 7f, 0f, 0f);

    if (Input.GetButtonDown("Jump") && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 0.001f && !(Input.GetAxis("Vertical") < 0))
    {
      GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
    }

    transform.rotation = Quaternion.identity;
  }

  private void Act()
  {
    if (Input.GetButtonDown("Fire1"))
    {
      var throwVector = new Vector2(FacingRight ? 10f : -10f, 5f);
      throwVector.x += GetComponent<Rigidbody2D>().velocity.x * 2;

      Throw(throwVector);
    }
  }

  private void Throw(Vector2 vector)
  {
    if (Item == null) return;
    GetComponent<Animator>().SetTrigger("TrAttack");

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
  }
}
