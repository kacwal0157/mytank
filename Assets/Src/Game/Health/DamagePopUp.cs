using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{

    [SerializeField]
    private Transform pfDamagePopUp;

    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    //CREATE DAMAGE POPUP
    public DamagePopUp Create(Vector3 position, float damageAmount, bool isCriticalHit)
    {
        Transform damagePopUpTransform = Instantiate(pfDamagePopUp, position, Quaternion.identity);

        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount, isCriticalHit);

        return damagePopUp;
    }

    public void Setup(float damageAmount, bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString("F0"));
        if(!isCriticalHit)
        {
            //normal hit
            textMesh.fontSize = 16;
            textColor = Color.white;
        }
        else
        {
            textMesh.fontSize = 24;
            textColor = Color.red;
        }
        
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        moveVector = new Vector3(.7f, .1f) * 60f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            //first half of the popup
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        
        
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            //START DISAPPEARING
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
