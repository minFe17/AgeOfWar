using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBar : MonoBehaviour
{
    [SerializeField] Image _bar;

    public void SetFillAmount(float value)
    {
            _bar.fillAmount = value;
    }
}
