using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{

    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;
    public Slider slider;
    public Image fillImage;
    public DamagePopUp damagePopUp;
    public float playerHealth;

    private float currentHealth;
    private float damage;
    private bool isCriticalHit = false;

    private void Start()
    {
        currentHealth = playerHealth;
        slider.maxValue = currentHealth;
        SetHealth();
    }

    private void SetHealth()
    {
        slider.value = currentHealth;

        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / playerHealth);
    }

    public void TakeDamage(float dif, float bulletRadius)
    {
        float difAbs = Mathf.Abs(dif);

        if (difAbs < bulletRadius)
        {
            float damageFactor = (bulletRadius - difAbs);
            damage = Random.Range(damageFactor * 90f, damageFactor * 110f);
            if (damage > 100f)
            {
                isCriticalHit = true;
            }
            else
            {
                isCriticalHit = false;
            }
        }

        damagePopUp.Create(this.gameObject.transform.localPosition, damage, isCriticalHit);
        currentHealth -= damage;
        SetHealth();

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
