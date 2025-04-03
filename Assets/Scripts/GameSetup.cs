using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManagerPrefab;
    public UIManager uiManagerPrefab;
    public InspectionSystem inspectionSystemPrefab;
    public XRayScanner xrayScannerPrefab;
    public InterrogationSystem interrogationSystemPrefab;
    public ItemGenerator itemGeneratorPrefab;

    [Header("Spawn Points")]
    public Transform itemSpawnPoint;
    public Transform playerPosition;

    private void Awake()
    {
        // Instantiate GameManager if it doesn't exist
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }

        // Instantiate UIManager
        UIManager uiManager = Instantiate(uiManagerPrefab);

        // Instantiate and setup InspectionSystem
        InspectionSystem inspectionSystem = Instantiate(inspectionSystemPrefab);
        inspectionSystem.transform.position = playerPosition.position;

        // Instantiate and setup XRayScanner
        XRayScanner xrayScanner = Instantiate(xrayScannerPrefab);
        inspectionSystem.xrayScanner = xrayScanner;

        // Instantiate and setup InterrogationSystem
        InterrogationSystem interrogationSystem = Instantiate(interrogationSystemPrefab);
        inspectionSystem.interrogationSystem = interrogationSystem;

        // Instantiate and setup ItemGenerator
        ItemGenerator itemGenerator = Instantiate(itemGeneratorPrefab);
        itemGenerator.spawnPoint = itemSpawnPoint;

        // Link systems to GameManager
        GameManager.Instance.inspectionSystem = inspectionSystem;
        GameManager.Instance.itemGenerator = itemGenerator;
        GameManager.Instance.uiManager = uiManager;

        // Initialize game state
        GameManager.Instance.StartGame();
    }
}