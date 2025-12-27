using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class EnemyOnScene : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private Sprite mySprite;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Slider mySlider;

    //Transform parent;
    private void Start()
    {
        Transform parent = transform.parent;
        Canvas canvasTransform = parent.GetComponentInChildren<Canvas>();

        maxHealth = enemyData.MaxHealth;
        currentHealth = enemyData.MaxHealth;
        mySprite = enemyData.CharacterSprite;
        mySlider = canvasTransform.GetComponentInChildren<Slider>();
        myAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.L)) 
        {
           DoAttack();
        }
    }
    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        mySlider.value = currentHealth/maxHealth;
        myAnimator.SetTrigger("damaged");
        
    }

    public void DoAttack()
    {
        myAnimator.SetTrigger("attack");
    }
}
