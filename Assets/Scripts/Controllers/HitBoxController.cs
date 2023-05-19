using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
   
   private float dt = 0.2f;
    
    private void Update()
    {
        dt -= Time.deltaTime;
        if (dt < 0)
        {
            Managers.Resource.Destroy(gameObject);
            dt = 0.2f;
        }
    }

}
