using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class ChangeWeaponAnimation : MonoBehaviour
{
    private SkeletonGraphic skelGraph;
    public AnimationReferenceAsset Normal;
    public AnimationReferenceAsset Change;

    private void Awake()
    {
        skelGraph = this.GetComponent<SkeletonGraphic>();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skelGraph.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void ChangeWeapon(int id)
{
    StartCoroutine(SetAnimationChange()); 

    switch (id)
    {
        case 0:
        case 1:
            SetSkin("Dagger");
            break;
        case 2:
            SetSkin("Sword");
            break;
        case 3:
            SetSkin("Axe");
            break;
        case 4:
            SetSkin("Lance");
            break;
        case 5:
            SetSkin("Bilama");
            break;
    }
}

public void ChangeEv(int id)
{
    StartCoroutine(SetAnimationChange()); 

    switch (id)
    {
        case 0:
        case 1:
            SetSkin("Bat");
            break;
        case 2:
            SetSkin("Golem");
            break;
        case 3:
            SetSkin("Knight");
            break;
        case 4:
            SetSkin("Healer");
            break;
        case 5:
            SetSkin("Unicorn");
            break;
        case 6:
            SetSkin("Dragon");
            break;
    }
}


private void SetSkin(string skinName)
{
    var skeleton = skelGraph.Skeleton;
    skeleton.SetSkin(skinName);
}



public void SetAnimation(string animationName, bool loop, float timeScale)
{
    AnimationReferenceAsset animation = null;
    switch (animationName)
    {
        case "Normal":
            animation = Normal;
            break;
        case "Change":
            animation = Change;
            break;
    }
    if (animation != null)
    {
        skelGraph.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }
}

public IEnumerator SetAnimationChange()
{
    SetAnimation(Change, false, 1f);

    yield return new WaitForSeconds(0.9f);

    SetAnimation(Normal, true, 1f);
}
}


