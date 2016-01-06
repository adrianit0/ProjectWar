using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameEditor {
	public class MapCreator : MonoBehaviour {

		public int valorX = 40;
		public int valorY = 30;

		public RectTransform seleccionPosicionTile;
		public RectTransform seleccionPosicionHerramienta;
		public RectTransform rectanguloGUI;

		public Image zoomMas;
		public Image zoomMenos;
		public Sprite[] zoomSprite = new Sprite[3];

		public Image botonDeshacer;
		public Sprite[] deshacerSprite = new Sprite[2];

		int seleccionItem = 0;
		int seleccionHerramienta = 0;

		int zoomActual = 10;

		bool puedeEscribir = true;
		bool escribiendo = false;

		Vector2 posicionCamara;
		Vector2 currentPosicion;

		Vector2 lastPosition;

		int cantidadGuardada = 0;
		int cantidadTotalDeshacer = 3;

		int[,,] listaMapa;

		List<Vector2> tablaRellenar;

        public GameManager game;
        
		void Start () {
			game.mapa = new int[valorX, valorY];
			game.InicializarMapa ();
			listaMapa = new int[cantidadTotalDeshacer, valorX, valorY];
			zoomActual = (int) Camera.main.orthographicSize;
		}

		void Update (){
			if(puedeEscribir){
				if (Input.GetMouseButtonDown (0)){
					AñadirDeshacer();
					escribiendo = true;
					lastPosition = new Vector2 (Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
				}
				
				//Poder cambiar de herramienta
				switch (seleccionHerramienta){
				case 0:
					brocha(seleccionItem);
					break;
				case 1:
					brocha(0);
					break;
				case 2:
					Rellenar(seleccionItem);
					break;
				case 3:
					//Linea recta
					break;
				case 4:
					//Cuadrado
					break;
				case 5:
					//Circulo
					break;
				default:
					//No hacer nada
					break;
				}


				if (Input.GetMouseButtonUp (0)){
					escribiendo = false;
				}
			}

			if(Input.GetKeyDown("1")){
				seleccionItem = 0;
			}
			if(Input.GetKeyDown("2")){
				seleccionItem = 1;
			}
			if(Input.GetKeyDown("3")){
				seleccionItem = 2;
			}
			if(Input.GetKeyDown("4")){
				seleccionItem = 3;
			}

			if(Input.GetKeyDown (KeyCode.Z)){
				Deshacer();
			}

			//Vector2 resultado = Vector2.zero;

			//Debug.Log (rectanguloGUI.anchoredPosition3D + " - " + Camera.main.ScreenToWorldPoint(Input.mousePosition) + " - " + RectTransformUtility.ScreenPointToLocalPointInRectangle (rectanguloGUI, Input.mousePosition, Camera.main, out resultado) + " - " + resultado);

			//Debug.Log (RectTransformUtility.RectangleContainsScreenPoint(rectanguloGUI, Input.mousePosition, Camera.main));

			//Debug.Log (RectTransformUtility.ScreenPointToWorldPointInRectangle (rectanguloGUI, Input.mousePosition, Camera.main, out resultado) + " - " + resultado);

			if(Input.GetMouseButtonDown (1)){
				posicionCamara = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				currentPosicion = Camera.main.transform.position;
			}

			if(Input.GetMouseButton (1)){
				Camera.main.transform.position = new Vector3 (currentPosicion.x + posicionCamara.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, currentPosicion.y + posicionCamara.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y, Camera.main.transform.position.z);
				currentPosicion = Camera.main.transform.position;
			}

			seleccionPosicionTile.anchoredPosition = Vector2.MoveTowards(seleccionPosicionTile.anchoredPosition, new Vector2((float) seleccionItem * 60 +40, seleccionPosicionTile.anchoredPosition.y), 50);
			seleccionPosicionHerramienta.anchoredPosition = Vector2.MoveTowards(seleccionPosicionHerramienta.anchoredPosition, new Vector2((float) seleccionHerramienta * 60 + 40, seleccionPosicionHerramienta.anchoredPosition.y), 50);
		}

		void brocha (int tile) {
			if(Input.GetMouseButton(0) && escribiendo){
				Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(direction,direction);
                IntVector2 direccionEntera = direction;
				
				if (hit.collider != null) {
					//Debug.Log ("Direccion mundo: " + direction + "Direccion Tile: " + _tile + "GameObject: " + hit.transform.gameObject.name);
					if(game.mapa[direccionEntera.x, direccionEntera.y] != tile){
						game.mapa[direccionEntera.x, direccionEntera.y] = tile;
                        game.ActualizarMapaLocal(direccionEntera.x, direccionEntera.y);
                        LineaRecta(lastPosition, (IntVector2) direction, tile);
					}
				}
			}
		}

		void Rellenar (int tile){
			if(Input.GetMouseButtonDown(0) && escribiendo){
				Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(direction,direction);
				
				if (hit.collider != null) {
					//Debug.Log ("Direccion mundo: " + direction + "Direccion Tile: " + _tile + "GameObject: " + hit.transform.gameObject.name);
					if(game.mapa[Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y)] != tile){
						tablaRellenar = new List<Vector2>();
						int valorAntiguo = game.mapa[Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y)];
						tablaRellenar.Add (new Vector2 (Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y)));

						for(int i = 0; i < tablaRellenar.Count; i++){
							ExtraRellenar(new int[2] {Mathf.RoundToInt(tablaRellenar[i].x), Mathf.RoundToInt(tablaRellenar[i].y+1)}, tile, valorAntiguo, i);
							ExtraRellenar(new int[2] {Mathf.RoundToInt(tablaRellenar[i].x-1), Mathf.RoundToInt(tablaRellenar[i].y)}, tile, valorAntiguo, i);
							ExtraRellenar(new int[2] {Mathf.RoundToInt(tablaRellenar[i].x+1), Mathf.RoundToInt(tablaRellenar[i].y)}, tile, valorAntiguo, i);
							ExtraRellenar(new int[2] {Mathf.RoundToInt(tablaRellenar[i].x), Mathf.RoundToInt(tablaRellenar[i].y-1)}, tile, valorAntiguo, i);
						}
						game.ActualizarMapa();
					}
				}
			}
		}

		void ExtraRellenar (int[] posicion, int tileActual, int tileAntiguo, int pos){
			if(game.mapa[posicion[0],posicion[1]]==tileAntiguo){
				tablaRellenar.Add(new Vector2 (posicion[0], posicion[1]));
				game.mapa[posicion[0],posicion[1]]=tileActual;
			}
		}

		void LineaRecta (IntVector2 inicio, IntVector2 destino, int tileSet){
			IntVector2 distanciaActual = new IntVector2 (Mathf.RoundToInt (destino.x), Mathf.RoundToInt (destino.y));
			Vector2 diferencia = new Vector2 (distanciaActual.x - inicio.x, distanciaActual.y - inicio.y);
			float maximo = Mathf.Max (Mathf.Abs (diferencia.x), Mathf.Abs (diferencia.y));
			if (maximo > 1){
				for (int i = 1; i < maximo; i++){
					Vector2 nuevaDireccion = new Vector2 ((diferencia.x * i)/maximo, (diferencia.y * i)/maximo);
					IntVector2 nuevaDireccionEntera = new IntVector2 (Mathf.RoundToInt(nuevaDireccion.x + inicio.x), Mathf.RoundToInt(nuevaDireccion.y + inicio.y));

					if(game.mapa[nuevaDireccionEntera.x, nuevaDireccionEntera.y] != tileSet){
						game.mapa[Mathf.RoundToInt(nuevaDireccion.x), Mathf.RoundToInt(nuevaDireccion.y)] = tileSet;
                        game.ActualizarMapaLocal(nuevaDireccionEntera.x, nuevaDireccionEntera.y);
					}
				}
			}
			lastPosition = (Vector2) distanciaActual;
		}

		void AñadirDeshacer() {
			if(cantidadGuardada < cantidadTotalDeshacer){
				for(int x = 0; x < valorX; x++){
					for(int y = 0; y < valorY; y++){
						listaMapa[cantidadGuardada, x, y] = game.mapa[x, y];
					}
				}
			}else{
				for(int c = 0; c < cantidadTotalDeshacer-1; c++){
					for(int x = 0; x < valorX; x++){
						for(int y = 0; y < valorY; y++){
							listaMapa[c, x, y] = listaMapa[c+1, x, y];
						}
					}
				}
				cantidadGuardada--;
				for(int x = 0; x < valorX; x++){
					for(int y = 0; y < valorY; y++){
						listaMapa[cantidadGuardada, x, y] = game.mapa[x, y];
					}
				}
			}
			botonDeshacer.sprite = deshacerSprite [1];
			cantidadGuardada++;
		}

		public void Deshacer(){
			if(cantidadGuardada > 0){
				for(int x = 0; x < valorX; x++){
					for(int y = 0; y < valorY; y++){
						game.mapa[x, y] = listaMapa[cantidadGuardada-1, x, y];
					}
				}
				cantidadGuardada--;
				if(cantidadGuardada==0){
					botonDeshacer.sprite = deshacerSprite [0];
				}
			}
			game.ActualizarMapa ();
		}

		public void Zoom (bool aumento) {
			int _zoom = zoomActual;
			if(!aumento){
				if(zoomActual<18){
					zoomActual += 2;
					zoomActual = Mathf.Clamp (zoomActual, 4, 18);
					StartCoroutine (hacerzoom(2,  _zoom));
				}
					
			}else{
				if(zoomActual>4){
					zoomActual -= 2;
					zoomActual = Mathf.Clamp (zoomActual, 4, 18);
					StartCoroutine (hacerzoom(-2,  _zoom));
				}
			}
			if(zoomActual==4){
				zoomMas.sprite = zoomSprite[2];
			}else if(zoomActual==18){
				zoomMenos.sprite = zoomSprite [2];
			}else{
				zoomMas.sprite = zoomSprite[0];
				zoomMenos.sprite = zoomSprite [1];
			}
		}

		IEnumerator hacerzoom (float zoom, float currentZoom){
			bool hecho = false;
			while (!hecho){
				if(currentZoom != zoomActual){
					yield return null;
				}
				Camera.main.orthographicSize += zoom*Time.deltaTime*10;
				if(currentZoom+zoom == Mathf.Round (Camera.main.orthographicSize)){
					Camera.main.orthographicSize = currentZoom+zoom;
					hecho = true; 
				}
				yield return null;
			}
		}

		public void SeleccionTile (int tile){
			seleccionItem = tile;
		}

		public void SeleccionHerramienta (int herramienta){
			seleccionHerramienta = herramienta;
		}

		public void entrarSalirEventTrigger (bool entrar){
			if(entrar){
				puedeEscribir = false;
			}else{
				puedeEscribir = true;
			}
		}
	}
}