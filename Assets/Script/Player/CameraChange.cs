using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraChange : MonoBehaviour
{
    public GameObject MainCam;
    public GameObject BossCam;

    public CinemachineCamera Camera;

    public GameObject Boss;
    private void Start()
    {
        Camera = FindObjectOfType<CinemachineCamera>();
    }

    public void ChangeMain()
    {
        BossCam.SetActive(false);
        MainCam.SetActive(true);

    }

    public void ChangeBoss()
    {
        MainCam.SetActive(false);
        BossCam.SetActive(true);
    }

    public void BorderChange(int Index)
    {
        Camera.change(Index);
    }

    public void BossDeath()
    {
        StartCoroutine(End());
    }
    IEnumerator End()
    {
        float time = 2f;
        yield return new WaitForSeconds(time);
        SceneManager.LoadSceneAsync("End");
    }

}
