using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField]private int gas = 100;
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
            if(gas <= 0)
                break;
            
            yield return new WaitForSeconds(1f);
            
        }
        
        
    }
    
    
    public void Move(float direction)
    {
        transform.Translate(Vector3.right * (direction * Time.deltaTime));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,-2.0f,2.0f), 0, transform.position.z);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gas"))
        {
            gas += 30;

        }
    }
}
