using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTankWeaponSystem
{
    public class MissileService 
    {
        private Rigidbody rigidBody;
        private Transform transform;
        public Vector3 targetPoint = Vector3.zero;

        private float Damping = 8f;

        private Vector3 Noise = new Vector3(0.3f, 0.3f, 0.3f);

        public MissileService(GameObject bullet)
        {
            rigidBody = bullet.GetComponent<Rigidbody>();
            transform = bullet.transform;
        }
        public void onUpdate()
        {
            if(!targetPoint.Equals(Vector3.zero)) {
                Quaternion rotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
            }
        }

        public void onFixedUpdate()
        {
            rigidBody.velocity += new Vector3(Random.Range(-Noise.x, Noise.x), Random.Range(-Noise.y, Noise.y), Random.Range(-Noise.z, Noise.z));
        }
    }
}

