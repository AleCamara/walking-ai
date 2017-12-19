using UnityEngine;

public class TestSlugMovement : MonoBehaviour
{
    private void Update()
    {
        SlugAgent[] slugAgents = FindObjectsOfType<SlugAgent>();
        foreach (SlugAgent slug in slugAgents)
        {
            if (Time.frameCount % 1000 == 0)
            {
                slug.AgentReset();
            }
            else
            {
                const float angleRange = 100f;
                const float tRange = 5f;
                float[] action = { Random.Range(-angleRange, angleRange), Random.Range(-tRange, tRange) };
                slug.FakeContinuosAgentStep(action);
            }
        }
    }
}
