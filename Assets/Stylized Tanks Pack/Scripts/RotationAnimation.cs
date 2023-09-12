
using UnityEngine;

namespace ModularTanksPack
{
    public class RotationAnimation : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotationAxis;
        [SerializeField]
        private float rotationSpeed;

        private void Update()
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}
