using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class GameManager : MonoBehaviour
{
    [SerializeField] Camera myCamera;
    [SerializeField] GameObject background;
    [SerializeField] Text manaValueText;
    [SerializeField] Text manaMaxText;
    [SerializeField] Text TankDelayText1;
    [SerializeField] Text TankDelayText2;
    [SerializeField] Text NormalDelayText1;
    [SerializeField] Text NormalDelayText2;
    [SerializeField] Text FastDelayText1;
    [SerializeField] Text FastDelayText2;
    [SerializeField] Text levelText;
    [SerializeField] Animator PopUp1;
    [SerializeField] Animator PopUp2;
    [SerializeField] Animator PopUp3;
    [SerializeField] Animator PopUp4;
    [SerializeField] Animator PopUp5;
    [SerializeField] Animator PopUp6;
    [SerializeField] Animator PopUp7;
    [SerializeField] Animator PlayerTag;
    [SerializeField] Animator hltLevel;
    [SerializeField] Animator atkLevel;
    [SerializeField] Animator spdLevel;
    [SerializeField] myManaBar ManaBar;
    [SerializeField] EnemyController enemy;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] GameObject gameOverText;
    [SerializeField] AudioSource levelUpSound;
    [SerializeField] AudioSource healSound;
    [SerializeField] AudioSource scatterSound;
    [SerializeField] AudioSource rangeSound;
    [SerializeField] AudioSource overSound;
    [SerializeField] AudioSource timerSound;
    [SerializeField] StatBtn statBtn;
    [SerializeField] SpawnShips spawnShips;
    [SerializeField] GameObject greenArrow;
    
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject nextBtn;
    [SerializeField] GameObject blueArrow;

    [SerializeField] GameObject redArrow;

    public List<GameObject> perkList;
    public int manaValue = 10;
    public int manaMax = 10;
    public int healCount =0;
    public int rangeCount=0;
    public int scatterCount=0;
    public float modifier=1;
    public int perkChance=10;
    public bool tutorialDestroy=false;
    float tankDelayValue = 1.5f;
    float defaulttankDelayValue = 1.5f;
    float normalDelayValue = 1.0f;
    float defaultnormalDelayValue = 1.0f;
    float fastDelayValue = 0.5f;
    float defaultfastDelayValue = 0.5f;

    int atkValue=1;
    int hltValue=1;
    int spdValue=1;
    int level=1;

    float gameHeight;
    float gameWidth;
    public Texture2D cursorTexture;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        
        perkList = new List<GameObject>();

        UpdateManaUI();
        gameHeight = myCamera.orthographicSize * 2;
        gameWidth = gameHeight * myCamera.aspect;
        
        background.transform.localScale = new Vector3(1.5f*gameWidth, 1.5f*gameWidth, 1);
        if(Variables.isTutorial){
            StartTutorial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateManaUI(){
        manaValueText.text = manaValue.ToString();
        manaMaxText.text = manaMax.ToString();
    }

    public void nextLevel(){
        if(level==0){
            tutorialDestroy=true;

            PopUp7.SetBool("Pop", true);
            Invoke("LevelUpSelfDelay", 2.5f);
            EndTutorial();

            return;
        }
        
        levelUpSound.Play();
        if(level%3==0){
            if(level>=13){
                PopUp5.SetBool("PopWe", true);
            }else{
                PopUp3.SetBool("PopWe", true);
            }
            
        }
        if(level>=13){
            PopUp4.SetBool("Pop", true);
        }else{
            PopUp1.SetBool("Pop", true);
            PopUp2.SetBool("Pop", true);
            PlayerTag.SetBool("LevelUp", true);

        }
        
        Invoke("ResetPopup", 2.5f);
        
    }

    void playRangeSound(){
        rangeSound.PlayOneShot(rangeSound.clip);
    }
    void playScatterSound(){
        scatterSound.PlayOneShot(scatterSound.clip);
    }


    void StartTutorial(){
        level=0;
        levelText.text = "Level " + level.ToString();

        manaMax=30;
        manaValue=manaMax;
        UpdateManaUI();
        ManaBar.setManaBar(manaMax);
        Tutorial.GetComponent<Animator>().SetBool("PopTut", true);
        greenArrow.SetActive(true);
    } 

    void EndTutorial(){
        tutorialDestroy=true;

        nextBtn.SetActive(false);
        greenArrow.SetActive(false);
        redArrow.SetActive(false);
        blueArrow.SetActive(false);
        Tutorial.SetActive(false);
        int total = spawnShips.shipList.Count;
        for(int j =0;j< total;j++){
            Debug.Log ("Destroy: "+j);
            Destroy(spawnShips.shipList[0]);
            spawnShips.shipList.RemoveAt(0);
        }

        manaMax=10;
        level++;
        levelText.text = "Level " + level.ToString();
        manaValue=manaMax;
        UpdateManaUI();
        ManaBar.setManaBar(manaMax);
        enemy.health=enemy.maxHealth;
        enemy.updateHealth();
        Variables.isTutorial=false;
    }
    void LevelUp(){
        level++;
        levelText.text = "Level " + level.ToString();
        manaMax+=5;
        manaValue=manaMax;
        UpdateManaUI();
        ManaBar.setManaBar(manaMax);
        enemy.health+=50;
        enemy.health+=healCount*40;
        if(enemy.health>enemy.maxHealth){
            enemy.health=enemy.maxHealth;
        }
        enemy.range += 0.7f*rangeCount;
        if(enemy.attackSpeed>0.1){
            enemy.attackSpeed -= rangeCount*0.03f;
            enemy.GetComponent<Animator>().SetFloat("FireSpeed", 0.5f/enemy.attackSpeed);
            enemy.bulletSpeed += rangeCount;
        }
        enemy.scatter += scatterCount;
        
        
        foreach(GameObject element in perkList){
            Destroy(element);
        }
        if(healCount>0){
            healSound.PlayOneShot(healSound.clip);
        }
        if(rangeCount>0){
            Invoke("playRangeSound", 1f);
        }
        if(scatterCount>0){
            Invoke("playScatterSound", 0.5f);
            tankDelayValue= defaulttankDelayValue *modifier;
            normalDelayValue= defaultnormalDelayValue* modifier;
            fastDelayValue= defaultfastDelayValue *modifier;
            updateDelayDisplay();
        }
        healCount=0;
        rangeCount=0;
        scatterCount=0;
        
        if(level>13){
            return;
        }
        int i = Random.Range(0,3);
        bool hlthMax = false;
        bool atkMax = false;
        bool spdMax = false;
        switch(i){
            case 0:
                if(hltValue==5){
                    hlthMax=true;
                    break;
                }
                hltLevel.SetInteger("Level", ++hltValue);
                enemy.maxHealth +=100;
                enemy.health=enemy.maxHealth;
                break;
            case 1:
                if(atkValue==5){
                    atkMax=true;
                    break;
                }
                atkLevel.SetInteger("Level", ++atkValue);
                enemy.attackDamage +=4;

                break;
            case 2:
                if(spdValue==5){
                    spdMax = true;
                    break;
                }
                spdLevel.SetInteger("Level", ++spdValue);
                navMeshAgent.speed +=2f;
                enemy.speed+=2f;
                break;
        }
        while(hlthMax||atkMax||spdMax){
            i = Random.Range(0,3);

            hlthMax=false;
            atkMax=false;
            spdMax=false;
            switch(i){
            case 0:
                if(hltValue==5){
                    hlthMax=true;
                    break;
                }
                hltLevel.SetInteger("Level", ++hltValue);
                enemy.maxHealth +=100;
                enemy.health=enemy.maxHealth;
                break;
            case 1:
                if(atkValue==5){
                    atkMax=true;
                    break;
                }
                atkLevel.SetInteger("Level", ++atkValue);
                enemy.attackDamage +=4;

                break;
            case 2:
                if(spdValue==5){
                    spdMax = true;
                    break;
                }
                spdLevel.SetInteger("Level", ++spdValue);
                navMeshAgent.speed +=2f;
                enemy.speed+=2f;
                break;
        }
        }
        statBtn.updateEnemyStats();
        enemy.updateHealth();

    }

    void updateDelayDisplay(){
        TankDelayText1.text = tankDelayValue.ToString()[0].ToString();
        TankDelayText2.text = tankDelayValue.ToString().Substring(2, Mathf.Min(tankDelayValue.ToString().Length-2, 2))+"s";
        NormalDelayText1.text = normalDelayValue.ToString()[0].ToString();
        NormalDelayText2.text = normalDelayValue.ToString().Substring(2, Mathf.Min(normalDelayValue.ToString().Length-2, 2))+"s";
        FastDelayText1.text = fastDelayValue.ToString()[0].ToString();
        FastDelayText2.text = fastDelayValue.ToString().Substring(2, Mathf.Min(fastDelayValue.ToString().Length-2, 2))+"s";
    
    }
    void ResetPopup(){
        PopUp1.SetBool("Pop", false);
        PopUp2.SetBool("Pop", false);
        PopUp4.SetBool("Pop", false);
        PlayerTag.SetBool("LevelUp", false);
        if(level%3==0){
            timerSound.PlayOneShot(timerSound.clip);
            modifier*=0.8f;
            tankDelayValue*=modifier;
            normalDelayValue*=modifier;
            fastDelayValue*=modifier;
            updateDelayDisplay();
        }
        Invoke("LevelUpSelfDelay", 2.5f);
        
        LevelUp();
        
    }

    void LevelUpSelfDelay(){
        PopUp3.SetBool("PopWe", false);
        PopUp5.SetBool("PopWe", false);
        PopUp6.SetBool("Pop", false);
        PopUp6.SetBool("Pop", false);
        tutorialDestroy=false;
    }

    public void gameOver(){
        if(level==0){

            PopUp6.SetBool("Pop", true);
            Invoke("LevelUpSelfDelay", 2.5f);
            EndTutorial();

            return;
        }
        Destroy(enemy.gameObject);
        if(PlayerPrefs.HasKey("lowscore")){
            int lowscore = PlayerPrefs.GetInt("lowscore");
            if(level<lowscore){
                lowscore=level;
                PlayerPrefs.SetInt("lowscore", lowscore);
                PlayerPrefs.Save();
            }
        }else{
            PlayerPrefs.SetInt("lowscore", level);
            PlayerPrefs.Save();
        }
        overSound.Play();
        gameOverText.SetActive(true);
        gameOverText.GetComponent<Text>().text = "Game over"+"\n" +"low score: ";
        gameOverText.GetComponent<Text>().text+=PlayerPrefs.GetInt("lowscore").ToString();
    }
}
