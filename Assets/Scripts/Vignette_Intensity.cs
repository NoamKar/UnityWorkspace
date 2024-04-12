using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class Vignette_Intensity : MonoBehaviour
{
    private Volume pp;
    Vignette _vig;

    public float changeSpeed = 0.5f;
    private float currentIntensity = 0f;

    public InputActionReference vignetteActionMap;
    public InputActionReference moveActionMap;
    public InputActionReference turnActionMap;

    private void Awake()
    {
        pp = this.GetComponent<Volume>();

        Vignette tmp;
        if (pp.profile.TryGet<Vignette>(out tmp))
        {
            _vig = tmp;
        }

        currentIntensity = _vig.intensity.value;
    }

    private void LateUpdate()
    {
        float value1 = vignetteActionMap.action.ReadValue<float>();
        Vector2 move = moveActionMap.action.ReadValue<Vector2>();
        Vector2 turn = turnActionMap.action.ReadValue<Vector2>();

        if (move != Vector2.zero)
        {
            currentIntensity = Mathf.MoveTowards(currentIntensity, value1, changeSpeed * Time.deltaTime);
            _vig.intensity.Override(currentIntensity);
        }
        if (move == Vector2.zero && turn == Vector2.zero)
        {
            value1 = 0;
            currentIntensity = Mathf.MoveTowards(currentIntensity, value1, changeSpeed / 4 * Time.deltaTime);
            _vig.intensity.Override(currentIntensity);
        }
        if (move == Vector2.zero && turn != Vector2.zero)
        {
            currentIntensity = Mathf.MoveTowards(currentIntensity, value1, changeSpeed * Time.deltaTime);
            _vig.intensity.Override(currentIntensity);
        }
    }
}
