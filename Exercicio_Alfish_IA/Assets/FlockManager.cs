using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    [Header("Configurações do Cardume")]//para fazer testes em tempo real
    [Range(0.0f, 5.0f)]
    public float minSpeed;//controle de velocidade
    [Range(0.0f, 5.0f)]
    public float maxSpeed;//controle de velocidade
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;//controle de distancia do peixe vizinho
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;//controle de rotação

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numFish];
        for(int i = 0; i<numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y),
                Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);//criar quantidade do cardume
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
            Random.Range(-swinLimits.y, swinLimits.y),
            Random.Range(-swinLimits.z, swinLimits.z));
    }
}
