using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//questa classe getisce come appare il player. Quindi in sostanza fa un update delle immagini del player quando muore
public class PlayerAppearence : MonoBehaviour {

    [Header("Immagini dei pezzi del corpo ")]
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer faceRenderer;
    public SpriteRenderer helmetRenderer;
    public SpriteRenderer leftArmRenderer;
    public SpriteRenderer leftForarmRenderer;
    public SpriteRenderer rightarmRenderer;

    public SpriteMask bodySpriteMask;

    private int explosionDamageLevel = 0;
    private int faceDamageLevel = 1;
    private int helmetDamageLevel = 0;
    private int spikeDamageLevel = 0;
    private int cutDamageLevel = 0;


    private GameObject bloodFountainParticle;



    //metodo invocato dalla playerDeath che cambia le immagini del player a seconda del tipo di morte fatto 
    public void ChangeBodyPiecesSprite(string typeOfDeath)
    {
        string faceName = UpdateFaceSprite();
        string helmetName = UpdateHelmetSprite();
        string bodyName = UpdateBodySprite(typeOfDeath);
        string leftArmName = UpdateArmSprite(typeOfDeath);
        string leftForarmName = UpdateForarmSprite(typeOfDeath);

        bodyRenderer.sprite = Resources.LoadAll<Sprite>("PlayerPieces/Body").First(s => s.name == bodyName);
        faceRenderer.sprite = Resources.LoadAll<Sprite>("PlayerPieces/Facce").First(s => s.name == faceName);
        helmetRenderer.sprite = Resources.LoadAll<Sprite>("PlayerPieces/Helmet").First(s => s.name == helmetName);
        leftArmRenderer.sprite = Resources.LoadAll<Sprite>("PlayerPieces/BracciaeGambe").Single(s => s.name == leftArmName);
        leftForarmRenderer.sprite = Resources.LoadAll<Sprite>("PlayerPieces/BracciaeGambe").Single(s => s.name == leftForarmName);

        bodySpriteMask.sprite = bodyRenderer.sprite;
    }

    //restituisce il nome dell'immagine della prossima faccia da mettere
    string UpdateFaceSprite()
    {
        if (faceDamageLevel < 6)
            faceDamageLevel++;     
        return "Facce" + faceDamageLevel.ToString();
    }

    //restituisce il nome dell'immagine del prossimmo elmetto da mettere
    string UpdateHelmetSprite()
    {
        if (helmetDamageLevel < 1)
            helmetDamageLevel++;
        return "Helmet" + helmetDamageLevel.ToString();
    }

    //restituisce il nome dell'immagine del prossimmo corpo da mettere
    string UpdateBodySprite(string typeOfDeath) {
        if (typeOfDeath == "explosion")
        {
            if (explosionDamageLevel < 4)
                explosionDamageLevel++;
            return "BodyExplosion" + explosionDamageLevel.ToString();
        }
        else if (typeOfDeath == "spike") {
            if (cutDamageLevel < 2)
                cutDamageLevel++;
            return "BodyCut" + cutDamageLevel.ToString();
        }
        else
            return "BodyNormal";
    }

    string UpdateArmSprite(string typeOfDeath) {
        if (typeOfDeath == "explosion")
            return "ArmExplosion";
        else if (typeOfDeath == "spike")
            return "ArmCut1";
        else
            return "ArmNormal";
    }

    string UpdateForarmSprite(string typeOfDeath) {
        if (typeOfDeath == "explosion")
            return "ForarmExplosion";
        else if (typeOfDeath == "spike")
            return "ForarmCut";
        else
            return "ForarmNormal";
    }

    public void AssignBloodFountainParticle(GameObject particles) {
        bloodFountainParticle = particles;
    }

    public void DestroyBloodFountainParticle() {
        if (bloodFountainParticle != null)
            Destroy(bloodFountainParticle.gameObject);
    }

}
