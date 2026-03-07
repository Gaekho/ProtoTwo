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
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Slider mySlider;
    [SerializeField] private EnemyPatternData currentPattern;
    [SerializeField] private List<AllyUnit> targetHero;
    [SerializeField] private List<EnemyOnScene> targetEnemy;

    //Transform parent;
    private void Start()
    {
        Transform parent = transform.parent;
        myCanvas = parent.GetComponentInChildren<Canvas>();
        mySprite = GetComponent<SpriteRenderer>();
        mySlider = myCanvas.GetComponentInChildren<Slider>();
        myAnimator = GetComponent<Animator>();

        currentHealth = enemyData.MaxHealth;
        mySprite.sprite = enemyData.EnemySprite;
        SetRandomPattern();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GetDamage(10f);
            //Die();
            StartCoroutine(DieRoutine());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SetRandomPattern();
        }

        if (Input.GetKeyDown(KeyCode.L)) 
        {
            UsePattern();
        }
    }
    public void GetDamage(float damage)
    {
        myAnimator.SetTrigger("Damaged");
        currentHealth -= damage;
        mySlider.value = currentHealth/enemyData.MaxHealth;
        
        if(currentHealth < 0)
        {
            StartCoroutine (DieRoutine());
        }
    }

    public IEnumerator DieRoutine()
    {

        BattleManager.Instance.EnemyDead(this);
        myAnimator.SetTrigger("Die");
        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);

    }

    public void DoAttack()
    {
        myAnimator.SetTrigger("Attack");
    }

    public void GetTarget(EnemyPatternAction patternAction )
    {
        targetHero.Clear();
        targetEnemy.Clear();

        switch (patternAction.ActionTargetType)
        {
            case PatternTargetType.Self:
                targetEnemy.Add(this);
                break;

            case PatternTargetType.RandomMob:
                break;

            case PatternTargetType.TurnHero:
                targetHero.Add(BattleManager.Instance.TurnCharacter);        //need to catch TurnHero for BattleManager.Instance.PlayerParty
                break;

            case PatternTargetType.AllHero:
                targetHero.AddRange(BattleManager.Instance.PlayerParty);
                break;

            case PatternTargetType.RandomTwoHero:
                targetHero.Add(BattleManager.Instance.TurnCharacter);
                break;
        }
        
    }

    public void SetRandomPattern()
    {
        int index = Random.Range(0, enemyData.PatternList.Count);
        currentPattern = enemyData.PatternList[index];
        myCanvas.GetComponentInChildren<Image>().sprite = currentPattern.PatternImage;
    }
    public void UsePattern()
    {
        myAnimator.SetTrigger(currentPattern.PatternType.ToString());  //Only one Animation for a pattern

        foreach(EnemyPatternAction action in currentPattern.ActionList)
        {
            Debug.Log(action.PatternActionType.ToString());
            GetTarget(action);
            Debug.Log(enemyData.EnemyName + "'s Target Hero : " + targetHero.Count);
            //Debug.Log(enemyData.EnemyName + "'s Target : " + targetHero[0]== null ? "List[0] is null" : targetHero[0].CharacterData.name);
            Debug.Log(enemyData.EnemyName + "'s Target Enemy : " + targetEnemy.Count);
            //EnemyPatternProcessor.GetPattern(action.PatternActionType).DoAction(new EnemyActionParameters(action.PatternValue, targetHero, targetEnemy));
        }
    }

    public IEnumerator UsePatternRoutine()
    {
        UsePattern();
        yield return new WaitForSeconds(0.7f);
    }
}
