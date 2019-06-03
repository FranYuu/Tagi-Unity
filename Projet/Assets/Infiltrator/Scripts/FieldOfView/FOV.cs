using UnityEngine;
using System.Collections;
using System;

public class FOV : MonoBehaviour
{
    //Variable pour associé le point de vue à l'ennemi
    internal GameObject source;

    public GameObject light;


    private Transform laCible;
    private int pointDetection;
    protected float cornerBoxColliderCibleHeight;
    protected float cornerBoxColliderCibleWidth;



    //variable qui définit la portée du cône
    public float lgVision;
    public float heightVision;
    public float angleVision;

    //Varaible pour dessiner le champ de vision
    private Vector3 v3Haut;
    private Vector3 v3Bas;

    //Raycast qui permet de détecter si il y a un mur
    internal RaycastHit2D hitInfo;

    //Variable pour le mouvement vers le joueur
    public GameObject player;


    protected bool underLight;

    public BoxCollider2D boxPlayer;


    // Use this for initialization
    protected void Start()
    {
        cornerBoxColliderCibleHeight = boxPlayer.size.y / 4;
        cornerBoxColliderCibleWidth = boxPlayer.size.x / 4;
        pointDetection = 0;
        laCible = player.transform;
        hitInfo = Physics2D.Raycast(source.transform.position, CalculCoordVecteur(laCible.position.x, laCible.position.y, source.transform.position.x, source.transform.position.y));
    }


    //Permet de calculer les coordonnées d'un vecteur à partir de la position de 2 points
    private Vector3 CalculCoordVecteur(float v1X, float v1Y, float v2X, float v2Y)
    {
        return new Vector3(v1X - v2X, v1Y - v2Y);
    }

    //Permet de calculer le produit vectoriel de 2 vecteurs et renvoie le nouveau vecteur formé
    private Vector3 CalculVectoriel(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }


