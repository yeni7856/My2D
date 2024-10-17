using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    //캐릭터와 관련된 이벤트 함수들 관리 클래스 스태틱으로 관리
    public class CharacterEvents
    {
        //캐릭터가 데미지를 입을때 등록된 함수 호출
        public static UnityAction<GameObject, float> characterDamaged;
        //캐릭터가 힐할때 등록된 함수 호출
        public static UnityAction<GameObject, float> characterHealed;
    }
}
