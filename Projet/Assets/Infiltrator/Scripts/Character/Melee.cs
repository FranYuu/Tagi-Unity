using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {
    private float timeBtwAttack;
    private float startTimeBtwAttack;
    public Transform attackPosition;
    private bool flip;
    public float attackRange;
    public LayerMask enemyOnly;
    public GameObject testDebug;

    // Update is called once per frame
    void Start()
    {
        startTimeBtwAttack = 0.4f;
        flip = true;
    }

	void Update () {

        if (timeBtwAttack <= 0){
            testDebug.transform.localScale = new Vector3(attackRange, attackRange, 0);//zone juste pour les test
            testDebug.transform.position = attackPosition.position;
            if (Input.GetButton("Fire3"))
            {
                gameObject.SendMessage("playAnimationMelee");
                Collider2D[] enemiesToKILL = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemyOnly);
                for (int i=0; i < enemiesToKILL.Length; i++)
                {
                    if (enemiesToKILL == null)
                        break;
                    if (enemiesToKILL[i].GetComponent<FieldOfViewEnemy>().facingRight == flip) {
                        //enemiesToKILL[i].GetComponent<EnnemyController>().Death();
                        Destroy(enemiesToKILL[i].gameObject);
                        break;
                    }
                    testDebug.transform.localScale = new Vector3(0, 0, 0);
                }
            }
            else
                timeBtwAttack -= Time.deltaTime;
        }
        else
            timeBtwAttack = startTimeBtwAttack;


    }
    void MeleeFlip()
    {
        flip = !flip;
    }

}
