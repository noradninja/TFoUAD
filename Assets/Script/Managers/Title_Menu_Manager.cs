﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Title_Menu_Manager : MonoBehaviour {
private  string joystick1 = "joystick 1 button ";
	private  int CROSS = 0;
	private  int CIRCLE = 1;
	private  int SQUARE = 2;
	private  int TRIANGLE = 3;
	private  int SELECT = 6;
	private  int START = 7;
	private  int UP = 8;
	private  int RIGHT = 9;
	private  int DOWN = 10;
	private  int LEFT = 11;

	public int selectedSlot = 1;
	public int selectedSLSlot = 1;
	public Color baseColor;
	public Color hilightColor;
	public Color selectedColor;
	public RawImage slot1;
	public RawImage slot2;
	public RawImage slot3;
	public GameObject saveManager;
	public GameObject optionsManager;
	public GameObject ButtonManager;
	public CanvasGroup mainMenuCanvas;
	public CanvasGroup saverCanvas;
	public CanvasGroup optionCanvas;
	public CanvasGroup dialogCanvas;
	public CanvasGroup faderCanvas;
	public CanvasGroup selectedCanvas;
	public GameObject audioManager;
	public float targetValue;
	public bool mainMenuEnabled = false;
	public bool saverEnabled = false;
	public bool optionEnabled = false;
	public bool dialogEnabled = false;
	public bool delayTimer = false;
	public float timer = 0.0f;
	private float delay = 0.125f;
	public List<AudioClip> clipList;
	public AudioSource audioSource;
	public GameObject currentSelection;
	public GameObject previousSelection;
	public Animator anim;

	// Use this for initialization
	void Start () {
		//set the color of the initially selected slot
		setColor();
		if (!Application.isEditor) return;
		//because the DS3 registers the buttons differently in Windows
		TRIANGLE = 0;
		CIRCLE = 1;
		CROSS = 2;
		SQUARE = 3;
		START = 8;
		SELECT = 9;
		//these are mapped to L3/R3 because the fucking dpad is a set of axes in Windows ఠ ͟ಠ
		UP = 10;
		DOWN = 11;
		//EventManager.GetComponent<SaveSerial>();
		//clipList = audioManager.GetComponent<AudioManager>().SFXList;
	}
	
	// Update is called once per frame
void Update () {
		if (saverCanvas.alpha == 0){
				// saveManager.GetComponent<SaveManager_Inputs>().timer = 0.0f;
				// saveManager.GetComponent<SaveManager_Inputs>().setColor();
				saverEnabled = false;
				setColor();
		}
		if (optionCanvas.alpha == 0){
			// optionsManager.GetComponent<OptionsManagerInputs>().timer = 0.0f;
			optionEnabled = false;
			setColor();
		}
		if (dialogCanvas.alpha == 0){
				dialogEnabled = false;
				saveManager.GetComponent<LoadManager_Inputs>().SetColor();
				setColor();
		}
		//change button color when we choose an option
		if (saverCanvas.alpha > 0){
			selectColor();
			saverEnabled = true;	
		}
		if (optionCanvas.alpha > 0){
			selectColor();
			optionEnabled = true;
		}
		if (dialogCanvas.alpha > 0){
			dialogEnabled = true;
		}
		if ((PauseManager.isPaused) == true){
			PlayerController.delayButton = true;
			if (Input.GetButtonDown ("Circle") && !dialogEnabled && !delayTimer){
				if (optionEnabled){
					StartCoroutine(FadeScreen(1, 0 , 0.0F));
					selectedCanvas = mainMenuCanvas;
				}
				if (saverEnabled){
					StartCoroutine(FadeScreen(1, 0 , 0.0F));
					selectedCanvas = mainMenuCanvas;
				}
				if (dialogEnabled){
					StartCoroutine(FadeScreen(1, 0 , 0.0F));
					selectedCanvas = mainMenuCanvas;
				}
				if (mainMenuEnabled){
					StartCoroutine(FadeScreen(1, 0 , 0.5F));
					PauseManager.isPaused = false;
					StartCoroutine(PlayerController.ButtonDelayTimer(0.5f));
				}
				
			}
			if (optionEnabled == false && saverEnabled == false && dialogEnabled == false){
				mainMenuEnabled = true;
				timer = timer += 0.01f;
				if (!(timer > delay)) return;
				//Decrement slot by -1 if you press up
				if (Input.GetButtonDown ("Up")){
					//audioSource.PlayOneShot(clipList[2]);
					if (selectedSlot > 1){
						//set slot to 2 if you are at slot 1 to wrap selection
						selectedSlot -= 1;
					}
					//decrement the slot for each up press
					else if (selectedSlot == 1){
						//set slot to 1 if you are at slot 2 to wrap selection
						selectedSlot = 3;
					}
					//set the color of the selected slot
					setColor();
					//animateButtons();
				}
					
				//Increment slot by +1 if you press down
				if (Input.GetButtonDown ("Down")){
					//audioSource.PlayOneShot(clipList[3]);
					if (selectedSlot < 3){
						//set slot to 1 if you are at slot 2 to wrap selection
						selectedSlot += 1;
					}
					//increment the slot by 1 for each down press
					else if (selectedSlot == 3){
						//set slot to 1 if you are at slot 2 to wrap selection
						selectedSlot = 1;
					}
					//set the color of the selected slot
					setColor();
					//animateButtons();
				}
					
				if (Input.GetButtonDown ("Cross") && saverEnabled == false && optionEnabled == false && dialogEnabled == false){
	
					switch (selectedSlot)
					{
						case 1:
							selectedCanvas = faderCanvas;
							timer = 0.0f;
							StartCoroutine(FadeScreen(0, 1 , 0.25F));
							break;
						case 2:
							//animateButtons();
							selectedCanvas = saverCanvas;
							timer = 0.0f;
							StartCoroutine(FadeScreen(0, 1 , 0.0F));
							break;
						case 3:
							//animateButtons();
							selectedCanvas = optionCanvas;
							timer = 0.0f;
							StartCoroutine(FadeScreen(0, 1 , 0.0F));
							break;
					}
						
				}
			}
			else mainMenuEnabled = false;
		}
}		


//this method checks which slot is currently selected and changes the colors of all the slots to give you a hilight 
//on the selected slot
	void setColor(){
		
		if (selectedSlot==1){
			slot1.color = hilightColor;
			slot2.color = baseColor;
			slot3.color = baseColor;
		}
		else if (selectedSlot==2){
			slot1.color = baseColor;
			slot2.color = hilightColor;
			slot3.color = baseColor;
		}
		else if (selectedSlot==3){
			slot1.color = baseColor;
			slot2.color = baseColor;
			slot3.color = hilightColor;
		}
		else if (selectedSlot==4){
			slot1.color = baseColor;
			slot2.color = baseColor;
			slot3.color = baseColor;
		}
	}
	void selectColor(){
			if (selectedSlot==1){
			slot1.color = selectedColor;
			slot2.color = baseColor;
			slot3.color = baseColor;
		}
		else if (selectedSlot==2){
			slot1.color = baseColor;
			slot2.color = selectedColor;
			slot3.color = baseColor;
		}
		else if (selectedSlot==3){
			slot1.color = baseColor;
			slot2.color = baseColor;
			slot3.color = selectedColor;
		}
		else if (selectedSlot==4){
			slot1.color = baseColor;
			slot2.color = baseColor;
			slot3.color = baseColor;
		}
	}
		void animateButtons()
		{
			switch (selectedSlot)
			{
				case 2:
					currentSelection = GameObject.Find("Options");
					previousSelection = GameObject.Find("New_Game");
					// anim = currentSelection.GetComponent<Animator>();
					// anim.SetTrigger("MakeBounce");
					// previousSelection.GetComponent<Animator>().SetTrigger("SteadyState");
					break;
				// else anim.SetTrigger("SteadyState");
				case 1:
					currentSelection = GameObject.Find("New_Game");
					previousSelection = GameObject.Find("Options");
					// anim = currentSelection.GetComponent<Animator>();
					// anim.SetTrigger("MakeBounce");
					// previousSelection.GetComponent<Animator>().SetTrigger("SteadyState");
					break;
			}

			// else anim.SetTrigger("SteadyState");
		}

		private IEnumerator FadeScreen(float startValue, float targetValue, float duration) {
        float fadeTime = 0;
		//fade out the loadscreen canvas group
        while (fadeTime < duration)
        {
            selectedCanvas.alpha = Mathf.Lerp(startValue, targetValue, fadeTime / duration);
            fadeTime += Time.deltaTime;
            yield return null;
        }
		selectedCanvas.alpha = targetValue;
		if (selectedCanvas == faderCanvas){
			print ("Fader Canvas");
			SetScenes.sceneToLoad = SetScenes.nextScene;
			SceneManager.LoadSceneAsync("LoadScreen", LoadSceneMode.Single);
		}
	}
}
