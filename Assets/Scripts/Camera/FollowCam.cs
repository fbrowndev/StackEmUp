using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    #region Variables
    [Header("Camera Settings")]
    [SerializeField] private Transform _targetBlock;
    [SerializeField] private float _smoothSpeed = 2f;
    [SerializeField] private float _verticalOffset = 3f;

    private float _startY; //initial camera position

    #endregion

    void Start()
    {
        _startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(_targetBlock != null)
        {
            float targetHeight = _targetBlock.position.y + _verticalOffset;

            if(targetHeight > _startY)
            {
                Vector3 newPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPosition, _smoothSpeed * Time.deltaTime);
            }
        }
    }

    public void UpdateTargetBlock(Transform newTarget)
    {
        _targetBlock = newTarget;
    }
}
