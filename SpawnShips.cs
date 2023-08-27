using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;


public class SpawnShips : MonoBehaviour
{
    [SerializeField] GameObject shipTank;
    [SerializeField] GameObject shipNormal;
    [SerializeField] GameObject shipFast;
    [SerializeField] GameManager gameManager;
    [SerializeField] myManaBar manaBar;
    [SerializeField] Transform enemyTransform;
    [SerializeField] AudioSource spawnSound;
    [SerializeField] AudioSource delaySound;
    public bool isUI = false;
    public List<GameObject> shipList;
    NavMeshSurface surface2D;
    public int shipId=0;
    public bool selected= false;
    bool canSpawn = true;
    public int id=0;
    

    // Start is called before the first frame update
    void Start()
    {
        shipList = new List<GameObject>();
        surface2D = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        surface2D.BuildNavMeshAsync();
    }

    void resetCanSpawn(){
        canSpawn=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(isUI){
                return;
            }
            if(!selected || !canSpawn){
                delaySound.PlayOneShot(delaySound.clip);
                return;
            }
            
            // Debug.Log(Input.mousePosition);
            GameObject spawn = null;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if((Vector2.Distance(worldPosition,enemyTransform.position))<3.5){
                delaySound.PlayOneShot(delaySound.clip);

                return;
            }
            switch(shipId){
                case 1:
                    if(gameManager.manaValue<5){
                        break;
                    }
                    spawn = Instantiate(shipTank, new Vector3(worldPosition.x, worldPosition.y, 0) , Quaternion.identity);
                    gameManager.manaValue-=5;
                    manaBar.updateManaBar(5);
                    Invoke("resetCanSpawn", 1.5f*gameManager.modifier);
                    break;
                case 2:
                    if(gameManager.manaValue<3){
                        break;
                    }
                    spawn = Instantiate(shipNormal, new Vector3(worldPosition.x, worldPosition.y, 0) , Quaternion.identity);

                    gameManager.manaValue-=3;
                    manaBar.updateManaBar(3);
                    Invoke("resetCanSpawn", 1f*gameManager.modifier);
                    break;
                case 3:
                    if(gameManager.manaValue<2){
                        break;
                    }

                    spawn = Instantiate(shipFast, new Vector3(worldPosition.x, worldPosition.y, 0) , Quaternion.identity);
                    gameManager.manaValue-=2;

                    manaBar.updateManaBar(2);
                    Invoke("resetCanSpawn", 0.5f*gameManager.modifier);

                    break;
            }
            gameManager.UpdateManaUI();
            if(spawn){
                spawnSound.PlayOneShot(spawnSound.clip);
                canSpawn=false;
                spawn.GetComponent<ShipController>().id=shipList.Count;
                // Debug.Log(shipList.Count);
                spawn.transform.localScale *= 1.5f;
                shipList.Add(spawn);
            }else{
                delaySound.PlayOneShot(delaySound.clip);

            }
        }
        surface2D.UpdateNavMesh(surface2D.navMeshData);
    }
}
