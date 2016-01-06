using UnityEngine;
using System.Collections;

public enum TIPOSUELO { AGUA, TIERRA};

public class GameManager : MonoBehaviour {

	GameObject motherObject;
	GameObject camara;

    public GameObject tilePrefab;
    public Material materialSprite;

    //Sprites
    public TileSet[] tileSets = new TileSet[5];
    public Sprite agua;
    public Sprite tierra;
    

	public int[,] mapa;
    public PixelSprite[,] tileSprites;
    
	void Awake () {
		if(motherObject == null){
			motherObject = new GameObject ();
			motherObject.transform.position = Vector3.zero;
			motherObject.name = "Objetos";
		}
		if(camara == null){
			camara = Camera.main.gameObject;
		}
	}

	public void InicializarMapa() {
        tileSprites = new PixelSprite [mapa.GetLength(0), mapa.GetLength(1)];

        camara.transform.position = new Vector3 (((float) mapa.GetLength (0) - 1) / 2, ((float) mapa.GetLength (1) - 1) / 2, camara.transform.position.z);

		for (int x = 0; x < mapa.GetLength(0); x++){
			for (int y = 0; y < mapa.GetLength(1); y++){
				if(x==0||x==mapa.GetLength(0)-1||y==0||y==mapa.GetLength(1)-1){
					mapa[x,y] = -1;
				}else{
                    GameObject _prefab = Instantiate(tilePrefab);
                    tileSprites[x, y] = _prefab.GetComponent<PixelSprite>();
                    _prefab.transform.position = new Vector2 (x, y);
                    _prefab.name = "Sprite (" + x + ", " + y +")";
                    _prefab.transform.parent = motherObject.transform;
                }
			}
		}

        ActualizarMapa();
	}

	public void ActualizarMapa () {
		for (int x = 1; x < mapa.GetLength(0) - 1; x++){
			for (int y = 1; y < mapa.GetLength(1) - 1; y++){
                tileSprites[x,y].capas[2].sprite = SeleccionarTileSet (mapa[x,y], new Vector2 (x, y), true);
			}
		}
	}

    public void ActualizarMapaLocal (int _x, int _y) {
        if (_x > 0 && _x < mapa.GetLength(0) - 1 && _y > 0 && _y < mapa.GetLength(1) - 1) {
            tileSprites[_x, _y].capas[2].sprite = SeleccionarTileSet(mapa[_x, _y], new Vector2(_x, _y), true);
        }
    }

    public void ActualizarMapaLocal (Vector2 _pos) {
        ActualizarMapaLocal (Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.y));
    }

	Sprite SeleccionarTileSet (int valor, Vector2 position, bool actualizacion = false) {
        if(valor == 0)
            return null;

        valor = valor - 1;

		int _direccion = (actualizacion) ? tipoSprite(valor, position) : 0;

        valor = Mathf.Clamp(valor, 0, tileSets.Length - 1);

        return tileSets[valor].spriteEnTierra[_direccion];
	}

	int tipoSprite (int valor, Vector2 position, params int[] valoresReferencias){
		bool[] x = new bool[4];
		int[] _pos = new int[2];
		_pos [0] = (int) position.x;
		_pos [1] = (int) position.y;

        if(valoresReferencias.Length == 0)
            valoresReferencias = new int[1] { valor + 1 };

        for (int i = 0; i < valoresReferencias.Length; i++) {
            if(mapa[_pos[0], _pos[1] + 1] == valoresReferencias[i])
                x[0] = true;
            if(mapa[_pos[0] - 1, _pos[1]] == valoresReferencias[i])
                x[1] = true;
            if(mapa[_pos[0] + 1, _pos[1]] == valoresReferencias[i])
                x[2] = true;
            if(mapa[_pos[0], _pos[1] - 1] == valoresReferencias[i])
                x[3] = true;
        }

		if (x [0] && x [1] && x [2] && x [3])
			return 4;
		else if (x [0] && x [1] && x [2] && !x [3])
			return 7;
		else if (x [0] && x [1] && !x [2] && x [3])
			return 5;
		else if (x [0] && x [1] && !x [2] && !x [3])
			return 8;
		else if (x [0] && !x [1] && x [2] && x [3])
			return 3;
		else if (x [0] && !x [1] && x [2] && !x [3])
			return 6;
		else if (x [0] && !x [1] && !x [2] && x [3])
			return 10;
		else if (x [0] && !x [1] && !x [2] && !x [3])
			return 11;
		else if (!x [0] && x [1] && x [2] && x [3])
			return 1;
		else if (!x [0] && x [1] && x [2] && !x [3])
			return 13;
		else if (!x [0] && x [1] && !x [2] && x [3])
			return 2;
		else if (!x [0] && x [1] && !x [2] && !x [3])
			return 14;
		else if (!x [0] && !x [1] && x [2] && x [3])
			return 0;
		else if (!x [0] && !x [1] && x [2] && !x [3])
			return 12;
		else if (!x [0] && !x [1] && !x [2] && x [3])
			return 9;
		else if (!x [0] && !x [1] && !x [2] && !x [3])
			return 15;

		return 4;
	}

	public string MostrarNombre(int ID){
		switch (ID){
		case -1: return "Borde";
		case 0: return "Carretera";
		case 1: return "Bosque";
		case 2: return "Montaña";
		case 3: return "Muro";
		case 4: return "Ciudad";
		case 5: return "Fabrica";
		case 6: return "Puerto";
		case 7: return "Aeropuerto";
		case 8: return "Base";
		}
		return "Nombre no encontrado";
	}
}

[System.Serializable]
public class TileSet {
    public string nombreTile;

    public bool AptoParaTierra = true;
    public bool AptoParaAgua = true;

    public bool compartirSprites = false;

    public Sprite[] spriteEnTierra = new Sprite[16];
    public Sprite[] spriteEnAgua = new Sprite[16];
}

[System.Serializable]
public class TileSetPrimario {
    public string nombreTile;

    public bool animacion = false;
    public bool diversificarSprite = false;

    public Sprite[] sprites = new Sprite [1];
}