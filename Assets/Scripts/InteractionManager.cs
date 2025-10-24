using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float chekRate = 0.05f;
    private float lastChekTime;
    public float maxChekDistanse;
    public LayerMask layerMask;

    private GameObject currentInteractableGameobject;
    private IInteractable currentInteractable;

    public TextMeshProUGUI prompttext;

    // Для 2D можно использовать разные подходы:
    public Vector2 checkOffset = Vector2.zero; // Смещение проверки
    public Vector2 checkSize = new Vector2(0.5f, 0.5f); // Размер области проверки

    private void Update()
    {
        OnInteractInput();
        InteractFunction();
    }

    private void InteractFunction()
    {
        if (Time.time - lastChekTime > chekRate)
        {
            lastChekTime = Time.time;

            Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + checkOffset,
                checkSize,0f,layerMask);

            if (hit != null && hit.gameObject != currentInteractableGameobject)
            {
                currentInteractableGameobject = hit.gameObject;
                currentInteractable = hit.GetComponent<IInteractable>();

                if (currentInteractable != null)
                {
                    SetPrompText();
                }
                else
                {
                    ClearInteractable();
                }
            }
            else if (hit == null)
            {
                ClearInteractable();
            }
        }
    }

    //private void OnInteractInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        currentInteractable.OnInteract();
    //        currentInteractable = null;
    //        currentInteractableGameobject = null;
    //    }
    //}

    private void OnInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            // Сохраняем ссылки перед вызовом, так как объект может быть уничтожен
            IInteractable interactable = currentInteractable;
            GameObject interactableObject = currentInteractableGameobject;

            // Очищаем перед вызовом, чтобы избежать повторного использования
            currentInteractable = null;
            currentInteractableGameobject = null;
            prompttext.gameObject.SetActive(false);

            // Вызываем взаимодействие
            interactable.OnInteract();
        }

        // в дополнении можно сделать чтоб работало при поиске на поляне предметов
        // по щелчку мышки и ray луч был на курсоре мышки. и тогда механика будет работать так 
        // Персонаж нажимает на чёрный экран поляны, туман расходится, пока попытки не кончились. 
        // когда попытки кончились срабатывает bool и теперь на курсоре мышки появляется Луч и при наведении на ппредмет 
        // его можно взять и поместить в лоток выпавших предметов.
        // Или сделать другой вариант При нажатии на участок чёрный. если в поле этого участка появляется предмет, 
        // хоть край, то он автоматически или даже сразу перемещается в лоток чтоб всё взять
    }

    private void ClearInteractable()
    {
        currentInteractable = null;
        currentInteractableGameobject = null;
        prompttext.gameObject.SetActive(false);
    }

    private void SetPrompText()
    {
        prompttext.gameObject.SetActive(true);
        prompttext.text = string.Format("<B>[E]</B> {0}", currentInteractable.GetInteractPrompt());
    }

    // Визуализация области проверки в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)checkOffset, checkSize);

        // Для круговой проверки:
        // Gizmos.DrawWireSphere(transform.position, maxChekDistanse);
    }
}