using System;
using UnityEngine;

public class PlayerRightArmBob : PlayerBob
{
    // Member variables
    [SerializeField] private float m_centerDist = 10f;

    
    // Update
    protected override void Update()
    {
        base.Update();
        // Weapon ecnter fix
        WeaponCenterFix();
    }
    
    // Bob effect weapon center fix
    private void WeaponCenterFix()
    {
        // Update player right arm y rotation based on bob effect x offset
        float sin = transform.localPosition.x / m_centerDist;
        float angle = -Mathf.Asin(sin) * Mathf.Rad2Deg;

        Vector3 newEulerAngles = transform.localEulerAngles;
        newEulerAngles.y = angle;
        transform.localEulerAngles = newEulerAngles;
    }
}
