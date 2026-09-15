using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CinemachineCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;

    public PolygonCollider2D[] borders;

    public void PlayerSearch(Transform PlayerPos)
    {
        virtualCam.Follow = PlayerPos;
    }

    public void change(int Index)
    {
        virtualCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = borders[Index];
    }


}
