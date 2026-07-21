===ASH_01===
#speaker:시타
"Test of Ink First!"

#speaker:솔퓌르
"Next Line is <color=yellow> this </color>"

#speaker:리겔
혹시 모르니 한글도.

#speaker:솔퓌르
"Next Line is <color=yellow> this </color>"

#speaker:리겔
<color="yellow">4th</color> try of <color=\#FFC900>this.</color>
->DONE

===ASH_02===
#speaker:물약 상인 #sheet:DialogueSprite01 #mood:Sulphur #layout:right
어흐흑.....

#speaker:시타 #sheet:SitaMoodSheet #mood:Plain #layout:left
(...)
*[왜 우는지 묻는다]
    #speaker:시타 #sheet:SitaMoodSheet #mood:Plain #layout:left
    왜 울고있지?
    #speaker:물약 상인 #sheet:DialogueSprite01 #mood:Sulphur #layout:right
    도둑들이 <color="yellow">가마</color>를 훔쳐가서...
    #speaker:시타 #sheet:SitaMoodSheet #mood:Contempt #layout:left
    <color="yellow">가마</color>라고...? 그게 그렇게 소중한건가?
    #speaker:물약 상인 #sheet:DialogueSprite01 #mood:Sulphur #layout:right
    어흐흑.... 그게 있어야 장사를 할 수 있거든요.....
    알고 있습니다. 왜 하필 도둑들이 그렇게 크고 무거운걸 훔쳐갔는지 저도 잘 모르겠어요... 유리병 하나에 단가가 얼만데... 그런거나 훔쳐가지...
    #speaker:시타 #sheet:SitaMoodSheet #mood:Anxious #layout:left
    (그런건 훔쳐가도 되는건가...?)
    (아니 애초에 도둑맞은 사람이 훔쳐간 물건을 보통 따지나?)
    #speaker:물약 상인 #sheet:DialogueSprite01 #mood:Sulphur #layout:right
    저번에 연구 자료만 훔쳐갔을때는 그래도 상도덕이 있는 놈들인줄줄 알았는데....
    #speaker:시타 #sheet:SitaMoodSheet #mood:Anger #layout:left
    뭐라고? 연구 자료? 너 마법사야? 평범한 상인이 아니야?
    당장 말해! 무슨 연구를 했으며, 왜 빼앗겼으며, 누가 가져갔지? 그놈들은 그 자료로 무엇을 하려는거지?
    #speaker:물약 상인 #sheet:DialogueSprite01 #mood:Sulphur #layout:right
    ...
    마법 얘기가 나오니까 흥분하시네요?
    그쪽은 교단은 아닌 것 같은데...
    #speaker:리겔 #sheet:DialogueSprite01 #mood:Rigel #layout:left
    시타. 참아. 너의 힘과 감정을 통제하려면...
    #speaker:시타 #sheet:SitaMoodSheet #mood:Plain #layout:left
    닥쳐.
    #speaker:시타 #sheet:SitaMoodSheet #mood:Anger #layout:left
    너는 빨리 말해. 죽을 일 만들지 말고.
    #speaker:물약 상인 #sheet:DialogueSprite01 #mood:Sulphur #layout:right
    ...아하.
    그런거구나.
    그럼 우선 연구 자료를 좀 찾아다줄래요?
    적어도 이 도시에서 당신이 찾는건 그 안에 다 있을텐데.
    #speaker:시타 #sheet:SitaMoodSheet #mood:Plain #layout:left
    (...)
    **[연구 자료를 찾아다준다]
        #speaker:시타 #sheet:SitaMoodSheet #mood:Plain #layout:left
        거짓말이면... 죽이겠어.
        ->END
    
    **[거절한다]
        #speaker:시타 #sheet:SitaMoodSheet #mood:Plain #layout:left
        난 지금 듣고싶어.
        ->END
*[다그친다]
    #speaker:시타 #sheet:SitaMoodSheet #mood:Anger #layout:left
    닥치지 못해?
    ->END
->DONE