using UnityEngine;
using System.Collections;
using EGUI;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MySwing.Balance
{



	public class EGUICurve:BaseImageGraph
	{
		public List<float> listPoint = new List<float> ();
		public int lineWidth=2;


		[SerializeField] private float OriginXStart = 0.1f;
		[SerializeField] private float OriginYStart=0.1f;
		[SerializeField] private float CoordinateXEnd = 0.9f;
		[SerializeField] private float CoordinateYEnd=0.9f;


		[SerializeField] private float YStartDraw=0.2f;
		[SerializeField] private float YEndDraw=0.8f;
		[SerializeField] private float coordinateLineWidth=0.0001f;


		[SerializeField] private float XReferencePos_1=0.2f;



		public Text texPrefab;



		public int xScaleCount=5;
		public int yScaleCount=5;

		public float xMin=0.0f;
		public float xMax=100.0f;
		public float yMin=0.0f;
		public float yMax=100.0f;




		public void setScalePos(){
			
			base.Start();
			float _xOffsetValue = (xMax - xMin) / (xScaleCount - 1);
			float _yOffsetValue=(yMax-yMin)/(yScaleCount-1);

			float _xOffsetPos=(CoordinateXEnd-OriginXStart)/(xScaleCount - 1);
			float _yOffsetPos=(CoordinateYEnd-OriginYStart)/(yScaleCount - 1);


			giveAllChild (this.gameObject.transform);
			for (int i = 0; i < xScaleCount; i++)
			{
				
				GameObject _go = getANewGameObject ();
				if (_go != null)
				{
					RectTransform _rt = _go.GetComponent<RectTransform> ();
					_rt.SetParent (texPrefab.transform.parent, false);
					_rt.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, (OriginXStart + i * _xOffsetPos) * width, 100);
					_rt.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Bottom, OriginYStart * height-40, 30);
					_rt.gameObject.GetComponent<Text> ().text = (xMin + i * _xOffsetValue).ToString("0.0");
				}
			}


			for (int i = 0; i < yScaleCount; i++)
			{

				GameObject _go = getANewGameObject ();
				if (_go != null)
				{
					RectTransform _rt = _go.GetComponent<RectTransform> ();
					_rt.SetParent (texPrefab.transform.parent, false);
					_rt.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Bottom, (OriginYStart + i * _yOffsetPos) * height, 20);
					_rt.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, OriginXStart * width-40, 100);
					_rt.gameObject.GetComponent<Text> ().text = (yMin + i * _yOffsetValue).ToString("0.0");
				}
			}


		}


		protected override void OnRectTransformDimensionsChange ()
		{
			base.OnRectTransformDimensionsChange ();
		}



		public bool isTrued=false;
		public override void ModifyMesh (UnityEngine.UI.VertexHelper vh)
		{
			initImage();
			vh.Clear();
			Vector2 StartPoint = new Vector2 (OriginXStart * width,OriginYStart*height);
			Vector2 EndPointX = new Vector2 (CoordinateXEnd * width, OriginYStart*height);
			Vector2 EndPointY = new Vector2 (OriginXStart * width, CoordinateYEnd * height);
			#region 数据曲线：

			int count=listPoint.Count;
			float minValue=float.MaxValue;
			float maxValue=float.MinValue;

			for(int i=0;i<listPoint.Count;i++){
				if(listPoint[i]>maxValue){
					maxValue=listPoint[i];
				}
				if(listPoint[i]<minValue){
					minValue=listPoint[i];
				}
			}
			xMin=0;
			xMax=listPoint.Count;
			yMin=minValue;
			yMax=maxValue;

			float xoffset=(EndPointX.x-StartPoint.x)/count;

			float yoofset=((YEndDraw-YStartDraw)*height)/(maxValue-minValue);

			List<Vector2> listTemp=new List<Vector2>();
			for(int i=0;i<listPoint.Count;i++){
				
				float x=i*xoffset+StartPoint.x;
				float y=yoofset*(listPoint[i]-minValue)+YStartDraw*height;
				listTemp.Add(new Vector2(x,y));
			}



			List<UIVertex> lis = EMeshTools.getTriangleStrame (listTemp, 1,Color.green	
				,true);
			vh.AddUIVertexTriangleStream (lis);



			#endregion

			#region 画坐标轴
		

			UIVertex[] lineX = GetQuaLine (StartPoint, EndPointX, 2,Color.gray);
			if (lineX != null && lineX.Length == 4) {
				vh.AddUIVertexQuad (lineX);
			}

			UIVertex[] lineY = GetQuaLine (StartPoint, EndPointY, 2,Color.gray);
			if (lineY != null && lineY.Length == 4) {
				vh.AddUIVertexQuad (lineY);
			}

			#endregion

		    
			#region 绘制参考线
			/*
			Vector2 StartPoint_R1 = new Vector2 (XReferencePos_1 * width,OriginYStart*height);
			Vector2 EndPoint_R1 = new Vector2 (XReferencePos_1 * width, CoordinateYEnd*height);
			UIVertex[] line_X1 = GetQuaLine (StartPoint_R1, EndPoint_R1, 1,Color.red);
			if (lineX != null && lineX.Length == 4) {
				vh.AddUIVertexQuad (line_X1);
			}
			*/
			#endregion
		
		
		}

		private void initImage()
		{
			try
			{
				base.Start();
			}
			catch(System.Exception e1)
			{
				Debug.Log("initImage Exception:" + e1.ToString());
			}
		}




		#region external function
		/// <summary>
		/// 响应播放帧的变化，
		/// </summary>
		/// <param name="ClubFrame">Club frame.</param>
		/// <param name="PressureClub">Pressure club.</param>
		public void PlayFrame(int ClubFrame,int PressureClub){
			
		}

		#endregion







		//资源池
		public List<GameObject> texResourcePool = new List<GameObject> (); 


		private GameObject getANewGameObject(){
			GameObject _temp = null;
			if (texResourcePool.Count > 0) {
				_temp = texResourcePool [0];
				texResourcePool.RemoveAt (0);

			} else {
				_temp= GameObject.Instantiate (texPrefab.gameObject) as GameObject;
			}
			if(_temp!=null)
			  _temp.gameObject.SetActive (true);
			return _temp;
		}


		private void giveBackGameObject(GameObject go){
			texResourcePool.Add (go);
			go.SetActive (false);
		}

		private void giveAllChild(Transform T){
			foreach (Transform t in T) {
				giveBackGameObject (t.gameObject);
			}
		}



//		void OnGUI(){
//			if (GUILayout.Button ("ddfff")) {
//				this.gameObject.GetComponent<Image> ().SetAllDirty ();
//			}
//		}
	}

}
