using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace EGUI
{ 

     public static class EGUITools
     {

        public static Color DefaultHightLight = new Color(31/255.0f,181/255.0f,171/255.0f);
        public static Bounds getWorldBounds(RectTransform rect)
        {
            if (rect == null) return new Bounds();
            Vector3[] m_Corners = new Vector3[4];
            rect.GetWorldCorners(m_Corners);

            var vMin = new Vector3(float.MaxValue,float.MaxValue,float.MaxValue);
            var vMax = new Vector3(float.MinValue,float.MinValue,float.MaxValue);
            for(int i = 0; i < 4; i++)
            {
                vMin = Vector3.Min(vMin,m_Corners[i]);
                vMax = Vector3.Max(vMax, m_Corners[i]);
            }
            var bounds = new Bounds(vMin,Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }
	       
    }
}
