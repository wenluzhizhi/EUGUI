using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;




public class BaseImageGraph : BaseMeshEffect
{

    public Vector2 AnchorPoivot = Vector2.zero;
    public float width = 0.0f;
    public float height = 0.0f;
    public float widthOffset = 0.0f;
    public float heightOffset = 0.0f;
    public Image _image;

    protected override void Start()
    {
        AnchorPoivot = this.gameObject.GetComponent<Image>().rectTransform.pivot;
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        height = this.gameObject.GetComponent<RectTransform>().rect.height;
        _image = this.gameObject.GetComponent<Image>();
    }
    public override void ModifyMesh(VertexHelper vh)
    {
		//base.ModifyMesh (vh);
    }
    private float correctionPosX(float posX)
    {
        return posX - AnchorPoivot.x * width;
    }
    private float correctionPosY(float posY)
    {
        return posY - AnchorPoivot.y * height;
    }
    public Vector2 correctionPos(Vector2 v)
    {
        return new Vector2(correctionPosX(v.x), correctionPosY(v.y));
    }


    public UIVertex[] getQuaNotModify(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color c)
    {
        UIVertex[] list = new UIVertex[4];

        UIVertex u1 = new UIVertex();
        u1.position = v1;
        u1.color = c;
        list[0] = u1;

        UIVertex u2 = new UIVertex();
        u2.position = v2;
        u2.color = c;
        list[1] = u2;


        UIVertex u3 = new UIVertex();
        u3.position = v3;
        u3.color = c;
        list[2] = u3;


        UIVertex u4 = new UIVertex();
        u4.position = v4;
        u4.color = c;
        list[3] = u4;
        return list;
    }
    public UIVertex[] getQua(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color c)
    {
        UIVertex[] list = new UIVertex[4];

        UIVertex u1 = new UIVertex();
        u1.position = correctionPos(v1);
        u1.color = c;
        list[0] = u1;

        UIVertex u2 = new UIVertex();
        u2.position = correctionPos(v2);
        u2.color = c;
        list[1] = u2;


        UIVertex u3 = new UIVertex();
        u3.position = correctionPos(v3);
        u3.color = c;
        list[2] = u3;


        UIVertex u4 = new UIVertex();
        u4.position = correctionPos(v4);
        u4.color = c;
        list[3] = u4;


        return list;
    }
    public UIVertex[] getQua(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color c1,Color c2, Color c3, Color c4)
    {
        UIVertex[] list = new UIVertex[4];

        UIVertex u1 = new UIVertex();
        u1.position = correctionPos(v1);
        u1.color = c1;
        list[0] = u1;

        UIVertex u2 = new UIVertex();
        u2.position = correctionPos(v2);
        u2.color = c2;
        list[1] = u2;
        


        UIVertex u3 = new UIVertex();
        u3.position = correctionPos(v3);
        u3.color = c3;
        list[2] = u3;


        UIVertex u4 = new UIVertex();
        u4.position = correctionPos(v4);
        u4.color = c4;
        list[3] = u4;


        return list;
    }


	public UIVertex[] GetQuaLine(Vector2 startPoint,Vector2 EndPoint,float lineWidth,Color c)
	{
		if (Vector2.Distance(startPoint, EndPoint) > 0.0f)
		{
			Vector2 _temp = (EndPoint -startPoint).normalized;
			Vector2 _tempNoraml = new Vector3(-_temp.y, _temp.x) * lineWidth/2;
			return getQua (startPoint - _tempNoraml, startPoint + _tempNoraml, EndPoint + _tempNoraml, EndPoint - _tempNoraml, c);
		}
		return null;
	}
}
