using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//只绘制曲线，不再做其它逻辑，由其父类完成
using EGUI;



public class ESingleCurve : BaseImageGraph
{

	#region curve var
	public List<float> dataList=new List<float>();  //最终应该 private
	[SerializeField] private float startY=0.2f;
	[SerializeField] private float endY=0.8f;   //即 曲线的高度只占用图片（画布）的一部分  宽度占图片的全部



	[SerializeField] private float xoffset=0.0f;
	[SerializeField] private float yoffset=0.0f;
	[SerializeField] private float yMin=0.0f;
	[SerializeField] private float yMax=0.0f;
	[SerializeField] private int dataCount = 0;

	private Color curveColor=new Color(34/255.0f,180/255.0f,172/255.0f);
	[SerializeField] private bool isSmallScale = true;
	#endregion 



	#region  coordinate var
	public bool isUseCoordinate = false; //是否需要为该曲线添加坐标轴 ，默认情况下 坐标轴充满整个图线区域
	public float useYRefrence=-1f;
	[SerializeField] private Image img_xcoordinate;
	[SerializeField] private Image img_ycoordinate;
	[SerializeField] private Text tex_scalePrefab;
	[SerializeField] private int xScaleCount=5;
	[SerializeField] private int yScaleCount = 5;
	[SerializeField] private Text GraphTitle;
	[SerializeField] private Image PlayRefrenceLine; //当播放数据时，实时显示播放的进度
	#endregion



	#region   external function

	public void SetDatas(List<float> list,Color color,ref Vector2 outVector){
		
		if (list != null && list.Count > 0) {
			this.dataList = list;
		}
		if (color !=Color.white) {
			curveColor = color;
		}

		dataCount = dataList.Count;

		if (isUseCoordinate) 
		{
			yMin = float.MaxValue;
			yMax = float.MinValue;
			for (int i = 0; i < dataList.Count; i++) {
				if (dataList [i] < yMin)
					yMin = dataList [i];
				if (dataList [i] > yMax)
					yMax = dataList [i];
			}
			outVector.x = yMin;
			outVector.y = yMax;
			PlayRefrenceLine.gameObject.SetActive (true);
		} 
		RefreshLayout (isSmallScale);
	}

	/// <summary>
	/// 附加逻辑，由于存在多条曲线 在同一个坐标系下的情况，所以多条曲线需要在同一个坐标系环境下显示（ymax ymin ect）
	/// </summary>
	public void SetCurveInfo(float yMin ,float yMax){
		this.yMax = yMax;
		this.yMin = yMin;
	}
	public void RefreshLayout(bool isSmallScale=true){
		base.Start ();
		this.isSmallScale = isSmallScale;
		xoffset = this.width / dataCount;
		yoffset = (endY - startY) * this.height / (yMax - yMin);
		this._image.SetAllDirty ();
		if (isUseCoordinate) {
			if (isSmallScale) {
				GraphTitle.fontSize = 16;
			} else {
				GraphTitle.fontSize = 32;
			}
		} 
		if (dataList.Count == 0) {
			yMin = 0;
			yMax = 0;
			dataCount = 0;
			useYRefrence = -1;
		}
		setCoordinate ();
	}


	public void PlaySingleFrameData(int PressureFrame){
		if (PressureFrame >= 0 && PressureFrame < dataCount&&isUseCoordinate)
		{
			float x = PressureFrame*Xoffset;
			float _y=PlayRefrenceLine.rectTransform.anchoredPosition.y;
			PlayRefrenceLine.rectTransform.anchoredPosition = new Vector2 (x,_y);
		}
	}


	public void Clear(){
		dataList.Clear ();
		if (isUseCoordinate) {
			PlayRefrenceLine.gameObject.SetActive (false);
			useYRefrence = -1;
			giveAllChildText (img_xcoordinate.transform);
			giveAllChildText (img_ycoordinate.transform);
		}
		this._image.SetAllDirty ();
	}

	public float getFrameData(int num){
		if (num >= 0 && num < dataList.Count) {
			return dataList[num];
		}
		return 0.0f;
	}


	public float Xoffset{
		get{ return xoffset;}
	}
	#endregion



