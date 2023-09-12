
using UnityEngine;

namespace ModularTanksPack
{
    public class TrackUVAnimation : MonoBehaviour
    {
        private readonly int MainTexturePropertyID = Shader.PropertyToID( "_MainTex_ST");
        private readonly int NormalMapPropertyID = Shader.PropertyToID( "_DetailNormalMap_ST");

        [SerializeField]
        private float speed;
        private MaterialPropertyBlock materialPropertyBlock;
        private Renderer m_Renderer;

        private Vector4 currentOffset;

        private void Awake()
        {
            currentOffset = new Vector4(1f, 1f, 0f, 0f);
            materialPropertyBlock = new MaterialPropertyBlock();
            m_Renderer = GetComponent<Renderer>();
            m_Renderer.GetPropertyBlock(materialPropertyBlock);
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        private void Update()
        {
            currentOffset.w = Mathf.Repeat(currentOffset.w + speed * Time.deltaTime, 1f);
            materialPropertyBlock.SetVector(MainTexturePropertyID, currentOffset);
            //materialPropertyBlock.SetVector(NormalMapPropertyName, currentOffset);
            m_Renderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
