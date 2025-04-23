using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;
    public float arcHeight = 2f;
    public int resolution = 20;
    public int showResolution = 5;
    public float animationSpeed = 20;
    public Color startColor = Color.red;
    public Color endColor = Color.yellow;
    private int currentIndex => (int)timeFlow % (resolution + showResolution);
    public float timeFlow = 0;
    
    void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.positionCount = resolution;
    }
    
    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            return;
        }
        UpdateAttackLine(startPoint.position, endPoint.position);
    }

    public void SetTransform(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
        if (start == null || end == null)
        {
            lineRenderer.positionCount = 0;
            timeFlow = 0;
        }
    }

    void UpdateAttackLine(Vector3 start, Vector3 end)
    {
        // 计算控制点（固定高度）
        timeFlow += animationSpeed * Time.deltaTime;
        Vector3 midPoint = (start + end) * 0.5f;
        Vector3 controlPoint = midPoint + Vector3.up * arcHeight;
        int endIdx = Mathf.Min(resolution, currentIndex);
        int startIdx = Mathf.Max(0, currentIndex - showResolution);
        lineRenderer.positionCount = endIdx - startIdx;
        // 生成曲线点
        for (int i = startIdx; i < endIdx; i++)
        {
            float t = i / (float)(resolution - 1);
            Vector3 point = CalculateQuadraticBezierPoint(t, start, controlPoint, end);
            lineRenderer.SetPosition(i-startIdx, point);
        }
    }
    
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0; 
        p += 2 * u * t * p1; 
        p += tt * p2; 
        return p;
    }
}
