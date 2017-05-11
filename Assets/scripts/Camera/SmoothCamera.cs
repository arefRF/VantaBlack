using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;
    public float left_bound, right_bound, upper_bound, lower_bound;
    private Coroutine last_co;

    public bool auto = true;
    // Use this for initialization
    void Start () {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (auto)
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            newPos.x = Mathf.Clamp(newPos.x, left_bound, right_bound);
            newPos.y = Mathf.Clamp(newPos.y, lower_bound, upper_bound);
            if(auto)
                transform.position = newPos;
            m_LastTargetPosition = target.position;
        }
    }
    public void ChangeOffset(float left, float right, float lower, float upper,bool manual)
    {
        if (manual)
            auto = false;
        if (right != 0)
            right_bound = right;
        if (left != 0)
            left_bound = left;
        if (upper != 0)
            upper_bound = upper;
        if (lower != 0)
            lower_bound = lower;
        if (manual)
        {
            auto = false;
            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            aheadTargetPos.x = Mathf.Clamp(aheadTargetPos.x, left_bound, right_bound);
            aheadTargetPos.y = Mathf.Clamp(aheadTargetPos.y, lower_bound, upper_bound);
            if (last_co != null)
                StopCoroutine(last_co);
            last_co = StartCoroutine(SmoothMove(aheadTargetPos));
        }
    }

    private IEnumerator SmoothMove(Vector3 pos)
    {
        float remain = (float)Toolkit.GetDistance(pos, transform.position);
        while(remain > float.Epsilon)
        {
           
            remain = (float)Toolkit.GetDistance(pos, transform.position);
            //Vector3 newPos = Vector3.SmoothDamp(transform.position, pos, ref m_CurrentVelocity, damping);
            Vector3 newPos = Vector3.MoveTowards(transform.position, pos, Time.smoothDeltaTime / 0.5f);
            transform.position = newPos;
            yield return null;
        }
        auto = true;
       
    }
}
