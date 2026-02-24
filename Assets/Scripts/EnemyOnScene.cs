using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using Proto2.Enums;

public class EnemyOnScene : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Slider mySlider;
    [SerializeField] private EnemyPatternData currentPattern;
    [SerializeField] private List<CharacterOnScene> targetHero;

    //Transform parent;
    private void Start()
    {
        Transform parent = transform.parent;
        myCanvas = parent.GetComponentInChildren<Canvas>();
        mySprite = GetComponent<SpriteRenderer>();
        mySlider = myCanvas.GetComponentInChildren<Slider>();
        myAnimator = GetComponent<Animator>();

        maxHealth = enemyData.MaxHealth;
        currentHealth = enemyData.MaxHealth;
        mySprite.sprite = enemyData.EnemySprite;
        SetRandomPattern();
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
        myAnimator.SetTrigger("Attack");
    }

    public void GetTarget()
    {
        switch (currentPattern.PatternTargetType)
        {
            case PatternTargetType.TurnHero:
                targetHero.Add(BattleManager.Instance.TurnCharatcer);                 //need to catch TurnHero for BattleManager.Instance.PlayerParty
                break;

            case PatternTargetType.AllHero:
                targetHero.AddRange(BattleManager.Instance.PlayerParty);
                break;
        }
        
    }

    public void SetRandomPattern()
    {
        int index = Random.Range(0, enemyData.PatternList.Count);
        currentPattern = enemyData.PatternList[index];
        myCanvas.GetComponentInChildren<Image>().sprite = currentPattern.PatternImage;
    }
    public void UsePattern(EnemyPatternData patternData)
    {
        myAnimator.SetTrigger(patternData.PatternType.ToString());  //Only one Animation for a pattern

        foreach(EnemyPatternAction action in patternData.ActionList)
        {
            EnemyPatternProcessor.GetPattern(action.PatternType).DoAction(new EnemyActionParameters(action.PatternValue, targetHero[0],this));
        }
    }
}
