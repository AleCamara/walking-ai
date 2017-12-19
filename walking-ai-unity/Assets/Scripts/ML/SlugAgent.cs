using System.Collections.Generic;
using UnityEngine;

public class SlugAgent : Agent
{
    [Header("Specific to Slug")]
    public Transform bodyPreA;
    public Transform bodyA;
	public Transform bodyC;
    public Transform bodyB;
    public Transform bodyPreB;

    public float totalDisplacementToReset = 5f;
    public float bodySeparation = 0.5f;
    public float maxAlphaChangeDeg = 2f;

    [Header("Rewards")]
    public float stepPenalty = 0.01f;
    public float multiplierForActionSquared = 0.01f;
    public float multiplierForBodyVelocity = 1.0f;

    private float _previousAlpha = 0f;
    private float _previousT = 0f;
    private float _previousPositionC = 0f;
    private float _previousVelocityC = 0f;

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        state.Add(bodyA.position.x - bodyC.position.x);
        state.Add(bodyB.position.x - bodyC.position.x);
        float velocity = (bodyC.position.x - _previousPositionC) / Time.fixedDeltaTime;
        state.Add(velocity);
        float acceleration = (velocity - _previousVelocityC) / Time.fixedDeltaTime;
        state.Add(acceleration);

        _previousPositionC = bodyC.position.x;
        _previousVelocityC = velocity;

        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] action)
    {
        if (brain.brainParameters.actionSpaceType == StateType.continuous)
        {
            FakeContinuosAgentStep(action);
        }
        else
        {
            FakeDiscreteAgentStep(action);
        }
    }

    public void FakeContinuosAgentStep(float[] action)
    {
        Debug.Log(string.Format("Agent Step (continuous): {0}, {1}", action[0].ToString(), action[1].ToString()));

        float alpha_change = action[0];
        alpha_change = Mathf.Clamp(alpha_change, -1f, 1f) * maxAlphaChangeDeg * Mathf.Deg2Rad;

        float change_division = action[1];
        change_division = Mathf.Clamp01(change_division);

        DoUpdateSlugBody(alpha_change, change_division);
    }

    public void FakeDiscreteAgentStep(float[] action)
    {
        float alpha_change = 0f;
        float change_division = 0f;

        int actionInt = (int)action[0];
        Debug.Log(string.Format("Agent Step (discrete): {0}", actionInt.ToString()));
        if (actionInt == 0 || actionInt == 1)
        {
            actionInt = (actionInt * 2) - 1;
            alpha_change = actionInt * maxAlphaChangeDeg * Mathf.Deg2Rad;
            change_division = 0f;
        }
        if (actionInt == 2 || actionInt == 3)
        {
            actionInt = ((actionInt - 2) * 2) - 1;
            alpha_change = actionInt * maxAlphaChangeDeg * Mathf.Deg2Rad;
            change_division = 1f;
        }

        DoUpdateSlugBody(alpha_change, change_division);
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
        _previousPositionC = 0f;
        _previousVelocityC = 0f;
		_previousAlpha = 0f;
        _previousT = 0f;

        bodyPreA.localPosition = Vector3.zero;
        bodyA.localPosition = Vector3.zero + Vector3.right * bodySeparation;
        bodyC.localPosition = Vector3.zero + Vector3.right * 2f * bodySeparation;
        bodyB.localPosition = Vector3.zero + Vector3.right * 3f * bodySeparation;
        bodyPreB.localPosition = Vector3.zero + Vector3.right * 4f * bodySeparation;
    }

    public void DoUpdateSlugBody(float alphaChange, float t)
    {
        Debug.Log(string.Format("Update Slug Body: {0}, {1}", alphaChange.ToString(), t.ToString()));
        float currentAlpha = Mathf.Clamp(_previousAlpha + alphaChange, 0f, Mathf.PI * 0.5f);

        float delta_d = 2f * bodySeparation * (Mathf.Cos(currentAlpha) - Mathf.Cos(_previousAlpha));
        float delta_xa = (t - 1f) * delta_d;
        float delta_xb = t * delta_d;

        bodyA.position += Vector3.right * delta_xa;
        bodyB.position += Vector3.right * delta_xb;

        Vector3 newPositionC = bodyC.position;
        newPositionC.x = (bodyA.position.x + bodyB.position.x) * 0.5f;
        newPositionC.y = bodyA.position.y + bodySeparation * Mathf.Sin(currentAlpha);
        bodyC.position = newPositionC;

        bodyPreA.position = bodyA.position - Vector3.right * bodySeparation;
        bodyPreB.position = bodyB.position + Vector3.right * bodySeparation;

        if (done == false)
        {
            float velocityC = (bodyC.position.x - _previousPositionC) / Time.fixedDeltaTime;

            reward  = 0f;
            // Only apply bonus if displacement is positive (no increased penalty)
            reward += velocityC * multiplierForBodyVelocity;
            reward -= stepPenalty;
            reward -= alphaChange * alphaChange * multiplierForActionSquared;

#if UNITY_EDITOR
            Debug.Log("Reward: " + reward.ToString() + " ; Cum: " + CumulativeReward.ToString());
#endif
        }

		_previousAlpha = currentAlpha;
        _previousT = t;
    }
}
