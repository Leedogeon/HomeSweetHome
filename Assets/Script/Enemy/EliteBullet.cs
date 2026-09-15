using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBullet : Bullet
{
    protected override void Start()
    {
        bulletSpeed = 40f;
        base.Start();
    }
}
