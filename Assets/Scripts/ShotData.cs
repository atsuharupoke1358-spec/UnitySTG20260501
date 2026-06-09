using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STG/ShotData")]
public class ShotData : ScriptableObject
{
    public ShotType shotType;
    public BulletData bulletData;
    public int shotCount = 5;
    public float spreadAngle = 10f;
    public float spiralSpeed = 10f;
    public float clusterRadius = 1f;
    public float fireRate = 1f;
}

public enum ShotType
{
    Aim,
    Spiral,
    ClusterAim,
    NWay,
}
