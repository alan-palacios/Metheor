using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour {


	const float viewerMoveThresholdForChunkUpdate = 10f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public Transform viewer;
	public bool randPos;

	Vector2 viewerPosition;
	Vector2 viewerPositionOld;

	int chunksVisibleInViewDst;
	public int chunkSize;
	public float maxViewDst;
	public Vector2 xLimits;
	public Vector2 yLimits;

	Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
	List<Chunk> visibleChunks = new List<Chunk>();
	public ObjectPlacingList objectPlacingList;
	public GameObject background;

	void Start() {

		//chunkSize = biomesList.biomes[0].meshSettings.chunkSize;//-1
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

		UpdateVisibleChunks();

		//StartCoroutine(DisplayPlayer());
	}

	/*IEnumerator DisplayPlayer(){
		yield return new WaitForSeconds(0);

		float yHeight = terrainChunkDictionary [new Vector2(0,0)].heightMap.values[ (int)chunkSize/2,  (int)chunkSize/2];
		viewer.position = new Vector3(0, yHeight+0.5f , 1);
	}*/

	void Update() {
		//testColor+=new Color(0.0F, 0.0F, 0.01F, 0.0F);
		//biomesList.biomes[0].textureData.material.SetColor("testColor", testColor );

		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z);//mapGenerator.biomeData.meshSettings.meshScale;

		if (viewerPosition!=viewerPositionOld) {
			/*foreach (Chunk chunk  in visibleTerrainChunks) {
				chunk.UpdateCollisionMesh();
			}*/
		}
		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();

		}
	}

	void UpdateVisibleChunks() {
		//Debug.Log(chunkDictionary.Count);
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();

		for (int i = visibleChunks.Count-1 ; i >=0 ; i--) {
			alreadyUpdatedChunkCoords.Add(visibleChunks [i].coord);
			visibleChunks [i].UpdateChunk();
		}

		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord)) {
					if (chunkDictionary.ContainsKey (viewedChunkCoord)) {
						chunkDictionary [viewedChunkCoord].UpdateChunk ();

					} else {

						Chunk newChunk = new Chunk (viewedChunkCoord, transform, viewer, objectPlacingList, background, maxViewDst, chunkSize);
						chunkDictionary.Add (viewedChunkCoord, newChunk);
						newChunk.onVisibilityChanged +=onTerrainChunkVisibilityChanged;
						newChunk.destroyCoroutineReference = StartCoroutine( destroyCoroutine(newChunk) );
						newChunk.UpdateChunk( );
					}
				}

			}

		}
	}

	void onTerrainChunkVisibilityChanged(Chunk chunk, bool isVisible,  Coroutine coroutine){

		if (isVisible) {
			visibleChunks.Add (chunk);
			StopCoroutine( coroutine );

		}else {
			visibleChunks.Remove (chunk);
			coroutine = StartCoroutine( destroyCoroutine(chunk) );
		}
	}

	IEnumerator destroyCoroutine(Chunk chunk){
		yield return new WaitForSeconds(5);
		//Debug.Log("eliminando chunk "+chunk.coord);
		Destroy(chunk.chunkGameObject);
		chunkDictionary.Remove(chunk.coord);
		visibleChunks.Remove(chunk);
	}

	}



[System.Serializable]
public struct LODInfo {
	public int lod;
	public float visibleDstThreshold;

	public float sqrVisibleDstThreshold{
		get{
			return visibleDstThreshold*visibleDstThreshold;
		}
	}
}
