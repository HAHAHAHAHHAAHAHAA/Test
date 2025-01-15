using UnityEngine;
public abstract class BaseAction : MonoBehaviour
{
    public abstract void ExecuteAction();
}
public class ActionScript : MonoBehaviour
{
    public BaseAction actionClass;

    public void ActionMethod()
    {
        actionClass.ExecuteAction();
        this.gameObject.tag = "Untagged";
        Destroy(this.gameObject.GetComponent<Collider>());
        Destroy(this.gameObject.GetComponent<ActionScript>());
    }
}
