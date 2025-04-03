using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Game UI")]
    public GameObject gameUI;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text processedItemsText;
    public TMP_Text moneyText;
    public Image timeBar;

    [Header("Menu UI")]
    public GameObject mainMenuUI;
    public Button startButton;
    public Button campaignButton;
    public Button endlessButton;
    public Button competitiveButton;

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public TMP_Text finalScoreText;
    public Button restartButton;
    public Button menuButton;

    [Header("Upgrade UI")]
    public GameObject upgradeUI;
    public Button snifferDogButton;
    public Button radiationSensorButton;
    public Button droneButton;
    public TMP_Text upgradeCostText;

    private void Start()
    {
        // Initialize button listeners
        startButton.onClick.AddListener(StartGame);
        campaignButton.onClick.AddListener(() => StartGameMode(GameMode.Campaign));
        endlessButton.onClick.AddListener(() => StartGameMode(GameMode.Endless));
        competitiveButton.onClick.AddListener(() => StartGameMode(GameMode.Competitive));
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
        
        snifferDogButton.onClick.AddListener(() => PurchaseUpgrade(UpgradeType.SnifferDog));
        radiationSensorButton.onClick.AddListener(() => PurchaseUpgrade(UpgradeType.RadiationSensor));
        droneButton.onClick.AddListener(() => PurchaseUpgrade(UpgradeType.Drone));

        ShowMainMenu();
    }

    private void Update()
    {
        if (GameManager.Instance != null && gameUI.activeSelf)
        {
            UpdateGameUI();
        }
    }

    private void UpdateGameUI()
    {
        timerText.text = $"Time: {Mathf.CeilToInt(GameManager.Instance.currentTime)}s";
        scoreText.text = $"Score: {GameManager.Instance.score}";
        processedItemsText.text = $"Processed: {GameManager.Instance.processedItems}";
        moneyText.text = $"Money: ${GameManager.Instance.money}";
        
        timeBar.fillAmount = GameManager.Instance.currentTime / GameManager.Instance.timeLimit;
    }

    private void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        GameManager.Instance.StartGame();
    }

    private void StartGameMode(GameMode mode)
    {
        GameManager.Instance.currentGameMode = mode;
        StartGame();
    }

    private void RestartGame()
    {
        gameOverUI.SetActive(false);
        gameUI.SetActive(true);
        GameManager.Instance.StartGame();
    }

    private void ReturnToMenu()
    {
        gameOverUI.SetActive(false);
        gameUI.SetActive(false);
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameUI.SetActive(false);
        gameOverUI.SetActive(false);
        upgradeUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
        finalScoreText.text = $"Final Score: {GameManager.Instance.score}";
    }

    public void ShowUpgradeMenu()
    {
        upgradeUI.SetActive(true);
        UpdateUpgradeButtons();
    }

    private void UpdateUpgradeButtons()
    {
        snifferDogButton.interactable = !GameManager.Instance.hasSnifferDog;
        radiationSensorButton.interactable = !GameManager.Instance.hasRadiationSensor;
        droneButton.interactable = !GameManager.Instance.hasDrone;

        snifferDogButton.GetComponentInChildren<TMP_Text>().text = 
            GameManager.Instance.hasSnifferDog ? "Purchased" : "Sniffer Dog ($500)";
        radiationSensorButton.GetComponentInChildren<TMP_Text>().text = 
            GameManager.Instance.hasRadiationSensor ? "Purchased" : "Radiation Sensor ($800)";
        droneButton.GetComponentInChildren<TMP_Text>().text = 
            GameManager.Instance.hasDrone ? "Purchased" : "Drone ($1200)";
    }

    private void PurchaseUpgrade(UpgradeType upgrade)
    {
        int cost = upgrade switch
        {
            UpgradeType.SnifferDog => 500,
            UpgradeType.RadiationSensor => 800,
            UpgradeType.Drone => 1200,
            _ => 0
        };

        if (GameManager.Instance.money >= cost)
        {
            GameManager.Instance.money -= cost;
            GameManager.Instance.PurchaseUpgrade(upgrade);
            UpdateUpgradeButtons();
        }
    }
}

public enum GameMode
{
    Campaign,
    Endless,
    Competitive
}