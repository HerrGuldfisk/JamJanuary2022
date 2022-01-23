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
    [SerializeField] GameObject hurtSFXPopPrefab;
    [SerializeField] GameObject deathSFXPopPrefab;
    [SerializeField] List<Component> componentsToTurnOffOnDeath;
    [SerializeField] List<GameObject> objectsToTurnOffOnDeath;
    [SerializeField] Text finalScoreText;
    
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
    }

    public void TakeDamage(int damage){
        
        currentHealth -= damage;
        UpdateBar();

        if (currentHealth <= 0){
            Die();
        }
        else{
            //DMG IMAGE
            recentlyDamaged = true;
            dmgImage.alpha = dmgImageAlphaMax;

            //DMG SFX
            GameObject.Instantiate(hurtSFXPopPrefab, transform.position, Quaternion.identity);
        }
        
    }

    void Die(){
        deathScreen.alpha = 1;
        dmgImage.alpha = 0;
        finalScoreText.text = GameObject.FindObjectOfType<ScoreSystem>().currentScore.ToString() + " BEES KILLED";
        Instantiate(deathSFXPopPrefab);

        foreach(Component comp in componentsToTurnOffOnDeath){
            Destroy(comp);
        }
        foreach(GameObject obj in objectsToTurnOffOnDeath){
            obj.SetActive(false);
        }

        //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Enemy")){
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        else if (other.transform.CompareTag("Respawn")){
            Die();
        }

    }
}
