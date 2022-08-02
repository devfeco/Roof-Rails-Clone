using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackCollision : MonoBehaviour
{
    [SerializeField]
    private AudioClip gemSound;
    [SerializeField]
    private float slideSpeed;
    private Transform parent;
    [SerializeField]
    private Material material;
    private void Start()
    {
        parent = transform.parent;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Gem"))
        {
            collectGem(other);
        }
        if (other.transform.CompareTag("Cutter"))
        {
            StartCoroutine(slice(other.transform));
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Slider"))
        {
            slidePlayer();
        }
    }

    private void slidePlayer()
    {
        parent.GetComponent<Rigidbody>().velocity = Vector3.forward * slideSpeed;
    }

    private void collectGem(Collider other)
    {
        General.totalScore++;
        AudioSource.PlayClipAtPoint(gemSound, other.transform.localPosition);
        Vibrator.vibrate();
        Destroy(other.gameObject);
    }

    private IEnumerator slice(Transform other)
    {
        float currentScale = transform.localScale.y;
        float distance;
        if (other.position.x < 0)
        {
            currentScale -= transform.parent.position.x;
            distance = (currentScale + other.position.x) / 2;
            transform.localScale = new Vector3(transform.localScale.x , transform.localScale.y - distance , transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x + distance , transform.localPosition.y , transform.localPosition.z);
            GameObject slicedObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            slicedObj.transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);
            slicedObj.transform.rotation = transform.rotation;
            slicedObj.transform.position = new Vector3(-(currentScale / 1.5f - slicedObj.transform.localScale.y) , transform.position.y , transform.position.z);
            slicedObj.GetComponent<MeshRenderer>().material = material;
            Destroy(slicedObj.transform.GetComponent<CapsuleCollider>());
            slicedObj.AddComponent<Rigidbody>();
            Destroy(slicedObj, 1f);
            yield return new WaitForSeconds(1);
            moveOrigin();
        }
        else
        {
            currentScale += transform.parent.position.x;
            distance = (currentScale - other.position.x) / 2;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - distance, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x - distance, transform.localPosition.y, transform.localPosition.z);
            GameObject slicedObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            slicedObj.transform.localScale = new Vector3(transform.localScale.x, distance / 2, transform.localScale.z);
            slicedObj.transform.rotation = transform.rotation;
            slicedObj.transform.position = new Vector3(currentScale / 1.5f - slicedObj.transform.localScale.y, transform.position.y, transform.position.z);
            slicedObj.GetComponent<MeshRenderer>().material = material;
            Destroy(slicedObj.transform.GetComponent<CapsuleCollider>());
            slicedObj.AddComponent<Rigidbody>();
            Destroy(slicedObj , 1f);
            yield return new WaitForSeconds(1);
            moveOrigin();
        }
    }

    private void moveOrigin()
    {
        transform.DOMoveX(0f , 1f);
    }

}
