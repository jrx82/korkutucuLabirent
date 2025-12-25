using UnityEngine;
using UnityEngine.AI; 

public class TakipciDusman : MonoBehaviour
{
    public Transform hedef; 
    private NavMeshAgent ajan; 

    void Start()
    {
        
        ajan = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
        if (hedef != null)
        {
           
            ajan.SetDestination(hedef.position);
        }
    }
}