using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float bulletSpeed = 8;
    public float speed = 3;
    [SerializeField] Projectile bulletPrefab;
    [SerializeField] SpawnShips spawnShips;
    [SerializeField] Camera myCamera;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource fireSound;
    
    [SerializeField] Transform Arrow;
    public int health = 100;
    public int maxHealth=100;
    public int attackDamage = 10;
    public float attackSpeed = 0.4f;    
    [SerializeField] FloatingSlider floatingSlider;
    public float range = 12.5f;
    public int scatter = 1;
    [SerializeField] GameManager gameManager;
    float gameHeight;
    float gameWidth;
    bool canFire = true;
    bool isFiring = false;
    Vector3 targetPosition;
    Vector2 moveDirection;
    NavMeshAgent agent;
    SpriteRenderer spriteRenderer;
    List<int> scatterTarget;
    // Start is called before the first frame update
    void Start()
    {
        scatterTarget = new List<int>();
        maxHealth = health;
        floatingSlider.updateHealthBar(health, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        gameHeight = myCamera.orthographicSize * 2;
        gameWidth = gameHeight * myCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnShips.shipList.Count!=0 && canFire){
            isFiring = true;
            for(int i=0;i<scatter;i++){
                if(spawnShips.shipList.Count>i){
                    StartCoroutine(Fire(i));
                }
                
            }
            scatterTarget.Clear();
        }
        if(agent.remainingDistance<0.01f){
            setRandomWaypoint();
        }
        
        
    }

    

    IEnumerator Fire(int targetID){
        Vector2 shootDirection=Vector2.zero;
        if(!spawnShips.shipList[targetID]){
            yield break;
        }
        for(int i=targetID;i<spawnShips.shipList.Count;i++){
            shootDirection = spawnShips.shipList[i].transform.position - transform.position;
            if(shootDirection.magnitude<range && !scatterTarget.Contains(i)){
                scatterTarget.Add(i);
                break;
            }
            if(i==spawnShips.shipList.Count-1){
                yield break;
            }
        }
        fireSound.PlayOneShot(fireSound.clip);
        animator.SetBool("Fire", true);
        canFire = false;
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
        isFiring = false;
    }

    

    void setRandomWaypoint(){
        float x = Random.Range(-gameWidth/2, gameWidth/2);
        float y = Random.Range(-gameHeight/2, gameHeight/2);
        targetPosition = new Vector2(x, y);
        while(spawnShips.shipList.Count>1 && Vector2.Distance(targetPosition, spawnShips.shipList[spawnShips.shipList.Count-1].transform.position)<3){
            x = Random.Range(-gameWidth/2, gameWidth/2);
            y = Random.Range(-gameHeight/2, gameHeight/2);
            targetPosition = new Vector2(x, y);
        }

        moveDirection = targetPosition - transform.position;
        
        agent.SetDestination(targetPosition);
        if(!isFiring){
            if(transform.eulerAngles.z >90 && transform.eulerAngles.z<270){
                spriteRenderer.flipY = true;
            }else{
                spriteRenderer.flipY = false;
            }
            transform.right=moveDirection.normalized;
            Debug.Log("turn");
            // float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    

    public void Hit(int damage, string source){
        // Debug.Log(source+" hit: " + damage);
        health-=damage;
        updateHealth();
        if(health<=2*maxHealth/3){
            floatingSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color32(224,184,51,255);
        }else if(health<=maxHealth/3){
            floatingSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color32(212,47,58,255);
        }
        if(health<=0){
            gameManager.gameOver();
        }
    }

    public void updateHealth(){
        // Debug.Log(health + " / " + maxHealth);
        floatingSlider.updateHealthBar(health, maxHealth);
        if(health<=2*maxHealth/3){
            floatingSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color32(224,184,51,255);
        }else if(health<=maxHealth/3){
            floatingSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color32(212,47,58,255);
        }else{
            floatingSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = new Color32(147,189,47,255);

        }
    }

    // IEnumerator DelaySetRandomTarget(){
    //     float x = Random.Range(-gameWidth/2, gameWidth/2);
    //     float y = Random.Range(-gameHeight/2, gameHeight/2);
    //     targetPosition = new Vector2(x, y);
    //     yield return new WaitForSeconds(0.3f);
    //     canRoam = true;
    //     Vector3 v = (targetPosition - transform.position).normalized;
    //     transform.up = v;
    // }
    
    // void RandomRoam(){
    //     transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    // }
}

