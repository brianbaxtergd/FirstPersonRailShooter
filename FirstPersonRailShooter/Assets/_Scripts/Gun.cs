using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Inscribed")]
    public AudioSource fireAudio;
    public GameObject hitParticlesPrefab;
    public Image cylinderImage;
    public Text reloadText;
    public int damage;
    public float reloadTime;

    int bulletCount;
    int bulletCountMax = 6;
    bool isReloading = false;
    float reloadTimer = 0f;
    float rayDistanceMax = 100f;

    private void Start()
    {
        bulletCount = bulletCountMax;
    }

    private void Update()
    {
        if (!isReloading)
        {
            // Fire on user input.
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Fire();
            // Reload user input.
            if (Input.GetKeyDown(KeyCode.Mouse1))
                Reload();
        }
    }

    void Fire()
    {
        if (bulletCount > 0)
        {
            // Fire weapon.
            fireAudio.Play();
            // Decrement bullet count.
            bulletCount -= 1;
            if (bulletCount == 0)
                reloadText.gameObject.SetActive(true);
            // Update cylinder (ammo) UI.
            cylinderImage.fillAmount -= 1f / (float)bulletCountMax;
            // Raycast for collision.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistanceMax))
            {
                if (hit.collider != null) // Checking if we actually hit something.
                {
                    // Spawn instance of hit particles.
                    GameObject hitPartGO = Instantiate(hitParticlesPrefab);
                    hitPartGO.transform.position = hit.point;
                    hitPartGO.transform.rotation = Quaternion.LookRotation(hit.normal);

                    if (hit.collider.gameObject.tag == "Enemy") // Checking if an enemy was hit.
                    {
                        EnemyHealth eH = hit.collider.gameObject.GetComponent<EnemyHealth>();
                        if (eH != null)
                        {
                            eH.TakeDamage(damage);
                        }
                    }
                }
            }
        }
        else
        {
            // If player fires while out of ammo, force reload.
            Reload();
        }
    }

    void Reload()
    {
        if (bulletCount == bulletCountMax)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        // Initialize reload state.
        if (reloadText.gameObject.activeSelf)
            reloadText.gameObject.SetActive(false);
        isReloading = true;
        reloadTimer = (reloadTime / (float)bulletCountMax) * (float)bulletCount;
        // Pause for reloadTime (seconds).
        while (reloadTimer < reloadTime)
        {
            // Timer tick.
            reloadTimer += Time.deltaTime;
            // Update cylinder (ammo) UI image.
            cylinderImage.fillAmount = reloadTimer / reloadTime;
            yield return null;
        }
        // Reset values.
        bulletCount = bulletCountMax;
        isReloading = false;
        reloadTimer = 0f; // Although this is redundant, it ensures (reloadTimer / reloadTime) will always be accurate between reloads.
        cylinderImage.fillAmount = 1f;
    }
}
