using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfileldGenerator : MonoBehaviour
{

    public int numberOfPlanet;
    public int numberOfStars;
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
        numberOfPlanet = Random.Range (1, 10);
        numberOfStars = Random.Range (100, 666);

        for (int i = 0; i < numberOfPlanet; i++)
        {
            Vector3 position = new Vector3 (Random.Range(-fieldSize.x / 2,fieldSize.x/2), Random.Range(-maxDeep, -20), Random.Range(-fieldSize.y / 2, fieldSize.y / 2));
            GameObject planet = Instantiate (planetPrefab,position, Quaternion.Euler(90,0,0), transform);

            // Choose a picture randomly
            SpriteRenderer spriteRenderer = planet.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && planetSprites.Length > 0)
            {
                spriteRenderer.sprite = planetSprites[Random.Range(0, planetSprites.Length)];
            }
        }

        for (int i = 0; i < numberOfStars; i++) 
        {
            Vector3 position = new Vector3(Random.Range(-fieldSize.x / 2, fieldSize.x / 2), Random.Range(-maxDeep, -20), Random.Range(-fieldSize.y / 2, fieldSize.y / 2));
            GameObject star = Instantiate(starPrefab, position, Quaternion.Euler(90, 0, 0), transform);

            SpriteRenderer spriteRenderer2 = star.GetComponent<SpriteRenderer>();
            if (spriteRenderer2 != null && starSprites.Length > 0)
            {
                spriteRenderer2.sprite = starSprites[Random.Range(0, starSprites.Length)];
            }
        }
    }
}
