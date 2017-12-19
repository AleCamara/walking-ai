using UnityEngine;

public class UpdateSlugBody : MonoBehaviour
{
    public LineRenderer lineRenderer = null;
    public Transform[] slugTransforms = new Transform[0];

    private void Update()
    {
        int numPositions = slugTransforms.Length;
        Vector3[] positions = new Vector3[numPositions];
        for (int positionIdx = 0; positionIdx < numPositions; ++positionIdx)
        {
            positions[positionIdx] = slugTransforms[positionIdx].position;
        }
        lineRenderer.positionCount = numPositions;
        lineRenderer.SetPositions(positions);
    }
}
