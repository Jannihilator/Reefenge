using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StatBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text BtnText;
    [SerializeField] GameObject statsPanel;
    [SerializeField] SpawnShips spawnShips;
    [SerializeField] Stats stats;
    [SerializeField] GameObject enemyStatsPanel;
    [SerializeField] EnemyController enemy;
    bool isShowing = false;
    // public Texture2D cursorTexture;
    // public CursorMode cursorMode = CursorMode.Auto;
    // public Vector2 hotSpot = Vector2.zero;
    // Start is called before the first frame update

    void Start(){
        updateEnemyStats();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        spawnShips.isUI=true;
        // Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        spawnShips.isUI=false;
        // Cursor.SetCursor(null, Vector2.zero, cursorMode);

    }
    public void OnPointerClick(PointerEventData pointerEventData){
        if(!isShowing){
            BtnText.text = "hide stats";
            statsPanel.SetActive(true);
            enemyStatsPanel.SetActive(true);
        }else{
            BtnText.text = "show stats";
            statsPanel.SetActive(false);

            enemyStatsPanel.SetActive(false);
        }
        isShowing = !isShowing;
    }

    public void updateEnemyStats(){
        enemyStatsPanel.transform.GetChild(0).GetComponent<Text>().text="health | "+enemy.health.ToString();
        enemyStatsPanel.transform.GetChild(1).GetComponent<Text>().text="damage | "+enemy.attackDamage.ToString();
        enemyStatsPanel.transform.GetChild(2).GetComponent<Text>().text="rate | "+(1/enemy.attackSpeed).ToString().Substring(0, Mathf.Min(4, (1/enemy.attackSpeed).ToString().Length));
        enemyStatsPanel.transform.GetChild(3).GetComponent<Text>().text="range | "+enemy.range.ToString();
        enemyStatsPanel.transform.GetChild(4).GetComponent<Text>().text="proj spd | "+enemy.bulletSpeed.ToString();
        enemyStatsPanel.transform.GetChild(5).GetComponent<Text>().text="move spd | "+enemy.speed.ToString();
    }

    public void showStats(int id=0){
        switch(id){
            case 1:
                statsPanel.transform.GetChild(0).GetComponent<Text>().text="health | "+stats.tankShip.health.ToString();
                statsPanel.transform.GetChild(1).GetComponent<Text>().text="damage | "+stats.tankShip.damage.ToString();
                statsPanel.transform.GetChild(2).GetComponent<Text>().text="rate | "+(1/stats.tankShip.atkSpeed).ToString().Substring(0, Mathf.Min(4, (1/stats.tankShip.atkSpeed).ToString().Length));
                statsPanel.transform.GetChild(3).GetComponent<Text>().text="range | "+stats.tankShip.range.ToString();
                statsPanel.transform.GetChild(4).GetComponent<Text>().text="proj spd | "+stats.tankShip.bulletSpeed.ToString();
                statsPanel.transform.GetChild(5).GetComponent<Text>().text="move spd | "+stats.tankShip.speed.ToString();
                break;
            case 2:
                statsPanel.transform.GetChild(0).GetComponent<Text>().text="health | "+stats.normalShip.health.ToString();
                statsPanel.transform.GetChild(1).GetComponent<Text>().text="damage | "+stats.normalShip.damage.ToString();
                statsPanel.transform.GetChild(2).GetComponent<Text>().text="rate | "+(1/stats.normalShip.atkSpeed).ToString().Substring(0, Mathf.Min(4, (1/stats.normalShip.atkSpeed).ToString().Length ));
                statsPanel.transform.GetChild(3).GetComponent<Text>().text="range | "+stats.normalShip.range.ToString();
                statsPanel.transform.GetChild(4).GetComponent<Text>().text="proj spd | "+stats.normalShip.bulletSpeed.ToString();
                statsPanel.transform.GetChild(5).GetComponent<Text>().text="move spd | "+stats.normalShip.speed.ToString();
                break;
            case 3:
                statsPanel.transform.GetChild(0).GetComponent<Text>().text="health | "+stats.fastShip.health.ToString();
                statsPanel.transform.GetChild(1).GetComponent<Text>().text="damage | "+stats.fastShip.damage.ToString();
                statsPanel.transform.GetChild(2).GetComponent<Text>().text="rate | "+(1/stats.fastShip.atkSpeed).ToString().Substring(0, Mathf.Min(4,(1/stats.fastShip.atkSpeed).ToString().Length ));
                statsPanel.transform.GetChild(3).GetComponent<Text>().text="range | "+stats.fastShip.range.ToString();
                statsPanel.transform.GetChild(4).GetComponent<Text>().text="proj spd | "+stats.fastShip.bulletSpeed.ToString();
                statsPanel.transform.GetChild(5).GetComponent<Text>().text="move spd | "+stats.fastShip.speed.ToString();
                break;
        }
    }

    
}
