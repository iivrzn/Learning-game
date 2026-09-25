using System;
using UnityEngine;

public class SpinningScript : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float speedInDeg;
    [SerializeField] private bool isClockwise;

    [SerializeField] private GameObject spinningObject;
    [SerializeField] private int cubeCount;
    [SerializeField] private bool AreArranged;
    [SerializeField] private float chainDistanceInDeg;

    private Transform[] cubes;
    private float currentAngle = 0;
    void Awake()
    {
        cubes = new Transform[cubeCount];
        for (int i = 0; i < cubeCount; i++)
        {
            GameObject cube = Instantiate(spinningObject, transform);
            cubes[i] = cube.transform;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (cubes == null || cubes.Length == 0) return;

        int direction = isClockwise ? 1 : -1;
        currentAngle += direction * speedInDeg * Time.deltaTime;

        float angleDifference = AreArranged ? 360f / cubeCount : chainDistanceInDeg;
        for (int i = 0; i < cubeCount; i++)
        {
            float angle = (currentAngle + (i * angleDifference)) * Mathf.Deg2Rad;

            Vector3 cubeOffset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            cubes[i].position = transform.position + cubeOffset;

            cubes[i].LookAt(transform.position);
        }
    }
}
