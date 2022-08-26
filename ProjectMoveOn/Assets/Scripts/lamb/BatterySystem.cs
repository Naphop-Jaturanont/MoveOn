using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    public float Battery = 100f;
    public float MaxBattery = 100f;
    public GameObject FlashLight;
    public float removeBattery = 1.0f;
    public float secondRemove = 1.0f;
    public Slider BatterySlider;
    public GameObject PickUpPanel;
    public bool FlashlightON = false;
    public bool failSafe = false;

    public static BatterySystem instance;
    public GameObject panel;
    public bool open;

    void Start()
    {
        
        Battery = MaxBattery;
        BatterySlider.GetComponent<Slider>().maxValue = MaxBattery;
        BatterySlider.GetComponent<Slider>().value = Battery;
        panel.SetActive(open);
        
        
    }
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        BatterySlider.GetComponent<Slider>().value = Battery;
        if (Battery / MaxBattery * 100 <= 50)
        {
            FlashLight.GetComponent<Light>().intensity = 0.5f;
        }
        if (Battery / MaxBattery * 100 <= 25)
        {
            FlashLight.GetComponent<Light>().intensity = 0.3f;
        }
        if (Battery / MaxBattery * 100 <= 0)
        {
            FlashLight.GetComponent<Light>().intensity = 0.0f;
        }
        if (Input.GetButtonDown("turnFlashlight"))
        {
            
            if (FlashlightON == true && failSafe == false)
            {
                turnOnFlashlight();                
            }
            else
            {
                turnoffFlashlight();
            }
            
        }
    }

    public void OpenBagpack()
    {
        open = !open;
        panel.SetActive(open);
    }

    public void turnOnFlashlight()
    {
        Debug.Log("turnFlashlightIfFalse");
        FlashLight.SetActive(false);
        //Playsound
        FlashlightON = false;
        StartCoroutine(FailSafe());
    }
    public void turnoffFlashlight()
    {
        FlashlightON = !FlashlightON;
        Debug.Log("turnFlashlightIfTrue");
        FlashLight.SetActive(FlashlightON);
        //Playsound        
        StartCoroutine(FailSafe()); 
        if (FlashlightON == true)
        {
            StartCoroutine(RemoveBattery(removeBattery, secondRemove));
        }
        
    }
    public void UseBattery(float value)
    {        
        Battery += value;
        if(Battery > 100)
        {
            Battery = 100;
        }
    }
    public IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(5.0f);
        failSafe = false;
    }
    public IEnumerator RemoveBattery(float value,float time)
    {
        if (FlashlightON == true)
        {
            while (FlashlightON)
            {
                yield return new WaitForSeconds(time += Time.deltaTime);
                if (Battery > 0)
                {
                    Battery -= value;
                }
            }
        }
        
    }
}
