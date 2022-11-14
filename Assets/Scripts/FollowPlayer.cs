using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public Transform target = null;                    
    public float reactDelay = 0.5f;                    
    public float recordFPS = 15f;                    
    public float followSpeed = 5f;                    
    public float targetDistance = 0f;                
    public bool targetDistanceY = false;            
    public bool targetPositionOnStart = false;        
    public bool start = false;

    public int life = 2;  
    public float jumpForce;   
    public GameObject lifesPanel;
    private Rigidbody2D rigidBody2D;
    public float horizontal;
    public float cooldownTime = 1f;
    private bool isGrounded;
    private bool isInCooldown;
    private Animator animator;
    private Vector2 initialPosition;
    public Vector2 respawnPoint;
    private GameObject destinyWarp;

#if UNITY_EDITOR
         public bool gizmo = true;                        
#endif

    [Serializable]
    public class TargetRecord
    {
        public Vector2 position;                           

        public TargetRecord(Vector2 position)
        {
            this.position = position;
        }
    }

    private TargetRecord[] _records = null;            
    private float _t = 0f;
    private int _i = 0;                                
    private int _j = 1;                                
    private float _interval = 0f;
    private TargetRecord _record = null;            
    private bool _recording = true;                   
    private int _arraySize = 1;

    public void Start()
    {
        Initialize();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        respawnPoint = initialPosition;
    }

    public void Initialize()
    {
        if (targetPositionOnStart)
            transform.position = target.position;

        _interval = 1 / recordFPS;

        _arraySize = Mathf.CeilToInt(recordFPS * reactDelay);
        if (_arraySize == 0)
            _arraySize = 1;

        _records = new TargetRecord[_arraySize];
    }

    void Update()
    {     
        if (Time.timeScale > 0)
        {
            horizontal = Input.GetAxis("Horizontal");
            if (horizontal < 0.0f)
            {
                transform.localScale = new Vector2(-0.7f, 0.7f);
            }
            else if (horizontal > 0.0f)
            {
                transform.localScale = new Vector2(0.7f, 0.7f);
            }
            animator.SetBool("isRunning", horizontal != 0.0f);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                StartCoroutine(TimeDelayJump());
            }

            if (!isGrounded)
            {
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }

            if (Input.GetKeyDown(KeyCode.S) && destinyWarp)
            {
                transform.position = destinyWarp.transform.position;
            }

            DeathOnFall();
        }
      
    }

    public void LateUpdate()
    {
        if (start)
        {            
            RecordData(Time.deltaTime);
          
            if (targetDistance <= 0f)
            {
                if (_record != null)
                    transform.position = Vector2.MoveTowards(transform.position, _record.position, Time.deltaTime * followSpeed);
            }
            else if ((target.position - transform.position).magnitude > targetDistance)
            {
                if (!_recording)
                {
                    ResetRecordArray();
                    _recording = true;
                }

                if (_record != null)
                    transform.position = Vector2.MoveTowards(transform.position, _record.position, Time.deltaTime * followSpeed);
            }
            else if (targetDistanceY && Mathf.Abs(target.position.y - transform.position.y) > 0.05f)
            {
                if (_record != null)
                    transform.position = Vector2.Lerp(transform.position, new Vector3(transform.position.x, target.position.y, transform.position.z), Time.deltaTime * followSpeed);
            }
            else
            {
                _recording = false;
            }
        }
    }

    public void Death()
    {
        transform.position = initialPosition;
        respawnPoint = initialPosition;
        if (life < 0)
        {
            StartCoroutine(retrasoEscena("Lose"));
        }
    }

    public void Hit(float knockback, GameObject enemy)
    {
        if (!isInCooldown)
        {
            StartCoroutine(cooldown());
            if (life >= 0)
            {
                lifesPanel.transform.GetChild(life).gameObject.SetActive(false);
                life -= 1;
                if (enemy)
                {
                    Vector2 difference = (transform.position - enemy.transform.position);
                    float knockbackDirection = difference.x >= 0 ? 1 : -1;
                    rigidBody2D.velocity = new Vector2(knockbackDirection * knockback, knockback);
                }
            }
            else
            {
                Death();
            }
        }
    }

    IEnumerator TimeDelayJump()
    {
        yield return new WaitForSeconds(0.1f);

        Jump();
    }

    IEnumerator cooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isInCooldown = false;
    }

    IEnumerator retrasoEscena(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    private void Jump()
    {
        rigidBody2D.AddForce(Vector2.up * jumpForce);
    }

    private void DeathOnFall()
    {
        if (transform.position.y < -25f)
        {
            transform.position = respawnPoint;
            Hit(0, null);
        }
    }

    private void FixedUpdate()
    {
        if (!isInCooldown)
        {
            rigidBody2D.velocity = new Vector2(horizontal, rigidBody2D.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Tilemap")
        {
            isGrounded = true;
        }

        if (collider.name == "FallingPlatform")
        {
            isGrounded = true;
        }

        if (collider.name == "warpA" || collider.name == "warpB")
        {
            GameObject warp = collider.transform.parent.gameObject;
            if (collider.name == "warpA")
            {
                destinyWarp = warp.transform.Find("warpB").gameObject;
            }
            else
            {
                destinyWarp = warp.transform.Find("warpA").gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Tilemap")
        {
            isGrounded = false;
        }
        if (collider.name == "warpA" || collider.name == "warpB")
        {
            destinyWarp = null;
        }
    }

    private void RecordData(float deltaTime)
    {
        if (!_recording)
            return;
    
        if (_t < _interval)
        {
            _t += deltaTime;
        }
       
        else
        {           
            _records[_i] = new TargetRecord(target.position);
           
            if (_i < _records.Length - 1)
                _i++;
            else
                _i = 0;

            if (_j < _records.Length - 1)
                _j++;
            else
                _j = 0;

            _record = _records[_j];

            _t = 0f;
        }
    }

    private void ResetRecordArray()
    {
        _i = 0;
        _j = 1;
        _t = 0f;

        _records = new TargetRecord[_arraySize];

        for (int i = 0; i < _records.Length; i++)
        {
            _records[i] = new TargetRecord(transform.position);
        }

        _record = _records[_j];
    }

    public TargetRecord currentRecord
    {
        get
        {
            return _record;
        }
    }

    #if UNITY_EDITOR
     public void OnDrawGizmos()
     {
         if(gizmo)
         {
             if(_records == null || _records.Length < 2)
                 return;
             
             Gizmos.color = Color.red;
             for(int i = 0; i < _i-1; i++)
             {
                 if(_records[i] != null && _records[i+1] != null)
                     Gizmos.DrawLine(_records[i].position, _records[i+1].position);
             }
             
             //Gizmos.color = Color.green;
             for(int j = _j; j < _records.Length-1; j++)
             {
                 if(_records[j] != null && _records[j+1] != null)
                     Gizmos.DrawLine(_records[j].position, _records[j+1].position);
             }
             
             //Gizmos.color = Color.yellow;
             if(_records[0] != null && _records[_records.Length-1] != null)
                 Gizmos.DrawLine(_records[0].position, _records[_records.Length-1].position);
             
             Gizmos.color = Color.white;
             if(_record != null)
                 Gizmos.DrawLine(_record.position, transform.position);
         }
     }    
    #endif
}
