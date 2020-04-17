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

	public ObjectPlacingList objectPlacingListOriginal;
	ObjectPlacingList objectPlacingList;

	public PlayerMove playerScript;
	public GameObject background;

	void Start() {
		objectPlacingList = ScriptableObject.CreateInstance<ObjectPlacingList>();
		objectPlacingList.objectsSettings = (ObjectData[]) objectPlacingListOriginal.objectsSettings.Clone();
		playerScript.objPlacingList = objectPlacingList;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

		UpdateVisibleChunks();

		//generandole estrellas unicamente
		ObjectGenerator.GenerateStarsInGame(objectPlacingList, chunkSize,  background, new Vector2(0,0),
			viewer.localScale.x, PlayerMove.score );
	}

	void Update() {

		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z);//mapGenerator.biomeData.meshSettings.meshScale;
		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();

		}
	}

	void UpdateVisibleChunks() {
		
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

						Chunk newChunk = new Chunk (viewedChunkCoord, transform, viewer, objectPlacingList, maxViewDst, chunkSize);
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
		yield return new WaitForSeconds(1);
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
