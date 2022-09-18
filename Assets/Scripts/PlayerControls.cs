using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] InputAction movement;

    // Start is called before the first frame update
    void Start() {
    }

    void OnEnable() {
        movement.Enable();
    }

    void OnDisable() {
        movement.Disable();
    }

    // Update is called once per frame
    void Update() {
        // TODO: Read this tutorial:
        // https://blog.logrocket.com/building-third-person-controller-unity-new-input-system/
        float horizontalThrow = movement.ReadValue<Vector2>().x;
        float verticalThrow = movement.ReadValue<Vector2>().y;
        if (horizontalThrow != 0) {
            float newXPos = transform.localPosition.x + (horizontalThrow * Time.deltaTime);
            transform.localPosition = new Vector3(
                newXPos, transform.localPosition.y, transform.localPosition.z);
        }
        if (verticalThrow != 0) {
        }
    }
}
