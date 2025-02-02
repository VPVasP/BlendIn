using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Components")]
    private CharacterController characterController;
    [Header("Values")]
    [SerializeField] private float walkSpeed;
    private Vector3 playerVelocity;
    private AudioSource aud;
    public bool isInvisible;
    private new Renderer renderer;
    private IEnumerator coroutine;
    private Animator anim;
    [SerializeField] private AudioClip invisibleClip;
    [SerializeField] private AudioClip resetInvisibleClip;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material invisibleMaterial;
    private Color previousColor;
    private void Start()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponentInChildren<Renderer>();
        characterController = GetComponent<CharacterController>();
        gameObject.tag = "Player";
        aud = GetComponent<AudioSource>();
        renderer.material.color = Color.white;
        previousColor = Color.white;
    }
    private void Update()
    {

        MovementFunction();
        ChangeMaterial();
        ResetVisibility();
    }
    private void MovementFunction()
    {
        //handle the player movement 
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(moveVector * Time.deltaTime * walkSpeed);

        //rotate the player 
        if (moveVector != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref playerVelocity.y, 0.1f);
            angle = Mathf.Repeat(angle, 360f);

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        //walking animation
        if (characterController.velocity.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

    }
    private void ChangeMaterial()
    {
        //change the player's material based on the input of the user
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            if (renderer != null)
            {
                aud.clip = resetInvisibleClip;
                aud.Play();
                renderer.material.color = Color.white;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            if (renderer != null)
            {
                aud.clip = resetInvisibleClip;
                aud.Play();
                renderer.material.color = Color.yellow;
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            if (renderer != null)
            {
                aud.clip = resetInvisibleClip;
                aud.Play();
                renderer.material.color = Color.blue;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            if (renderer != null)
            {
                aud.clip = resetInvisibleClip;
                aud.Play();
                renderer.material.color = Color.red;
            }
        }
        //become invisible 
        if (Input.GetKeyDown(KeyCode.Space) && !isInvisible && SliderManager.instance.invisibilitySlider.value==100)
        {

            if (renderer != null)
            {
                aud.clip = invisibleClip;
                aud.Play();
                previousColor = renderer.material.color;
                renderer.material = invisibleMaterial;
                isInvisible = true;
            }
        }
    }
    //reset the invisibility 
    private void ResetVisibility()
    {
        if (isInvisible && coroutine == null)
        {
            SliderManager.instance.ResetSliderValue();
            //call a coroutine to reset invisibility after 4 seconds
            coroutine = ResetInvisibilityEnumerator(4.0f);
            StartCoroutine(coroutine);
        }
    }

    //reset the invisibility coroutine
    private IEnumerator ResetInvisibilityEnumerator(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            //reset the material and visibility

            if (renderer != null)
            {
                aud.clip = resetInvisibleClip;
                aud.Play();
                renderer.material = normalMaterial;
                renderer.material.color = previousColor;
                isInvisible = false;
                
            }
            coroutine = null;
            yield break; 
        }
    }
}

