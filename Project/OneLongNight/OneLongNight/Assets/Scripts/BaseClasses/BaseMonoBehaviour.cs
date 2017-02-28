using UnityEngine;

public abstract class BaseMonoBehaviour : MonoBehaviour
{
	protected virtual void Awake()
	{
		UpdateManager.AddBehaviour(this);
	}

	public virtual void UpdateNormal(){}

	public virtual void UpdateFixed(){}

	public virtual void UpdateLate(){}
	
}
