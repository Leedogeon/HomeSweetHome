using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject[] SpawnPoint;
    public GameObject[] PortalPoint;
    [SerializeField] private CameraChange cameraChange;
    [SerializeField] private BossControl boss;
    [SerializeField] private UI ui;
    private void Start()
    {
        cameraChange = FindObjectOfType<CameraChange>();
        boss = FindObjectOfType<BossControl>();
        ui = FindObjectOfType<UI>();
    }
    public Vector2 NextMap(GameObject portal)
    {
        int nextIndex = Array.IndexOf(PortalPoint, portal) + 1;
        if (nextIndex == PortalPoint.Length)
        {
            cameraChange.ChangeMain();
            /*return portal.transform.position;*/
            return SpawnPoint[0].transform.position;
        }
        else
        {
            if (nextIndex == PortalPoint.Length - 1)
            {
                cameraChange.ChangeBoss();
                ui.CamChange("Boss");
                boss.BossStart = true;

            }
            else cameraChange.BorderChange(nextIndex);
            return SpawnPoint[nextIndex].transform.position;
        }

        
    }
    public Vector2 PrevMap(GameObject spawn)
    {
        int prevIndex = Array.IndexOf(SpawnPoint, spawn) -1;
        if (prevIndex == -1)
        {
            return SpawnPoint[0].transform.position;
        }
        else
        {
            if (prevIndex == PortalPoint.Length-2)
            {
                cameraChange.ChangeMain();
                ui.CamChange("Main");
                boss.BossStart = false;
            }
            cameraChange.BorderChange(prevIndex);
            return PortalPoint[prevIndex].transform.position;
        }
    }

    public void TP()
    {
        cameraChange.BorderChange(4);
    }

}
