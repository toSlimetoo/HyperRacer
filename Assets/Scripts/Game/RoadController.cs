using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    
    [SerializeField] private GameObject[] gasObjects;
    [SerializeField] private GameObject[] enemyObjects;

    private void OnEnable()
    {
        
    }


    private void Start()
    {
        foreach (var gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }

        foreach (var enemyObject in enemyObjects)
        {
            enemyObject.SetActive(false);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SpawnRoad(transform.position+new Vector3(0,0,10));
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DestroyRoad(gameObject);
            foreach (var gasObject in gasObjects)
            {
                gasObject.SetActive(false);
            }
            foreach (var enemyObject in enemyObjects)
            {
                enemyObject.SetActive(false);
            }
            
        }
        
    }

    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }

    public void SpawnEnemy()
    {
        int index = Random.Range(0, gasObjects.Length);
        enemyObjects[index].SetActive(true);
        
    }
    
    
}
