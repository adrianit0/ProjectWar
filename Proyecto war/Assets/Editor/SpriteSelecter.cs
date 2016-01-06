using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class SpriteSelecter : EditorWindow {
	GameManager game;
	Texture2D texture;
	Vector3 scrollPosition;

	int pest = 0;

	[MenuItem ("Creador/Selector de Sprite")]
	static void Init () {
		SpriteSelecter window = (SpriteSelecter)EditorWindow.GetWindow (typeof (SpriteSelecter));
		//window.position = Rect (50, 50, 250, 60);
		window.Show();
	}
	
	void OnGUI () {
		if(game != null){
			MostrarContenido();
		}else{
			PedirContenido();
		}
	}

	void PedirContenido () {
		game = (GameManager) GameObject.FindObjectOfType(typeof(GameManager));
		if(game == null){
			GUILayout.Label ("Hace falta un script \"GameManager\" para hacer funcionar este plugin.", EditorStyles.boldLabel/*, EditorStyles.largeLabel*/);
			GUILayout.Label ("No se ha encontrado el Script, insertalo aquí:", EditorStyles.boldLabel);
			game = (GameManager) EditorGUILayout.ObjectField ("Dialogos",game, typeof(GameManager), true, GUILayout.MinWidth(50), GUILayout.MaxWidth(300));
			GUILayout.Label ("Si no tienes ninguno puedes crear uno aqui:", EditorStyles.boldLabel);
			if(GUILayout.Button("Crear nuevo", GUILayout.MinWidth(20), GUILayout.MaxWidth(100))){
				GameObject _obj = new GameObject();
				_obj.name = "DialogueManager";
				_obj.AddComponent<GameManager>();
			}
		}else{
			MostrarContenido();
		}
	}

	void MostrarContenido () {
		EditorGUILayout.BeginVertical ("box");
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

		EditorGUILayout.BeginHorizontal ();

		if(pest != -1){
			if(GUILayout.Button("Mapa ASCII", GUILayout.MinWidth(60), GUILayout.MaxWidth(250))){
				pest = -1;
			}
		}else{
			GUILayout.Label ("Mapa ASCII", EditorStyles.centeredGreyMiniLabel , GUILayout.MinWidth(60), GUILayout.MaxWidth(250));
		}

        if(pest != 0) {
            if(GUILayout.Button("Agua/Tierra", GUILayout.MinWidth(60), GUILayout.MaxWidth(250))) {
                pest = 0;
            }
        } else {
            GUILayout.Label("Agua/Tierra", EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(60), GUILayout.MaxWidth(250));
        }

        if(pest != 1) {
            if(GUILayout.Button("TileSets", GUILayout.MinWidth(60), GUILayout.MaxWidth(250))) {
                pest = 1;
            }
        } else {
            GUILayout.Label("TileSets", EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(60), GUILayout.MaxWidth(250));
        }


        /* for (int x = 0; x < 11; x++){
			if(pest != x){
				if(GUILayout.Button(game.MostrarNombre(x), GUILayout.MinWidth(60), GUILayout.MaxWidth(250))){
					pest = x;
				}
			}else{
				GUILayout.Label (game.MostrarNombre(x), EditorStyles.centeredGreyMiniLabel , GUILayout.MinWidth(60), GUILayout.MaxWidth(250));
			}
		} */
        EditorGUILayout.EndHorizontal ();

		//game.agua[0] = (Sprite) EditorGUI.ObjectField(new Rect(3, 3, 90, 90), game.agua[0], typeof(Sprite), false);

		ShowSprite (pest);
		
		//game.carretera[1] = (Sprite) EditorGUI.ObjectField(new Rect(55, 60, 50, 50), game.carretera[1],  typeof(Sprite), false);
		//texture = (Texture2D) EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Textura 2D", texture, typeof(Texture2D), false);
		//if(game.agua[0] != null){
			//EditorGUI.PrefixLabel(Rect(25,45,100,15),0,GUIContent("Preview:"));
		//}

		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
	}

	void ShowSprite (int pestaña){
		switch (pestaña) {
		case -1:
			if(game.mapa != null){
				EditorGUILayout.BeginVertical("box");
				for(int x = 0; x < game.mapa.GetLength(0); x++){
					EditorGUILayout.BeginHorizontal();
					for(int y = 0; y < game.mapa.GetLength(1); y++){
						game.mapa[x,y] =  EditorGUILayout.IntField(game.mapa[x,y], GUILayout.Width (20));
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndVertical ();
			}
			break;
		case 0:
			//game.agua[0] = (Sprite) EditorGUI.ObjectField(new Rect(50, 50, 50, 50), game.agua[0],  typeof(Sprite), false);
			//game.agua[1] = (Sprite) EditorGUI.ObjectField(new Rect(120, 50, 50, 50), game.agua[1],  typeof(Sprite), false);
			break;
		case 1:
			/*for (int x = 0; x < 6; x++){
				for (int y = 0; y < 3; y++){
					if(!(x==5&&y!=0)){
						//game.tierra[x+y*6] = (Sprite) EditorGUI.ObjectField(new Rect(50+50*x, 50+50*y, 50, 50), game.tierra[x+y*6],  typeof(Sprite), false);
					}
				}
			}*/

            for (int i = 0; i < game.tileSets.Length; i++) {
                MostrarSpriteVertical(game.MostrarNombre(i), ref game.tileSets[i].spriteEnTierra, ref game.tileSets[i].spriteEnAgua);

            }
            
			break;
		case 2:
			for (int x = 0; x < 6; x++){
				for (int y = 0; y < 3; y++){
					if(!(x==5&&y!=0)){
						//game.carretera[x+y*6] = (Sprite) EditorGUI.ObjectField(new Rect(50+50*x, 50+50*y, 50, 50), game.carretera[x+y*6],  typeof(Sprite), false);
					}
				}
			}
			break;
		case 3:
			for (int x = 0; x < 6; x++){
				for (int y = 0; y < 3; y++){
					if(!(x==5&&y!=0)){
						//game.bosque[x+y*6] = (Sprite) EditorGUI.ObjectField(new Rect(50+50*x, 50+50*y, 50, 50), game.bosque[x+y*6],  typeof(Sprite), false);
					}
				}
			}
			break;
		case 4:
			for (int x = 0; x < 6; x++){
				for (int y = 0; y < 3; y++){
					if(!(x==5&&y!=0)){
						//game.montaña[x+y*6] = (Sprite) EditorGUI.ObjectField(new Rect(50+50*x, 50+50*y, 50, 50), game.montaña[x+y*6],  typeof(Sprite), false);
					}
				}
			}
			break;
		case 5:
			for (int x = 0; x < 6; x++){
				for (int y = 0; y < 3; y++){
					if(!(x==5&&y!=0)){
						//game.muro[x+y*6] = (Sprite) EditorGUI.ObjectField(new Rect(50+50*x, 50+50*y, 50, 50), game.muro[x+y*6],  typeof(Sprite), false);
					}
				}
			}
			break;
		default:
			break;
		}
	}

    void MostrarSpriteVertical (string nombreTitulo, ref Sprite[] spritesTierra, ref Sprite[] spritesAgua) {
        GUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(nombreTitulo);
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        MostrarSpriteHorizontal(ref spritesTierra, 0, 1, 2, 9);
        MostrarSpriteHorizontal(ref spritesTierra, 3, 4, 5, 10);
        MostrarSpriteHorizontal(ref spritesTierra, 6, 7, 8, 11);
        GUILayout.Space(20);
        MostrarSpriteHorizontal(ref spritesTierra, 12, 13, 14, 15);
        GUILayout.EndVertical();

        GUILayout.Space(40);

        GUILayout.BeginVertical();
        MostrarSpriteHorizontal(ref spritesAgua, 0, 1, 2, 9);
        MostrarSpriteHorizontal(ref spritesAgua, 3, 4, 5, 10);
        MostrarSpriteHorizontal(ref spritesAgua, 6, 7, 8, 11);
        GUILayout.Space(20);
        MostrarSpriteHorizontal(ref spritesAgua, 12, 13, 14, 15);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void MostrarSpriteHorizontal (ref Sprite [] sprites, int i1, int i2, int i3, int i4) {
        GUILayout.BeginHorizontal();
        sprites[i1] = (Sprite)EditorGUILayout.ObjectField(sprites[i1], typeof(Sprite), false, GUILayout.Width (50), GUILayout.Height (50));
        sprites[i2] = (Sprite)EditorGUILayout.ObjectField(sprites[i2], typeof(Sprite), false, GUILayout.Width(50), GUILayout.Height(50));
        sprites[i3] = (Sprite)EditorGUILayout.ObjectField(sprites[i3], typeof(Sprite), false, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.Space(20);
        sprites[i4] = (Sprite)EditorGUILayout.ObjectField(sprites[i4], typeof(Sprite), false, GUILayout.Width(50), GUILayout.Height(50));
        GUILayout.EndHorizontal();
    }
 }