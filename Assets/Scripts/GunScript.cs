
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    [SerializeField] AudioClip gun01 = null;
    [SerializeField] AudioClip gun02 = null;
    [SerializeField] AudioClip gun03 = null;

    public ParticleSystem muzzleFlash;
    public Camera fpsCam;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
        muzzleFlash.Play();
        int s = Random.Range(0, 3);
        if (s == 0)
        {
            AudioManager.PlayClip2D(gun01, 1);
        }
        else if (s == 1)
        {
            AudioManager.PlayClip2D(gun02, 1);
        }
        else if (s == 2)
        {
            AudioManager.PlayClip2D(gun03, 1);
        }
    }
}
