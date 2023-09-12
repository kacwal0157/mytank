using MyTankWeaponSystem;
using System.Collections;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    GameObject deathEffect;

    [HideInInspector]
    public GameObject activePlayer;

    [HideInInspector]
    public bool afterBulletExplosion = false;

    private MissileService missileService;
    private GameManager.Stage previousStage;
    private PublicGameObjects publicGameObjects;

    private SphereCollider triggerSphereCollider;
    private GameObject triggerBallRadius;
    private Rigidbody rigidB;
    private GameObject explosionEffect;
    private Collision collisionObject;

    private Vector3 canonBallTouchPosition;
    private Vector3 enemyPosition;
    private Vector3 playerPosition;

    private bool runCoroutine = true;
    private bool inWindZone = false;
   
    private void Awake()
    {
        //TODO: TRY TO FIND BETTER WAY THAN "GAMEOBJECT.FIND" FOR FINDING GAMEOBJECTS/COMPONENTS
        publicGameObjects = GameObject.Find("PublicGameObjects").GetComponent<PublicGameObjects>();
        explosionEffect = publicGameObjects.smallExplosionParticle;

        triggerSphereCollider = this.gameObject.AddComponent<SphereCollider>();
        triggerSphereCollider.isTrigger = true;

        triggerBallRadius = GameObject.Find("PlayerConfiguration");
        PlayerConfigurationService playerConfiguration = triggerBallRadius.GetComponent<PlayerConfigurationService>();
        triggerSphereCollider.radius = playerConfiguration.triggerBallSize;

        rigidB = GetComponent<Rigidbody>();
        previousStage = GameManager.currentStage;

        missileService = new MissileService(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.GetInstanceID() + " : collides with: " + collision.gameObject.name + " afterBulletExplosion " + afterBulletExplosion + " stage: " + previousStage);

        collisionObject = collision;
        if (!collision.gameObject.GetInstanceID().Equals(activePlayer.GetInstanceID()) && !afterBulletExplosion)
        {
            afterBulletExplosion = true;

            if (afterBulletExplosion)
            {
                if ((collision.transform.gameObject.tag == "enemyCollider" || collision.transform.gameObject.tag == "canonBase" || collision.transform.gameObject.tag == "turret")
                    && previousStage == GameManager.Stage.PLAYER_TURN_SHOOTING)
                {
                    canonBallTouchPosition = this.gameObject.transform.position;
                    enemyPosition = collision.transform.parent.position;

                    float diff = Vector3.Distance(canonBallTouchPosition, enemyPosition);
                    Debug.Log("TAKE DAMAGE");
                    EnemiesHealthManager.takeDamage(diff, triggerSphereCollider.radius, collision.transform.parent.gameObject, publicGameObjects.listOfPlayersContent);
                    afterBulletExplosion = false;
                }

                if (collision.gameObject.tag == "Player" && previousStage == GameManager.Stage.OPONNENT_TURN_SHOOTING)
                {
                    canonBallTouchPosition = this.gameObject.transform.position;
                    playerPosition = collision.transform.position;

                    float diff = Vector3.Distance(canonBallTouchPosition, playerPosition);
                    EnemiesHealthManager.takeDamage(diff, triggerSphereCollider.radius, collision.transform.gameObject, publicGameObjects.listOfPlayersContent);
                    afterBulletExplosion = false;
                }

                if (runCoroutine)
                {
                    StartCoroutine(destroyBulletAfterTime(0));
                    runCoroutine = false;
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }

        if(collision.gameObject.CompareTag("Object"))
        {
            collision.gameObject.SetActive(false);
            collision.gameObject.transform.parent.transform.GetChild(1).localScale = collision.transform.localScale;
            collision.gameObject.transform.parent.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if(inWindZone)
        {
            rigidB.AddForce(WindController.windDirection * WindController.windStrengh);
        }

        if (TargetInterface.shootingMode.Equals(TargetInterface.SHOOTING_MODE.MINI_ROCKET))
        {
            missileService.onFixedUpdate();
        }
    }

    private void Update()
    {
        if(TargetInterface.shootingMode.Equals(TargetInterface.SHOOTING_MODE.MINI_ROCKET))
        {
            missileService.onUpdate();
        } 
    }

    private void OnTriggerStay(Collider other)
    {
       // Debug.Log(this.GetInstanceID() + " : collides with: " + other.gameObject.name);
        if (other.gameObject.tag == "WindArea")
        {
            inWindZone = true;
        }

        
    }

    IEnumerator destroyBulletAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Camera.main.transform.parent = GameObject.FindGameObjectWithTag("Camera").transform;

        Instantiate(deathEffect, transform.position, Quaternion.LookRotation(collisionObject.contacts[0].normal));
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void IgnoreSelf(GameObject owner)
    {
        if (GetComponent<Collider>() && owner)
        {
            if (owner.GetComponent<Collider>())
                Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>());

            if (owner.transform.root)
            {
                foreach (Collider col in owner.transform.root.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(GetComponent<Collider>(), col);
                }
            }
        }
    }

    public void setTargetPoint(Vector3 targetPoint)
    {
        missileService.targetPoint = targetPoint;
    }
}