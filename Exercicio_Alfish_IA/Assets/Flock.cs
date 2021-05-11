using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float speed;
    public bool turning = false;//se está se distanciando e respeitando a quarentena

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);//altera a velocidade
        //speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        //Bounds limita o espaço para os peixes, senao eles perdem referencia e se perdem
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit();//Raycast identifica reflexão
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;

        }else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))//verifica a pilastra para não bater
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            //Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            if (Random.Range(0, 100) < 20)
            {
                ApplyRules();//regras aplicadas
            }
        }

        transform.Translate(0, 0, Time.deltaTime * speed);//velocidade de movimentação
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;//cada peixe vai receber a informação

        Vector3 vcentre = Vector3.zero;//calculo do ponto medio
        Vector3 vavoid = Vector3.zero;//evita colisao
        float gSpeed = 0.01f;//velocidade para formação dos peixes e equilibrar "colisão"
        float nDistance;//calculo da distancia de pontos
        int groupSize = 0;//quantidade de peixes no grupo

        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);//cálculo entre as distancias
                if(nDistance <= myManager.neighbourDistance)
                {
                    //distanciamento
                    vcentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);//desviar se estiver próximo de colidir
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;//trocar velocidade para equilibrar
                }
            }
        }
        if(groupSize > 0)//contagem de grupo(quando estão em rotação)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,//movementação mais suave
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }

}
