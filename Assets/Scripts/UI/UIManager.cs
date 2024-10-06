using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerController player;
    public float playerSpeed;
    public float temperature;

    public Slider staminaBar;   // ������
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
        // �ƶ�����������
        if (playerSpeed > 0 && staminaBar.value > 0)
        {
            recoverTimer = 0;
            ConsumeStamina();
        }
        // ��ֹ recoverDelay ��󣬻ָ�����
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
        // �ٶ�Խ�죬�¶�Խ�ߣ���������Խ��
        staminaBar.value -= Time.deltaTime * consumeRate * playerSpeed * temperature / 50;

        // ��������1/4ʱ����������죬�ٶ�������ֵ���½��������0.2��
        if(staminaBar.value / staminaBar.maxValue <= 0.25 && player.staminaArgs > 0.2)
        {
            staminaBar.fillRect.GetComponent<Image>().color = redColor;
            player.staminaArgs = staminaBar.value / staminaBar.maxValue * 4;
        }

        if (staminaBar.value <= 0)
        {
            staminaBar.value = 0;
            // ֹͣ�ƶ����������ָ�
            player.staminaArgs = 0;
        }
    }
    void RecoverStamina()
    {
        staminaBar.value += Time.deltaTime * recoverRate * 10;

        // ��������1/4ʱ����������ɫ�������ٶ�����ߵ�����ֵ
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
