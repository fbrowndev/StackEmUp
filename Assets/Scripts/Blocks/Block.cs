using System.Collections;
using System.Collections.Generic;
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

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _rb.gravityScale = 0; //Disabling gravity until dropped

        float _screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        _screenEdgeX = _screenHalfWidth - 0.5f; ;
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
        if(!_hasLanded && (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Tower_Base")))
        {
            _hasLanded = true;
            CheckPerfectStack(collision.gameObject);

            FindObjectOfType<BlockSpawner>().SpawnBlock();

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
            FindObjectOfType<GameManager>().AddScore(100);
        }
    }

    #endregion
}
