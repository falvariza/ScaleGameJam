using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Transform BlastHaloTransform;
    [SerializeField] private Player player;

    private Animator animator;

    private void Awake()
    {
        BlastHaloTransform.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player.GetSizeSystem().OnSizeIncreased += SizeSystem_OnSizeIncreased;
        player.GetBlastAttackSystem().OnBlastRadiusChanged += GetBlastAttackSystem_OnBlastRadiusChanged;
        player.GetBlastAttackSystem().OnBlastEnemiesEnded += GetBlastAttackSystem_OnBlastEnemiesEnded;
        player.OnExpandingSizeStarted += Player_OnExpandingSizeStarted;
        player.OnExpandingSizeFinished += Player_OnExpandingSizeFinished;
    }

    private void SizeSystem_OnSizeIncreased(object sender, System.EventArgs e)
    {
        BlastHaloTransform.localScale = new Vector3(1f, 1f, 1f);
        BlastHaloTransform.gameObject.SetActive(true);
    }

    private void GetBlastAttackSystem_OnBlastRadiusChanged(object sender, System.EventArgs e)
    {
        float blastSize = player.GetBlastAttackSystem().GetBlastRadius() * 2;
        float playerScale = player.GetActualPlayerScale();
        float blastHaloScale = blastSize / playerScale;
        BlastHaloTransform.localScale = new Vector3(blastHaloScale, blastHaloScale, 1f);
    }

    private void GetBlastAttackSystem_OnBlastEnemiesEnded(object sender, System.EventArgs e)
    {
        BlastHaloTransform.gameObject.SetActive(false);
    }

    private void Player_OnExpandingSizeStarted(object sender, System.EventArgs e)
    {
        animator.SetBool("isExpanding", true);
    }

    private void Player_OnExpandingSizeFinished(object sender, System.EventArgs e)
    {
        animator.SetBool("isExpanding", false);
    }
}
