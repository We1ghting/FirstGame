using UnityEngine;
using TMPro;

public class DebugConsoleManager : MonoBehaviour
{
    public static DebugConsoleManager Instance;
    public GameObject consolePanel;
    public TMP_InputField inputField;
    public TextMeshProUGUI logText;

    public static bool isInvincible = false;
    public static bool isSeckill = false;

    void Awake()
    {
        Instance = this;
        consolePanel.SetActive(false);
    }

    void Update()
    {
        // 【修改261015】按 ` 键切换控制台，并同步暂停/恢复游戏
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            bool isActive = consolePanel.activeSelf;
            consolePanel.SetActive(!isActive);

            if (consolePanel.activeSelf)
            {
                inputField.ActivateInputField();
                Time.timeScale = 0f; // 暂停游戏
            }
            else
            {
                Time.timeScale = 1f; // 恢复游戏
            }
        }

        // 【修改261015】如果控制台打开，按回车键提交命令
        if (consolePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand(inputField.text);
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    void ExecuteCommand(string command)
    {
        if (string.IsNullOrEmpty(command)) return;

        string[] parts = command.Trim().Split(' ');
        string cmd = parts[0].ToLower();

        if (cmd == "/g" && parts.Length > 1)
        {
            if (int.TryParse(parts[1], out int cardIndex))
            {
                // 判空保护
                if (CardManager.Instance != null && CardManager.Instance.allCards != null && cardIndex > 0 && cardIndex <= CardManager.Instance.allCards.Length)
                {
                    CardData card = CardManager.Instance.allCards[cardIndex - 1];
                    if (card != null)
                    {
                        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                        player.ApplyCardEffect(card);
                        Log($"获得了卡牌：{card.cardName}");
                    }
                    else
                    {
                        Log($"编号 {cardIndex} 的卡牌为空。");
                    }
                }
                else
                {
                    Log($"找不到编号为 {cardIndex} 的卡牌，请检查 CardManager 里的 All Cards 数组有没有填满18张。");
                }
            }
        }
        else if (cmd == "/levelup")
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.LevelUp();
            Log("执行：升一级");
        }
        else if (cmd == "/noinjury")
        {
            isInvincible = !isInvincible;
            Log($"无敌模式：{(isInvincible ? "开启" : "关闭")}");
        }
        else if (cmd == "/seckill")
        {
            isSeckill = !isSeckill;
            Log($"秒杀模式：{(isSeckill ? "开启" : "关闭")}");
        }
        else
        {
            Log($"未知指令：{command}");
        }
    }

    void Log(string message)
    {
        if (logText != null)
        {
            logText.text += "\n> " + message;
        }
        Debug.Log(message); // 顺便在 Unity 控制台也打印一下，方便排查
    }
}