using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInputHandler : MonoBehaviour
{
    public Skill sweepSkill;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sweepSkill.TryActivateSkill();
        }
    }
}
