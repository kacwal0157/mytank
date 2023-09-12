using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HWRWeaponSystem
{
	public class CameraShaker : MonoBehaviour
	{
		public float ShakeMult = 0.1f;
		public float MaxDistance = 100;
		public Vector3 PositionShaker;
		public Vector3 ShakeMagnitude;
		private bool enable = false;
		public int enableCounter = 0;
		private Vector3 positionTmp;

		void Start ()
		{
			CameraEffects.Shaker = this;
		}

		Vector3 forcePower;

		public void Shake (Vector3 power, Vector3 position)
		{
			PositionShaker = position;
			forcePower = -power;
		}

		void Update ()
		{
			forcePower = Vector3.Lerp (forcePower, Vector3.zero, Time.deltaTime * 5);	
			ShakeMagnitude = new Vector3 (Mathf.Cos (Time.time * 80) * forcePower.x, Mathf.Cos (Time.time * 80) * forcePower.y, Mathf.Cos (Time.time * 80) * forcePower.z);

			if(enable)
            {
				Camera.main.transform.position += (CameraEffects.Shaker.ShakeMagnitude * 0.2f);
				enableCounter++;
				if(enableCounter >=60)
                {
					enableCounter = 0;
					enable = false;
                }
			}
		}

		public void setEnable(bool enable)
        {
			this.enable = enable;
			enableCounter = 0;
        }
	}

	public static class CameraEffects
	{
		public static CameraShaker Shaker;
		private static Vector3 pow = Vector3.up;

		public static void Shake (Vector3 power, Vector3 position)
		{
			if (Shaker != null)
				Shaker.Shake (power, position);
		}

		public static void Shake(Vector3 position)
        {

			
			if (Shaker != null)
				Shaker.Shake(pow, position);

			if (pow.Equals(Vector3.up))
			{
				pow = Vector3.down;
			} else
            {
				pow = Vector3.up;
			}
		}
	}

}