using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class Player : MonoBehaviour
{
    public enum Directions
    {
        ZPlus,
        XPlus,
        XMinus
    }
    public Directions direction;
    public float currentScale=1;
    public float minScale, maxScale;
    [SerializeField] float XSpeed;
    public GameManager gm;
    public float rushSpeed, normalSpeed, slowSpeed;
    public float runSpeed;
    [SerializeField] float MaxX;
    float maxXstartValue = 1;
    public Vector3 MovementVector;
    private bool Go;
    public Animator anim;
    public Transform shadowParent;
    public List<GameObject> shadows;
    public bool isAlive = true;
   public bool pushing;
    float pushingTimer;
    public bool isJumping;
    public bool isRopeArea,isTubeArea;
    public PlayerChild playerChild;
    public bool win;
    public bool turning;
    public bool isHoldArea;
    
    public Transform camLookingObject;
    
    void Start()
    {
        runSpeed = normalSpeed;
        MovementVector = new Vector3(0, 0, runSpeed);
        anim=GetComponent<Animator>();
        for (int i = 0; i < shadowParent.childCount; i++)
        {
            shadows.Add(shadowParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < shadows.Count; i++)
        {
            shadows[i].gameObject.SetActive(false);
        }
        maxXstartValue = 1.5f;  
        MaxX = maxXstartValue;
      
    }

    private void Update()
    {
        shadowParent.position = playerChild.transform.position;
        shadowParent.rotation = transform.rotation;
      
    }
    void FixedUpdate()
    {
       
        Movement();
    }
   
    private void Movement()
    {


        
        if (isAlive)
        {
            if (!Go)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Go = true;
                    anim.SetBool("run", true);
                }
                return;
            }
            if (pushing)
            {
                MovementVector.x = 0;
                runSpeed = slowSpeed;
                
                pushingTimer += Time.deltaTime;
                if (pushingTimer > 0.3f)
                {
                    pushingTimer = 0;
                    currentScale -= 0.1f;
                    if (currentScale < minScale)
                    {
                        currentScale = minScale;
                        Dead();
                        pushing = false;

                    }
                    playerChild.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.3f).SetEase(Ease.InOutBack);
                    gm.ScaleImageChange();

                }
            }
            else if (isRopeArea || isTubeArea || isHoldArea || isJumping)
            {
                MovementVector.x = 0;
            }
            else
            {
                
                if (Input.GetMouseButton(0))
                {
                    MovementVector.x = Input.GetAxis("Mouse X") * XSpeed;
                }
                else
                {
                    MovementVector.x = 0;
                }

            }

            MovementVector = new Vector3(MovementVector.x, MovementVector.y, runSpeed);
            if (!turning)
            {
                transform.Translate(Time.deltaTime * new Vector3(0, 0, MovementVector.z));
            }
         
            playerChild.transform.Translate(Time.deltaTime * new Vector3(MovementVector.x, 0, 0));
            playerChild.transform.localPosition = new Vector3(Mathf.Clamp(playerChild.transform.localPosition.x, -MaxX, MaxX), 0, 0);
        }
    }

  
    public void Dead()
    {
        isAlive = false;
        anim.SetBool("dead", true);
        anim.SetBool("idle", false);
        anim.SetBool("run", false);
        anim.SetBool("push", false);
        camLookingObject.parent = null;
        gm.pushingCam.Priority = 0;
        gm.restartPanel.SetActive(true);
    }
    public void PushingIsOver()
    {
        pushing = false;
        anim.SetBool("run", true);
        anim.SetBool("push", false);
        runSpeed = normalSpeed;
    }
    public void ScaleChangeShadow()
    {
        for (int i = 0; i < shadows.Count; i++)
        {
            shadows[i].gameObject.SetActive(true);
            shadows[i].transform.localScale = playerChild.transform.localScale;
        }
        StartCoroutine(ShadowCoroutine());
        
    }
    IEnumerator ShadowCoroutine()
    {
        for (int i = 0; i < shadows.Count; i++)
        {
            yield return new WaitForSeconds(0.05f);
            shadows[i].transform.DOScale(Vector3.one * currentScale, 0.05f).OnComplete(() => shadows[i].SetActive(false));
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void ScaleChangeBigShadow()
    {
        
        StartCoroutine(ShadowBigCoroutine());
    }
    IEnumerator ShadowBigCoroutine()
    {
        for (int i = shadows.Count-1; i >= 0; i--)
        {
            shadows[i].gameObject.SetActive(true);
            shadows[i].transform.localScale = playerChild.transform.localScale;
            yield return new WaitForSeconds(0.04f);
            shadows[i].transform.DOScale(Vector3.one * currentScale, 0.04f);
            if (i == 0)
            {
                playerChild.transform.DOScale(new Vector3(currentScale, currentScale, currentScale), 0.1f).SetEase(Ease.InOutBack);
                for (int j = 0; j < shadows.Count; j++)
                {
                    shadows[j].gameObject.SetActive(false);
                }
            }
          
            
        }
    }

    public void CameraShake()
    {
        gm.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 5f;
        DOVirtual.DelayedCall(0.5f, () => {
            gm.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        });
    }
    public void Walk()
    {
      //  gm.GetComponent<AudioSource>().PlayOneShot(gm.walk);
    }
}