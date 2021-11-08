using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    
    private float CurrentSpeed = 0;
    public float MaxSpeed;
    public float boostSpeed;
    private float RealSpeed; //not the applied speed
    [HideInInspector]
    public bool GLIDER_FLY;

    public Animator gliderAnim;

    [Header("Tires")]
    public Transform frontLeftTire;
    public Transform frontRightTire;
    public Transform backLeftTire;
    public Transform backRightTire;

    //drift and steering stuffz
    private float steerDirection;
    private float driftTime;

    public bool driftLeft = false;
    public bool driftRight = false;
    float outwardsDriftForce = 50000;

    public bool isSliding = false;
     
    private bool touchingGround;

    [Header("Particles Drift Sparks")]
    public Transform leftDrift;
    public Transform rightDrift;
    public Color drift1;
    public Color drift2;
    public Color drift3;

    [HideInInspector]
    public float BoostTime = 0;

    public Transform boostFire;
    public Transform boostExplosion;

    float time = 0f;
    bool cond = true;
    public GameObject obj;
    public GameObject obj2;

    private VictoryCheckpoint victoryList;
    public bool hasEndedRace;
    private bool canMove = true;
    public GameObject finalLapCamera;

    public AudioSource[] effects = new AudioSource[12];

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        victoryList = GameObject.Find("Track").GetComponent<VictoryCheckpoint>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        effects[0].Play();
        effects[1].Play();
        effects[2].Play();
        effects[3].Play();
        effects[4].Play();
        effects[5].Play();

        effects[0].Pause();
        effects[1].Pause();
        effects[2].Pause();
        effects[3].Pause();
        effects[4].Pause();
        effects[5].Pause();


        this.obj2 = GameObject.Find("Pista 1/GliderPanel");
    }

    void FixedUpdate()
    {
        canMove = victoryList.raceStarts;

        move();
        tireSteer();
        steer();
        groundNormalRotation();
        drift();
        boosts();


        if (hasEndedRace)
        {
            if (canMove)
            {
                Vector3 vel = transform.forward * (MaxSpeed);
                rb.velocity = vel;
            }
            else
            {
                Vector3 vel = transform.forward * 0;
                rb.velocity = vel;
                CurrentSpeed = 0;
                effects[0].Stop();
                effects[1].Stop();
                effects[2].Stop();
                effects[3].Stop();
                effects[4].Stop();
                effects[5].Stop();
                effects[6].Stop();
                effects[7].Stop();
                effects[8].Stop();
                effects[9].Stop();
                effects[10].Stop();
                effects[11].Stop();
            } 
        }
    }

    public void endRace(){
        StartCoroutine(endRaceCO());
    }

    private IEnumerator endRaceCO(){
        yield return new WaitForSeconds(1.5f);
        victoryList.FinishGame(this.transform);
        finalLapCamera.gameObject.SetActive(true);
        MaxSpeed = 0;
        hasEndedRace = true;
        victoryList.raceStarts = false;
    }

    protected void LateUpdate()
    {
        if (!GLIDER_FLY)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            if(this.transform.localEulerAngles.x < 23)
            {
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(-7, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    private void move()
    {
        RealSpeed = this.transform.InverseTransformDirection(rb.velocity).z; //real velocity before setting the value. This can be useful if say you want to have hair moving on the player, but don't want it to move if you are accelerating into a wall, since checking velocity after it has been applied will always be the applied value, and not real
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                this.CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, Time.deltaTime * 0.5f); //speed

                if (!GLIDER_FLY)
                {
                    effects[3].UnPause();
                    effects[1].Pause();
                    effects[2].Pause();
                    effects[0].Pause();
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.CurrentSpeed = Mathf.Lerp(CurrentSpeed, -MaxSpeed / 1.75f, 1f * Time.deltaTime);

                if (!GLIDER_FLY)
                {
                    effects[2].UnPause();
                    effects[1].Pause();
                    effects[3].Pause();
                    effects[0].Pause();
                }
            }
            else
            {
                this.CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0, Time.deltaTime * 1.5f); //speed

                if (!GLIDER_FLY)
                {
                    effects[1].UnPause();
                    effects[2].Pause();
                    effects[3].Pause();
                }
            }
        }
        else
        {
            effects[1].UnPause();
            if (Input.GetKeyDown(KeyCode.W))
            {
                effects[0].Play();
                effects[1].Pause();
                effects[2].Pause();
                effects[3].Pause();
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                effects[0].Stop();
                effects[1].UnPause();
                effects[2].Pause();
                effects[3].Pause();
            }
        }
  

        if (!GLIDER_FLY)
        {
            if(cond == true)
            {
                Vector3 vel = transform.forward * CurrentSpeed;
                vel.y = rb.velocity.y - 1f; //gravity
                this.rb.velocity = vel;
            }
            else
            {
                if (!this.effects[11].isPlaying)
                {
                    this.effects[11].PlayOneShot(effects[11].clip, 1f);  
                }
                Vector3 vel = transform.forward * time;
                vel.y = rb.velocity.y - 1f; //gravity
                obj.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, time*7, transform.localEulerAngles.z);
                time -= Time.deltaTime * 60;
                //rb.velocity = vel;

                if (time <= -50.0f)
                {
                    cond = true;
                    obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                    time = 0.0f;
                }
            }
        }
        else
        {
            //this.effects[9].UnPause();
            this.effects[0].Pause();
            this.effects[1].Pause();
            this.effects[2].Pause();
            this.effects[3].Pause();
            this.effects[4].Pause();
            this.effects[5].Pause();

            Vector3 vel = transform.forward * (MaxSpeed + 1);
            vel.y = rb.velocity.y * 0.6f - 1.5f; //gravity with gliding
            rb.velocity = vel;
            if (rb.transform.eulerAngles.y >= 200)
            {
                rb.transform.eulerAngles = new Vector3(transform.localEulerAngles.x, 200, transform.localEulerAngles.z);
            }
            if (rb.transform.eulerAngles.y <= 160)
            {
                rb.transform.eulerAngles = new Vector3(transform.localEulerAngles.x, 160, transform.localEulerAngles.z);
            }
        }
    }
    private void steer()
    {
        steerDirection = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        Vector3 steerDirVect; //this is used for the final rotation of the kart for steering

        float steerAmount;

        if (driftLeft && !driftRight)
        {
            steerDirection = Input.GetAxis("Horizontal") < 0 ? -2.5f : -1.5f;
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime);

            
            if(isSliding && touchingGround)
               rb.AddForce(transform.right * outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);

            effects[5].UnPause();
        }
        else if (driftRight && !driftLeft)
        {
            steerDirection = Input.GetAxis("Horizontal") > 0 ? 2.5f : 1.5f;
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime);

            if(isSliding && touchingGround)
                rb.AddForce(transform.right * -outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);

            effects[5].UnPause();
        }
        else
        {
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
            effects[5].Pause();
        }

        //since handling is supposed to be stronger when car is moving slower, we adjust steerAmount depending on the real speed of the kart, and then rotate the kart on its y axis with steerAmount
        steerAmount = RealSpeed > 30 ? RealSpeed / 4 * steerDirection : steerAmount = RealSpeed / 1.5f * steerDirection;

        //glider movements

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && GLIDER_FLY)  //left
        {
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 40), 2 * Time.deltaTime);          
        } // left 
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && GLIDER_FLY) //right
        {
          
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -40), 2 * Time.deltaTime);
        } //right
        else //nothing
        {
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0), 2 * Time.deltaTime);
        } //nothing

        /*if (Input.GetKey(KeyCode.UpArrow) && GLIDER_FLY) 
        {
            
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(25, transform.eulerAngles.y, transform.eulerAngles.z), 2 * Time.deltaTime);
            
            rb.AddForce(Vector3.down * 80000 * Time.deltaTime, ForceMode.Acceleration);
        } //moving down
        else if (Input.GetKey(KeyCode.DownArrow) && GLIDER_FLY)  
        {
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(-25, transform.eulerAngles.y, transform.eulerAngles.z), 2 * Time.deltaTime);
            rb.AddForce(Vector3.up * 4000 * Time.deltaTime, ForceMode.Acceleration);

        } //rotating up - only use this if you have special triggers around the track which disable this functionality at some point, or the player will be able to just fly around the track the whole time
        else
        {
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z), 2 * Time.deltaTime);
        }*/
        
        steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect , 3 * Time.deltaTime); 

    }
    private void groundNormalRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.75f))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7.5f * Time.deltaTime);
            touchingGround = true;
        }
        else
        {
            touchingGround = false;
        }
    }
    private void drift()
    {
        if (Input.GetKeyDown(KeyCode.Space) && touchingGround)
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Hop");
            if(steerDirection > 0)
            {
                driftRight = true;
                driftLeft = false;
            }
            else if(steerDirection < 0)
            {
                driftRight = false;
                driftLeft = true;
            }
        }


        if ((Input.GetKey(KeyCode.Space) && touchingGround && CurrentSpeed > 15 && Input.GetAxis("Horizontal") != 0 && driftLeft == true) || (Input.GetKey(KeyCode.Space) && touchingGround && CurrentSpeed > 15 && Input.GetAxis("Horizontal") != 0 && driftRight == true))
        {
            driftTime += Time.deltaTime;

            //particle effects (sparks)
            if (driftTime >= 1 && driftTime < 3)
            {
                effects[4].UnPause();
                effects[5].Pause();

                for (int i = 0; i < leftDrift.childCount; i++)
                {
                    ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //right wheel particles
                    ParticleSystem.MainModule PSMAIN = DriftPS.main;

                    ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //left wheel particles
                    ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;

                    PSMAIN.startColor = drift1;
                    PSMAIN2.startColor = drift1;

                    if (!DriftPS.isPlaying && !DriftPS2.isPlaying)
                    {
                        DriftPS.Play();
                        DriftPS2.Play();
                    }
                }
            }
            if (driftTime >= 3 && driftTime < 6)
            {
                effects[4].Pause();
                effects[4].UnPause();
                effects[5].Pause();

                //drift color particles
                for (int i = 0; i < leftDrift.childCount; i++)
                {
                    ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN = DriftPS.main;
                    ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;
                    PSMAIN.startColor = drift2;
                    PSMAIN2.startColor = drift2;
                }

            }
            if (driftTime >= 6)
            {
                effects[4].Pause();
                effects[4].UnPause();
                effects[5].Pause();

                for (int i = 0; i < leftDrift.childCount; i++)
                {
                    ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN = DriftPS.main;
                    ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;
                    PSMAIN.startColor = drift3;
                    PSMAIN2.startColor = drift3;
                }
            }
        }

        if (!Input.GetKey(KeyCode.Space) || RealSpeed < 15)
        {
            driftLeft = false;
            driftRight = false;
            isSliding = false; /////////

            //give a boost
            if (driftTime > 1 && driftTime < 3)
            {
                effects[4].Pause();
                effects[6].PlayOneShot(effects[6].clip, 0.5f);
                BoostTime = 0.75f;
            }
            if (driftTime >= 3 && driftTime < 6)
            {
                effects[4].Pause();
                effects[6].PlayOneShot(effects[6].clip, 0.5f);
                BoostTime = 1.5f;
                
            }
            if (driftTime >= 6)
            {
                effects[4].Pause();
                effects[6].PlayOneShot(effects[6].clip, 0.5f);
                BoostTime = 2.5f;
               
            }

            //reset everything
            driftTime = 0;
            //stop particles
            for (int i = 0; i < 5; i++)
            {
                ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //right wheel particles
                ParticleSystem.MainModule PSMAIN = DriftPS.main;

                ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //left wheel particles
                ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;

                DriftPS.Stop();
                DriftPS2.Stop();

            }
        }
    }
    private void boosts()
    {
        BoostTime -= Time.deltaTime;
        if(BoostTime > 0)
        {
            for(int i = 0; i < boostFire.childCount; i++)
            {
                if (! boostFire.GetChild(i).GetComponent<ParticleSystem>().isPlaying)
                {
                    boostFire.GetChild(i).GetComponent<ParticleSystem>().Play();
                }

            }
            MaxSpeed = boostSpeed;

            CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, 1 * Time.deltaTime);
        }
        else
        {
            for (int i = 0; i < boostFire.childCount; i++)
            {
                boostFire.GetChild(i).GetComponent<ParticleSystem>().Stop();
            }
            MaxSpeed = boostSpeed - 20;
        }
    }

    private void tireSteer()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 155, 0), 5 * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 155, 0), 5 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 205, 0), 5 * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 205, 0), 5 * Time.deltaTime);
        }
        else
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 180, 0), 5 * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 180, 0), 5 * Time.deltaTime);
        }

        //tire spinning

        if (CurrentSpeed > 20)
        {
            frontLeftTire.GetChild(0).Rotate(-90 * Time.deltaTime * CurrentSpeed * 0.5f, 0, 0);
            frontRightTire.GetChild(0).Rotate(-90 * Time.deltaTime * CurrentSpeed * 0.5f, 0, 0);
            backLeftTire.Rotate(90 * Time.deltaTime * CurrentSpeed * 0.5f, 0, 0);
            backRightTire.Rotate(90 * Time.deltaTime * CurrentSpeed * 0.5f, 0, 0);
        }
        else
        {
            frontLeftTire.GetChild(0).Rotate(-90 * Time.deltaTime * RealSpeed * 0.5f, 0, 0);
            frontRightTire.GetChild(0).Rotate(-90 * Time.deltaTime * RealSpeed * 0.5f, 0, 0);
            backLeftTire.Rotate(90 * Time.deltaTime * RealSpeed * 0.5f, 0, 0);
            backRightTire.Rotate(90 * Time.deltaTime * RealSpeed * 0.5f, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "GliderPanel")
        {
            if (this.obj2.gameObject.activeSelf == true)
            {
                if (this.GLIDER_FLY == true)
                {
                    this.GLIDER_FLY = false;
                }
                else if (this.GLIDER_FLY == false)
                {
                    if (!this.effects[7].isPlaying || !this.effects[8].isPlaying || !this.effects[9].isPlaying)
                    {
                        this.effects[7].PlayOneShot(effects[7].clip, 0.8f);
                        this.effects[8].PlayOneShot(effects[8].clip, 0.8f);
                        this.effects[9].PlayOneShot(effects[9].clip, 1f);
                    }
                }
                this.GLIDER_FLY = true;
                gliderAnim.SetBool("GliderOpen", true);
                gliderAnim.SetBool("GliderClose", false);
            }
        }
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.obj2.gameObject.activeSelf == true)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "OffRoad")
            {
                GLIDER_FLY = false;
                gliderAnim.SetBool("GliderOpen", false);
                gliderAnim.SetBool("GliderClose", true);
                this.effects[9].Stop();
                if (gliderAnim.enabled == false)
                {
                    this.effects[10].PlayOneShot(effects[10].clip);
                }
            }

            if (collision.gameObject.tag == "Martelo" && !GLIDER_FLY)
            {
                this.cond = false;
            }

            if (collision.gameObject.tag == "Lombada" && !GLIDER_FLY)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    if (CurrentSpeed >= 33)
                    {
                        CurrentSpeed = 33;
                        //CurrentSpeed = Mathf.Lerp(CurrentSpeed -= 0.4f, -MaxSpeed / 1.75f, 1f * Time.deltaTime);
                    }
                    /*else
                    {
                        CurrentSpeed = Mathf.Lerp(CurrentSpeed, -50 / 1.75f, 1f * Time.deltaTime);
                    }*/
                }
            }
        }
    }
}
