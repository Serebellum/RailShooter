using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputReader))]
public class PlayerControls : MonoBehaviour
{
    InputReader inputReader;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float xRange = 5f;
    [SerializeField] float yRange = 3f;
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = 4f;
    [SerializeField] float controlPitchRange = -15f;
    float pitchLerp = 0.5f;
    [SerializeField] float positionYawFactor = -4f;

    [SerializeField] float controlRollFactor = 4f;
    [SerializeField] float rollRange = -15f;
    float rollLerp = 0.5f;

    void Start() {
        inputReader = GetComponent<InputReader>();
    }

    void Update() {
        ProcessMovement();
        ProcessRotation();
    }

    void ProcessMovement() {
        // TODO: Read this tutorial:
        // https://blog.logrocket.com/building-third-person-controller-unity-new-input-system/
        float horizontalThrow = inputReader.moveComposite.x;
        float verticalThrow = inputReader.moveComposite.y;
        if (!AlmostEquals(horizontalThrow, 0f) || !AlmostEquals(verticalThrow, 0f)) {
            float newXPos = transform.localPosition.x + (horizontalThrow * movementSpeed * Time.deltaTime);
            newXPos = Mathf.Clamp(newXPos, -xRange, xRange);
            float newYPos = transform.localPosition.y + (verticalThrow * movementSpeed * Time.deltaTime);
            newYPos = Mathf.Clamp(newYPos, -yRange, yRange);
            transform.localPosition = new Vector3(
                newXPos, newYPos, transform.localPosition.z);
        }
    }

    void ProcessRotation() {
        float pitchFromPosition = transform.localPosition.y * positionPitchFactor;
        ComputePitchLerp();
        float pitchFromControl = Mathf.Lerp(-controlPitchRange, controlPitchRange, pitchLerp);
        float pitch = pitchFromPosition + pitchFromControl;
        float yaw = transform.localPosition.x * positionYawFactor;
        ComputeRollLerp();
        float roll = Mathf.Lerp(-rollRange, rollRange, rollLerp);
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ComputePitchLerp() {
        if (!AlmostEquals(inputReader.moveComposite.y, 0f)) {
            if (inputReader.moveComposite.y < 0f) {
                pitchLerp = Mathf.Max(0f, pitchLerp - controlPitchFactor * Time.deltaTime);
            } else {
                pitchLerp = Mathf.Min(1f, pitchLerp + controlPitchFactor * Time.deltaTime);
            }
        } else if (!AlmostEquals(pitchLerp, 0.5f)) {
            if (pitchLerp < 0.5f) {
                pitchLerp = Mathf.Min(0.5f, pitchLerp + controlPitchFactor * Time.deltaTime);
            } else {
                pitchLerp = Mathf.Max(0.5f, pitchLerp - controlPitchFactor * Time.deltaTime);
            }
        }
    }
    void ComputeRollLerp() {
        if (!AlmostEquals(inputReader.moveComposite.x, 0f)) {
            if (inputReader.moveComposite.x < 0f) {
                rollLerp = Mathf.Max(0f, rollLerp - controlRollFactor * Time.deltaTime);
            } else {
                rollLerp = Mathf.Min(1f, rollLerp + controlRollFactor * Time.deltaTime);
            }
        } else if (!AlmostEquals(rollLerp, 0.5f)) {
            if (rollLerp < 0.5f) {
                rollLerp = Mathf.Min(0.5f, rollLerp + controlRollFactor * Time.deltaTime);
            } else {
                rollLerp = Mathf.Max(0.5f, rollLerp - controlRollFactor * Time.deltaTime);
            }
        }
    }

    bool AlmostEquals(float f1, float f2) {
        return Mathf.Abs(f1 - f2) <= float.Epsilon * 5f;
    }
}
