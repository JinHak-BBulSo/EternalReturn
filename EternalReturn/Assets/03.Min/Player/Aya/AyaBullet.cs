using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaBullet : MonoBehaviour
{
    public float damage = 0f;
    public PlayerBase shootPlayer = default;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * 8f * Time.deltaTime * 3f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBase>() != null)
        {
            DamageMessage dm = new DamageMessage(shootPlayer.gameObject, damage);
            other.GetComponent<PlayerBase>().TakeDamage(dm);
            Destroy(gameObject);
        }
        else if (other.GetComponent<Monster>() != null)
        {
            DamageMessage dm = new DamageMessage(shootPlayer.gameObject, damage);
            other.GetComponent<Monster>().TakeDamage(dm);
            Destroy(gameObject);
        }
    }
}
