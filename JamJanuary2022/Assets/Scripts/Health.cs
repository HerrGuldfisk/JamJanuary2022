using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int totalHealth = 5;
    [SerializeField] ProgressBar progressBar;
    [SerializeField] CanvasGroup deathScreen;
    [SerializeField] CanvasGroup dmgImage;
    [SerializeField] float dmgImageAlphaMax = 0.5f;
    [SerializeField] float dmgImageFalloffTime = 0.5f;
    
    int currentHealth;
    bool recentlyDamaged = false;

    void Start()
    {
        currentHealth = totalHealth;
        UpdateBar();
        deathScreen.alpha = 0;
    }

    private void Update() {
        if (recentlyDamaged){
            dmgImage.alpha -= dmgImageAlphaMax*Time.deltaTime/dmgImageFalloffTime;
            if (dmgImage.alpha <= 0) recentlyDamaged = false;
        }
    }

    void UpdateBar(){
        progressBar.BarValue = ((float)currentHealth/(float)totalHealth)*100;
        Debug.Log("health is now " + currentHealth);
        Debug.Log("bar value is " + progressBar.BarValue);
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        recentlyDamaged = true;
        dmgImage.alpha = dmgImageAlphaMax;
        UpdateBar();
        if (currentHealth <= 0) Die();
    }

    void Die(){
        deathScreen.alpha = 1;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Enemy")){
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
