using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [Header("Tweakable Variables")]
    public int bulletsLeft = 4;
    public int currentPlayerHealth = 3;
    public float pistolReloadTime = 1f;
    private readonly int pistolMaxBullets = 4, maxPlayerHealth = 3;
    // If pistolMaxBullets is changed, bullet indicators need to be added in Unity, and assigned to bulletIndicators[]

    [Header("Private Components / Other Variables")]
    private float nextPlayerDmg = 0.0f;
    private const float playerDmgInterval = 2f;
    private GameManager _gM;
    private SoundManager _soundManager;
    private Animator _spriteAnimator;
    private PlayerController _playerController;
    [SerializeField] private Transform respawnPoint = default;

    private void Start()
    {
        _gM = ObjectsInPlay.i.gameManager;
        _soundManager = GetComponent<SoundManager>();
        _spriteAnimator = transform.GetChild(2).GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        FindComponentsPistol();
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Hitting player damages them
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("LargeEnemy"))
        {
            DamagePlayer();
        }
        else if (col.gameObject.CompareTag("Scrap") && Input.GetKey(KeyCode.LeftShift))
        {
            Destroy(col.gameObject);
        }
    }

    public void ResetPosition()
    {
        transform.position = respawnPoint.position;
    }

}
