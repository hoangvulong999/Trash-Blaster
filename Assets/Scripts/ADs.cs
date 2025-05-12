using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADs : MonoBehaviour {

	public static ADs _instance; //instance for easier usage

	public float timeToshowInterstitial = 300;
	float adTimer = 0; //Displaying interstitial ads

	private void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {
	}

	/// <summary>
	/// Showing interstitial AD and sending unity custom event
	/// </summary>
	public void ShowInterstitial() {
		// Removed ad code
	}

	public void ShowRewardedGetMoney() {
		CurrencyManager._instance.AddCoins(1000);
		MessageHandler._instance.ShowMessage("Reward Collected", 1f, Color.green);
	}


	public int planetToUnlock = -1;

	public void ShowRewardedGetPlanet1(int id) {
		PlanetUI p = ShopManager._instance.GetPlanetById(id);
		if (p.IsBought()) {
			p.OnClick();
			return;
		}

		planetToUnlock = id;
		PlayerPrefs.SetInt("PLANET_BOUGHT" + planetToUnlock, 1);
		ShopManager._instance.SetPlanets();
		MessageHandler._instance.ShowMessage("Reward Collected", 1f, Color.green);
	}

	public void ShowRewardedGetMoneyAfterGame() {
		CurrencyManager._instance.AddCoins(CurrencyManager._instance.coinsInLastRound);
		MessageHandler._instance.ShowMessage("Reward Collected", 1f, Color.green);
	}

}