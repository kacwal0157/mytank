using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public int health = 100;

    private Animation anim;
    private PublicGameObjects publicGameObjects;
    private GameObject destroyExplosionParticle;

	// Update is called once per frame
    void Start()
    {
        //anim = GetComponent<Animation>();
        publicGameObjects = GameObject.Find("PublicGameObjects").GetComponent<PublicGameObjects>();
        destroyExplosionParticle = publicGameObjects.bigExplosionParticle;
    }

	public void GetDamage(int damage)
	{
		health -= damage;

        if(health <= 0)
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<HealthBar>().HealthbarPrefab.gameObject.SetActive(false);

            Instantiate(destroyExplosionParticle, transform.position, Quaternion.identity);
        }
    }
}
