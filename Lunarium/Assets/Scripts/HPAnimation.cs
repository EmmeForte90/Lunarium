using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class HPAnimation : MonoBehaviour
{
    private SkeletonGraphic skelGraph;
    [SerializeField] public AnimationReferenceAsset full;
    [SerializeField] public AnimationReferenceAsset broke;

    private void Awake()
    {
        skelGraph = this.GetComponent<SkeletonGraphic>();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skelGraph.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void restoreHP()
    {
        SetAnimation(full, false, 1f);
    }

    public void removeHP()
    {
        SetAnimation(broke, false, 1f);
    }
}
