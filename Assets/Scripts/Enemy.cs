using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Enemy : MonoBehaviour
{
    bool isShooting;
    public Transform player;
    public GameObject particle;
    public float xForce, yForce, zForce;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (!isShooting)
        {
            if (Vector3.Distance(transform.position, player.position) < 10)
            {
                GetComponent<Animator>().SetBool("shoot", true);
                isShooting = true;
                particle.SetActive(true);
            }
        }
        
    }
    public void Force()
    {

        
            GetComponent<Animator>().enabled = false;

      
       
        
        GetComponent<BoxCollider>().enabled = false;
        
            transform.GetChild(0).GetComponent<Rigidbody>().AddForce(xForce, yForce, zForce);
       
       
    }
}
