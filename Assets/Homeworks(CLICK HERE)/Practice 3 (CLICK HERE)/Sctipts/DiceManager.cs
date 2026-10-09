
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(PlayerInput))]
public class DiceManager : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<DiceType, Dice> prefabDict;
    [SerializeField] private List<DiceType> initialDices = new List<DiceType> { DiceType.D6, DiceType.D4 };
    private List<Dice> activeDices;

    [SerializeField] private float minForce = 1f;
    [SerializeField] private float maxForce = 2f;
    [SerializeField] private float minTorque = 10f;
    [SerializeField] private float maxTorque = 16f;


    private void Awake()
    {
        activeDices = new List<Dice>();
    }
    private void Start()
    {
        foreach (var type in initialDices)
        {
            SpawnDice(type);
        }
    }

    private void OnRoll(InputValue value)
    {
        if (value.isPressed)
        {
            ThrowAll();
        }
    }

    public void SpawnDice(DiceType type)
    {
        if (prefabDict.TryGetValue(type, out var prefab) && prefab != null)
        {
            var dice = Instantiate(prefab, transform.position + Random.insideUnitSphere, Quaternion.identity);
            activeDices.Add(dice);
        }
        else
            Debug.LogError("Префаба нет");
    }

    public void RemoveDice(Dice dice)
    {
        if (activeDices.Contains(dice))
        {
            activeDices.Remove(dice);
            Destroy(dice.gameObject);
        }
    }

    public void RemoveDice(DiceType type)
    {
        foreach (var dice in activeDices)
        {
            if (dice.Type == type)
            {
                activeDices.Remove(dice);
                Destroy(dice.gameObject);
                break;
            }
        }
    }

    private bool AllDiceStopped()
    {
        foreach (var dice in activeDices)
        {
            if (!dice.IsStopped)
            {
                return false;
            }
        }
        return true;
    }
    public int GetAllValues()
    {
        int total = 0;
        foreach (var dice in activeDices)
        {
            int value = dice.GetValue();
            total += value;
            Debug.Log($"кубик {dice.Type} выбросил {value}");
        }

        return total;
    }

    public void ThrowAll()
    {
        StopAllCoroutines();

        foreach (var dice in activeDices)
        {
            Vector3 forceDirection = (Vector3.up + Random.insideUnitSphere).normalized;
            Vector3 force = forceDirection * Random.Range(minForce, maxForce);

            Vector3 torque = Random.insideUnitSphere.normalized * Random.Range(minTorque, maxTorque);

            dice.Throw(force, torque);
        }

        StartCoroutine(CalculateCoRoutine());
    }

    private IEnumerator CalculateCoRoutine()
    {
        yield return new WaitUntil(AllDiceStopped);

        int totalScore = GetAllValues();
        Debug.Log($"cумма очков {totalScore}");
    }
}
