using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCollision : MonoBehaviour
{
    private Transform child;
    [SerializeField]
    private AudioClip nodeSound;
    private float nodeLength = 0.045f;
    [SerializeField]
    private Material material;
    private byte counter = 0;
    private void Start()
    {
        child = transform.GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Node"))
        {
            addNode(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        counter++;
        if (counter%4 == 0)
        {
            slice();
        }
    }
    private void addNode(Collider other)
    {
        AudioSource.PlayClipAtPoint(nodeSound, other.transform.localPosition);
        Vibrator.vibrate();
        Vector3 newScale = child.localScale;
        newScale.y += other.transform.localScale.y * 2;
        child.localScale = newScale;
        Destroy(other.gameObject);
        StartCoroutine(makeStackBigger());
    }
    private void burnNode()
    {
        if (child.localScale.y > 0)
        {
            child.localScale = new Vector3(child.localScale.x, child.localScale.y - nodeLength, child.localScale.z);
        }
        else
        {
            Debug.Log("oyun bitti");
        }
    }
    private IEnumerator makeStackBigger()
    {
        Vector3 scale = child.localScale;
        child.DOScale(scale * 1.5f, 0.1f).OnComplete(() => child.DOScale(scale, 0.1f));
        yield return new WaitForSeconds(0.05f);
    }
    private void slice()
    {
        Debug.Log("calisti");
        GameObject slicedRight = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        slicedRight.GetComponent<MeshRenderer>().material = material;
        slicedRight.transform.localScale = new Vector3(child.transform.localScale.x, nodeLength, child.transform.localScale.z);
        slicedRight.transform.rotation = child.localRotation;
        slicedRight.transform.position = new Vector3(child.localScale.y / 2 - nodeLength, child.transform.position.y, child.transform.position.z - nodeLength);
        Destroy(slicedRight.transform.GetComponent<CapsuleCollider>());
        slicedRight.AddComponent<Rigidbody>();
        Destroy(slicedRight , 1f);
        GameObject slicedLeft = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        slicedLeft.GetComponent<MeshRenderer>().material = material;
        slicedLeft.transform.localScale = new Vector3(child.transform.localScale.x, nodeLength * 2, child.transform.localScale.z);
        slicedLeft.transform.rotation = child.localRotation;
        slicedLeft.transform.position = new Vector3(-(child.localScale.y / 2 - nodeLength), child.transform.position.y, child.transform.position.z - nodeLength);
        Destroy(slicedLeft.transform.GetComponent<CapsuleCollider>());
        slicedLeft.AddComponent<Rigidbody>();
        Destroy(slicedLeft , 1f);
        burnNode();
    }
}
