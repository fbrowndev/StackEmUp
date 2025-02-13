using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Block Components")]
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Transform _spawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        Instantiate(_blockPrefab, _spawnPoint.position, Quaternion.identity);
    }
}
