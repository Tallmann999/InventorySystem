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

    // ��� 2D ����� ������������ ������ �������:
    public Vector2 checkOffset = Vector2.zero; // �������� ��������
    public Vector2 checkSize = new Vector2(0.5f, 0.5f); // ������ ������� ��������

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
            // ��������� ������ ����� �������, ��� ��� ������ ����� ���� ���������
            IInteractable interactable = currentInteractable;
            GameObject interactableObject = currentInteractableGameobject;

            // ������� ����� �������, ����� �������� ���������� �������������
            currentInteractable = null;
            currentInteractableGameobject = null;
            prompttext.gameObject.SetActive(false);

            // �������� ��������������
            interactable.OnInteract();
        }

        // � ���������� ����� ������� ���� �������� ��� ������ �� ������ ���������
        // �� ������ ����� � ray ��� ��� �� ������� �����. � ����� �������� ����� �������� ��� 
        // �������� �������� �� ������ ����� ������, ����� ����������, ���� ������� �� ���������. 
        // ����� ������� ��������� ����������� bool � ������ �� ������� ����� ���������� ��� � ��� ��������� �� �������� 
        // ��� ����� ����� � ��������� � ����� �������� ���������.
        // ��� ������� ������ ������� ��� ������� �� ������� ������. ���� � ���� ����� ������� ���������� �������, 
        // ���� ����, �� �� ������������� ��� ���� ����� ������������ � ����� ���� �� �����
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

    // ������������ ������� �������� � ���������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)checkOffset, checkSize);

        // ��� �������� ��������:
        // Gizmos.DrawWireSphere(transform.position, maxChekDistanse);
    }
}