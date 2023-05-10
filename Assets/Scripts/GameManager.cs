using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Player player;
    public CinemachineVirtualCamera vCam;
    public CinemachineVirtualCamera pushingCam;
    CinemachineTransposer transposer;
    public Image scaleImage;
    public Vector3 minCamValues;
    public Vector3 normalCamValues;
    public Vector3 maxCamValues;
    public AudioClip wallBraeak,tank,window,car,electric,big,small,walk,rush,jump;
    public ParticleSystem rushParticle;
    public TextMeshProUGUI levelText,levelToText;
    public GameObject restartPanel,winPanel;
    public GameObject finishConfetti;
    private void Start()
    {
        //Application.targetFrameRate = 5;
        transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        VcamTransposerNormal();
        if(levelText != null)
        {
            levelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
            levelToText.text = (SceneManager.GetActiveScene().buildIndex + 2).ToString();
        }
    }
    public void WinLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex== SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void VcamTransposerMin()
    {
        DOTween.To(x => transposer.m_FollowOffset.y = x, transposer.m_FollowOffset.y, minCamValues.y, 1f);
        DOTween.To(x => transposer.m_FollowOffset.z = x, transposer.m_FollowOffset.z, minCamValues.z, 1f);
    }
    public void VcamTransposerNormal()
    {
        DOTween.To(x => transposer.m_FollowOffset.y = x, transposer.m_FollowOffset.y, normalCamValues.y, 1f);
        DOTween.To(x => transposer.m_FollowOffset.z = x, transposer.m_FollowOffset.z, normalCamValues.z, 1f);
    }
    public void VcamTransposerMax()
    {
        DOTween.To(x => transposer.m_FollowOffset.y = x, transposer.m_FollowOffset.y, maxCamValues.y, 1f);
        DOTween.To(x => transposer.m_FollowOffset.z = x, transposer.m_FollowOffset.z, maxCamValues.z, 1f);
    }
    public void ScaleImageChange()
    {
        scaleImage.fillAmount = player.currentScale / (player.maxScale );
    }
    public void WallBreakAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(wallBraeak);
    }
    public void TankAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(tank);
    }
    public void WindowAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(window);
    }
    public void CarAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(car);
    }
    public void Electric()
    {
        GetComponent<AudioSource>().PlayOneShot(electric);
    }
    public void Big()
    {
        GetComponent<AudioSource>().PlayOneShot(big);
    }
    public void Small()
    {
        GetComponent<AudioSource>().PlayOneShot(small);
    }
    public void Rush()
    {
        GetComponent<AudioSource>().PlayOneShot(rush);
    }
    public void Jump()
    {
        GetComponent<AudioSource>().PlayOneShot(jump);
    }
}
