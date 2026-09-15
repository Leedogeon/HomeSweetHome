# HomeSweetHome

https://github.com/TimeFlies-KR/home-sweet-home
커밋 기록


키보드 커스텀 설정<br>
<img width="640" height="360" alt="2025-07-13-gameplay" src="https://github.com/user-attachments/assets/a08af647-8197-4046-910e-6d9febe2d73e" />

>[!Note]
> 게임 동작을 KeyAction 열거형으로 구분하고, 각 동작에 대응하는 KeyCode를 Dictionary에 저장

public enum KeyAction { UP, DOWN, LEFT, RIGHT ,ATTACK,JUMP,DASH,BLOCK,SKILL,OPEN,KEYCOUNT}
public static class KeySetting { public static Dictionary<KeyAction, KeyCode>keys  = new Dictionary<KeyAction, KeyCode>();}


public class KeyManager : MonoBehaviour
{
    public GameObject KeyboardCanvas;
    private SelectOption selectOption;

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
    int key = -1;
    public void ChangeKey(int num)
    {
        key = num;
    }

    public void retrunBtn()
    {
        if (selectOption != null)
        {
            selectOption.UIVisible(selectOption.self);
        }
        KeyboardCanvas.SetActive(false);
    }
}



하단점프<br>
<img width="640" height="360" alt="2025-07-13-20-05-39-github" src="https://github.com/user-attachments/assets/67e16544-e786-459f-ab8e-9bd542029193" />


- 코드로 점프, 하단점프시 바닥에 대한 판정을 처리할려고 하니 발판과 겹치게되면 캐릭터가 튀어올라 발판위로 올라가는 현상이 발생하여
- 유니티에 있는 Platform Effector 2D를 이용하여 처리하였더니 문제가 해결되었음<br>
<img width="360" height="200" alt="image" src="https://github.com/user-attachments/assets/bc7b385a-a1a4-4d52-bb5f-5ed56239f892" />
