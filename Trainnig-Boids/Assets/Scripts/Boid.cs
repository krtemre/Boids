using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    SettingsBoid settings;

    //State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    public Vector3 velocity;

    //To update
    [HideInInspector]
    public Vector3 avgFlockDirection; //Flock heading vector
    [HideInInspector]
    public Vector3 avgAviodanceFlok; //Aviodince Obstacles
    [HideInInspector]
    public Vector3 centrerOfFlockmates; //Center of flock heading
    [HideInInspector]
    public int numPercievedFlockmates;

    //Cached
    Material material;
    Transform cachedTransform;
    Transform target;
    private void Awake()
    {
        material = this.gameObject.GetComponent<MeshRenderer>().material;
        cachedTransform = this.gameObject.transform;
    }

    public void Initialize(SettingsBoid settings, Transform target)
    {
        this.settings = settings;
        this.target = target;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.maxSpeed - settings.minSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void SetColor(Color color)
    {
        if(material != null)
        {
            material.color = color;
        }
    }

    public void UpdateBoid()
    {
        Vector3 accelerationn = Vector3.zero;

        if(target != null)
        {
            Vector3 ofsetToTarget = target.position - position;
            accelerationn = SteerTowards(ofsetToTarget) * settings.targetWeight;
        }

        if (numPercievedFlockmates != 0)
        {
            centrerOfFlockmates /= numPercievedFlockmates;
            Vector3 ofsetFlockmatecenter = (centrerOfFlockmates - position);

            var alignmetForce = SteerTowards(avgFlockDirection) * settings.alignWeight;
            var seperationForce = SteerTowards(avgAviodanceFlok) * settings.seperateWeight;
            var centerForce = SteerTowards(ofsetFlockmatecenter) * settings.cohesionWeight;

            accelerationn += alignmetForce;
            accelerationn += seperationForce;
            accelerationn += centerForce;
        }

        if (IsHeadForCollesion())
        {
            Vector3 collisionAviodDir = ObstacleRays();
            Vector3 collisionAviodForce = SteerTowards(collisionAviodDir) * settings.avoidCollisionWeight;
            accelerationn += collisionAviodForce;
        }

        velocity += accelerationn * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);

        cachedTransform.position += velocity * Time.deltaTime;
        cachedTransform.forward = dir;
        position = cachedTransform.position;
        forward = dir;
    }

    private Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for(int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(position, dir);

            if(!Physics.SphereCast(ray, settings.boundRadius, settings.avoidCollisionDistance, settings.mask))
            {
                return dir;
            }
        }

        return forward;
    }

    private bool IsHeadForCollesion()
    {
        RaycastHit hit;
        if(Physics.SphereCast(position, settings.boundRadius,forward, out hit, settings.avoidCollisionDistance, settings.mask))
        {
            return true;
        }

        return false;
    }

    private Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }
}
