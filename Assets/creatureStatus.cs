using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class creatureStatus : MonoBehaviour {

	private TextMesh targetText;
	SpriteRenderer monsterRenderer;

	// 速度
	public Vector2 SPEED = new Vector2(0.3f, 0.3f);

    int ageCount = 0;
    float countTime = 0;
	int time = 0;
	int goodSleepTime = 0;
	
	bool wakeup = false;
	bool isLightOn = false;
	int hungryCount = 0;
	int hungryCountMax = 4;
	int hungryReduceTime = 0;
	int foodCount = 0;

	int stressCount = 0;
	int stressCountMax = 4;
	int stressIncreaseTime = 0;
	
	int unhappyCount = 0;

	GameObject foodObject = null;
	GameObject lightObject = null;
	GameObject ageObject = null;
	GameObject timeObject = null;
	GameObject wakeupObject = null;
	GameObject sleepObject = null;
	GameObject hungryObject = null;
	GameObject stressObject = null;
	GameObject unhappyObject = null;

	GameObject monsterObject = null;

	public Sprite monster0Image;
	public Sprite monster1Image;
	public Sprite monster2Image;
	public Sprite monster3Image;
	public Sprite monster4Image;
	public Sprite monster5Image;
	public Sprite monster6Image;
	public Sprite monster7Image;
	public Sprite monster8Image;

	// Use this for initialization
	void Start () {
		// initialize objects
		foodObject = GameObject.Find("food");
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

		updateAllText();
	}
	
	// Update is called once per frame
	void Update () {

		countTime += Time.deltaTime; 

		// a time is one hour
		if(countTime > 1.0f){
			time++;

			// 24hour is a day
			if(time >= 24){
				ageCount++;
				targetText = ageObject.GetComponent<TextMesh>();
				targetText.text = "年齢 " + ageCount.ToString("F0") + " 歳";

				if(ageCount == 1){
					// age 1
					monsterRenderer.sprite = (foodCount > 3 ? monster1Image : monster2Image);
				}
				else if(ageCount == 3){
					// age 3
					monsterRenderer.sprite = (goodSleepTime > 20 ? monster3Image : monster4Image);
				}
				else if(ageCount == 5){
					// age 5
					monsterRenderer.sprite = (unhappyCount > 6 ? monster5Image : monster6Image);
				}

				time = 0;
			}

			targetText = timeObject.GetComponent<TextMesh>();
			targetText.text = "Time " + time.ToString("F0") + ":00";

			// creature is sleeping while 21:00 - 07:00
			wakeup = !((time >= 21 && time < 24) || (time >= 0 && time < 7));

			if(hungryCount > 0){
				hungryReduceTime++;
			}
			if(wakeup == true){
				stressIncreaseTime++;
			}

			targetText = wakeupObject.GetComponent<TextMesh>();
			targetText.text = wakeup ? "起きてる" : "寝てる";

			if(wakeup == false && isLightOn == false){
				// good sleeping
				goodSleepTime++;
				targetText = sleepObject.GetComponent<TextMesh>();
				targetText.text = "快眠 " + goodSleepTime.ToString("F0") + "ｈ";
			}
			
			countTime = 0.0f;

			// 移動処理
			Move();
		}

		// creature is hungry
		if(hungryReduceTime >= 4){
			if(hungryCount > 0){
				hungryCount--;
				updateHungryText(hungryCount);
			}
			hungryReduceTime = 0;
		}

		// creature get stress
		if(stressIncreaseTime >= 5){
			if(stressCount < stressCountMax){
				stressCount++;
				updateStressText(stressCount);
			}
			else if(stressCount >= stressCountMax){
				unhappyCount++;

				targetText = unhappyObject.GetComponent<TextMesh>();
				targetText.text = "不満 " + unhappyCount.ToString() + " 回";
			}
			stressIncreaseTime = 0;
		}
	}

	public void onClickPlayButton(){
		if(wakeup){
			if(stressCount > 0){
				stressCount--;
				updateStressText(stressCount);
			}
		}
	}

	public void onClickFoodButton () {
		if(wakeup){
			if(hungryCount < hungryCountMax){
				hungryCount++;
				updateHungryText(hungryCount);

				foodCount++;
				targetText = foodObject.GetComponent<TextMesh>();
				targetText.text = "エサ" + foodCount.ToString() + "個";
			}
		}
	}
	
	public void onClickLightButton(){
		isLightOn = !isLightOn;
		
		targetText = lightObject.GetComponent<TextMesh>();
		targetText.text = "電気 " + (isLightOn ? "ON" : "OFF");
	}

	public void onClickClearButton(){
		time = 0;
		ageCount = 0;
		foodCount = 0;
		hungryCount = 0;
		stressCount = 0;
		unhappyCount = 0;
		goodSleepTime = 0;

		isLightOn = false;
		
		monsterRenderer.sprite = monster0Image;

		updateAllText();
	}

	private void updateHungryText(int hungryCount){
		targetText = hungryObject.GetComponent<TextMesh>();
		string hungryCountText = string.Empty;
		int i = 0;
		for(; i < hungryCount; i++){
			hungryCountText += " ●";
		}
		for(; i < hungryCountMax; i++){
			hungryCountText += " ○";
		}
		targetText.text = "お腹 " + hungryCountText;	
	}

	private void updateStressText(int stressCount){
		targetText = stressObject.GetComponent<TextMesh>();
		string stressCountText = string.Empty;
		int i = 0;
		for(; i < stressCount; i++){
			stressCountText += " ●";
		}
		for(; i < stressCountMax; i++){
			stressCountText += " ○";
		}
		targetText.text = "ストレス " + stressCountText;
	}

	private void updateAllText(){

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

		updateHungryText(hungryCount);
		updateStressText(stressCount);
	}

	bool toLeft = true;

	// 移動関数
	void Move(){
		// 現在位置をPositionに代入
		Vector2 Position = monsterObject.transform.position;
		
		if(Position.x < -0.8f){
			toLeft = false;
		} else if (Position.x > 0.8f){
			toLeft = true;
		}

		if(toLeft){
			Position.x -= SPEED.x * 10;
		}else{
			Position.x += SPEED.x * 10;
		}
/*
		// 左キーを押し続けていたら
		if(Input.GetKey("left")){
			// 代入したPositionに対して加算減算を行う
			Position.x -= SPEED.x;
		} else if(Input.GetKey("right")){ // 右キーを押し続けていたら
			// 代入したPositionに対して加算減算を行う
			Position.x += SPEED.x;
		} else if(Input.GetKey("up")){ // 上キーを押し続けていたら
			// 代入したPositionに対して加算減算を行う
			Position.y += SPEED.y;
		} else if(Input.GetKey("down")){ // 下キーを押し続けていたら
			// 代入したPositionに対して加算減算を行う
			Position.y -= SPEED.y;
		}
 */		// 現在の位置に加算減算を行ったPositionを代入する
		monsterObject.transform.position = Position;
	}
}
