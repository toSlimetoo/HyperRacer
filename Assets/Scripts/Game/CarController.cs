using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField]private int gas = 1000;
    [SerializeField]private float moveSpeed = 1f;
    
    public int Gas
    { get => gas; }

    void Start()
    {
        StartCoroutine(GasCoroutine());

    }

    IEnumerator GasCoroutine()
    {
        while (true)
        {
            gas -= 10;
            
            
            yield return new WaitForSeconds(1f);
            if(gas <= 0)
                break;
            
        }
        GameManager.Instance.EndGame();
        
    }
    
    
    public void Move(float direction)
    {
        
        
            transform.Translate(Vector3.right * (direction * Time.deltaTime));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x,-1.5f,1.5f), 0, transform.position.z);
            
            
            
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gas"))
        {
            gas += 30;

            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            gas -= 100;
            
            other.gameObject.SetActive(false);
        }
        
    }
}
