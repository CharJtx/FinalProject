using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfileldGenerator : MonoBehaviour
{

    public int maxNumberOfPlanet;
    public int minNumberOfPlanet=10;
    public int maxNumberOfStars;
    public int minNumberOfStars = 500;
    public int maxDeep = 50;
    public Vector2 fieldSize = new Vector2 (100, 100);
    public GameObject planetPrefab;
    //public GameObject backgroundPrefab;
    public GameObject starPrefab;
    public Sprite[] planetSprites;
    public Sprite[] starSprites;

    // Start is called before the first frame update
    void Start()
    {
        //GenerateBackground();
        GenerateStarfield();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void GenerateBackground()
    //{
    //    Vector3 position = new Vector3 (0,-maxDeep,0);

    //    GameObject background = Instantiate(backgroundPrefab, position, Quaternion.Euler(90,0,0));
    //    background.transform.localScale = new Vector3(fieldSize.x, fieldSize.y, 1);

    //}

    void GenerateStarfield()
    { 
        // Generate a random value for star and planet
        int numberOfPlanet = Random.Range (minNumberOfPlanet, maxNumberOfPlanet);
        int numberOfStars = Random.Range (minNumberOfStars, maxNumberOfStars);

        // Generatae planets 
        for (int i = 0; i < numberOfPlanet; i++)
        {
            // random deepth and position
            Vector3 position = new Vector3 (Random.Range(-fieldSize.x / 2,fieldSize.x/2), Random.Range(-maxDeep, -20), Random.Range(-fieldSize.y / 2, fieldSize.y / 2));
            GameObject planet = Instantiate (planetPrefab,position, Quaternion.Euler(90,0,0), transform);

            // Choose a picture randomly
            SpriteRenderer spriteRenderer = planet.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && planetSprites.Length > 0)
            {
                spriteRenderer.sprite = planetSprites[Random.Range(0, planetSprites.Length)];
            }
        }

        // Generate stars
        for (int i = 0; i < numberOfStars; i++) 
        {
            // random deepth and position
            Vector3 position = new Vector3(Random.Range(-fieldSize.x / 2, fieldSize.x / 2), Random.Range(-maxDeep, -20), Random.Range(-fieldSize.y / 2, fieldSize.y / 2));
            GameObject star = Instantiate(starPrefab, position, Quaternion.Euler(90, 0, 0), transform);

            // Choose a picture randomly
            SpriteRenderer spriteRenderer2 = star.GetComponent<SpriteRenderer>();
            if (spriteRenderer2 != null && starSprites.Length > 0)
            {
                spriteRenderer2.sprite = starSprites[Random.Range(0, starSprites.Length)];
            }
        }
    }
}
