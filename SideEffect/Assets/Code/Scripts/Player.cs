using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
  public int Health { get; set; }
  public BaseItem Item { get; set; }
  public List<BaseEffect> Effects { get; set; } = new List<BaseEffect>();
  public PauseMenu PauseScreen;
  public ulong ClientId { get; set; } = 0;
  
  private bool hasReceivedEffect = false;

  private NetworkVariable<bool> _facingRight = new NetworkVariable<bool>(
    true,
    NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Owner
  );
  private NetworkVariable<bool> _hasItem = new NetworkVariable<bool>(
    false,
    NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Server
  );

  public override void OnNetworkSpawn()
  {
    _facingRight.OnValueChanged += (oldValue, newValue) =>
    {
      GetComponent<SpriteRenderer>().flipX = !newValue;
    };
  }

  void Start()
  {
    ClientId = OwnerClientId;
    Debug.Log($"Player {ClientId} has spawned");
  }

  void Update()
  {
    if (!IsOwner) return;

    CheckDeathServerRpc();
    ApplyVisual();
    Move();
    Act();
  }

  private void ApplyVisual() {
    var walking = Input.GetAxis("Horizontal") != 0;
    if (walking)
    {
      _facingRight.Value = Input.GetAxis("Horizontal") > 0;
    }
  }

  public void AddEffect(BaseEffect effect)
  {
    if (!IsOwner) return;

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
      StartCoroutine(RemoveEffect(effect));
    }
  }
  
  public IEnumerator RemoveEffect(BaseEffect effect)
  {
    yield return new WaitForSecondsRealtime(7.5f);
    StartCoroutine(effect.RemoveEffect());
    Effects.Remove(effect);
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
      Debug.Log("Jump");
      GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
    }

    transform.rotation = Quaternion.identity;
  }

  private void Act()
  {
    if (Input.GetButtonDown("Fire1"))
    {
      var throwVector = new Vector2(_facingRight.Value ? 10f : -10f, 5f);
      throwVector.x += GetComponent<Rigidbody2D>().velocity.x * 2;

      Throw(throwVector);
    }
  }

  private void Throw(Vector2 vector)
  {
    if (!_hasItem.Value) return;
    GetComponent<Animator>().SetTrigger("TrAttack");
    ThrowItemServerRpc(vector.x, vector.y, gameObject.transform.position.x, gameObject.transform.position.y);
  }

  void OnTriggerStay2D(Collider2D collision)
  {
    // error 
    if ((Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.E)) && collision.gameObject.tag == "Item")
    {
      PickUpItemServerRpc(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
    }
  }

  [ServerRpc]
  private void CheckDeathServerRpc()
  {
    if (transform.position.y < -10)
    {
      Health = 3;
      _hasItem.Value = false;
      Item = null;
      RemoveEffectsClientRpc();
    }
  }

  [ClientRpc]
  private void RemoveEffectsClientRpc() {
    transform.position = new Vector3(0, 0, 0);
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    foreach (var effect in Effects)
    {
      
      Debug.Log($"Remove {effect.GetType().Name}");
      StartCoroutine(effect.RemoveEffect());
    }
    Effects.Clear();
  }

  [ServerRpc]
  private void PickUpItemServerRpc(float x, float y)
  {
    // list all items in the scene
    var items = FindObjectsOfType<BaseItem>();
    // round x to 2 decimal places
    x = (int)x*1000;
    y = (int)y*1000;

    var item = items.FirstOrDefault(i => {
      var itemX = (int)i.transform.position.x*1000;
      var itemY = (int)i.transform.position.y*1000;
      return itemX == x && itemY == y;
    });
    
    Throw(new Vector2(0, 7f));
     
    if (item == null) return;
    
    _hasItem.Value = true;
    Item = item;
    Item.PickUpItem();
  }

  [ServerRpc]
  private void ThrowItemServerRpc(float x, float y, float newPosX, float newPosY)
  {
    Item.transform.position = new Vector2(newPosX, newPosY);
    Item.ThrowItem(new Vector2(x, y));
    _hasItem.Value = false;
    Item = null;
  }
}
