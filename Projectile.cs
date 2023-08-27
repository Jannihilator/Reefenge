using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D rb;
    int damage;
    
    SpawnShips spawnShips;
    public void Fire(float speed, Vector3 direction, int _damage) {
        damage = _damage;
        rb.velocity = direction * speed;
        Destroy(gameObject, 3);
        spawnShips = GameObject.Find("GameManager").GetComponent<SpawnShips>();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(gameObject.tag =="Projectile"&&(col.tag=="ShipFast" || col.tag=="ShipTank"|| col.tag=="ShipNormal")){
            Destroy(gameObject);
            col.GetComponent<ShipController>().Hit(damage);
        }
        if(col.tag=="Enemy" && gameObject.tag !="Projectile" ){
            col.GetComponent<EnemyController>().Hit(damage, gameObject.name);
            Destroy(gameObject);
            if(gameObject.tag=="ShipTankBullet"){
                // Debug.Log("tank Hit");
            }
        }
        if(col.tag=="Projectile"){
            if(gameObject.tag=="ShipTankBullet" || gameObject.tag =="Projectile"){
                return;
            }
            Destroy(gameObject);
        }
    }
}
