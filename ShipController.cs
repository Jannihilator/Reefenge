using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NavMeshPlus.Components;

public class ShipController : MonoBehaviour
{
    [SerializeField] int health;
    int maxHealth;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float fireRange;
    [SerializeField] float bulletSpeed;
    [SerializeField] float attackSpeed;
    [SerializeField] int attackDamage;


    Animator animator;
    [SerializeField] Projectile bulletPrefab;

    SpawnShips spawnShips;
    Transform enemyTransform;
    [SerializeField] Transform Arrow;
    bool canFire = true;
    EnemyController enemyController;
    SpriteRenderer spriteRenderer;
    [SerializeField] FloatingSlider floatingSlider;

    [SerializeField] GameObject healPerk;
    [SerializeField] GameObject rangePerk;
    [SerializeField] GameObject scatterPerk;

    GameManager gameManager;

    AudioSource TankFire;
    AudioClip TankFireClip;
    AudioSource FastFire;
    AudioSource NormalFire;
    public int id;


    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.tag!="ShipTank"){
            fireRange=range;
        }
        maxHealth = health;
        floatingSlider.updateHealthBar(health, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyTransform = GameObject.Find("Enemy").GetComponent<Transform>();
        enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
        spawnShips = GameObject.Find("GameManager").GetComponent<SpawnShips>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        TankFire = GameObject.Find("TankFire").GetComponent<AudioSource>();
        NormalFire = GameObject.Find("NormalFire").GetComponent<AudioSource>();
        FastFire = GameObject.Find("FastFire").GetComponent<AudioSource>();
        TankFire.time=1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyTransform!=null){

            Vector3 gap = (enemyTransform.position - transform.position);
            if(gap.magnitude>range || gameObject.tag!="ShipTank"){
                gap = gap.normalized * range;
                transform.position = Vector3.MoveTowards(transform.position, enemyTransform.position - gap, speed * Time.deltaTime);
            }
            float angle = Mathf.Atan2(gap.y, gap.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if(transform.eulerAngles.z >90 && transform.eulerAngles.z<270){
                spriteRenderer.flipY = true;
            }else{
                spriteRenderer.flipY = false;
            }
            if(enemyController.health > 0 && canFire && Vector3.Distance(enemyTransform.position, transform.position)<fireRange){
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire(){
        if(gameObject.tag=="ShipTank"){
            TankFire.PlayOneShot(TankFire.clip);
        }
        if(gameObject.tag=="ShipNormal"){
            NormalFire.PlayOneShot(NormalFire.clip);
        }
        if(gameObject.tag=="ShipFast"){
            FastFire.PlayOneShot(FastFire.clip);
        }
        animator.SetBool("Fire", true);
        canFire = false;
        Vector2 shootDirection = enemyController.transform.position - transform.position;
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if(transform.eulerAngles.z >90 && transform.eulerAngles.z<270){
            spriteRenderer.flipY = true;
        }else{
            spriteRenderer.flipY = false;
        }
        var bullet = Instantiate(bulletPrefab, Arrow.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        bullet.Fire(bulletSpeed, bullet.transform.right, attackDamage);
        yield return new WaitForSeconds(attackSpeed);
        canFire = true;
        animator.SetBool("Fire", false);
    }

    public void Hit(int damage){
        health-=damage;
        floatingSlider.updateHealthBar(health, maxHealth);
        if(health<=maxHealth/2){
            floatingSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color32(212,47,58,255);
        }
        if(health<=0){
            gameObject.GetComponent<NavMeshModifier>().overrideArea = false;
            if(gameObject.tag=="ShipTank"&&!Variables.isTutorial){
                int i = Random.Range(0,gameManager.perkChance);
                if(i<7){
                    gameManager.perkList.Add(Instantiate(healPerk, transform.position, Quaternion.identity));
                }
                else if(i<9){
                    gameManager.perkList.Add(Instantiate(healPerk, transform.position, Quaternion.identity));
                    gameManager.perkList.Add(Instantiate(rangePerk, transform.position+Vector3.right*0.9f, Quaternion.identity));
                    gameManager.perkChance= Mathf.RoundToInt(gameManager.perkChance*1.5f);
                    
                    gameManager.rangeCount++;
                }else if(i<10){
                    gameManager.perkList.Add(Instantiate(healPerk, transform.position, Quaternion.identity));
                    gameManager.perkList.Add(Instantiate(scatterPerk, transform.position+Vector3.right*0.9f, Quaternion.identity));
                    gameManager.perkChance*=3;
                    gameManager.modifier*=0.6f;
                    gameManager.scatterCount++;
                }else{
                    gameManager.healCount-=1;
                }
                gameManager.healCount+=1;
            }
            Destroy(gameObject);


            
            // gameObject.GetComponent<BoxCollider2D>().enabled=false;
        }
    }

    void OnDestroy(){
        // Debug.Log("-----"+id+" killed-----");
        //     for(int k=0;k<spawnShips.shipList.Count;k++){
        //         Debug.Log("id: "+spawnShips.shipList[k].GetComponent<ShipController>().id);
        //     }
            if(gameManager.tutorialDestroy){
                return;
            }
            Debug.Log(gameManager.tutorialDestroy);
            spawnShips.shipList.RemoveAt(id);
            for(int j=id;j<spawnShips.shipList.Count;j++){
                spawnShips.shipList[j].GetComponent<ShipController>().id=j;
            }
            if(spawnShips.shipList.Count == 0 && gameManager.manaValue <2){
                gameManager.nextLevel();
            }
            // Debug.Log("-----Remained-----");
            // for(int k=0;k<spawnShips.shipList.Count;k++){
            //     Debug.Log("id: "+spawnShips.shipList[k].GetComponent<ShipController>().id);
            // }
    }
}
