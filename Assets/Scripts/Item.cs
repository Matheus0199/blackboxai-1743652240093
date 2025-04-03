using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemStatus
    {
        Legal,
        UnderDeclared,
        Illegal
    }
    
    public ItemStatus status;
    public float declaredValue;
    public float actualValue;
    public bool isHidden;
    public Sprite xrayImage;
    public Sprite normalImage;
    
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Initialize(ItemStatus itemStatus, float declared, float actual, bool hidden, Sprite normalSprite, Sprite xraySprite)
    {
        status = itemStatus;
        declaredValue = declared;
        actualValue = actual;
        isHidden = hidden;
        normalImage = normalSprite;
        xrayImage = xraySprite;
        
        UpdateAppearance();
    }
    
    public float CalculateTax()
    {
        return actualValue * 0.2f; // 20% tax rate for example
    }
    
    public void UpdateAppearance()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = normalImage;
        }
    }
    
    public void ShowXRayImage()
    {
        if (spriteRenderer != null && xrayImage != null)
        {
            spriteRenderer.sprite = xrayImage;
        }
    }
    
    public void HideItem()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
    
    public void ShowItem()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            UpdateAppearance();
        }
    }
}