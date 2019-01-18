using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;


public class creatureStatus : MonoBehaviour
{

    // 速度
    public Vector2 SPEED = new Vector2(0.3f, 0.3f);

    public Sprite monster0Image;
    public Sprite monster1Image;
    public Sprite monster2Image;
    public Sprite monster3Image;
    public Sprite monster4Image;
    public Sprite monster5Image;
    public Sprite monster6Image;
    public Sprite monster7Image;
    public Sprite monster8Image;

    GameObject foodObject = null;  // 餌を与える
    GameObject meetObject = null;  // 肉を与える
    GameObject vesetableObject = null; // 野菜を与える
    GameObject treatObject = null; // おやつを与える
    GameObject lightObject = null;
    GameObject ageObject = null;
    GameObject timeObject = null;
    GameObject wakeupObject = null;
    GameObject sleepObject = null;
    GameObject hungryObject = null;
    GameObject stressObject = null;
    GameObject unhappyObject = null;
    GameObject monsterObject = null;
    SpriteRenderer monsterRenderer;


    float countTime = 0;    //
    int totalTime = 0;      // 
    int ageCount = 0;       //
    int goodSleepTime = 0;  //

    bool isSleeping = false; // 寝てるか
    bool isLightOn = false;  // ライト点灯状態
    bool isSick = false;     // 病気か
    bool hasPoo = false;     // うんちの有無
    bool toLeft = true;      // 左方向への移動フラグ

    int hungryCount = 0;
    int hungryCountMax = 4;
    int hungryReduceTime = 0;
    int foodCount = 0;

    int stressCount = 0;
    int stressCountMax = 4;
    int stressIncreaseTime = 0;
    
    int unhappyCount = 0;

    int activePt = 5;   // 行動ポイント(0 to 5)
    int fitness = 0;     // 体力(0 to 10)
    int inteligence = 0; // 知能(0 to 10)
    int intimacy = 0;    // 親密(0 to 10)
    int healthiness = 10; // 健康(0 to 10)


    // Use this for initialization
    void Start()
    {
        // initialize objects
        foodObject = GameObject.Find("food");
        meetObject = GameObject.Find("meet");
        vesetableObject = GameObject.Find("vesetable");
        treatObject = GameObject.Find("treat");
        lightObject = GameObject.Find("light");
        ageObject = GameObject.Find("age");
        timeObject = GameObject.Find("time");
        wakeupObject = GameObject.Find("wakeup");
        sleepObject = GameObject.Find("sleep");
        hungryObject = GameObject.Find("hungry");
        stressObject = GameObject.Find("stress");
        unhappyObject = GameObject.Find("unhappy");
        monsterObject = GameObject.Find("monster");
        monsterRenderer = monsterObject.GetComponent<SpriteRenderer>();

        this.resetStatus();
        this.updateStatus();
        this.updateText();
    }


    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime; 

