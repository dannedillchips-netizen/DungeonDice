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
    
    private Rigidbody rb;

    public MyStruct[] AllSides;
    public Component[] Sides;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            hasHit = true;
            if (collision.gameObject.tag == "CritBox")
            {
                collision.gameObject.transform.GetComponentInParent<EnemyHealth>().TakeDamage(AllSides[AllSides.Length - 1].DamageNumber,true);
            }
            else
            {
                collision.gameObject.transform.GetComponentInParent<EnemyHealth>().TakeDamage(AllSides[Random.Range(0,AllSides.Length - 1)].DamageNumber,false);
            }
            animator.SetBool("Despawn", true);
            Vector3 deflectV = collision.impulse.normalized;
            deflectV.y = 1;
            rb.AddForce(deflectV * 20f, ForceMode.Impulse);
        }
    }
}
