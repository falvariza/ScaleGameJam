using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Transform blastHaloTransform;
    [SerializeField] private Player player;

    private Animator animator;

    private void Awake()
    {
        blastHaloTransform.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player.GetSizeSystem().OnSizeIncreased += SizeSystem_OnSizeIncreased;
        player.GetBlastAttackSystem().OnBlastRadiusChanged += GetBlastAttackSystem_OnBlastRadiusChanged;
        player.GetBlastAttackSystem().OnBlastEnemiesEnded += GetBlastAttackSystem_OnBlastEnemiesEnded;
        player.OnExpandingSizeStarted += Player_OnExpandingSizeStarted;
        player.OnExpandingSizeFinished += Player_OnExpandingSizeFinished;
        player.OnResetPlayer += Player_OnResetPlayer;
    }

    private void SizeSystem_OnSizeIncreased(object sender, System.EventArgs e)
    {
        blastHaloTransform.localScale = new Vector3(1f, 1f, 1f);
        blastHaloTransform.gameObject.SetActive(true);
    }

    private void GetBlastAttackSystem_OnBlastRadiusChanged(object sender, System.EventArgs e)
    {
        float blastSize = player.GetBlastAttackSystem().GetBlastRadius() * 2;
        float playerScale = player.GetActualPlayerScale();
        float blastHaloScale = blastSize / playerScale;
        blastHaloTransform.localScale = new Vector3(blastHaloScale, blastHaloScale, 1f);
    }

    private void GetBlastAttackSystem_OnBlastEnemiesEnded(object sender, System.EventArgs e)
    {
        blastHaloTransform.gameObject.SetActive(false);
    }

    private void Player_OnExpandingSizeStarted(object sender, System.EventArgs e)
    {
        animator.SetBool("isExpanding", true);
    }

    private void Player_OnExpandingSizeFinished(object sender, System.EventArgs e)
    {
        animator.SetBool("isExpanding", false);
    }

    private void Player_OnResetPlayer(object sender, System.EventArgs e)
    {
        animator.SetBool("isExpanding", false);
        blastHaloTransform.gameObject.SetActive(false);
    }
}
