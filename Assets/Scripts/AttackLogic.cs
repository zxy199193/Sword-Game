using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class AttackLogic : MonoBehaviour
{
    public float attackValue;
    public float actualDamage;
    public float attackFactor;
    public ActionEnum currentActionEnum;
    public Text actionName;
    public Text actionDamage;
    public ActionSelection actionSelection;
    public ActionData currentActionData;
    public event System.Action<float,int> OnAttackDamage;
    public int actionAni;
    public bool isAIAttacking = false;
    public GameObject attackButton;
    public GameObject popupPanel;

    private bool isAiming = true;
    private bool isSlowDown = false;
    public float sliderValue;
    public float sliderSpeed;
    private float initialsliderValue = 0.5f;
    private float initialSliderSpeed = 0.005f;
    public float lastSliderValue = 0f;
    private float decayRate = 0.01f;
    public int directionFactor = 1;
    public Slider slider;
    public Text sliderValueText;
    public int hitLevel = 0;

    public int hitSectio_Lv0 = 100;
    public int hitSectio_Lv1 = 60;
    public int hitSectio_Lv2 = 20;
    public int hitSectio_Lv3 = 5;
    public int hitSectio_Lv4 = 0;
    public int hitSectio_Lv5 = 0;

    public float hitFactor_Lv0 = 0;
    public float hitFactor_Lv1 = 1f;
    public float hitFactor_Lv2 = 1.35f;
    public float hitFactor_Lv3 = 2f;
    public float hitFactor_Lv4 = 3f;
    public float hitFactor_Lv5 = 5f;

    public GameObject hitSection_Lv1;
    public GameObject hitSection_Lv2;
    public GameObject hitSection_Lv3;
    public GameObject hitSection_Lv4;
    public GameObject hitSection_Lv5;

    public float aiBreakPos = 0f;
    public float aiBreakDirection = 0f;
    private static readonly System.Random rand = new System.Random();

    private void Update()
    {
        UpdateUI();
        SliderMovement();
        Slash();
        if(isSlowDown)
        {
            if(sliderSpeed <= 0)
            {
                GetHitFactor(sliderValue);
                StartCoroutine(DealActualDamage());
                isSlowDown = false;
            }
        }
        if (isAIAttacking)
        {
            if (sliderValue >= aiBreakPos - 0.005 && sliderValue <= aiBreakPos+0.005 && directionFactor == aiBreakDirection)
            {
                OnSlashButton();
                isAIAttacking = false;
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
        attackValue = currentActionData.attackDamage;
        actionDamage.text = "Basic Damage: " + attackValue.ToString();
        initialSliderSpeed = currentActionData.attackSliderSpeed;
        decayRate = currentActionData.attackSliderDecay;
        hitSectio_Lv1 = currentActionData.attackHitSection_Lv1;
        hitSectio_Lv2 = currentActionData.attackHitSection_Lv2;
        hitSectio_Lv3 = currentActionData.attackHitSection_Lv3;
        hitSectio_Lv4 = currentActionData.attackHitSection_Lv4;
        hitSectio_Lv5 = currentActionData.attackHitSection_Lv5;
        actionAni = currentActionData.actionAni;
        Initialize();
    }
    
    public void Initialize()
    {
        sliderSpeed = initialSliderSpeed;
        sliderValue = initialsliderValue;
        slider.value = sliderValue;
        isAiming = true;
        hitLevel = 0;
        RectTransform rectTransform_Lv1 = hitSection_Lv1.GetComponent<RectTransform>();
        rectTransform_Lv1.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hitSectio_Lv1 * 5);
        RectTransform rectTransform_Lv2 = hitSection_Lv2.GetComponent<RectTransform>();
        rectTransform_Lv2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hitSectio_Lv2 * 5);
        RectTransform rectTransform_Lv3 = hitSection_Lv3.GetComponent<RectTransform>();
        rectTransform_Lv3.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hitSectio_Lv3 * 5);
        RectTransform rectTransform_Lv4 = hitSection_Lv4.GetComponent<RectTransform>();
        rectTransform_Lv4.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hitSectio_Lv4 * 5);
        RectTransform rectTransform_Lv5 = hitSection_Lv5.GetComponent<RectTransform>();
        rectTransform_Lv5.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hitSectio_Lv5 * 5);
    }
    
    public void UpdateUI()
    {
        if (Mathf.Abs(sliderValue - lastSliderValue) > Mathf.Epsilon)
        {
            slider.value = sliderValue;
            sliderValueText.text = "Aiming: " + (sliderValue*100-50).ToString("F1");
            lastSliderValue = sliderValue;
        }
    }
    public void SliderMovement()
    {
        sliderValue = Mathf.Clamp01(sliderValue + directionFactor * sliderSpeed*Time.deltaTime);
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
        actualDamage = attackValue * attackFactor;
        Debug.Log("Attack Dmg: "+attackValue+ "  /Attack Factor: " + attackFactor+ "  /Slider Value: " + sliderValue+ "  /Actual Dmg: " + actualDamage);
        yield return new WaitForSeconds(1.5f);
        OnAttackDamage?.Invoke(actualDamage,actionAni);
        popupPanel.SetActive(false);
        attackButton.SetActive(true);
        yield return null;
    }

    public void GetHitFactor(float a)
    {
        float num = a*100-50;
        if (num >= -hitSectio_Lv0/2 && num <= hitSectio_Lv0/2) hitLevel = 0;
        if (num >= -hitSectio_Lv1/2 && num <= hitSectio_Lv1/2) hitLevel = Mathf.Max(hitLevel, 1);
        if (num >= -hitSectio_Lv2/2 && num <= hitSectio_Lv2/2) hitLevel = Mathf.Max(hitLevel, 2);
        if (num >= -hitSectio_Lv3/2 && num <= hitSectio_Lv3/2) hitLevel = Mathf.Max(hitLevel, 3);
        if (num >= -hitSectio_Lv4/2 && num <= hitSectio_Lv4/2) hitLevel = Mathf.Max(hitLevel, 4);
        if (num >= -hitSectio_Lv5/2 && num <= hitSectio_Lv5/2) hitLevel = Mathf.Max(hitLevel, 5);
        switch (hitLevel)
        {
            case 0:
                attackFactor = hitFactor_Lv0;
                break;
            case 1:
                attackFactor = hitFactor_Lv1;
                break;
            case 2:
                attackFactor = hitFactor_Lv2;
                break;
            case 3:
                attackFactor = hitFactor_Lv3;
                break;
            case 4:
                attackFactor = hitFactor_Lv4;
                break;
            case 5:
                attackFactor = hitFactor_Lv5;
                break;

        }
        Debug.Log("Hit Level =" + hitLevel + "/" + "Attack Factor = " + attackFactor);
    }

    public IEnumerator OnAIButton()
    {
        attackButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        float devFactor = 0.01f;
        float deviation = (float)((-devFactor) + (2 * devFactor) * rand.NextDouble());
        BrakeResult result = BallBrakeSolver.GetBrakePosition(0.5f, sliderValue, directionFactor, sliderSpeed, decayRate, 0);
        //Debug.Log($"减速点: {result.breakPos}, 方向: {result.direction}");
        aiBreakPos = deviation+0.5f +result.breakPos;
        aiBreakDirection = result.direction;
        isAIAttacking = true;
    }
}
