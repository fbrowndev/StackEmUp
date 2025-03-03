using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Block Components")]
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private float _nextBlockHeight = 1.5f;

    private bool _isFirstBlock = true;

    //[SerializeField] private Transform _spawnPoint;

    private GameObject _lastBlock;
    private FollowCam _followCam;

    // Start is called before the first frame update
    void Start()
    {
        _followCam = Camera.main.GetComponent<FollowCam>();
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Vector3 spawnPos;

        if(_isFirstBlock)
        {
            spawnPos = new Vector3(0, -.8f, 0);
        }
        else
        {
            spawnPos = _lastBlock.transform.position + new Vector3(0, _nextBlockHeight, 0);
        }

        _lastBlock = Instantiate(_blockPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = _lastBlock.GetComponent<Rigidbody2D>();
        if (_isFirstBlock)
        {
            rb.isKinematic = true; // Prevent falling automatically
            rb.gravityScale = 0;
            _isFirstBlock = false;  // Disable flag after first block is placed
        }

        _followCam.UpdateTargetBlock(_lastBlock.transform);
    }
}
