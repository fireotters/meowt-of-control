using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrap : MonoBehaviour
{
    private float scrapHealth = 1f, healthLostPerTick = 0.002f;
    private bool scrapNotBigChungus = true;

    public void ScrapIsBigChungus() {
        scrapNotBigChungus = false;
        transform.localScale *= 1.6f;
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (scrapNotBigChungus) {
            if (col.transform.name.Contains("BasicEnemy")) {
                scrapHealth -= healthLostPerTick;
                if (scrapHealth < 0) {
                    Destroy(gameObject);
                }
            
                if (scrapHealth < .2f) {
                    transform.localScale = new Vector3(0.05f, 0.05f, 1f);
                }
                else if (scrapHealth < .4f) {
                    transform.localScale = new Vector3(0.10f, 0.10f, 1f);
                }
                else if (scrapHealth < .6f) {
                    transform.localScale = new Vector3(0.15f, 0.15f, 1f);
                }
                else if (scrapHealth < .8f) {
                    transform.localScale = new Vector3(0.20f, 0.20f, 1f);
                }
            }
        }
    }
}
