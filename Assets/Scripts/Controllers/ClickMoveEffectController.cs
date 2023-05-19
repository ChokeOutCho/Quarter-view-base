using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMoveEffectController : MonoBehaviour
{
    float destroyTime = 0.4f;
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            Managers.Resource.Destroy(gameObject);
            destroyTime = 0.4f;
        }
    }
}
