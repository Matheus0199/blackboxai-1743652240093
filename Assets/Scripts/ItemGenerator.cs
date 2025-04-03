using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    public int maxItems = 5;
    
    [Header("Item Settings")]
    public Sprite[] legalItemSprites;
    public Sprite[] illegalItemSprites;
    public Sprite[] xrayImages;
    public float minValue = 10f;
    public float maxValue = 500f;
    
    private float timer;
    private List<GameObject> activeItems = new List<GameObject>();
    
    private void Update()
    {
        if (GameManager.Instance.currentTime > 0 && activeItems.Count < maxItems)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnItem();
                timer = 0f;
            }
        }
    }
    
    private void SpawnItem()
    {
        GameObject newItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        Item item = newItem.GetComponent<Item>();
        activeItems.Add(newItem);
        
        // Randomly determine item status with weighted probabilities
        float statusRoll = Random.Range(0f, 1f);
        Item.ItemStatus status;
        
        if (statusRoll < 0.6f) status = Item.ItemStatus.Legal; // 60% chance
        else if (statusRoll < 0.9f) status = Item.ItemStatus.UnderDeclared; // 30% chance
        else status = Item.ItemStatus.Illegal; // 10% chance
        
        // Set values
        float baseValue = Random.Range(minValue, maxValue);
        float declaredValue = status == Item.ItemStatus.UnderDeclared ? 
            baseValue * Random.Range(0.1f, 0.7f) : baseValue;
        
        // Illegal items have 30% chance to be hidden
        bool isHidden = status == Item.ItemStatus.Illegal && Random.Range(0f, 1f) < 0.3f;
        
        // Select appropriate sprites
        Sprite normalSprite = status == Item.ItemStatus.Illegal ? 
            illegalItemSprites[Random.Range(0, illegalItemSprites.Length)] : 
            legalItemSprites[Random.Range(0, legalItemSprites.Length)];
        
        Sprite xraySprite = isHidden && xrayImages.Length > 0 ? 
            xrayImages[Random.Range(0, xrayImages.Length)] : null;
        
        // Initialize item
        item.Initialize(status, declaredValue, baseValue, isHidden, normalSprite, xraySprite);
        
        // Set up destruction when item leaves screen
        ItemDestroyer destroyer = newItem.AddComponent<ItemDestroyer>();
        destroyer.OnDestroyed += () => activeItems.Remove(newItem);
    }
    
    public void ClearAllItems()
    {
        foreach (var item in activeItems)
        {
            if (item != null) Destroy(item);
        }
        activeItems.Clear();
    }
}

public class ItemDestroyer : MonoBehaviour
{
    public delegate void DestroyEvent();
    public event DestroyEvent OnDestroyed;
    
    private void OnBecameInvisible()
    {
        if (OnDestroyed != null) OnDestroyed();
        Destroy(gameObject);
    }
}