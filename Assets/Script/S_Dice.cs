using UnityEngine;

public class S_Dice : MonoBehaviour
{
    [System.Serializable] 
    public struct MyStruct
    { 
        public Component Side;
        public int DamageNumber;
    }

    private bool hasHit = false;
    public Animator animator;

    public MyStruct[] AllSides;
    public Component[] Sides;
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            hasHit = true;
            if (collision.gameObject.tag == "CritBox")
            {
                Debug.Log("Crit");
                collision.gameObject.transform.GetComponentInParent<EnemyHealth>().TakeDamage(AllSides[AllSides.Length - 1].DamageNumber,true);
                
            }
            else
            {
                collision.gameObject.transform.GetComponentInParent<EnemyHealth>().TakeDamage(AllSides[Random.Range(0,AllSides.Length - 1)].DamageNumber,false);
            }
            animator.SetBool("Despawn", true);
        }
    }
}
