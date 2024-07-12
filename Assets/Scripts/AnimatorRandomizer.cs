using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class AnimatorRandomizer : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.speed *= Random.Range(0.9f, 1.1f);
        animator.Update(Random.Range(0.5f, 3));

    }

    // Update is called once per frame
   
}
