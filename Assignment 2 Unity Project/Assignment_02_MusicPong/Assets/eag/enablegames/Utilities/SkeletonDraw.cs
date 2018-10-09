using UnityEngine;
using System.Collections;
/** 
 * Draw skeleton of skeletonRoot with boneMesh for bones, and lines for axes.  Must be attached to camera for PostRender to work.
 */
public class SkeletonDraw : MonoBehaviour
{

	public GameObject skeletonRoot; //! The root of the hierarchy to start drawing the skeleton from
	public Mesh boneMesh; 			//! A mesh to use to represent the bones (must be unit length)
	public Material boneMat; 		//! The material to draw the boneMesh with
	public float boneScale = 0.05f;	//! Scale of the boneMesh in the non-bone dimension.  The bone length is the other dimension.
	public float axisLen = 0.05f;	//! The length of each axis (X,Y,Z) that shows the orientation of the bone.
	public Material lineMat;  		//! The material to shade the lines.  Must use ColorBlended or UnlitColor shader material to get correct colors set by GL.Color
	//private Material lineMat;

	/**
	 * Initialize the skeleton
	 */
	void Awake()
	{
	//	lineMat = new Material (Shader.Find ("ColorBlended"));
    //  There should be a way to create the appropriate shader. If wrong shader is used, lines will not appear R,G,B
	}

	/**
	 * Draws bone as line and mesh based solely on position, and draws XYZ axis with orienation based soley on local rotation
	 */
	void drawbone(Transform t)
	{
		//renderer.SetWidth(5.0, 2.0);
		foreach ( Transform child in t)
		{
			float len = axisLen;
			Vector3 localX = new Vector3(len,0,0);
			Vector3 localY = new Vector3(0,len,0);
			Vector3 localZ = new Vector3(0,0,len);
			localX = child.rotation * localX;
			localY = child.rotation * localY;
			localZ = child.rotation * localZ;

			//draw bone line (from 10-90% along line between parent and child)
			DrawGLLine(t.position * 0.1f + child.position * 0.9f,  t.position * 0.9f + child.position * 0.1f, Color.white);
			//draw bone mesh
			DrawGLMesh(t.position * 0.1f + child.position * 0.9f,  t.position * 0.9f + child.position * 0.1f, Color.white);

			//draw axes 
			DrawGLLine(child.position,  child.position+localX, Color.red); //BUG hack: not sure why have to draw twice to get red...
			DrawGLLine(child.position,  child.position+localX, Color.red); 
			DrawGLLine(child.position,  child.position+localY, Color.green); 
			DrawGLLine(child.position,  child.position+localZ, Color.blue); 

			drawbone(child);
		}
		//LineRenderer.SetWidth(1.0, 1.0);
	}


	/**
	 * Draws a line between two points in immediate mode GL
	 */
	void DrawGLLine(Vector3 start, Vector3 end, Color color ) {

		//print("DrawGLLine:" + start + " TO " + end);
		//print("COLOR=" + color);
		GL.Begin (GL.LINES);
		lineMat.SetPass (0);
		GL.Color (color);
		GL.Vertex3 (start.x, start.y, start.z);
		GL.Vertex3 (end.x, end.y, end.z);
		GL.End ();

	}

	/*
	 * Draws a mesh in immediate mode GL
	 */
	void DrawGLMesh(Vector3 start, Vector3 end, Color color) {
		if (boneMesh != null) {
			boneMat.SetPass (0);
			Vector3 direction = end - start;
			Vector3 scale = new Vector3(boneScale,boneScale, direction.magnitude);
			Quaternion rotation = Quaternion.LookRotation(direction.normalized);
			//Quaternion rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
			Vector3 position = (start + end)/2.0f;
			Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);

			Graphics.DrawMeshNow (boneMesh, m);	
		}
	}
		
	/**
	 * Draws on the camera's post-render.  Must be attached to camera to work
	 */
	void OnPostRender() {
		if (skeletonRoot != null)
			drawbone(skeletonRoot.transform);
	}

	/**
	 * Draws on the camera's gizmo cycle.  Must be attached to camera to work
	 */
	void OnDrawGizmos()
	{
		if (skeletonRoot != null)
			drawbone(skeletonRoot.transform);
	}
}

/* 
 * OLD CODE.  Other ways to draw lines.
				 
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(t.position * 0.1f + child.position * 0.9f,  t.position * 0.9f + child.position * 0.1f);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(child.position,  child.position+localX);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(child.position,  child.position+localY);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(child.position,  child.position+localZ);
			Debug.DrawLine(t.position * 0.1f + child.position * 0.9f,  t.position * 0.9f + child.position * 0.1f, Color.white);

			Debug.DrawLine(child.position,  child.position+localX, Color.red); 
			Debug.DrawLine(child.position,  child.position+localY, Color.green); 
			Debug.DrawLine(child.position,  child.position+localZ, Color.blue); 
		
			* 
			* Depricated.  Makes a ton of GameObjects.
			void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.20f)
			{
				GameObject myLine = new GameObject();
				myLine.transform.position = start;
				myLine.AddComponent<LineRenderer>();
				LineRenderer lr = myLine.GetComponent<LineRenderer>();
				lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
				lr.SetColors(color, color);
				lr.SetWidth(0.1f, 0.1f);
				lr.SetPosition(0, start);
				lr.SetPosition(1, end);
				GameObject.Destroy(myLine, duration);
			}
*/

				/*
				DrawLine(t.position * 0.1f + child.position * 0.9f,  t.position * 0.9f + child.position * 0.1f, Color.white);

				DrawLine(child.position,  child.position+localX, Color.red); 
				DrawLine(child.position,  child.position+localY, Color.green); 
				DrawLine(child.position,  child.position+localZ, Color.blue); 
				*/
