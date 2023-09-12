using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemiesHealthManager
{
    public static Dictionary<int, float> enemiesHealthDict = new Dictionary<int, float>();
    //public static Dictionary<int, HealthBar> enemiesHealthBarDict = new Dictionary<int, HealthBar>();
    public static Dictionary<int, EnemyHealth> enemyHealthScriptDict = new Dictionary<int, EnemyHealth>();

    private static DamagePopUp damagePopUp;
    private static float damage;
    private static bool criticalHit = false;

    public EnemiesHealthManager(PublicGameObjects publicGameObjects)
    {
        damagePopUp = publicGameObjects.damagePopUp.GetComponent<DamagePopUp>();
    }

    public static void takeDamage(float dif, float bulletRadius, GameObject enemy, GameObject playersListContent)
    {
        float difAbs = Mathf.Abs(dif);

        if (difAbs < bulletRadius)
        {
            float damageFactor = (bulletRadius - difAbs);
            damage = Random.Range(damageFactor * 90f, damageFactor * 110f);
            criticalHit = damage > 100f ? true : false;
        }

        Vector3 newDamagePopUpPosition = new Vector3(enemy.transform.position.x, 4f, enemy.transform.position.z);
        damagePopUp.Create(newDamagePopUpPosition, damage, criticalHit);

        foreach(Transform t in playersListContent.transform)
        {
            var health = t.GetChild(3).transform.GetChild(0).GetComponent<Slider>().value;
            if (t.name.Equals(enemy.name) && health - damage > 0f)
            {
                t.GetChild(3).transform.GetChild(0).GetComponent<Slider>().value -= damage;
            }
            
            if(t.name.Equals(enemy.name) && health - damage <= 0f)
            {
                t.GetChild(3).transform.GetChild(0).GetComponent<Slider>().value = 0f;
                t.GetComponent<Button>().interactable = false;
            }
        }

        enemyHealthScriptDict[enemy.GetInstanceID()].GetDamage((int)damage);

        Debug.Log("Damage: " + damage);
        enemiesHealthDict[enemy.GetInstanceID()] -= damage;
        //currentHealth = enemiesHealthDict[enemy.GetInstanceID()];
    }
}
