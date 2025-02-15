using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Block Components")]
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Transform _spawnPoint;

    private GameObject _currentBlock;


    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Vector3 spawnPos = _spawnPoint.position + new Vector3(0, 0.5f, 0);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);

        _currentBlock = Instantiate(_blockPrefab, spawnPos, spawnRotation);

        Rigidbody2D rb = _currentBlock.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0;

        _currentBlock.GetComponent<Block>().ActivatePhysics();
    }
}
