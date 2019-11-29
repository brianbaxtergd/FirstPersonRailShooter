using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Inscribed")]
    public AudioSource fireAudio;
    public GameObject hitParticlesPrefab;
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
        // Fire gun on user input.
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading)
            Fire();
    }

    void Fire()
    {
        if (bulletCount > 0)
        {
            // Fire weapon.
            fireAudio.Play();
            // Decrement bullet count.
            bulletCount -= 1;
            //
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistanceMax))
            {
                if (hit.collider != null) // Checking if we actually hit something.
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
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
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        // Initialize reload state.
        isReloading = true;
        reloadTimer = 0f;
        // Pause for reloadTime (seconds).
        while (reloadTimer < reloadTime)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }
        // Reset values.
        bulletCount = bulletCountMax;
        isReloading = false;
        reloadTimer = 0f; // Although this is redundant, it ensures (reloadTimer / reloadTime) will always be accurate between reloads.
    }
}
