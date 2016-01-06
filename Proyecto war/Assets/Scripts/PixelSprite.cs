using UnityEngine;
using System.Collections;

public class PixelSprite : MonoBehaviour {

    //CAPAS:
    //CAPA 0: La tierra y el agua, para añadir animaciones, etc.
    //CAPA 1: La playa, intercepción entre el agua y el mar.
    //CAPA 2: El resto de escenario.

    public TIPOSUELO tipoSuelo = TIPOSUELO.AGUA;
    public SpriteRenderer[] capas;
}
