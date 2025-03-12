using System.Collections;
using UnityEngine;



public interface ISelected
{
    public void Selected();
    public void ActionWhenSelected(Vector3 Pos);
    public void UnSelected();
}
public class Solder : MonoBehaviour, ISelected
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private GameObject SelectedBackground;
    [SerializeField] float HP = 100f;
    Animator Solderanimator;
    bool isMoving;
    void Start()
    {
        Solderanimator = GetComponent<Animator>();
    }
    void Update() 
    {

    }
    public void Selected()
    {
        SelectedBackground.SetActive(true);
    }
    public void ActionWhenSelected(Vector3 pos)
    {
        StartMoveToPos(pos);
    }
    public void UnSelected()
    {
        SelectedBackground.SetActive(false);
    }
    private void MoveToPos(Vector3 newPos)
    {   
        transform.position = Vector3.MoveTowards(transform.position, newPos, MoveSpeed * Time.deltaTime);
        if (transform.position != newPos)
        {
            if (!isMoving)
            {
                Solderanimator.SetBool("isWalking", true);
                isMoving = true;
            }
        }
        else
        {
            Solderanimator.SetBool("isWalking", false);
            isMoving = false;
        }
    }
    private IEnumerator IEMove(Vector3 newPos)
    {
        while (transform.position != newPos)
        {
            MoveToPos(newPos);
            yield return null;
        }
    }
    public void StartMoveToPos(Vector3 NewPos)
    {
        StopAllCoroutines();
        StartCoroutine(IEMove(NewPos));
    }
    public void Attack()
    {

    }
    public void takedamage(float Damage)
    {
        HP -= Damage;
        Solderanimator.SetTrigger("Hurt");
        if (HP <= 0)
        {
            Solderanimator.SetTrigger("Death");
            Invoke("Death", 0.6f);
        }
    }
    void Death()
    {
        gameObject.SetActive(false);
    }
}
