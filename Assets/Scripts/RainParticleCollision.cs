using UnityEngine;

public class RainParticleCollision : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        // Check if the collided object has the SmudgeEffectController
        SmudgeEffectController smudgeEffect = other.GetComponent<SmudgeEffectController>();
        if (smudgeEffect != null)
        {
            // Trigger the smudge effect on the object
            smudgeEffect.TriggerSmudge();
            Debug.Log("Rain hit: " + other.name + ", smudge triggered.");
        }
    }
}
