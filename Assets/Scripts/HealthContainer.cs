using System;
using UnityEngine;

public class HealthContainer : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int maxHealth = 3;

    private int curHealth;
    private bool isDead;

    public int MaxHealth => maxHealth;

    public int CurHealth => curHealth;

    public bool IsDead => isDead;

    private void Awake()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(int damage, object src)
    {
        if (isDead) return;
        
        curHealth = Mathf.Clamp(curHealth - damage, 0, maxHealth);

        if (curHealth <= 0)
        {
            isDead = true;

            Time.timeScale = 0;
            Debug.Log("I Am Now Become Death Destroyer O-timescale set to 0 due to player's death");
        }
    }
}