using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int totalHealth = 5;
    [SerializeField] ProgressBar progressBar;
    [SerializeField] CanvasGroup deathScreen;
    int currentHealth;

    void Start()
    {
        currentHealth = totalHealth;
        UpdateBar();
        deathScreen.alpha = 0;
    }

    void UpdateBar(){
        progressBar.BarValue = ((float)currentHealth/(float)totalHealth)*100;
        Debug.Log("health is now " + currentHealth);
        Debug.Log("bar value is " + progressBar.BarValue);
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
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
