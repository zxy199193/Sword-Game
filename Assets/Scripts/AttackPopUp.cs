using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class AttackPopUp : MonoBehaviour
{
    public float attackValue;
    public float actualDamage;
    public float attackFactor;
    public ActionEnum currentActionEnum;
    public Text actionName;
    public Text actionDamage;
    public ActionSelection actionSelection;
    public ActionData currentActionData;
    public event System.Action<float> OnAttackDamage;

    private bool isAiming = true;
    private bool isSlowDown = false;
    public float sliderValue;
    public float sliderSpeed;
    public float initialsliderValue = 0.5f;
    public float initialSliderSpeed = 0.005f;
    public float lastSliderValue = 0f;
    public float decayRate = 0.01f;
    public int directionFactor = 1;
    public Slider slider;
    public Text sliderValueText;

    private void Update()
    {
        UpdateUI();
        SliderMovement();
        Slash();
        if(isSlowDown)
        {
            if(sliderSpeed <= 0)
            {
                StartCoroutine(DealActualDamage());
                isSlowDown = false;
            }
        }
    }
    public void UseAction()
    {
        foreach (ActionData action in actionSelection.actionList)
        {
            if (action.actionEnum == currentActionEnum)
            {
                currentActionData = action;
                break;
            }
        }
        actionName.text = currentActionData.actionName;
        attackValue = currentActionData.actionValue_01;
        actionDamage.text = "Basic Damage: " + attackValue.ToString();
        Initialize();
    }
    
    public void Initialize()
    {
        sliderSpeed = initialSliderSpeed;
        sliderValue = initialsliderValue;
        slider.value = sliderValue;
        isAiming = true;
    }
    
    public void UpdateUI()
    {
        if (Mathf.Abs(sliderValue - lastSliderValue) > Mathf.Epsilon)
        {
            slider.value = sliderValue;
            sliderValueText.text = "Aiming: " + sliderValue.ToString("F1") + "%";
            lastSliderValue = sliderValue;
        }
    }
    public void SliderMovement()
    {
        sliderValue = Mathf.Clamp01(sliderValue + directionFactor * sliderSpeed);
        if (sliderValue >= 1.0f)
        {
            directionFactor = -1;
        }
        else if (sliderValue <= 0)
        {
            directionFactor = 1;
        }
    }
    public void Slash()
    {
        if (!isAiming)
        {
            sliderSpeed = Mathf.Max(sliderSpeed - decayRate * Time.deltaTime, 0f);
        }
    }

    public void OnSlashButton()
    {
        isAiming = false;
        isSlowDown = true;
    }
    IEnumerator DealActualDamage()
    {
        actualDamage = attackValue * attackFactor * sliderValue;
        Debug.Log(attackValue);
        Debug.Log(attackFactor);
        Debug.Log(sliderValue);
        Debug.Log(actualDamage);
        yield return new WaitForSeconds(2f);
        OnAttackDamage?.Invoke(actualDamage);
        gameObject.SetActive(false);
        yield return null;
    }

}
