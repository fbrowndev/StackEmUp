using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField] private float _blockSpeed = 3f;

    private bool _isMoving = true;
    private Rigidbody2D _rb;

    //Variables for detecting screen size
    private float _screenEdgeX;

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
}
