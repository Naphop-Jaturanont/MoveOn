using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LighterSystem : MonoBehaviour
{
    public static LighterSystem instance;
    public float Stamina = 100f;
    public float MaxStamina = 100f;
    //public Slider StaminaSlider;
    public Light light;
    public bool openlamb = false;
    public keep keepcode;
    private StarterAssetsInputs _input;

    //public static LighterSystem lighterSystem;

    private void Awake()
    {
      //  lighterSystem = this;
    }
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        light = GetComponent<Light>();
        keepcode = GetComponent<keep>();
        Stamina = MaxStamina;
        //StaminaSlider.GetComponent<Slider>().maxValue = MaxStamina;
        //StaminaSlider.GetComponent<Slider>().value = Stamina;
    }
    void Update()
    {
        //StaminaSlider.GetComponent<Slider>().value = Stamina;

        //inputsystem
        if(keepcode.keeped == true)
        {
            if (Input.GetKeyDown(KeyCode.F) && openlamb == false  )
            {
                light.range = 20.00f;
                openlamb = true;
                return;
            }
            if (Input.GetKeyDown(KeyCode.F) && openlamb == true)
            {
                light.range = 5.00f;
                openlamb = false;
                return;
            }
        }

        
        
    }
   public IEnumerator RemoveStamina(float value, float time)
    {
        yield return new WaitForSeconds(time);
        if (Stamina > 0)
        {
            Stamina -= value;
        }
    }
    public void GainStamina(float value)
    {
        Stamina += value;
        if (Stamina > MaxStamina)
        {
            Stamina = MaxStamina;
        }
    }

    public IEnumerator RecoveryStamina(float value,float time)
    {
        yield return new WaitForSeconds(time);
        Stamina += value;
    }
}
