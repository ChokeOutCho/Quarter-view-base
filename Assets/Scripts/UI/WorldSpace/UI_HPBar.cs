using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{

    enum GameObjects
    {
        HPBar
    }

    Stat _stat;
    public Slider hpSlider;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = (float)_stat.Hp/_stat.MaxHp; //소수아래날림방지
     SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
         // GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
       hpSlider.value = ratio;
    }
}
