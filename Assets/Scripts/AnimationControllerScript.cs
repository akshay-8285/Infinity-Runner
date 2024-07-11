using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationControllerScript : MonoBehaviour
{
    Animator animator;
    int runningHash;
    int jumpingHash;
    
    public GameObject goButton;
    bool goButtonPressed = false;
    private PlayerMovmentScript playerMovementScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovementScript = GetComponent<PlayerMovmentScript>();

        runningHash = Animator.StringToHash("running");
        jumpingHash = Animator.StringToHash("jumping");

        // Initially, don't play the running animation
        animator.SetBool(runningHash, false);
        goButton.SetActive(true);
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        bool isJumping = animator.GetBool(jumpingHash);
        bool jumpPress = Input.GetKeyDown(KeyCode.Space);

        // Jumping animation
        if (!isJumping && jumpPress)
        {
            animator.SetBool(jumpingHash, true);
        }
        if (isJumping && !jumpPress)
        {
            animator.SetBool(jumpingHash, false);
        }
    }

    public void StartRunning()
    {
        if (!goButtonPressed)
        {
            animator.SetBool(runningHash, true);
            goButton.SetActive(false);
            goButtonPressed = true;
            playerMovementScript.StartRunning(); // Start player movement
        }
    }
}