    //Permet de renvoyer le produit scalaire de 2 vecteurs et renvoie leur résultat
    private float CalculScalaire(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    /* 
       Un point appartient à un triangle si et seulement si on vérifie la formule suivante :
       (vect(AB) ^ vect(AM)) . (vect(AM) ^ vect(AC)) >= 0
       (vect(BA) ^ vect(BM)) . (vect(BM) ^ vect(BC)) >= 0
       (vect(CA) ^ vect(CM)) . (vect(CM) ^ vect(CB)) >= 0
       où ^ désigne le produit vectoriel de deux vecteurs
       . désigne le produit scalaire
       
        Ainsi, A représente notre objet disposant du champ de vision
        B représente le point maximal en hauteur du champ de vision
        C représente le point minimal en hauteur du champ de vision
        M représente ainsi le personnage a détecté

    */

    private bool CalculConditionDetect(Transform a, float posCibleX, float posCibleY)
    {
        //(vect(AB) ^ vect(AM)). (vect(AM) ^ vect(AC))
        float condition1 = CalculScalaire(
            CalculVectoriel(CalculCoordVecteur(a.position.x + lgVision, a.position.y + heightVision * angleVision, a.position.x, a.position.y),
                            CalculCoordVecteur(posCibleX, posCibleY, a.position.x, a.position.y)),
            CalculVectoriel(CalculCoordVecteur(posCibleX, posCibleY, a.position.x, a.position.y),
                            CalculCoordVecteur(a.position.x + lgVision * angleVision, a.position.y - heightVision, a.position.x, a.position.y)));

        //(vect(BA) ^ vect(BM)). (vect(BM) ^ vect(BC))
        float condition2 = CalculScalaire(
            CalculVectoriel(CalculCoordVecteur(a.position.x, a.position.y, a.position.x + lgVision, a.position.y + heightVision * angleVision),
                            CalculCoordVecteur(posCibleX, posCibleY, a.position.x + lgVision, a.position.y + heightVision * angleVision)),
            CalculVectoriel(CalculCoordVecteur(posCibleX, posCibleY, a.position.x + lgVision, a.position.y + heightVision * angleVision),
                            CalculCoordVecteur(a.position.x + lgVision * angleVision, a.position.y - heightVision, a.position.x + lgVision, a.position.y + heightVision * angleVision)));

        //(vect(CA) ^ vect(CM)). (vect(CM) ^ vect(CB))
        float condition3 = CalculScalaire(
            CalculVectoriel(CalculCoordVecteur(a.position.x, a.position.y, a.position.x + lgVision * angleVision, a.position.y - heightVision),
                            CalculCoordVecteur(posCibleX, posCibleY, a.position.x + lgVision * angleVision, a.position.y - heightVision)),
            CalculVectoriel(CalculCoordVecteur(posCibleX, posCibleY, a.position.x + lgVision * angleVision, a.position.y - heightVision),
                            CalculCoordVecteur(a.position.x + lgVision, a.position.y + heightVision * angleVision, a.position.x + lgVision * angleVision, a.position.y - heightVision)));


        return condition1 >= 0 && condition2 >= 0 && condition3 >= 0;


    }


    private bool InFieldOfView()
    {
        if (hitInfo.collider.gameObject.tag == laCible.tag)
        {
            //Verifie si le joueur est a distance de detection
            //On vérifie les 4 angles de la box Collider de la Cible, si 2 sont détectés alors la cible est détectée
            if (!underLight)
            {
                if (CalculConditionDetect(source.transform, laCible.position.x + cornerBoxColliderCibleWidth, laCible.position.y + cornerBoxColliderCibleHeight))
                {
                    pointDetection++;
                }
                if (CalculConditionDetect(source.transform, laCible.position.x + cornerBoxColliderCibleWidth, laCible.position.y - cornerBoxColliderCibleHeight))
                {
                    pointDetection++;
                }
                if (CalculConditionDetect(source.transform, laCible.position.x - cornerBoxColliderCibleWidth, laCible.position.y + cornerBoxColliderCibleHeight))
                {
                    pointDetection++;
                }
                if (CalculConditionDetect(source.transform, laCible.position.x - cornerBoxColliderCibleWidth, laCible.position.y - cornerBoxColliderCibleHeight))
                {
                    pointDetection++;
                }
                if (pointDetection >= 2)
                {
                    return true;
                }
                else if (Math.Abs(laCible.position.x - transform.position.x) < 5 && Math.Abs(laCible.position.y - transform.position.y) < 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true; //sous la lumière et détecté par le raycast
        }
        else
        {
            return false; //pas détecté du tout
        }
    }


    internal RaycastHit2D calcRaycast()
    {
        return Physics2D.Raycast(source.transform.position, CalculCoordVecteur(laCible.position.x, laCible.position.y, source.transform.position.x, source.transform.position.y));
    }

    // Update is called once per frame
    protected void Update()
    {

        hitInfo = calcRaycast();


        pointDetection = 0;
        CalculDist();
    }

    internal void CreateFieldOfViewLine()
    {
        //Permet d'avoir les points délimitant le champ de vision
        v3Haut = new Vector3(source.transform.position.x + lgVision, source.transform.position.y + heightVision * angleVision);
        v3Bas = new Vector3(source.transform.position.x + lgVision * angleVision, source.transform.position.y - heightVision);

        CreateVisionLine(source.transform.position, v3Haut);
        CreateVisionLine(source.transform.position, v3Bas);
        CreateVisionLine(v3Haut, v3Bas);
    }


    private void CreateVisionLine(Vector3 origin, Vector3 position)
    {
        if (Time.timeScale != 0)
        {
            GameObject ligneVision = new GameObject();
            ligneVision.AddComponent<LineRenderer>();
            ligneVision.name = "Trace Ligne FOV";
            LineRenderer lr = ligneVision.GetComponent<LineRenderer>();
            //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lr.startColor = Color.yellow;
            lr.endColor = Color.yellow;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.SetPosition(0, origin);
            lr.SetPosition(1, position);
            GameObject.Destroy(ligneVision, 0.02f);
        }
    }

    //Verifie la position du joueur
    private void CalculDist()
    {
        //Un joueur est présent
        if (laCible)
        {
            //Vérifie si la cible est dans le champ de vision
            if (InFieldOfView())
            {
                ComportementDetection();//Appel de methode
            }

            else
            {
                PlusAdistance();
            }

        }
    }


    //Comportement lorsque l'ennemi détecte la cible
    protected virtual void ComportementDetection()
    {

    }

    //Comportement lorsque l'ennemi perd la cible de vue
    protected virtual void PlusAdistance()
    {
        //print("Pas détecté !!");
        //Pause
    }


}