using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaBullet : MonoBehaviour
{
    public float damage = 0f;
    public PlayerBase shootPlayer = default;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootPlayer.transform.forward * 24f * Time.deltaTime;
        distance += 24f * Time.deltaTime;

        if (distance >= 8f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBase>() != null)
        {
            if (other.GetComponent<PlayerBase>() != shootPlayer)
            {
                DamageMessage dm = new DamageMessage(shootPlayer.gameObject, damage);
                other.GetComponent<PlayerBase>().TakeDamage(dm);
                Destroy(gameObject);
            }
        }
        else if (other.GetComponent<Monster>() != null)
        {
            DamageMessage dm = new DamageMessage(shootPlayer.gameObject, damage);
            other.GetComponent<Monster>().TakeDamage(dm);
            Destroy(gameObject);
        }
    }
}
