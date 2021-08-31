using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomBullet : MonoBehaviour
{
    public Transform target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform projectile;
    public Transform myTransform;

    private void Awake()
    {
        projectile = transform;
        myTransform = transform;
    }

    private void Start()
    {
        StartCoroutine(ParabolaShoot());
    }

    public IEnumerator ParabolaShoot()
    {
        projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        float targetDistance = Vector3.Distance(projectile.position, target.position);

        float projectileVelocity = targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = targetDistance / Vx;

        projectile.rotation = Quaternion.LookRotation(target.position - projectile.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
}
