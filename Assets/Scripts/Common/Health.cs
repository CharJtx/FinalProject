using System;

using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IHealth
{
    public int maxHealth = 100;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die() 
    {

        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.StopFollowing();

        GameManager.Instance.GameOver(false);

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
        
    }

    public int CurrentHealth
    { 
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            ChangeHPSlide();
        }
        
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public void IncreaseMaxHPbyPercent( int percent)
    {
        float percentage = percent / 100f;
        MaxHealth += (int)Math.Floor(MaxHealth * percentage);
    }

    public void IncreaseMaxHPbyValue( int value)
    {
        MaxHealth += value;
    }

    public void RestoreHPbyPercent(int percent)
    {
        float percentage = percent / 100;
        CurrentHealth = Mathf.Max(CurrentHealth + (int)Math.Floor(MaxHealth * percentage), maxHealth);
        Debug.Log(percentage);
    }

    public void ChangeHPSlide()
    {
        Transform canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
        if (canvasTransform != null)
        {
            Transform sliderTransform = MyTools.FindChildByName(canvasTransform, "HPSlider");
            if (sliderTransform != null)
            {
                Slider slider = sliderTransform.GetComponent<Slider>();
                slider.maxValue = MaxHealth;
                slider.value = CurrentHealth;

            }
        }
    }
}
