using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to harvest fruits from <see cref="BushFruits"/>
/// </summary>
public class PlayerHarvest : MonoBehaviour
{
    [SerializeField] float harvestRadius;
    [SerializeField] float harvestTime;
    [SerializeField] LayerMask bushesMask;

    [Header("Audio Settings")]
    [SerializeField] AudioClip harvestSound; // Kéo file âm thanh nhặt quả vào đây

    PlayerMovement playerMovement;
    PlayerBackpack backpack;
    AudioSource audioSource;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        backpack = GetComponent<PlayerBackpack>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryHarvestClose();
        }
    }

    void TryHarvestClose()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, harvestRadius / 2, bushesMask);

        if (hit != null)
        {
            BushFruits bush = hit.GetComponent<BushFruits>();
            if (bush.HasFruits())
            {
                // THAY ĐỔI Ở ĐÂY: Dùng PlayOneShot để không ngắt quãng âm thanh khác
                PlayHarvestAudio();

                playerMovement.HarvestStopMovement(harvestTime);
                backpack.AddFruits(bush.HarvestFruit());
            }
        }
        else
        {
            TryHarvestFruit();
        }
    }

    void TryHarvestFruit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, harvestRadius, bushesMask);

        if (hit != null)
        {
            BushFruits bush = hit.GetComponent<BushFruits>();
            if (bush.HasFruits())
            {
                // THAY ĐỔI Ở ĐÂY: Dùng PlayOneShot
                PlayHarvestAudio();

                playerMovement.HarvestStopMovement(harvestTime);
                backpack.AddFruits(bush.HarvestFruit());
            }
        }
    }

    // Hàm phụ trách phát âm thanh riêng biệt
    void PlayHarvestAudio()
    {
        if (audioSource != null && harvestSound != null)
        {
            // PlayOneShot giúp âm thanh này nổ ra độc lập, chồng lên các tiếng khác được
            audioSource.PlayOneShot(harvestSound);
        }
        else if (harvestSound == null)
        {
            // Nếu bạn lỡ quên chưa kéo Clip vào, nó sẽ gọi Play mặc định để chữa cháy
            audioSource.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, harvestRadius);
    }

    public void OnHarvestButtonPressed()
    {
        TryHarvestClose();
    }
}