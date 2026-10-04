using UnityEngine;
using UnityEngine.UI;

public class Relinquished : MonoBehaviour
{
    public int HP = 100;
    public Slider healthBar;

    void Update()
    {
        healthBar.value = HP;
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            GetComponent<Collider>().enabled = false;
        }
    }


}
