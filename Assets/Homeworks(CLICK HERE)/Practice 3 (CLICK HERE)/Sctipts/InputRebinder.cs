using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebinder : MonoBehaviour
{
    public void Rebind(InputAction actionToRebind)
    {
        actionToRebind.Disable();

        actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position") // фикс от ребаиндинга на поз мышиЫ
            .OnComplete(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose(); //защита от утечки памяти

                Debug.Log($"Перебинд кнопки - {actionToRebind.GetBindingDisplayString()}");
            }).Start();

    }
}
