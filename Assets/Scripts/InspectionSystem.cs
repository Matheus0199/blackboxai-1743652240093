using UnityEngine;
using UnityEngine.UI;

public class InspectionSystem : MonoBehaviour
{
    public Item currentItem;
    public XRayScanner xrayScanner;
    public InterrogationSystem interrogationSystem;
    public GameObject inspectionUI;
    public Text itemDescriptionText;
    public Button releaseButton;
    public Button taxButton;
    public Button confiscateButton;

    private void Start()
    {
        releaseButton.onClick.AddListener(() => MakeDecision(DecisionType.Release));
        taxButton.onClick.AddListener(() => MakeDecision(DecisionType.Tax));
        confiscateButton.onClick.AddListener(() => MakeDecision(DecisionType.Confiscate));
    }

    public void StartInspection(Item item)
    {
        currentItem = item;
        inspectionUI.SetActive(true);

        if (item.isHidden)
        {
            xrayScanner.ScanItem(item);
            itemDescriptionText.text = "Item appears suspicious. Scanning with X-ray...";
        }
        else
        {
            ShowItemDetails();
        }
    }

    private void ShowItemDetails()
    {
        string statusText = currentItem.status switch
        {
            Item.ItemStatus.Legal => "Legal",
            Item.ItemStatus.UnderDeclared => "Potentially Under-Declared",
            Item.ItemStatus.Illegal => "Illegal",
            _ => "Unknown"
        };

        itemDescriptionText.text = 
            $"Item Status: {statusText}\n" +
            $"Declared Value: ${currentItem.declaredValue:0.00}\n" +
            $"Estimated Actual Value: ${currentItem.actualValue:0.00}";
    }

    public void MakeDecision(DecisionType decision)
    {
        bool correctDecision = false;
        string resultMessage = "";

        switch (decision)
        {
            case DecisionType.Release:
                correctDecision = currentItem.status == Item.ItemStatus.Legal;
                resultMessage = correctDecision ? 
                    "Correct! Item was legal." : 
                    "Wrong! Item should have been taxed or confiscated.";
                break;

            case DecisionType.Tax:
                correctDecision = currentItem.status == Item.ItemStatus.UnderDeclared;
                resultMessage = correctDecision ? 
                    $"Correct! Added ${currentItem.CalculateTax():0.00} in taxes." : 
                    "Wrong! Item was either legal or illegal.";
                break;

            case DecisionType.Confiscate:
                correctDecision = currentItem.status == Item.ItemStatus.Illegal;
                resultMessage = correctDecision ? 
                    "Correct! Item was illegal and has been confiscated." : 
                    "Wrong! Item was legal or just under-declared.";
                break;
        }

        Debug.Log(resultMessage);
        GameManager.Instance.ProcessItem(correctDecision);
        inspectionUI.SetActive(false);
        currentItem = null;
    }
}

public enum DecisionType
{
    Release,
    Tax,
    Confiscate
}