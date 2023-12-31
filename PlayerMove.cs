using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }


    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                audioSource.Play();
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                audioSource.Play();
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                audioSource.Play();
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                audioSource.Play();
                break;
            case "DIE":
                audioSource.clip = audioDie;
                audioSource.Play();
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                audioSource.Play();
                break;
        }
    }

    void Update()
    {
        // 점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("is Jumping"))
        {
           rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
           anim.SetBool("is Jumping", true);
           PlaySound("JUMP");
        }

        
        // Stop Speed 
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전환
        if( Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        // 애니메이션
        if ( Mathf.Abs(rigid.velocity.x) < 0.3 )
        {
            anim.SetBool("is walking", false );
        }
        else
            anim.SetBool("is walking", true );
    }

    void FixedUpdate()
    {
        // Move speed
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)   // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1) )   // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);


        // Landing Platform
        if(rigid.velocity.y < 0)
        {
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if(rayHit.collider != null)
        {
            if(rayHit.distance < 0.5f)
                anim.SetBool("is Jumping", false);
        }
        
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else // Damaged
                OnDamaged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            // Point
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            
            if(isBronze)
                gameManager.stagePoint += 100;
            else if(isSilver)
                gameManager.stagePoint += 200;
            else if(isGold)
                gameManager.stagePoint += 300;
            // Deactive Item
            collision.gameObject.SetActive(false);

            // Sound
            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
           gameManager.NextStage();

           // Sound
           PlaySound("FINISH");
        }
    }

    void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;
        // Reaction Force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();

        // Sound
        PlaySound("ATTACK");
    }
    
    
    void OnDamaged(Vector2 targetPos)
    {
        // Health Down
        gameManager.HealthDown();
        
        
        // Change Layer  ( Immortal Active )
        gameObject.layer = 13;

        // View Alpha
        spriteRenderer.color = new Color(1,1,1,0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc,1)*7, ForceMode2D.Impulse);

        // Animation
        anim.SetTrigger("doDamaged");

        // Sound
        PlaySound("DAMAGED");
        
        
        
        Invoke("OffDamaged",3);
        
    }

    void OffDamaged()
    {
        gameObject.layer = 12;
        spriteRenderer.color = new Color(1,1,1,1);
    }

    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color (1,1,1,0.4f);
        // Sprite Filp Y
        spriteRenderer.flipY = true;
        // Collider Disable
        capsuleCollider.enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5,ForceMode2D.Impulse);

        //Sound
        PlaySound("DIE");
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }


}
