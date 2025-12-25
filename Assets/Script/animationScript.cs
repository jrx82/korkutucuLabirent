using UnityEngine;

public class animationScript : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
     
            
    }


    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            animator.SetBool("Run", true);

        }
        else
        {
            animator.SetBool("Wait", true);
            animator.SetBool("Run", false);
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Attack", true);
        }
        else { animator.SetBool("Attack", false); }

       
      

    }
}
