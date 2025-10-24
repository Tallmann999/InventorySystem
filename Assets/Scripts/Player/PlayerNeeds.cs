using System;
using TMPro;
using UnityEngine;

public class PlayerNeeds : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _thristText;
    [SerializeField] private TextMeshProUGUI _hungryText;
    [SerializeField] private TextMeshProUGUI _energyText;

    public float Health = 100;
    public float Thrist = 100;
    public float Hungry = 100;
    public float Energy = 100;


    private void Update()
    {
        _healthText.text= Health.ToString();
        _thristText.text= Thrist.ToString();
        _hungryText.text= Hungry.ToString();
        _energyText.text= Energy.ToString();
    }

    public void AddValue(float value)
    {
        Health += value;
    }

    public  void Heal(float value)
    {
        Health += value; // создание метода пополняющее здоровье до максимума
    }

    public void Eat(float value)
    {
        Hungry += value;
    }

    public void Drink(float value)
    {
        Thrist += value;
    }

    public void DrinkEnergy(float value)
    {
        Energy+= value;
    }
}
