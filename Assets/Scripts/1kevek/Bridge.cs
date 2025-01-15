using UnityEngine;
using static ActionScript;

public class Bridge : BaseAction
{ // Угол, к которому нужно повернуться
    public float rotationSpeed = 2f; // Скорость вращения
    private int count = 0;
    public override void ExecuteAction()
    {
        count++;
    }
    private void Update()
    {
        if (count == 1)
        {
            float currentAngle = transform.eulerAngles.x;

            // Плавно интерполируем угол
            float newAngle = Mathf.LerpAngle(currentAngle, -45, rotationSpeed * Time.deltaTime);

            // Применяем новый угол к объекту
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        if(count == 2)
        {
            float currentAngle = transform.eulerAngles.x;

            // Плавно интерполируем угол
            float newAngle = Mathf.LerpAngle(currentAngle, -91f, rotationSpeed * Time.deltaTime);

            // Применяем новый угол к объекту
            transform.eulerAngles = new Vector3(newAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
