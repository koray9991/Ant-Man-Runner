using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float minXforce, maxXforce, minYforce, maxYforce, minZforce, maxZforce;
    public ParticleSystem explosionParticle;
 public void Force()
    {
          GetComponent<Rigidbody>().AddForce(Random.Range(minXforce, maxXforce), Random.Range(minYforce, maxYforce), Random.Range(minZforce, maxZforce));
        var player = GameObject.FindGameObjectWithTag("Player");
        explosionParticle.Play();
        GetComponent<Rigidbody>().AddExplosionForce(400, player.transform.position, 5);
    }
}
