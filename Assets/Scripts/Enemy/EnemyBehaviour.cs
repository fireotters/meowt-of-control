using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject corpse;
    public int bigChungusLife;
    public int basicLife;
    public int sanicLife;
    public int playerLife;

    private int bigChungusMaxLife = 10;
    private int basicMaxLife = 4;
    private int sanicMaxLife = 2;

    public GameObject heldItem;

    private GameManager gM;

    // Health bar
    private float healthBarFullSize;
    public Transform healthBar;

    private Vector2 enemyLastPos;

    private void Awake()
    {
        AstarPath.active.logPathResults = PathLog.None;    // Disable seeker logs to clean up log output.
    }

    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        bigChungusLife = bigChungusMaxLife;
        basicLife = basicMaxLife;
        sanicLife = sanicMaxLife;
        playerLife = 6;
        healthBarFullSize = healthBar.localScale.x;
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        // Very redundant but actually works so idc I wanna sleep

        if (col.gameObject.tag == "Projectile" && gameObject.tag == "Enemy")
        {
            basicLife--;
            float percentOfLifeLeft = (float)basicLife / (float)basicMaxLife;
            ChangeHealthBar(percentOfLifeLeft);
        }

        if (col.gameObject.tag == "Projectile" && gameObject.tag == "BigChungusEnemy")
        {
            bigChungusLife--;
            float percentOfLifeLeft = (float)bigChungusLife / (float)bigChungusMaxLife;
            ChangeHealthBar(percentOfLifeLeft);
        }

        if (col.gameObject.tag == "Projectile" && gameObject.tag == "Sanic")
        {
            sanicLife--;
            float percentOfLifeLeft = (float)sanicLife / (float)sanicMaxLife;
            ChangeHealthBar(percentOfLifeLeft);
        }

        if (col.gameObject.tag == "Projectile" && basicLife == 0)
        {
            gM.KillEnemy();
            Vector2 enemyLastPos =
                new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy(gameObject);
            corpse.transform.position = enemyLastPos;
            Instantiate(corpse);
        }

        if (col.gameObject.tag == "Projectile" && bigChungusLife == 0)
        {
            gM.KillEnemy();
            Vector2 enemyLastPos =
                new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy(gameObject);
            heldItem.transform.position = enemyLastPos;
            Instantiate(heldItem);
        }

        if (col.gameObject.tag == "Projectile" && sanicLife == 0)
        {
            gM.KillEnemy();
            Vector2 enemyLastPos =
                new Vector2(col.gameObject.transform.position.x, col.gameObject.transform.position.y);
            Destroy(gameObject);
            heldItem.transform.position = enemyLastPos;
            Instantiate(heldItem);
        }
        
        if (col.gameObject.tag == "Player")
        {
            gM.DamagePlayer();
        }

        if (col.gameObject.tag == "Player" && playerLife <= 0)
        {
            Destroy(col.gameObject);
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void ChangeHealthBar(float percentLifeLeft)
    {
        Vector3 scaleChange = healthBar.localScale;
        scaleChange.x = percentLifeLeft;
        healthBar.localScale = scaleChange;
    }
}