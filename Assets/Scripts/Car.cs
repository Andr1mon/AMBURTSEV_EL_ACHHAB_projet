using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private GameObject[] lamps;
    [SerializeField] private Material turnOnMaterial;
    [SerializeField] private Material turnOffMaterial;
    private Animator _animator;
    private List<Car> _carsInteractingWith = new List<Car>();

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
    }

    // Set initial state
    private void OnEnable()
    {
        Sleep();
    }

    private void OnTriggerEnter(Collider other)
    {
        Car otherCar = other.gameObject.GetComponent<Car>();
        if (otherCar != null) 
        {
            AddCar(otherCar);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Car otherCar = other.gameObject.GetComponent<Car>();
        if (otherCar != null)
        {
            RemoveCar(otherCar);
        }
    }

    private void OnDisable() 
    {
        if (_carsInteractingWith.Count > 0)
        {
            foreach (Car car in _carsInteractingWith)
            {
                car.RemoveCar(this);
            }
        }
    }

    private void AddCar(Car car)
    {
        _carsInteractingWith.Add(car);
        Interact();
    }

    public void RemoveCar(Car car)
    {
        _carsInteractingWith.Remove(car);
        if (_carsInteractingWith.Count == 0)
        {
            Sleep();
        }
    }

    private void Sleep()
    {
        TurnOffLamps();
        _animator.SetBool("IsInteracting", false);
    }

    private void Interact() 
    {
        TurnOnLamps();
        _animator.SetBool("IsInteracting", true);
    }

    private void TurnOnLamps() 
    {
        foreach (GameObject lamp in lamps)
        {
            lamp.gameObject.GetComponent<Renderer>().material = turnOnMaterial;
        }
    }

    private void TurnOffLamps() 
    {
        foreach (GameObject lamp in lamps)
        {
            lamp.gameObject.GetComponent<Renderer>().material = turnOffMaterial;
        }
    }
}
