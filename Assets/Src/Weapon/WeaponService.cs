using HWRWeaponSystem;
using UnityEngine;

namespace MyTankWeaponSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponService : MonoBehaviour
    {
        public bool OnActive;
        [Header("Aiming")]
        private bool isTargetSet = false;
        public Vector3 targetPosition;
        

        [Header("Projectile")]
        public Transform[] MissileOuter;
        public GameObject Missile;
		public int NumBullet = 1;
		public float Spread = 1;
		public float ForceShoot = 8000;

		[Header("Other FX")]
        public GameObject Shell;
        public float ShellLifeTime = 4;
        public Transform[] ShellOuter;
        public int ShellOutForce = 300;
        public GameObject Muzzle;
        public float MuzzleLifeTime = 2;
        public Vector3 ShakeForce = Vector3.up;

        [Header("Sound FX")]
        public AudioClip[] SoundGun;
        public AudioClip SoundReloading;
        public AudioClip SoundReloaded;

		[Header("UZI Torque object")]
		public GameObject TorqueObject;
		public Vector3 TorqueSpeedAxis;

		public bool RigidbodyProjectile;

		private GameObject Owner;
        private AudioSource audioSource;
		private Camera CurrentCamera;
		private Vector3 torqueTemp;
		// target enemy
		private GameObject target;
		private int currentOuter = 0;

		private void Start()
        {
            if (!Owner)
                Owner = this.transform.root.gameObject;

            if (!audioSource)
            {
                audioSource = this.GetComponent<AudioSource>();
                if (!audioSource)
                {
                    this.gameObject.AddComponent<AudioSource>();
                }
            }
        }

		private void Update()
		{
			if (CurrentCamera == null)
			{

				CurrentCamera = Camera.main;

				if (CurrentCamera == null)
					CurrentCamera = Camera.current;
			}
			if (OnActive)
			{


				if (TorqueObject)
				{
					TorqueObject.transform.Rotate(torqueTemp * Time.deltaTime);
					torqueTemp = Vector3.Lerp(torqueTemp, Vector3.zero, Time.deltaTime);
				}
				
				if (isTargetSet)
				{
					float targetdistance = Vector3.Distance(transform.position, target.transform.position);
					Vector3 dir = (target.transform.position - transform.position).normalized;
					float direction = Vector3.Dot(dir, transform.forward);

				}
			}
		}

		public void Shoot()
		{
			CameraEffects.Shake(ShakeForce, this.transform.position);
			torqueTemp = TorqueSpeedAxis;
			Vector3 missileposition = this.transform.position;
			Quaternion missilerotate = this.transform.rotation;
			if (MissileOuter.Length > 0)
			{
				missilerotate = MissileOuter[currentOuter].rotation;
				missileposition = MissileOuter[currentOuter].position;
			}

			if (MissileOuter.Length > 0)
			{
				currentOuter += 1;
				if (currentOuter >= MissileOuter.Length)
					currentOuter = 0;
			}

			if (Muzzle)
			{
				GameObject muzzle;
				if (WeaponSystem.Pool != null)
				{
					muzzle = WeaponSystem.Pool.Instantiate(Muzzle, missileposition, missilerotate, MuzzleLifeTime);
				}
				else
				{
					muzzle = (GameObject)GameObject.Instantiate(Muzzle, missileposition, missilerotate);
					GameObject.Destroy(muzzle, MuzzleLifeTime);
				}

				muzzle.transform.parent = this.transform;
				if (MissileOuter.Length > 0)
				{
					muzzle.transform.parent = MissileOuter[currentOuter].transform;
				}
			}

			for (int i = 0; i < NumBullet; i++)
			{
				if (Missile)
				{
					Vector3 spread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread)) / 100;
					Vector3 direction = this.transform.forward + spread;
					missilerotate = Quaternion.LookRotation(direction);

					GameObject bullet;
					if (WeaponSystem.Pool != null)
					{
						bullet = WeaponSystem.Pool.Instantiate(Missile, missileposition, missilerotate);
					}
					else
					{
						bullet = (GameObject)GameObject.Instantiate(Missile, missileposition, missilerotate);
					}
					if (bullet)
					{
						DamageBase damangeBase = bullet.GetComponent<DamageBase>();
						if (damangeBase)
						{
							damangeBase.Owner = Owner;
							damangeBase.IgnoreSelf(this.gameObject);
						}
						WeaponBase weaponBase = bullet.GetComponent<WeaponBase>();
						if (weaponBase)
						{
							weaponBase.Owner = Owner;
							weaponBase.Target = target;
						}
						if (RigidbodyProjectile)
						{
							if (bullet.GetComponent<Rigidbody>())
							{
								if (Owner != null && Owner.GetComponent<Rigidbody>())
								{
									bullet.GetComponent<Rigidbody>().velocity = Owner.GetComponent<Rigidbody>().velocity;
								}
								bullet.GetComponent<Rigidbody>().AddForce(direction * ForceShoot);
							}
						}
					}
					bullet.transform.forward = direction;
				}
			}
			if (Shell)
			{
				Transform shelloutpos = this.transform;
				if (ShellOuter.Length > 0)
				{
					shelloutpos = ShellOuter[currentOuter];
				}
				GameObject shell;
				if (WeaponSystem.Pool != null)
				{
					shell = WeaponSystem.Pool.Instantiate(Shell, shelloutpos.position, Random.rotation, ShellLifeTime);
				}
				else
				{
					shell = (GameObject)Instantiate(Shell, shelloutpos.position, Random.rotation);
					GameObject.Destroy(shell.gameObject, ShellLifeTime);
				}

				if (shell.GetComponent<Rigidbody>())
				{
					shell.GetComponent<Rigidbody>().AddForce(shelloutpos.forward * ShellOutForce);
				}
			}

			if (SoundGun.Length > 0)
			{
				if (audioSource)
				{
					audioSource.PlayOneShot(SoundGun[Random.Range(0, SoundGun.Length)]);

				}
			}
	}

	}
}


