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
            animator.SetBool("Walk", true);

        }
        else
        {
            animator.SetBool("Wait", true);
            animator.SetBool("Walk", false);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);

        }
        else
        {
            animator.SetBool("Run", false);
            animator.SetBool("Wait", true);

        }

       
      

    }
}
