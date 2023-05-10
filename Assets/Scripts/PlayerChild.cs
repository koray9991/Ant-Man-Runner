using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerChild : MonoBehaviour
{
    public Player player;
    public GameManager gm;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Big")
        {

            if (player.currentScale == player.minScale)
            {
                gm.VcamTransposerNormal();
            }
            if (player.currentScale < player.maxScale)
            {
                player.currentScale += 0.2f;
                if (player.currentScale > player.maxScale)
                {
                    player.currentScale = player.maxScale;
                    gm.VcamTransposerMax();
                }
                for (int i = 0; i < player.shadows.Count; i++)
                {
                    player.shadows[i].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", Color.blue);
                    player.shadows[i].transform.GetChild(0).GetComponent<Outline>().OutlineColor = Color.blue;
                }
                player.ScaleChangeBigShadow();
                gm.ScaleImageChange();
                gm.Big();
            }
            other.gameObject.SetActive(false);  
        }
        if (other.tag == "Small")
        {
            if (player.currentScale == player.maxScale)
            {
                gm.VcamTransposerNormal();
            }
            if (player.currentScale > player.minScale)
            {
                player.currentScale -= 0.3f;
                if (player.currentScale < player.minScale)
                {
                    player.currentScale = player.minScale;
                    gm.VcamTransposerMin();
                }
                for (int i = 0; i < player.shadows.Count; i++)
                {
                    player.shadows[i].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", Color.red);
                    player.shadows[i].transform.GetChild(0).GetComponent<Outline>().OutlineColor = Color.red;
                }
                player.ScaleChangeShadow();
                transform.DOScale(new Vector3(player.currentScale, player.currentScale, player.currentScale), 0.3f).SetEase(Ease.InOutBack);
                gm.ScaleImageChange();
                gm.Small();
            }
            other.gameObject.SetActive(false);
            
        }
        if (other.tag == "Max")
        {
            if (player.currentScale != player.maxScale)
            {
                player.currentScale = player.maxScale;
                for (int i = 0; i < player.shadows.Count; i++)
                {
                    player.shadows[i].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", Color.blue);
                    player.shadows[i].transform.GetChild(0).GetComponent<Outline>().OutlineColor = Color.blue;
                }
                player.ScaleChangeBigShadow();
                gm.VcamTransposerMax();
                gm.Big();
                gm.ScaleImageChange();
            }
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Min")
        {
            if(player.currentScale != player.minScale)
            {
                player.currentScale = player.minScale;
                for (int i = 0; i < player.shadows.Count; i++)
                {
                    player.shadows[i].transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", Color.red);
                    player.shadows[i].transform.GetChild(0).GetComponent<Outline>().OutlineColor = Color.red;
                }
                player.ScaleChangeShadow();
                gm.VcamTransposerMin();
                transform.DOScale(new Vector3(player.currentScale, player.currentScale, player.currentScale), 0.3f).SetEase(Ease.InOutBack);
                gm.ScaleImageChange();
                gm.Small();
            }
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Wall")
        {
            if (player.currentScale >= 1)
            {
                other.GetComponent<MeshRenderer>().enabled = false;
                for (int i = other.transform.childCount - 1; i >= 0; i--)
                {
                    other.transform.GetChild(i).gameObject.SetActive(true);
                    other.transform.GetChild(i).parent = null;
                }
                player.CameraShake();
                if (other.GetComponent<Wall>().isWall)
                {
                    gm.WallBreakAudio();
                }
                if (other.GetComponent<Wall>().isWindow)
                {
                    gm.WindowAudio();
                }

            }
            else
            {
                player.Dead();
            }
        }
        if (other.tag == "Jump")
        {
            if (player.currentScale >= 1)
            {
                other.tag = "Untagged";
               
                player.isJumping = true;
                var y = player.transform.position.y;
                player.transform.DOLocalJump(other.GetComponent<Jump>().nextTarget.position, 2, 1, 1.5f).SetEase(Ease.InOutQuint);
                player.anim.SetBool("jump", true);
                player.anim.Play("Jump", -1, 0f);
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                player.anim.SetBool("run", false);
                gm.Jump();
            }
            else
            {
                DOVirtual.DelayedCall(0.2f, () => {
                    player.transform.DOMoveY(-3, 1);
                    player.Dead();
                });
                
            }
               
        }
        if (other.tag == "Tank")
        {
            other.transform.localScale = new Vector3(other.transform.localScale.x, other.transform.localScale.y, 2);
            other.GetComponent<Tank>().explosion.Play();
            player.CameraShake();
            gm.TankAudio();
        }
        if (other.tag == "Rush")
        {
            player.runSpeed = player.rushSpeed;
            player.anim.SetBool("rush", true);
            player.anim.SetBool("run", false);
            gm.rushParticle.gameObject.SetActive(true);
            gm.Rush();
        }
        if (other.tag == "RushOver")
        {
            player.runSpeed = player.normalSpeed;
            player.anim.SetBool("rush", false);
            player.anim.SetBool("run", true);
            gm.rushParticle.gameObject.SetActive(false);
                 
        }
        if (other.tag == "JumpOver")
        {
            
            player.isJumping = false;
           
            player.anim.SetBool("jump", false);


            player.anim.SetBool("run", true);
        }
        if (other.tag == "Respawn")
        {
           if(other.GetComponent<Respawn>().fall)
            {
                player.transform.DOMoveY(other.GetComponent<Respawn>().fallValue, other.GetComponent<Respawn>().fallTime);
            }
          
            player.Dead();
        }
        if (other.tag == "Win")
        {
            player.win = true;
        }
        if (other.tag == "Finish")
        {
            player.anim.SetBool("run", false);
            player.anim.SetBool("dance", true);
            player.runSpeed = 0;
            gm.winPanel.SetActive(true);
            gm.finishConfetti.SetActive(true);
        }
        if (other.tag == "Enemy")
        {
            if (player.currentScale >= 1.2f)
            {
               
                other.GetComponent<Enemy>().Force();
                player.CameraShake();
            }
            else if (player.currentScale < 1.5f && player.currentScale > 0.2f)
            {
                player.Dead();
            }
        }
        if (other.tag == "Fall")
        {
            player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime).SetEase(Ease.Linear);
        }
        if (other.tag == "Rope")
        {
            if (player.currentScale > 1)
            {
                DOVirtual.DelayedCall(0.5f, () => {
                    if (other.GetComponent<Fall>().isFail)
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().failFallValue, other.GetComponent<Fall>().failFallTime);
                        player.Dead();
                    }
                    else
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime);
                    }
                    for (int i = other.transform.childCount - 1; i >= 0; i--)
                    {
                        other.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                        other.transform.GetChild(i).gameObject.AddComponent<SphereCollider>();
                        other.transform.GetChild(i).parent = null;
                    }
                });
               
            }
            else
            {
                player.isRopeArea = true;
                if (player.direction == Player.Directions.ZPlus)
                {
                    transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                }
                if (player.direction == Player.Directions.XPlus || player.direction == Player.Directions.XMinus)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
                }
             
                player.anim.SetBool("rope", true);
                player.anim.SetBool("run", false);
                player.runSpeed = player.slowSpeed;
            }
            
        }
        if (other.tag == "RopeEnd")
        {
            player.isRopeArea = false;
            player.anim.SetBool("rope", false);
            player.anim.SetBool("run", true);
            player.runSpeed = player.normalSpeed;
        }
        if (other.tag == "Tube")
        {
            if (player.currentScale > other.GetComponent<Tube>().scale)
            {
                DOVirtual.DelayedCall(0.5f, () => {
                    if (other.GetComponent<Fall>().isFail)
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().failFallValue, other.GetComponent<Fall>().failFallTime);
                        player.Dead();
                    }
                    else
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime);
                    }
                    for (int i = other.transform.childCount - 1; i >= 0; i--)
                    {
                        other.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                        other.transform.GetChild(i).gameObject.AddComponent<SphereCollider>();
                        other.transform.GetChild(i).parent = null;
                    }
                });
                gm.WindowAudio();
            }
            else
            {
                player.isTubeArea = true;
                if (player.direction == Player.Directions.ZPlus)
                {
                    transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                }
                if (player.direction == Player.Directions.XPlus || player.direction == Player.Directions.XMinus)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
                }
            }

        }
        if (other.tag == "TubeEnd")
        {
            player.isTubeArea = false;
        }
        if (other.tag == "+X")
        {
            player.turning = true;
            player.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 90, 0), 0.6f).OnComplete(() => player.turning=false);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, other.transform.position.z);
            player.direction = Player.Directions.XPlus;
        }
        if (other.tag == "+Z")
        {
            player.turning = true;
            player.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.6f).OnComplete(() => player.turning = false);
            player.transform.position = new Vector3(other.transform.position.x, player.transform.position.y, player.transform.position.z);
            player.direction = Player.Directions.ZPlus;
        }
        if (other.tag == "-X")
        {
            player.turning = true;
            player.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, -90, 0), 0.6f).OnComplete(() => player.turning = false);
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, other.transform.position.z);
            player.direction = Player.Directions.XMinus;
        }
        if (other.tag == "Stair")
        {
            if (player.isAlive)
            {
                player.transform.DOMove(other.GetComponent<Stair>().StairEnd.position, other.GetComponent<Stair>().reachTime).SetEase(Ease.Linear).SetId("stair").OnComplete(() => Debug.Log("reach"));
            }
                
        }
        if (other.tag == "Electric")
        {
            player.Dead();
            gm.Electric();
        }
        if (other.tag == "Hold")
        {
            if (player.currentScale >= player.maxScale)
            {
                player.isHoldArea = true;
                player.anim.SetBool("hold", true);
                player.anim.SetBool("run", false);
                if (player.direction == Player.Directions.ZPlus)
                {
                    transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                }
                if (player.direction == Player.Directions.XPlus || player.direction == Player.Directions.XMinus)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
                }
             
            }
            else
            {
                DOVirtual.DelayedCall(0.5f, () => {
                    if (other.GetComponent<Fall>().isFail)
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().failFallValue, other.GetComponent<Fall>().failFallTime);
                        player.Dead();
                    }
                    else
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime);
                    }

                });
            }
            

        }
        if (other.tag == "HoldOver")
        {
            player.isHoldArea = false;
            player.anim.SetBool("hold", false);
            player.anim.SetBool("run", true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Window")
        {
            if (player.currentScale >= 1)
            {
                other.tag = "Untagged";
                other.GetComponent<MeshRenderer>().enabled = false;
                for (int i = other.transform.childCount - 1; i >= 0; i--)
                {
                    other.transform.GetChild(i).gameObject.SetActive(true);
                    other.transform.GetChild(i).parent = null;
                }
                if (other.GetComponent<Fall>().isFail)
                {
                    player.transform.DOMoveY(other.GetComponent<Fall>().failFallValue, other.GetComponent<Fall>().failFallTime);
                    player.Dead();
                }
                else
                {
                    player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime);
                }
                gm.WindowAudio();

            }
            else
            {
               
            }
        }
        if (other.tag == "Bridge")
        {
            if (player.currentScale >= 1)
            {
                other.tag = "Untagged";
                gm.WallBreakAudio();
                for (int i = other.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(other.transform.GetChild(i).gameObject, 2f);
                    other.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                    other.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                    other.transform.GetChild(i).parent = null;
                }
                if (other.GetComponent<Fall>().isFail)
                {
                    player.transform.DOMoveY(other.GetComponent<Fall>().failFallValue, other.GetComponent<Fall>().failFallTime);
                    player.Dead();
                }
                else
                {
                    player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime);
                }

            }
           
        }
        if (other.tag == "Stair")
        {
            if (player.isAlive)
            {
                if (player.currentScale > 1)
                {
                    if (other.GetComponent<Fall>().isFail)
                    {
                        player.Dead();
                        player.transform.DOMoveY(other.GetComponent<Fall>().failFallValue, other.GetComponent<Fall>().failFallTime);
                    }
                    else
                    {
                        player.transform.DOMoveY(other.GetComponent<Fall>().normalFallValue, other.GetComponent<Fall>().normalFallTime);
                    }
                    for (int i = 0; i < other.GetComponent<Stair>().meshes.transform.childCount; i++)
                    {
                        for (int j = 0; j < other.GetComponent<Stair>().meshes.transform.GetChild(i).childCount; j++)
                        {
                            other.GetComponent<Stair>().meshes.transform.GetChild(i).GetChild(j).gameObject.AddComponent<Rigidbody>();
                            other.GetComponent<Stair>().meshes.transform.GetChild(i).GetChild(j).gameObject.AddComponent<BoxCollider>();
                        }
                    }
                    other.tag = "Untagged";
                    DOTween.Kill("stair");
                    gm.WallBreakAudio();
                }
            }
           
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Push")
        {
            player.pushing = true;
            gm.pushingCam.Priority = 20;
            player.anim.SetBool("push", true);
            player.anim.SetBool("idle", false);
            player.anim.SetBool("run", false);
            collision.gameObject.GetComponent<PushingObject>().enabled = true;
            if (player.direction == Player.Directions.ZPlus)
            {
                collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                transform.position = new Vector3(collision.gameObject.transform.position.x, transform.position.y, transform.position.z);
            }
            if (player.direction == Player.Directions.XPlus || player.direction == Player.Directions.XMinus)
            {
                collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; ;
                transform.position = new Vector3(transform.position.x, transform.position.y, collision.gameObject.transform.position.z);
            }
          
        }
        if (collision.gameObject.tag == "Car")
        {
            if (player.currentScale >= 1.2f)
            {
                collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
                collision.gameObject.GetComponent<Car>().Force();
                player.CameraShake();
                gm.CarAudio();
            }
            else if (player.currentScale < 1.5f && player.currentScale > 0.2f)
            {
                player.Dead();
            }
        }   
    }
   
 
}
