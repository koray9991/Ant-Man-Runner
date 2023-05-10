using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PushingObject : MonoBehaviour
{
    public TextMeshPro healthText;
    public float maxHealth, currentHealth;
    public float timer;
    Player player;
    public MeshRenderer other1, other2;
    public Material transparentMat;
    public bool matBool;
    GameManager gm;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentHealth = maxHealth;
        healthText.text = currentHealth + "/" + maxHealth;
        GetComponent<PushingObject>().enabled = false;
    }
    private void Update()
    {
        if (!matBool)
        {
            other1.material = transparentMat; other2.material = transparentMat;
            matBool = true;
        }
        timer += Time.deltaTime;
        if (timer > 0.1f && player.pushing)
        {
            timer = 0;
            currentHealth -= 1;
            healthText.text = currentHealth + "/" + maxHealth;
        }
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            player.PushingIsOver();
            gm= GameObject.FindObjectOfType<GameManager>();
            gm.pushingCam.Priority = 0;
        }
    }



}
