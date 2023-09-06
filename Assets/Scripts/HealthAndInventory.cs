using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthAndInventory : MonoBehaviour
{
    // Start is called before the first frame update

    public Healthbar characterHealthBar;

    [SerializeField] private int maxHealth;

    private int currentHealth;
    void Start()
    {
        // start with full health
        currentHealth = maxHealth;
        characterHealthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.UpArrow)){
        //     TakeDamage(1);

        // }
    }

   public void TakeDamage(int damage){
        currentHealth -= damage;
        characterHealthBar.SetHealth(currentHealth);
    }
}
