using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    public float timeLimit = 120f;
    public int requiredProcessedItems = 10;
    public float currentTime;
    public int processedItems;
    public int score;
    public int money;
    
    [Header("Upgrades")]
    public bool hasSnifferDog;
    public bool hasRadiationSensor;
    public bool hasDrone;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        currentTime = timeLimit;
        StartGame();
    }
    
    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                EndGame();
            }
        }
    }
    
    public void StartGame()
    {
        processedItems = 0;
        score = 0;
        currentTime = timeLimit;
    }
    
    public void EndGame()
    {
        // Handle game over logic
        Debug.Log("Game Over! Final Score: " + score);
    }
    
    public void ProcessItem(bool correctDecision)
    {
        processedItems++;
        if (correctDecision)
        {
            score += 100;
            money += 50;
        }
        else
        {
            score -= 50;
        }
    }
    
    public void PurchaseUpgrade(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.SnifferDog:
                hasSnifferDog = true;
                break;
            case UpgradeType.RadiationSensor:
                hasRadiationSensor = true;
                break;
            case UpgradeType.Drone:
                hasDrone = true;
                break;
        }
    }
}

public enum UpgradeType
{
    SnifferDog,
    RadiationSensor,
    Drone
}