using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace EGUI
{

    public class EMeshTools : MonoBehaviour
    {
        /// <summary>
        ///给出两个点，返回四个点，用于刻画两点连线
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="lineWidth"></param>
        /// <returns></returns>
        public static List<Vector2> getTwoDotLine(Vector2 v1, Vector2 v2, float lineWidth)
        {
            return getFourDot(v1, v2, lineWidth);
        }
        private static List<Vector2> getFourDot(Vector2 v1, Vector2 v2, float lineWidth)
        {
            List<Vector2> _li = new List<Vector2>();
            float _halfWidth = lineWidth / 2;
            #region 策略2
            if (Vector3.Distance(v1, v2) == 0.0f) return null;
            Vector2 _temp = (v2 - v1).normalized;
            Vector2 _tempNoraml = new Vector2(-_temp.y, _temp.x) * _halfWidth;
            _li.Add(v1 - _tempNoraml);
            _li.Add(v1 + _tempNoraml);
            _li.Add(v2 + _tempNoraml);
            _li.Add(v2 - _tempNoraml);
            #endregion
            return _li;

        }

        private static List<Vector3> getFourDotVector3(Vector2 _v1, Vector2 _v2, float lineWidth)
        {
            List<Vector3> _li = new List<Vector3>();
            Vector3 v1 = new Vector3(_v1.x,_v1.y,0);
            Vector3 v2 = new Vector3(_v2.x, _v2.y, 0);
            float _halfWidth = lineWidth / 2;
            #region 策略2
            if (Vector3.Distance(v1, v2) == 0.0f) return null;
            Vector3 _temp = (v2 - v1).normalized;
            Vector3 _tempNoraml = new Vector2(-_temp.y, _temp.x) * _halfWidth;
            _li.Add(v1 - _tempNoraml);
            _li.Add(v1 + _tempNoraml);
            _li.Add(v2 + _tempNoraml);
            _li.Add(v2 - _tempNoraml);
            #endregion
            return _li;

        }


		/// <summary>
		/// 原始输入的序列是为点的序列，若想画线，需要将点的序列转化为能够画线的四边形序列
		/// </summary>
		/// <returns>The list dot.</returns>
		/// <param name="InList">In list.</param>
		/// <param name="lineWidth">Line width.</param>
        public static List<Vector3> getListDot(List<Vector2> InList, float lineWidth)
        {
            List<Vector3> _newList = new List<Vector3>();
            float _halfWidth = lineWidth / 2;
            for (int i = 0; i < InList.Count - 1; i++)
            {
                Vector3 _v1 = InList[i];
                Vector3 _v2 = InList[i + 1];
                if (Vector3.Distance(_v1, _v2) > 0.0f)
                {
                    Vector3 _temp = (_v2 - _v1).normalized;
                    Vector3 _tempNoraml = new Vector3(-_temp.y, _temp.x, 0) * _halfWidth;
                    _newList.Add(_v1 - _tempNoraml);
                    _newList.Add(_v1 + _tempNoraml);
                    _newList.Add(_v2 + _tempNoraml);
                    _newList.Add(_v2 - _tempNoraml);
                }
            }
            return _newList;
        }

        public static List<Vector3> getListDot(Vector2[] InList, float lineWidth)
        {
            List<Vector2> _newList1 = new List<Vector2>();
            for (int i = 0; i < InList.Length; i++)
            {
                _newList1.Add(InList[i]);
            }
            return getListDot(_newList1, lineWidth);
        }



        public static Mesh getNewMesh(List<Vector3> list)
        {
            try
            {

                Mesh mesh = new Mesh();

                List<int> tri = new List<int>();

                //RepairList(list,ref tri);
                for (int i = 0; i < list.Count; i += 4)
                {
                    tri.Add(i + 2);
                    tri.Add(i);
                    tri.Add(i + 1);

                    tri.Add(i + 2);
                    tri.Add(i + 3);
                    tri.Add(i);
                }
                mesh.SetVertices(list);

                mesh.SetTriangles(tri.ToArray(), 0);
                return mesh;
            }
            catch (System.Exception e1)
            {
                Debug.Log(e1.ToString());
                return null;
            }
        }
        public static Mesh getNewMesh(Vector2[] listArray)
        {

            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < listArray.Length; i++)
            {
                list.Add(listArray[i]);
            }
            return getNewMesh(list);
        }


        private static List<int> RepairList(List<Vector3> list, ref List<int> _listInt)
        {  //修补连接处的缺口

            #region 方案1 ，失败
            for (int i = 2; i < list.Count - 3; i += 4)
            {

                _listInt.Add(i);      //2
                _listInt.Add(i + 1);   //3
                _listInt.Add(i + 3);   //5

                _listInt.Add(i);
                _listInt.Add(i + 1);
                _listInt.Add(i + 2);   //4

                _listInt.Add(i + 2);
                _listInt.Add(i + 3);
                _listInt.Add(i);

                _listInt.Add(i + 2);
                _listInt.Add(i + 3);
                _listInt.Add(i + 1);
            }
            #endregion
            return _listInt;
        }


        public static List<Vector3> GetBSplineDot(List<Vector3> list)
        {
            List<Vector3> newList = new List<Vector3>();
            if (list != null && list.Count > 0)
            {
                newList.Add(list[0]);
                for (int i = 0; i < list.Count; i++)
                {
                    if (i + 2 < list.Count)
                    {
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.0f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.1f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.2f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.3f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.4f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.5f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.6f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.7f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.8f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.9f));
                        newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.99f));
                    }
                }
                newList.Add(list[list.Count - 1]);
            }
            return newList;
        }

		public static List<Vector2> GetBSplineDot(List<Vector2> list)
		{
			List<Vector2> newList = new List<Vector2>();
			if (list != null && list.Count > 0)
			{
				newList.Add(list[0]);
				for (int i = 0; i < list.Count; i++)
				{
					if (i + 2 < list.Count)
					{
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.0f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.1f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.2f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.3f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.4f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.5f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.6f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.7f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.8f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.9f));
						newList.Add(AddPoints(list[i], list[i + 1], list[i + 2], 0.99f));
					}
				}
				newList.Add(list[list.Count - 1]);
			}
			return newList;
		}


        private static Vector2 AddPoints(Vector2 v0, Vector2 v1, Vector2 v2, float t)
        {

            float xt = (v0.x + v1.x) * 0.5f + (v1.x - v0.x) * t + (v0.x - 2 * v1.x + v2.x) * 0.5f * t * t;
            float yt = (v0.y + v1.y) * 0.5f + (v1.y - v0.y) * t + (v0.y - 2 * v1.y + v2.y) * 0.5f * t * t;

            return new Vector2(xt, yt);
        }



        public static void MakeRect(CanvasRenderer canvasRender, Vector2 topLeft, Vector2 bottomRight, Material m)
        {

            Vector2[] m_points2 = new Vector2[5];
            int index = 0;
            m_points2[index] = new Vector2(topLeft.x, topLeft.y);
            m_points2[index + 1] = new Vector2(bottomRight.x, topLeft.y);
            m_points2[index + 2] = new Vector2(bottomRight.x, bottomRight.y);
            m_points2[index + 3] = new Vector2(topLeft.x, bottomRight.y);
            m_points2[index + 4] = new Vector2(topLeft.x, topLeft.y);

            List<Vector3> lis = getListDot(m_points2, 1);
            canvasRender.SetMaterial(m, null);
            canvasRender.SetMesh(getNewMesh(lis));

        }



        public static Mesh AddLine(Vector2 startPoint,Vector2 endPoint,float lineWidth)
        {
            try
            {
                Mesh mesh = new Mesh();
                List<Vector3> listPoint = getFourDotVector3(startPoint, endPoint, lineWidth);
                List<int> tri = new List<int>();
                for (int i = 0; i < listPoint.Count; i += 4)
                {
                    tri.Add(i + 2);
                    tri.Add(i);
                    tri.Add(i + 1);

                    tri.Add(i + 2);
                    tri.Add(i + 3);
                    tri.Add(i);
                }
                mesh.SetVertices(listPoint);
                mesh.SetTriangles(tri.ToArray(), 0);
                return mesh;

            }
            catch(System.Exception e1)
            {
                return null;
            }
        }


        #region  math


        /// <summary>
        /// 返回直线方程的参数：y=kx+b;
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="k"></param>
        /// <param name="b"></param>
        public static void getLineParameters(Vector2 v1,Vector2 v2,ref float k ,ref float b)
        {
            if (Vector3.Distance(v1, v2) <= 0.0f) return;
            k = (v2.y - v1.y) / (v2.x - v1.x);
            b = v2.y - k * v2.x;
            
        }
        #endregion



		#region  该处代码直接返回能够为VertexHelper 所用的 UIVertex 序列

		/// <summary>
		/// 根据根据
		/// </summary>
		/// <returns>The triangle strame.</returns>
		/// <param name="lis">Lis.</param>
		/// <param name="lineWidt">Line widt.</param>
		/// <param name="col">Col.</param>
		public static List<UIVertex> getTriangleStrame(List<Vector2> lis,float lineWidth,Color col,bool useBspline=false){
			if (useBspline) 
			{
				lis = GetBSplineDot (lis);
			}
			List<Vector3> lisV3=getListDot (lis, lineWidth);
			List<UIVertex> newUIVertexList = new List<UIVertex> ();
			for (int i = 0; i+3 < lisV3.Count; i+=4)
			{
				
			   getTriangle (lisV3 [i], lisV3 [i+1], lisV3 [i+2], lisV3 [i+3],newUIVertexList,col);
			}
			return newUIVertexList;
			 
		}

		private static void getTriangle(Vector3 v1,Vector3 v2, Vector3 v3,Vector3 v4,List<UIVertex> newUIVertexList,Color c){
			  
			UIVertex u1 = getUIVertByPos (v1);
			UIVertex u2 = getUIVertByPos (v2);
			UIVertex u3 = getUIVertByPos (v3);
			UIVertex u4 = getUIVertByPos (v4);
			u1.color = c;
			u2.color = c;
			u3.color = c;
			u4.color = c;

			newUIVertexList.Add (u1);
			newUIVertexList.Add (u2);
			newUIVertexList.Add (u3);

			newUIVertexList.Add (u3);
			newUIVertexList.Add (u4);
			newUIVertexList.Add (u1);
		}

		private static UIVertex getUIVertByPos(Vector3 v){
			UIVertex u1 = new UIVertex ();
			u1.position = v;

			return u1;
		}


		#endregion

    }
}
