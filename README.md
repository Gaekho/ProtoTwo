# 필수 정보

### 유니티 에디터 버전 : 2022.22.3f1

에디터 수정 및 추가가 있었기에 가급적이면 맞출 것.

### VS 유니코드 설정 : 서명없는 UTF-8 (권장)

# 효과 디자인 추가 방법 (코드)

### 액션 추가 방법

- 카드 액션 추가 :
    1. `public class CardActionBase.cs` 하위 클래스 추가.
    2. 클래스는 `[Serializable]`로 선언
    3. 필요한 필드는 `[SerializeField]`로 선언
    4. `DoAction` override

```csharp
// Assets/Scripts.Card/CardActionBase.cs 하위에 작성

[Serializable]
public NewCardActionBase : CardActionBase
{
	[SereializeField] private type ownField;	// float damage, int drawCount ...

	public override void DoAction(CardActionParameters actionParameters)
	{
		// 타겟을 지정하는 경우 foreach(BattleUnitBase target in ActionTargets(actionParameters)
		// 아니면 그냥 효과 작성.
	}
}
```

- 적 패턴 액션 추가 :
    1. `public class PatternActionBase` 하위 클래스 추가.
    2. 클래스는 `[Serializable]`로 선언
    3. 필요한 필드는 `[SerializeField]`로 선언
    4. `DoAction` override

```csharp
// Assets/Scripts/BattleUnit/PatternActionBase.cs 하위에 작성

[Serializable]
public class NewPatternActionBase : PatternActionBase
{
	[SereializeField] private type ownField;	// float damage, int drawCount ...

	public override void DoAction(CardActionParameters actionParameters)
	{
		// 타겟을 지정하는 경우 foreach(BattleUnitBase target in ActionTargets(actionParameters)
		// 아니면 그냥 효과 작성.
	}
}
```

### 버프 추가 방법

- 버프 추가 :
    1. `BuffTypes` enum에 버프 이름 추가.
    2. `BuffList.cs`에 `BuffBase` 하위 클래스 추가.
    3. 생성자 미리 구현. (인스펙터에서 추가 시 기본값)
    4. `TriggerTiming`에 맞춰서 작동부 override.
    5. 필요한 추가 필드 생성 및 생성자에 추가.
    6. (추가 필드/스펙) 전용 `BuffInstance` 하위 클래스 생성.
    7. (추가 필드/스펙) BuffBase 하위 클래스에서 `CreateInstance`, `MergeToSameBuff` override.

```csharp
// Assets/Scripts/Buff/BuffList 하위에 작성

[Serializable]
public class NewBuff : BuffBase
{
	[SerializeField] private type ownField;

	public type OwnField => ownField;	//Cache

	public NewBuff()
	{
		buffType = BuffTypes.New;
		isDebuff = false;
		buffName = "NewBuff";
		description = "Write description of this buff.";
		triggerTiming = BuffTriggerTiming.None;

		duration = 1;
		reduceTiming = ReduceTiming.EndOfOwnerTurn;
		//ownField = ??? 세팅하기.
	}

	public override void OnApply(BuffInstance instance) { }
	public override void OnRemove(BuffInstance instance) { }
	public override void OnTurnStart(BuffInstance instance) { }
	public override void OnTurnEnd(BuffInstance instance) { }

	// 추가 필드 / 스펙이 필요한 경우
	public override BuffInstance CreateInstance(BattleUnitBase owner, BattleUnitBase applier)
	{
		return new NewBuffInstance(this, owner, applier;
	}
	public override void MergeToSameBuff(BuffInstance originalBuff, BattleUnitBase newApplier) { }
}
```

```csharp
// 버프에 추가 필드 / 스펙이 필요한 경우
// Assets/Scripts/Buff/BuffInstance 하위에 작성

[Serializable]
public class NewBuffInstance : BuffInstance
{
	[SerializeField] private type ownField;

	public NewBuffInstance (NewBuff sourceBuff, BattleUnitBase owner, BattleUnitBase applier)
	{
		base.BuffInstance
		ownField = sourceBuff.OwnField
	}

}
```

---

# 맵 노드 추가

## 새로운 노드 카테고리 추가 방법

- 노드 추가 :
    1. `NodeTypes.cs`에 타입 enum 추가.
    2. `NodeBase` 상속받아서 노드 클래스 추가

```csharp
// Assets/Scripts/Enums/Notypes.cs 하위에 작성
 public enum NodeType
    {
        Battle = 0,
        Camp,
        Gift,
        Unknown,
        //평소에 나오는 노트 추가시 이 위에 추가할것
        EliteBattle,
        BossBattle
        //평소에는 안나오는 특수 노트 추가시  끝에 추가할것
     }
```

```csharp
// Assets/Scripts/Map

public class NewNode : NodeBase
{

    public override void OnClick(string sceneName)
    {
        base.OnClick(sceneName);
        Debug.Log("New Node Access");
    }
}
```
