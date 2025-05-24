using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyReward : MonoBehaviour
{
    public static DailyReward Instance { get; private set; }

    [SerializeField] private Button rewardButton;
    [SerializeField] private Text buttonText;
    [SerializeField] private float rewardAmount = 100f; // Amount of crystals to give as daily reward

    private const string LAST_REWARD_TIME_KEY = "LAST_DAILY_REWARD_TIME";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateRewardState();
    }

    private void UpdateRewardState()
    {
        if (CanClaimReward())
        {
            EnableRewardButton();
        }
        else
        {
            DisableRewardButton();
        }
    }

    private bool CanClaimReward()
    {
        if (!PlayerPrefs.HasKey(LAST_REWARD_TIME_KEY))
        {
            return true;
        }

        DateTime lastRewardTime = DateTime.Parse(PlayerPrefs.GetString(LAST_REWARD_TIME_KEY));
        DateTime currentTime = DateTime.Now;

        // Check if it's a new day (past midnight)
        return lastRewardTime.Date < currentTime.Date;
    }

    private void EnableRewardButton()
    {
        rewardButton.interactable = true;
        buttonText.text = "Claim";
    }

    private void DisableRewardButton()
    {
        rewardButton.interactable = false;
        buttonText.text = "Claimed";
    }

    public void ClaimReward()
    {
        if (!CanClaimReward()) 
        {
            MessageHandler._instance.ShowMessage("Already claimed today's reward!", 1f, Color.red);
            return;
        }

        // Add crystals using the existing CurrencyManager
        CurrencyManager._instance.AddCrystals(rewardAmount);

        // Save the current time as the last reward time
        PlayerPrefs.SetString(LAST_REWARD_TIME_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();

        // Show success message
        MessageHandler._instance.ShowMessage($"Daily reward claimed: {rewardAmount} crystals!", 1f, Color.green);

        // Update the button state
        DisableRewardButton();
    }

    // Call this method to check and update the reward state
    // You can call this from other scripts or when the game comes back from being paused
    public void CheckRewardState()
    {
        UpdateRewardState();
    }
}
