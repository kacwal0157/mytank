using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
    public class Damage : DamageBase
    {

        public bool Explosive;
        public float DamageRadius = 20;
        public bool RayChecker = false;
        public float ExplosionRadius = 20;
        public float ExplosionForce = 1000;
        public bool HitedActive = true;
        public float TimeActive = 0;
        private float timetemp = 0;
        private ObjectPool objPool;
        private Vector3 prevpos;

        private void Awake()
        {
            objPool = this.GetComponent<ObjectPool>();
        }


        private void OnEnable()
        {
            prevpos = this.transform.position - this.transform.forward;
            timetemp = Time.time;
        }

        private void Start()
        {

            if (!Owner || !Owner.GetComponent<Collider>())
                return;

            timetemp = Time.time;
        }

        private void Update()
        {
            if (objPool && !objPool.Active)
            {
                return;
            }

            float mag = Vector3.Distance(this.transform.position, prevpos);
            if (!RayChecker)
            {
                RaycastHit[] hits = Physics.RaycastAll(this.transform.position + (-this.transform.forward * mag), this.transform.forward, mag);
                foreach (RaycastHit hit in hits)
                {
                    Debug.Log("HIT NAME: " + hit.collider.name);
                    if (hit.collider.transform.root != this.transform.root && (Owner == null || hit.collider.transform.root != Owner.transform.root))
                    {
                        Debug.Log("##ACTIVE?");
                        Active(hit.point);
                        break;
                    }
                }
            }
            prevpos = this.transform.position;

            if (!HitedActive || TimeActive > 0)
            {
                if (Time.time >= (timetemp + TimeActive))
                {
                    Active(this.transform.position);
                }
            }
        }


        public void Active(Vector3 position)
        {
            if (Effect)
            {
                if (WeaponSystem.Pool != null)
                {
                    WeaponSystem.Pool.Instantiate(Effect, transform.position, transform.rotation, 3);
                }
                else
                {
                    GameObject obj = (GameObject)Instantiate(Effect, transform.position, transform.rotation);
                    Destroy(obj, 3);
                }
            }

            if (Explosive)
                ExplosionDamage();

            if (objPool)
            {
                objPool.OnDestroyed();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void ExplosionDamage()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Collider hit = hitColliders[i];
                if (!hit)
                    continue;

                if (hit.GetComponent<Rigidbody>())
                    hit.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 3.0f);

            }

            Collider[] dmhitColliders = Physics.OverlapSphere(transform.position, DamageRadius);

            for (int i = 0; i < dmhitColliders.Length; i++)
            {
                Collider hit = dmhitColliders[i];

                if (!hit)
                    continue;

                if (DoDamageCheck(hit.gameObject) && (Owner == null || (Owner != null && hit.gameObject != Owner.gameObject)))
                {
                    DamagePack damagePack = new DamagePack();
                    damagePack.Damage = Damage;
                    damagePack.Owner = Owner;
                    hit.gameObject.SendMessage("ApplyDamage", damagePack, SendMessageOptions.DontRequireReceiver);
                }
            }

        }

        private void NormalDamage(Collision collision)
        {
            DamagePack damagePack = new DamagePack();
            damagePack.Damage = Damage;
            damagePack.Owner = Owner;
            collision.gameObject.SendMessage("ApplyDamage", damagePack, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (objPool && !objPool.Active && WeaponSystem.Pool != null)
            {
                return;
            }

            if (HitedActive)
            {
                if (DoDamageCheck(collision.gameObject) && collision.gameObject.tag != this.gameObject.tag)
                {
                    if (!Explosive)
                        NormalDamage(collision);
                    Active(this.transform.position);
                }
            }
        }
    }
}