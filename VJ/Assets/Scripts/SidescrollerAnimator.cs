using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidescrollerAnimator : MonoBehaviour
{
    private Animator m_Animator;
    private Text m_textMesh;
    public double cooldown = 0.0f;
    public double runtime = 0.0f;
    public double jumptime = 0.0f;
    public bool running = false;
    public bool jumping = false;

    // Jump scripting
    private bool Jump_On = false;
    public void Jump(float time_to_jump)
    {
        Debug.Log("Jump function");
        Jump_On = true;
        while (Jump_On)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1.0f));
            time_to_jump -= Time.fixedDeltaTime;
            if (time_to_jump <= 0)
            {
                Jump_On = false;
            }
            //yield return new WaitForFixedUpdate();
        }
    }

    void FixedUpdate()
    {
        if (this.jumping && !Jump_On) // add !Jump_On to prevent start another jump when current in progress
        {
            //jump = false;
            Jump(0.3f); // 0.3 - time of jump in seconds (time of working force)
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_textMesh = GameObject.Find("Status").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // If we recently jumped, recover cooldown
        if (this.cooldown < 0.0f) this.cooldown += 0.1f * Time.deltaTime;

        //Press the up arrow button to reset the trigger and set another one
        //if (Input.GetKey(KeyCode.UpArrow))
        if (Input.GetMouseButtonDown(0) & this.cooldown >= 0.0f)
        {
            Debug.Log("Button 0 down");
            this.running = true;
            this.jumping = false;
            //Reset the "Crouch" trigger
            //m_Animator.ResetTrigger("Crouch");

            //Send the message to the Animator to activate the trigger parameter named "Jump"
            m_Animator.SetTrigger("StartRun");
        }
        else if (Input.GetMouseButtonUp(0) & this.running)
        {
            Debug.Log("Button 0 up");
            Debug.Log("Starting jump with running time: " + this.runtime.ToString("0.###"));
            this.cooldown -= 5.0f * this.runtime;
            this.running = false;
            this.jumping = true;
            this.runtime = 0.0f;
            //Reset the "Jump" trigger
            //m_Animator.ResetTrigger("Jump");

            //Send the message to the Animator to activate the trigger parameter named "Crouch"
            m_Animator.SetTrigger("StartJump");
        }
        else if (this.running)
        {
            this.runtime += 1.0f * Time.deltaTime;
            this.jumptime += 1.0f * Time.deltaTime;
            m_textMesh.text = "Running " + this.runtime.ToString("0.00");
            //Debug.Log(this.runtime.ToString());
        }
        else if (this.jumping & this.jumptime > 0.0f)
        {
            this.jumptime -= 1.0f * Time.deltaTime;
            m_textMesh.text = "Jumping " + this.jumptime.ToString("0.00");
            //Debug.Log(this.jumptime.ToString());
        }
        else if (this.jumping & this.jumptime <= 0.0f)
        {
            Debug.Log("Stopping jump");
            this.jumping = false;
            m_Animator.SetTrigger("StopJump");
        }
        else if (this.cooldown < 0.0f)
        {
            this.cooldown += 1.0f * Time.deltaTime;
            m_textMesh.text = "Jump cooldown " + this.cooldown.ToString("0.00");
        }
        else
        {
            m_textMesh.text = "Ready to go!";
        }
    }
}
