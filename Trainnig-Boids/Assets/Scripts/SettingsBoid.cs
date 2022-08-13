using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Settigns", menuName ="BoidSettings")]
public class SettingsBoid : ScriptableObject
{
    public float maxSpeed = 5f;
    public float minSpeed = 1f;
    public float perceptionRadius = 2f;
    public float aviodanceRadius = 1f;
    public float maxSteerForce = 3f;

    public float alignWeight = 1f;
    public float cohesionWeight = 1f;
    public float seperateWeight = 1f;

    public float targetWeight = 1f;

    [Header("Collision")]
    public LayerMask mask;
    public float boundRadius = 0.3f;
    public float avoidCollisionWeight = 10f;
    public float avoidCollisionDistance = 4f;
}
