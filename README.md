# HomeSweetHome

https://github.com/TimeFlies-KR/home-sweet-home
커밋 기록

Home Sweet Home<br>
2D 액션 플랫포머 팀 프로젝트<br>
개발 기간 : 2024.12 ~ 2025.06<br>
개발 인원 : 4명<br>
담당 : 플레이어 시스템 / 전투 / 카메라 / UI 등<br>
Engine : Unity 2022.3.50f1<br>
Language : C#<br>


키보드 커스텀 설정<br>
<img width="640" height="360" alt="2025-07-13-gameplay" src="https://github.com/user-attachments/assets/a08af647-8197-4046-910e-6d9febe2d73e" />

>[!Note]
> 게임 동작을 KeyAction 열거형으로 구분하고, 각 동작에 대응하는 KeyCode를 Dictionary에 저장

public enum KeyAction { UP, DOWN, LEFT, RIGHT ,ATTACK,JUMP,DASH,BLOCK,SKILL,OPEN,KEYCOUNT}
public static class KeySetting { public static Dictionary<KeyAction, KeyCode>keys  = new Dictionary<KeyAction, KeyCode>();}

>[!Note]
> 기본적인 키보드 조작은 미리 설정해두고 UI에서 클릭후 키보드 입력시 원하는 키로 변경, 특정키는 막아두고 싶다면 if로 막아두기

public class KeyManager : MonoBehaviour
{
    public GameObject KeyboardCanvas;
    private SelectOption selectOption;
    // 기본값을 미리 지정
    KeyCode[] defaultKeys = new KeyCode[] {KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.RightArrow,KeyCode.A,KeyCode.W,KeyCode.S,KeyCode.R, KeyCode.Q,KeyCode.E};
    
    // 생략 // 
    
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            // 0~3은 방향키 이동이라서 변경 금지
            if (key < 4) return;
            bool canChange = true;
            for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
            {
                // 이미 해당하는 키가 할당되어있는경우 무시
                if (KeySetting.keys[(KeyAction)i] == keyEvent.keyCode)
                {
                    canChange = false;
                    break;
                }
            }
            // 입력한 키로 할당
            if (canChange)
                KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;

            key = -1;
        }        
    }
    // 생략 //
}



하단점프<br>
<img width="640" height="360" alt="2025-07-13-20-05-39-github" src="https://github.com/user-attachments/assets/67e16544-e786-459f-ab8e-9bd542029193" />


- 코드로 점프, 하단점프시 바닥에 대한 판정을 처리할려고 하니 발판과 겹치게되면 캐릭터가 튀어올라 발판위로 올라가는 현상이 발생하여
- 유니티에 있는 Platform Effector 2D를 이용하여 처리하였더니 문제가 해결되었음<br>
<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/bc7b385a-a1a4-4d52-bb5f-5ed56239f892" />

>[!Note]
>하단점프시 현재 플레이어의 발과 접촉해있는 발판에대한 정보를 찾고 그 발판에 대한 물리접촉을 무시하게 처리하여 바닥을 뚫고 이동하며, 겹치는 경우에도 바닥을 무시하여 자연스럽게 하단점프

    void DropDown()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, FloorLayer);
       // Bhit은 경사로인지 체크하는 RaycastHit2D
        if(hit || Bhit)
        {
            hitCollider = hit.collider ? hit.collider : Bhit.collider;
        }
        if (hitCollider != null)
        {
            isDropping = true;

            if (!currentPlatforms.Contains(hitCollider))
            {
                print(hitCollider.name);
                currentPlatforms.Add(hitCollider);
                Physics2D.IgnoreCollision(playerCollider, hitCollider, true);
            }
        }
    }

>[!Note]
> IgnoreCollision 처리했던 발판의 경우는 멀어지면 아래 코드를 이용하여 IgnoreCollision을 false로 처리해서 정상적인 발판 상태가 되게 설정

void CheckColliderOverlap()
{
    //생략//

    // 현재 접촉중인 오브젝트가 하단점프한 오브젝트에 해당하는지 체크하고 없다면 제거할 리스트에 추가
    foreach (Collider2D col in currentPlatforms)
    {
        if (!currentHits.Contains(col))
            remove.Add(col);
    }
    if (remove.Count > 0)
    {
        foreach (Collider2D col in remove)
        {
            // 제거리스트에 저장된 오브젝트가 있는경우 Ignore을 해제하고 리스트에서 제거
            if (currentPlatforms.Contains(col))
            {
                Physics2D.IgnoreCollision(playerCollider, col, false);
                currentPlatforms.Remove(col);
            }

        }
    }
}


PolygonCollider를 이용하여 카메라의 범위를 제한<br>
<img width="367" height="404" alt="image" src="https://github.com/user-attachments/assets/816b6c62-657a-455b-84f2-e63c2b42e34a" /><br>
<img width="636" height="275" alt="image" src="https://github.com/user-attachments/assets/7c2b7343-41a6-4dbf-9f55-fb29e0412c07" /><br>
<img width="367" height="118" alt="image" src="https://github.com/user-attachments/assets/e379b10f-af75-4c45-92ab-0058eb3d9693" /><br>
시네마틱 카메라를 이용하여 현재있는 맵의 PolygonCollider를 넘기 못하게 설정

<img width="960" height="411" alt="2026-09-15-13-52-32-github" src="https://github.com/user-attachments/assets/bd7dcbdc-bf95-4391-b42a-f0161f5a5651" />

