using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public GameObject View;
    private void Start()
    {
        Vector3 camPos = new Vector3(View.transform.position.x, View.transform.position.y,-40);
        transform.position = camPos;
    }

}