	#region  internal function
	private void setCoordinate(){
		tex_scalePrefab.gameObject.SetActive (false);
		img_xcoordinate.gameObject.SetActive (isUseCoordinate);
		img_ycoordinate.gameObject.SetActive (isUseCoordinate);
		if (!isUseCoordinate)return;
		giveAllChildText (img_xcoordinate.transform);
		giveAllChildText (img_ycoordinate.transform);
		int fontSize = 24;
		if (isSmallScale)
			fontSize = 12;


		//x 轴
		float xCoordinateScaleOffset = this.width / (xScaleCount - 1);
		float xCoordinateScaleOffset_value = dataCount / (xScaleCount - 1);
		for (int i = 0; i < yScaleCount; i++) {
			Text t = getAText ();
			t.transform.SetParent (img_xcoordinate.transform,false);
			t.rectTransform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left,i * xCoordinateScaleOffset-30, 60);
			t.rectTransform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top,10, 30);
			t.text = (i * xCoordinateScaleOffset_value)+"";
			t.alignment = TextAnchor.MiddleCenter;
			t.fontSize = fontSize;
		}

		// y轴 
		float yCoordinateScaleOffset = this.height / (yScaleCount - 1);
		float yCoordinateScaleOffset_value = (yMax-yMin)/(endY-startY) /(yScaleCount - 1);
		float _dot0Value = getLineDot (new Vector2(startY,yMin),new Vector2(endY,yMax),0.0f);
		for (int i = 0; i < xScaleCount; i++) {
			Text t = getAText ();
			t.transform.SetParent (img_ycoordinate.transform,false);
		
			t.rectTransform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Right,10, 100);
			t.rectTransform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Bottom,i*yCoordinateScaleOffset-30, 60);
			t.text = (i*yCoordinateScaleOffset_value+_dot0Value).ToString("0.00");
			t.alignment = TextAnchor.MiddleRight;
			t.fontSize = fontSize;
		}

	}
	#endregion



	#region  resource pool

	//text
	private List<Text> listTex=new List<Text>();
	private Text getAText(){

		Text _t = null;
		if (listTex.Count > 0) {
			_t = listTex [0];
			listTex.RemoveAt (0);
		} else {
			GameObject go = GameObject.Instantiate (tex_scalePrefab.gameObject) as GameObject;
			_t = go.GetComponent<Text> ();
		}
		_t.gameObject.SetActive (true);
		return _t;
	}



	private void giveBackText(Text t){
		//t.gameObject.SetActive (false);
		listTex.Add (t);
	}

	private void giveAllChildText(Transform parent){
		foreach (Transform t in parent) {
			Text _tex=t.gameObject.GetComponent<Text> ();
			if ( _tex!= null) {
				_tex.text = "0.0";
				giveBackText (_tex);
			}
		}
	}


	#endregion


	#region   获取直线上的点  计算方法

	public float getLineDot(Vector2 d1,Vector2 d2,float x){
		float k = (d2.y - d1.y) / (d2.x - d1.x);
		float b =d1.y- d1.x * k;
		return (k * x + b);
	}

	public float getYRealPosByListValue(float v){
		return (yoffset * (v - yMin) + startY * height);
	}

	#endregion



	#region mono Event
	public override void ModifyMesh (UnityEngine.UI.VertexHelper vh)
	{
		base.ModifyMesh (vh);
		vh.Clear ();

		int lineSize = 3;
		if (isSmallScale)
			lineSize = 1;
		#region 数据曲线：
		List<Vector2> listTemp=new List<Vector2>();
		for(int i=0;i<dataList.Count;i++)
		{
			float x=i*xoffset;
			float y=yoffset*(dataList[i]-yMin)+startY*height;
			listTemp.Add(new Vector2(x,y));
		}
		List<UIVertex> lis = EMeshTools.getTriangleStrame (listTemp, lineSize, curveColor,true);
		vh.AddUIVertexTriangleStream (lis);
	    
		#endregion


		if (useYRefrence >= 0) {
			float _y = getYRealPosByListValue (useYRefrence);
			UIVertex[] lineX = GetQuaLine (new Vector2(0,_y),new Vector2(width,_y) ,1,Color.red);
			if (lineX != null && lineX.Length == 4) {
				vh.AddUIVertexQuad (lineX);
			}
		}

	}
		
	void Start(){
		if(PlayRefrenceLine!=null)
		   PlayRefrenceLine.gameObject.SetActive (false);
		useYRefrence = -1;
		if(this._image!=null)
		  this._image.SetAllDirty ();
	}

	#endregion
}
