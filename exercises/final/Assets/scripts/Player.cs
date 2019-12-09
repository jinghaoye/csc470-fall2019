using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{


    public Animator animator;
    public Text gamestart;
    public Text gamestop;
    public float Factor;
    public float MaxDistance;
    public GameObject Stage;
    private Vector3 current_vector;
    public Transform Camera;

    public GameObject model;

    public Text ScoreText;
    public Text highestscore;
    private int score = 0;
    private int highScore = 0;
    string highScoreKey = "HighScore";

    public GameObject Particle;
    private Rigidbody player_rigid;
    private float startTime;
    private GameObject currentStage;
    private Collider lastStage;
    private Vector3 CamRel_position;
    private Vector3 nextCamerapos;
    float moveSpeed = 30f;
    Vector3 tmp_scale;
    Vector3 direction = new Vector3(1, 0, 0);

    public AudioSource music;

    public AudioClip mysound1;
    public AudioClip mysound2;


    private float stageheight =0f;

    




    // Start is called before the first frame update
    void Start()
    {
        

        Time.timeScale = 0.0f;
        animator = model.GetComponent<Animator>();
        music = GetComponent<AudioSource>();
        player_rigid = GetComponent<Rigidbody>();
        player_rigid.centerOfMass = Vector3.zero;
        currentStage = Stage;
        lastStage = currentStage.GetComponent<Collider>();
        generateStage();
        CamRel_position = Camera.position - transform.position;
        current_vector = currentStage.transform.localPosition;
        
       
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);

        highestscore.text = highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gamestart.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            startTime = Time.time;
            Particle.SetActive(true);
            tmp_scale = currentStage.transform.localScale;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            var duration = Time.time - startTime;
            music.clip = mysound2;
            music.Play();
            jump(duration);
            currentStage.transform.localScale = tmp_scale;
            currentStage.transform.localPosition = current_vector;
            Particle.SetActive(false);
            animator.SetBool("isJumping", true);

        }
        if (Input.GetKey(KeyCode.Space))
        {
            
            currentStage.transform.localScale += new Vector3(0, -1, 0) * 0.6f * Time.deltaTime;
            currentStage.transform.localPosition += new Vector3(0, -1, 0) * 0.6f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            stopgame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            restatrgame();
        }

    }

    void jump(float duration)
    {
        player_rigid.AddForce((new Vector3(0, 1.3f, 0) + direction)*duration*0.8f*Factor,ForceMode.Impulse);

    }

    void generateStage()
    {
        var stage = Instantiate(currentStage);
        stage.transform.position = currentStage.transform.position + direction*Random.Range(8f, MaxDistance);

        var randomScale = Random.Range(5, 7);
        stage.transform.localScale = new Vector3(randomScale, 3, randomScale);

        stage.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1));

    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.SetBool("isJumping", false);
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.Contains("stage")&& collision.collider != lastStage)
        {


            player_rigid.Sleep();
            lastStage = collision.collider;
            
            currentStage = collision.gameObject;
            Destroy(currentStage, 7.0f);
            changedirection();
            current_vector = currentStage.transform.localPosition;
            generateStage();
            
            StartCoroutine(Adjust_Cam());
            
            score++;
            music.clip = mysound1;
            music.Play();

            ScoreText.text = score.ToString();
        }

        if(collision.gameObject.name == "ground")
        {
            if (score > highScore)
            {
                PlayerPrefs.SetInt(highScoreKey, score);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene(0);

        }

      
    }

    void changedirection()
    {
        var seed = Random.Range(0, 3);
        if (seed == 0)
        {
            direction = new Vector3(1, stageheight, 0);
            transform.rotation= Quaternion.Euler(0f,0f,0f);
        }
        else if (seed == 1)
        {
            direction = new Vector3(0, stageheight, 1);
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else
        {
            stageheight += 0.15f;
            direction = new Vector3(1, stageheight, 0);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        
    
    }


    IEnumerator Adjust_Cam()
    {
        nextCamerapos = transform.position + CamRel_position;
        while (Vector3.SqrMagnitude(nextCamerapos-Camera.position)>0.1f){ 
            nextCamerapos = transform.position + CamRel_position;
            Camera.position = Vector3.Lerp(Camera.position, nextCamerapos, Time.deltaTime * moveSpeed *(1f / 5f));
            yield return 0;
        }
    }


    void stopgame()
    {
        Time.timeScale = 0.0f;
        gamestop.gameObject.SetActive(true);
    }

    void restatrgame()
    {
        Time.timeScale = 1.0f;
        gamestop.gameObject.SetActive(false);
    }



}
