using Mirror.Examples.Tanks;
using UnityEngine;

public class NetProjectileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D projectileRb = GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = transform.forward * 1000f; // 10f é a velocidade do projétil
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
