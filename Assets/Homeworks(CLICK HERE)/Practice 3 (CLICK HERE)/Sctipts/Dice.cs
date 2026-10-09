using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private DiceType diceType;
    private DiceSide[] diceSides;
    private Rigidbody rigidBody;

    public float DiceSize
    {
        get => transform.localScale.x;
        set => transform.localScale = value * Vector3.one;
    }

    public DiceType Type
    {
        get => diceType;
        set
        {
            diceType = value;
            GenerateSides();
        }
    }

    public bool IsStopped => rigidBody != null && rigidBody.IsSleeping();

    private void OnValidate()
    {
        GenerateSides();
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        GenerateSides();
    }

    public void Throw(Vector3 force, Vector3 torque)
    {
        rigidBody.AddForce(force, ForceMode.Impulse);
        rigidBody.AddTorque(torque, ForceMode.Impulse);
    }

    public int GetValue()
    {
        var value = 0;
        var maxDot = float.MinValue;
        foreach (var diceSide in diceSides)
        {
            var sideWorldDir = transform.TransformDirection(diceSide.Direction);

            var currentDot = Vector3.Dot(Vector3.up, sideWorldDir);
            if (currentDot > maxDot) {
                maxDot = currentDot;
                value = diceSide.Value;
            }

        }
        return value;
    }

    private void GenerateSides()
    {
        switch (diceType)
        {
            case DiceType.D6:
                diceSides = new DiceSide[]
                {
                    new DiceSide(Vector3.up, 6),
                    new DiceSide(Vector3.forward, 3),
                    new DiceSide(Vector3.down, 5),
                    new DiceSide(Vector3.back, 1),
                    new DiceSide(Vector3.left, 2),
                    new DiceSide(Vector3.right, 4)
                };
                break;
            case DiceType.D4:
                diceSides = new DiceSide[]
                {
                    new DiceSide(new Vector3(-0.57735f,  0.57735f, -0.57735f), 1),
                    new DiceSide(new Vector3(-0.57735f, -0.57735f,  0.57735f), 2),
                    new DiceSide(new Vector3( 0.57735f, -0.57735f, -0.57735f), 3),
                    new DiceSide(new Vector3( 0.57735f,  0.57735f,  0.57735f), 4)
                };
                break;
            case DiceType.D8:
                diceSides = new DiceSide[]
                {
                    new DiceSide(new Vector3(-0.57735f,  0.57735f, -0.57735f), 1),
                    new DiceSide(new Vector3( 0.57735f,  0.57735f, -0.57735f), 2),
                    new DiceSide(new Vector3( 0.57735f,  0.57735f,  0.57735f), 3),
                    new DiceSide(new Vector3(-0.57735f,  0.57735f,  0.57735f), 4),
                    new DiceSide(new Vector3(-0.57735f, -0.57735f,  0.57735f), 5),
                    new DiceSide(new Vector3( 0.57735f, -0.57735f,  0.57735f), 6),
                    new DiceSide(new Vector3( 0.57735f, -0.57735f, -0.57735f), 7),
                    new DiceSide(new Vector3(-0.57735f, -0.57735f, -0.57735f), 8)
                };
                break;
            default:
                Debug.LogWarning("Нету типа кубика");
                break;
        }
    }
}

[Serializable]
public class DiceSide
{
    [SerializeField] private Vector3 localDirection;
    [SerializeField] private int value;

    public Vector3 Direction => localDirection;
    public int Value => value;
    
    public DiceSide(Vector3 LocalDirection, int sideValue)
    {
        this.localDirection = LocalDirection.normalized;
        value = sideValue;
    }
}


public enum DiceType
{
    D6,
    D4,
    D8
}
