using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Block : MonoBehaviour
{
    #region Variables
    [Header("Block Settings")]
    [SerializeField] private float _blockSpeed = 3f;

    private bool _isMoving = true;
    private Rigidbody2D _rb;

    //Variables for detecting screen size
    private float _screenEdgeX;

    //stack variables
    private bool _hasLanded = false;
    private float _perfectStackThreshold = 0.1f; //Max offset for perfect stack

    //Enums
    public enum BlockType { Normal, Sticky, Bouncy, Heavy, Ghost, Explosive }
    public BlockType blockType = BlockType.Normal;

    //Ghost Block Fading
    private SpriteRenderer _spriteRenderer;
    private float fadeSpeed = 2f;
    private float minAlpha = 0.2f;
    private float maxAlpha = 0.8f;
    private float alphaDirection = 1f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _rb.gravityScale = 0; //Disabling gravity until dropped

        float _screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        _screenEdgeX = _screenHalfWidth - 0.5f;

        SetVisualStyle();


    }

    // Update is called once per frame
    void Update()
    {
        if(_isMoving)
        {
            MoveBlock();
        }

        if(IsScreenTapped() || Input.GetKeyDown(KeyCode.Space) && _isMoving)
        {
            DropBlock();
        }
    }

    #region Block Movement
    void MoveBlock()
    {
        float _moveAmount = _blockSpeed * Time.deltaTime;
        transform.position += new Vector3(_moveAmount, 0, 0);

        //change direction when reaching screen edges
        if (transform.position.x > _screenEdgeX || transform.position.x < -_screenEdgeX)
        {
            _blockSpeed = -_blockSpeed;
        }
    }

    void DropBlock()
    {
        _isMoving = false;
        _rb.isKinematic = false;
        _rb.gravityScale = 1;
    }

    public void ActivatePhysics()
    {
        _rb.isKinematic = true;
        _rb.gravityScale = 0;
    }

    #endregion

    #region Touch Methods
    bool IsScreenTapped()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    #endregion

    #region Stack Methods
    void OnCollisionEnter2D(Collision2D collision)
    {
        #region Special Behaviours
        void ApplySpecialBehavior()
        {
            switch (blockType)
            {
                case BlockType.Sticky:
                    var joint = gameObject.AddComponent<FixedJoint2D>();
                    joint.connectedBody = GetComponent<Collider2D>().attachedRigidbody;
                    break;
                case BlockType.Bouncy:
                    _rb.sharedMaterial = Resources.Load<PhysicsMaterial2D>("BouncyMaterial");
                    break;
                case BlockType.Heavy:
                    _rb.mass *= 2f;
                    break;
                case BlockType.Explosive:
                    //May Expand on this later
                    Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                    foreach (var obj in nearby)
                    {
                        if (obj.CompareTag("Block") && obj.gameObject != this.gameObject)
                        {
                            Destroy(obj.gameObject);
                        }
                    }
                    break;
                case BlockType.Ghost:
                    Color c = _spriteRenderer.color;
                    c.a += alphaDirection * fadeSpeed * Time.deltaTime;
                    if(c.a >= maxAlpha)
                    {
                        c.a = maxAlpha;
                        alphaDirection = -1f;
                    }
                    else if(c.a <= minAlpha)
                    {
                        c.a = minAlpha;
                        alphaDirection = 1f;
                    }

                    _spriteRenderer.color = c;
                    break;
            }
        }
        #endregion

        if (!_hasLanded && (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Tower_Base")))
        {
            _hasLanded = true;
            GameManager.Instance.AddScore(10);

            CheckPerfectStack(collision.gameObject);
            FindObjectOfType<BlockSpawner>().SpawnBlock();

            ApplySpecialBehavior();

            //if(!collision.gameObject.CompareTag("Tower_Base"))
            //{
            //    FindObjectOfType<BlockSpawner>().SpawnBlock();
            //}
        }
    }

    void CheckPerfectStack(GameObject belowBlock)
    {
        float xOffset = Mathf.Abs(transform.position.x - belowBlock.transform.position.x);

        if(xOffset <= _perfectStackThreshold)
        {
            Debug.Log("Perfect Stack!");
            GameManager.Instance.AddScore(100);
        }
    }

    public void SetVisualStyle()
    {
        var sr = GetComponent<SpriteRenderer>();
        switch (blockType)
        {
            case BlockType.Sticky: 
                sr.color = Color.green; 
                break;
            case BlockType.Bouncy: 
                sr.color = new Color(1f, 0.5f, 0f); 
                break;
            case BlockType.Heavy: 
                sr.color = Color.black; 
                break;
            case BlockType.Ghost: 
                sr.color = new Color(1, 1, 1, 0.5f); 
                break;
            case BlockType.Explosive: 
                sr.color = Color.red; 
                break;
            default: 
                sr.color = Color.white; 
                break;
        }
    }

    #endregion
}
