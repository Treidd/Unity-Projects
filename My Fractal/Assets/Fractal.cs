using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

	public Mesh mesh;
	public Material material;
	public int maxDepth;
	public int depth;
	public float childScale;
	private Material[] materials;

	private static Vector3[] direction = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};

	private static Quaternion[] orientation = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f)
	};

	void Start () {
		gameObject.AddComponent<MeshFilter> ().mesh = mesh;
		gameObject.AddComponent<MeshRenderer> ().material = material;
		if (depth < maxDepth) {
			StartCoroutine (Generator ());
		}
	}

	private void Inicializar(Fractal parent,int childCount)
	{
		mesh = parent.mesh;
		material = parent.material;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		transform.parent = parent.transform;
		childScale = parent.childScale;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = direction [childCount] * (0.5f + 0.5f * childScale);
		transform.localRotation = orientation [childCount];
	}

	private IEnumerator Generator()
	{
		for (int i = 0; i < direction.Length; i++) {
			yield return new WaitForSeconds (Random.Range (0.2f,0.6f));
			new GameObject ("New Child " + depth).AddComponent<Fractal> ().Inicializar (this, i);
		}
	}

	private void InitializeMaterials (){
		materials = new Material[maxDepth + 1];
		for (int i = 0; i <= maxDepth; i++) {
			materials [i] = new Material (material);
			materials[i].color = Color.Lerp (Color.white, Color.red, (float)i/maxDepth);
		}
	}
}
