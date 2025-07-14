using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private SquareController sc;

    void Start() 
    {
        animator = GetComponent<Animator>();
        sc = GameObject.FindWithTag("Square").GetComponent<SquareController>();
    }

    void Update()
    {
        if(SquareController.levelEnd) {
            animator.SetTrigger("win");
        }
        animator.SetInteger("random", Random.Range(0,2));
    }

    
}
