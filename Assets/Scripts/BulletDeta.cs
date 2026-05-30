using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "STG/BulletData")]
public class BulletData : ScriptableObject
{
    public Sprite sprite;
    public Color color = Color.white;
    public float speed = 10f;
    public int damage = 1;
    public float scale = 1f;
}