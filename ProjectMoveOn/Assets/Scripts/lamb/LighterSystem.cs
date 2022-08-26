using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LighterSystem : MonoBehaviour
{
    public float Stamina = 100f;
    public float MaxStamina = 100f;
    public Slider StaminaSlider;
    private StarterAssetsInputs _input;

    public static LighterSystem lighterSystem;

    private void Awake()
    {
        lighterSystem = this;
    }
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        Stamina = MaxStamina;
        StaminaSlider.GetComponent<Slider>().maxValue = MaxStamina;
        StaminaSlider.GetComponent<Slider>().value = Stamina;
    }
    void Update()
    {
        StaminaSlider.GetComponent<Slider>().value = Stamina;

        //inputsystem

        /*if (_input.openLighter)
        {
            StartCoroutine(RemoveStamina(0.08f, 0.5f));
        }
        if(Stamina < MaxStamina)
        {
            StartCoroutine(RecoveryStamina(0.01f, 3f));
        }if(Stamina == 0)
        {
            _input.openLighter = false;
        }*/
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