        this.updateStatus();
        this.updateText();
    }


    public void onClickClearButton()
    {
        this.resetStatus();
    }


    public void onClickLightButton()
    {
        isLightOn = !isLightOn;
    }


    public void onClickCleanupButton()
    {
        hasPoo = false;

        // TODO: とりあえず病気も掃除で解消
        isSick = false;
    }


    public void onClickPlayButton()
    {
        if (isSleeping)
            return;

        stressCount = GetInRanged(0, stressCountMax, stressCount - 1);
    }


    public void onClickFoodButton()
    {
        if (isSleeping || hungryCount >= hungryCountMax)
            return;

        hungryCount++;

        foodCount++;
    }


    public void onClickMeetButton()
    {
        if (isSleeping || hungryCount >= hungryCountMax)
            return;

        // 体力+2, 知力-1, 健康+1, 親密+1, 満腹+1
        fitness     = GetInRanged(0, 10, fitness +2);
        inteligence = GetInRanged(0, 10, fitness -1);
        healthiness = GetInRanged(0, 10, fitness +1);
        intimacy    = GetInRanged(0, 10, fitness +1);
        hungryCount = GetInRanged(0, hungryCountMax, hungryCount +1);

        foodCount++;
    }


    public void onClickVesetableButton()
    {
        if (isSleeping || hungryCount >= hungryCountMax)
            return;

        // 体力−1, 知力+1, 健康+2, 親密-1, 満腹+1
        fitness     = GetInRanged(0, 100, fitness -1);
        inteligence = GetInRanged(0, 100, fitness +1);
        healthiness = GetInRanged(0, 100, fitness +2);
        intimacy    = GetInRanged(0, 100, fitness -1);
        hungryCount = GetInRanged(0, hungryCountMax, hungryCount +1);

        foodCount++;
    }


    public void onClickTreatButton()
    {
        if (isSleeping || hungryCount >= hungryCountMax)
            return;

        // 体力−1, 知力-1, 健康-1, 親密+2, 満腹+1
        fitness     = GetInRanged(0, 10, fitness -1);
        inteligence = GetInRanged(0, 10, fitness -1);
        healthiness = GetInRanged(0, 10, fitness -1);
        intimacy    = GetInRanged(0, 10, fitness -2);
        hungryCount = GetInRanged(0, hungryCountMax, hungryCount +1);

        foodCount++;
    }


    public void onClickStudy()
    {
        if (isSleeping || activePt <= 0)
            return;

        // 体力-1, 知力+3, AP-1
        fitness     = GetInRanged(0, 10, fitness -1);
        inteligence = GetInRanged(0, 10, fitness +3);
        activePt    = GetInRanged(0,  5, activePt -1);
    }


    public void onClickExercise()
    {
        if (isSleeping || activePt <= 0)
            return;

        // 体力+3, 知力-1, AP-1
        fitness     = GetInRanged(0, 10, fitness +3);
        inteligence = GetInRanged(0, 10, fitness -1);
        activePt    = GetInRanged(0,  5, activePt -1);
    }


    void resetStatus()
    {
        countTime = 0;
        totalTime = 0;
        ageCount = 0;
        goodSleepTime = 0;

        isSleeping = false;
        isLightOn = false;
        isSick = false;
        hasPoo = false;
        toLeft = true;

        hungryCount = 0;
        hungryReduceTime = 0;
        foodCount = 0;
        stressCount = 0;
        stressIncreaseTime = 0;
        unhappyCount = 0;

        activePt = 5;
        fitness = 0;
        inteligence = 0;
        intimacy = 0;
        healthiness = 10;

        monsterRenderer.sprite = monster0Image;
    }


   // 時間経過によるステータスの更新を行う
    void updateStatus()
    {
        if(countTime < 1.0f)
            return;
        countTime = 0.0f;

        // a time is one hour
        // 24hour is a day

        // ゲーム内時間の更新
        totalTime++;
        int time = totalTime % 24;

        // 年齢の更新
        bool one_day_passed = (totalTime > 0 && time == 0);
        if (one_day_passed)
            ageCount++;

        // 睡眠状態の更新
        // creature is sleeping while 22:00 - 08:00
        isSleeping = ((time >= 22 && time < 24) || (time >= 0 && time < 8));
        if(isSleeping && ! isLightOn )
            goodSleepTime++;

        // 満腹度の更新
        if(hungryCount > 0)
            hungryReduceTime++;
        if(hungryReduceTime >= 4){
            if(hungryCount > 0)
                hungryCount--;
            hungryReduceTime = 0;
        }

        // ストレス値の更新
        if( ! isSleeping)
            stressIncreaseTime++;
        if(stressIncreaseTime >= 5){
            if(stressCount < stressCountMax){
                stressCount++;
            }
            else if(stressCount >= stressCountMax){
                unhappyCount++;

            }
            stressIncreaseTime = 0;
        }

        // うんちの発生(6時間毎)
        bool half_day_passed = (totalTime > 0 && time % 6 == 0);
        if ( ! hasPoo)
            hasPoo = half_day_passed;

        // 病気の発生(ランダム)
        this.updateSicknessRandamly();

        // 進化状態の更新
        if (one_day_passed) {
            if(ageCount == 1){
                // age 1
                monsterRenderer.sprite =
                    (foodCount > 3 ? monster1Image : monster2Image);
            }
            else if(ageCount == 3){
                // age 3
                monsterRenderer.sprite =
                    (goodSleepTime > 20 ? monster3Image : monster4Image);
            }
            else if(ageCount == 5){
                // age 5
                monsterRenderer.sprite =
                    (unhappyCount > 6 ? monster5Image : monster6Image);
            }
        }

        // 移動処理
        this.move();
    }


    private void updateText()
    {
        TextMesh targetText;

        int time = totalTime % 24;
        targetText = timeObject.GetComponent<TextMesh>();
        targetText.text = "Time " + time.ToString("F0") + ":00";

        targetText = ageObject.GetComponent<TextMesh>();
        targetText.text = "年齢 " + ageCount.ToString("F0") + " 歳";

        targetText = foodObject.GetComponent<TextMesh>();
        targetText.text = "エサ " + foodCount.ToString() + " 個";

        targetText = lightObject.GetComponent<TextMesh>();
        targetText.text = "電気 " + (isLightOn ? "ON" : "OFF");

        targetText = unhappyObject.GetComponent<TextMesh>();
        targetText.text = "不満 " + unhappyCount.ToString() + " 回";

        targetText = sleepObject.GetComponent<TextMesh>();
        targetText.text = "快眠 " + goodSleepTime.ToString("F0") + "ｈ";

        // TODO: 病気、うんちのステータスをとりあえず wakeupObject に表示
        targetText = wakeupObject.GetComponent<TextMesh>();
        string s = "";
        if (isSleeping) s += "寝てる ";
        if (isSick) s += "病気 ";
        if (hasPoo) s += "うんち ";
        targetText.text = s;

        targetText = hungryObject.GetComponent<TextMesh>();
        string htext = "お腹 ";
        for(int i = 0; i < hungryCountMax; i++)
            htext += i < hungryCount ? " ●" : " ○";
        targetText.text = htext;

        targetText = stressObject.GetComponent<TextMesh>();
        string stext = "ストレス";
        for(int i = 0; i < stressCountMax; i++)
            stext += i < stressCount ? " ●" : " ○";
        targetText.text = stext;
    }


    void updateSicknessRandamly()
    {
        if (isSick)
            return;

        // TODO:
        // 4時間ごととかでいいのでは？

        // 病気になる確率 0 to 100
        // 健康度に応じて確率が変化
        int accuracy = 0;
        if (healthiness <= 2)
            accuracy = 60;
        else if (healthiness <= 4)
            accuracy = 40;
        else if (healthiness <= 6)
            accuracy = 20;
        else if (healthiness <= 8)
            accuracy = 10;

        // 0 to 100 のランダムな値を取得し、
        // その値が 0 to accuracy の範囲にあれば病気にする
        var rnd = new System.Random();
        int rnd_val = rnd.Next(0, 100);

        if (IsInRange(0, accuracy, rnd_val))
            isSick = true;
    }


    // 移動関数
    void move()
    {
        if (isSleeping)
            return;

        // 現在位置をPositionに代入
        Vector2 pos = monsterObject.transform.position;

        if (pos.x < -0.8f) {
            toLeft = false;
        } else if (pos.x > 0.8f) {
            toLeft = true;
        }
        float pos_diff = SPEED.x * 5;
        pos.x = toLeft ? pos.x - pos_diff : pos.x + pos_diff;

        /*
        // 左キーを押し続けていたら
        if(Input.GetKey("left")){
            // 代入したPositionに対して加算減算を行う
            pos.x -= SPEED.x;
        } else if(Input.GetKey("right")){ // 右キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            pos.x += SPEED.x;
        } else if(Input.GetKey("up")){ // 上キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            pos.y += SPEED.y;
        } else if(Input.GetKey("down")){ // 下キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            pos.y -= SPEED.y;
        }
        */

        // 現在の位置に加算減算を行ったPositionを代入する
        monsterObject.transform.position = pos;
    }


    // value が範囲内 (low <= value <= high) なら true を返す
    static bool IsInRange(int low, int high, int value)
    {
        if (value < low)
            return false;
        if (value > high)
            return false;

        return true;
    }

    // low <= value <= high の範囲になるように切り詰めた数値を返す
    static private int GetInRanged(int low, int high, int value)
    {
        if (value < low)
            return low;
        if (value > high)
            return high;
        return value;
    }
}
