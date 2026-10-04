using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHP = 100;
    public int HP;
    public Slider healthBar;
    private bool isDead;
    public Animator animator;

    void Start()
    {
        HP = MaxHP;
        healthBar.value = 1f;
    }

    public void TakeDamage(int damageAmount, bool isCritical)
    {
        if (!isDead)
        {
            HP -= damageAmount;
            float currentHealthProc = (float)HP / (float)MaxHP;
            Debug.Log(damageAmount);
            healthBar.value = currentHealthProc;
            if (isCritical)
            {
                animator.SetBool("GotCrit", true);
                Debug.Log("Critical Crited");
            }
            else
            {
                animator.SetBool("GotHit", true);
                Debug.Log("Critical Hit");
            }
            if (HP <= 0)
            {
               Debug.Log(gameObject.name + " is dead");
               isDead = true;
               animator.SetBool("IsDead", true);
            }
            Invoke("_ResetHit",0.04f);
        }
    }
    void _ResetHit()
    {
        animator.SetBool("GotCrit", false);
        animator.SetBool("GotHit", false);
    }


}