using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Properties;
using UnityEditor.Build.Player;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Linq.Expressions;

public class SpaceStationManager : MonoBehaviour
{
    public GameObject shopObj;
    public List<ShopItem> shopItems = new List<ShopItem>();
    public Button itemButton;
    public RectTransform shopScrollRectTransform;
    public RectTransform contentRectTransform;
    public Button exitButton;
    public List<IconData> iconDataList = new List<IconData>();

    private bool playerInTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitShop);
        }

        if (shopScrollRectTransform != null)
        {
            shopScrollRectTransform.gameObject.SetActive(false);
        }

        LoadIcons();
        LoadItems();
        
    }

    void LoadItems()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "shopItem").Replace("\\", "/");
        if (!File.Exists(filePath))
        {
            Debug.Log("Can not find item path");
            return;
        }
        else
        {
            string shopItemsData = File.ReadAllText(filePath);
            ShopItemList loadedData = JsonConvert.DeserializeObject<ShopItemList>(shopItemsData);
            shopItems = loadedData.items;

            contentRectTransform.sizeDelta = new Vector2(0, Mathf.Min(0, 200 * shopItems.Count + 50));

            for (int i = 0; i < shopItems.Count; i++)
            {
                int posX = 0;
                int posY = -(i * 200 + 100);

                Button newItemButton = Instantiate(itemButton,contentRectTransform);

                RectTransform rectTransform = newItemButton.GetComponent<RectTransform>();

                rectTransform.anchorMin = new Vector2(0.5f, 1);
                rectTransform.anchorMax = new Vector2(0.5f, 1);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                rectTransform.anchoredPosition = new Vector2(posX, posY);

                rectTransform.Find("Itemname").GetComponent<Text>().text = shopItems[i].itemName;
                rectTransform.Find("description").GetComponent<Text>().text = shopItems[i].description;
                rectTransform.Find("price").GetComponent<Text>().text = "$" + shopItems[i].price;
                
                IconData? iconData = FindIconByName(shopItems[i].icon);

                RectTransform iconRect = rectTransform.Find("Iconframe").GetComponent<RectTransform>().Find("Icon").GetComponent<RectTransform>();
                if (iconRect != null)
                {
                    iconRect.GetComponent<Image>().sprite = iconData.icon;
                }

                ShopItem shopItem = shopItems[i];

                newItemButton.onClick.AddListener(() => {ItemEffectTrigger(shopItem);});
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player" ) && !playerInTrigger)
        {
            playerInTrigger = true;
            Time.timeScale = 0f;
            Debug.Log("into the shop");
            OpenShop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("out Shop");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
    }

    private void ExitShop()
    {
        if (shopScrollRectTransform!= null)
        {
            shopScrollRectTransform.gameObject.SetActive(false);
        }
    }

    void OpenShop()
    {
        if (shopScrollRectTransform != null)
        {
            shopScrollRectTransform.gameObject.SetActive(true);
        }
    }

    void LoadIcons()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/UI/ShopItem/Icons");

        foreach (Sprite sprite in sprites)
        {
            IconData data = new IconData
            {
                icon = sprite,
                Name = sprite.name,
            };

            iconDataList.Add(data);
        }
    }

    public void ItemEffectTrigger(ShopItem item)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Component[] components = MyTools.GetComponentByName(player, item.branch);
            MyTools.CallMethod(components, item.method, item.value);
        }
        else 
        {
            Debug.LogError("can not find Player");
        }
    }

    IconData? FindIconByName(string name)
    {
        return iconDataList.FirstOrDefault(icon => icon.Name == name);
    }
}
