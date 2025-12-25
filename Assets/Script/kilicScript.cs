using UnityEngine;

public class kilicScript : MonoBehaviour
{
    public int damage = 50; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            
            dusmanAI enemy = other.GetComponent<dusmanAI>();

            if (enemy != null)
            {
               
                enemy.TakeDamage(damage);
            }
            else
            {
                dusmanAI enemyParent = other.GetComponentInParent<dusmanAI>();

                if (enemyParent != null)
                {
                    enemyParent.TakeDamage(damage);
                }
            }
        }
    }
}