using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerController player;
    public float playerSpeed;
    public float temperature;

    public Slider staminaBar;   // 体力条
    public float consumeRate;

    public float recoverDelay;
    public float recoverTimer;
    public float recoverRate;

    private Color normalColor;
    private Color redColor;

    private void Awake()
    {
        staminaBar.maxValue = 100;
        staminaBar.value = 100;
        player.staminaArgs = 1;
        normalColor = new Color(247/255f, 144/255f, 53/255f);
        redColor = Color.red;
    }
    private void Update()
    {
        playerSpeed = player.currentSpeedVec.magnitude;
        // 移动会消耗体力
        if (playerSpeed > 0 && staminaBar.value > 0)
        {
            recoverTimer = 0;
            ConsumeStamina();
        }
        // 静止 recoverDelay 秒后，恢复体力
        if (playerSpeed == 0 && staminaBar.value < staminaBar.maxValue)
        {
            if (recoverTimer < recoverDelay)
            {
                recoverTimer += Time.deltaTime;
            }
            else
            {
                RecoverStamina();
            }
        }
    }
    void ConsumeStamina()
    {
        // 速度越快，温度越高，体力消耗越快
        staminaBar.value -= Time.deltaTime * consumeRate * playerSpeed * temperature / 50;

        // 体力低于1/4时，体力条变红，速度由正常值逐渐下降，最低至0.2倍
        if(staminaBar.value / staminaBar.maxValue <= 0.25 && player.staminaArgs > 0.2)
        {
            staminaBar.fillRect.GetComponent<Image>().color = redColor;
            player.staminaArgs = staminaBar.value / staminaBar.maxValue * 4;
        }

        if (staminaBar.value <= 0)
        {
            staminaBar.value = 0;
            // 停止移动，等体力恢复
            player.staminaArgs = 0;
        }
    }
    void RecoverStamina()
    {
        staminaBar.value += Time.deltaTime * recoverRate * 10;

        // 体力高于1/4时，体力条颜色正常，速度逐渐提高到正常值
        if (staminaBar.value / staminaBar.maxValue <= 0.25)
        {
            staminaBar.fillRect.GetComponent<Image>().color = normalColor;
            player.staminaArgs = Mathf.Max(staminaBar.value / staminaBar.maxValue * 4, player.staminaArgs); 
        }

        if (staminaBar.value >= staminaBar.maxValue)
        {
            staminaBar.value = staminaBar.maxValue;
            recoverTimer = 0;
        }
    }
    public void SetMoveSpeed(float value)
    {
        playerSpeed = value;
    }
    public void SetTemperature(float value) 
    { 
        temperature = value; 
    }
}
