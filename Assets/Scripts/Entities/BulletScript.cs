using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public PlayerScript playerScript;
	public int number;

	[Space(10)]

	public Color color;

	[Header("Health")]
	public int damage;

	[Header("Movement")]
	public Vector2 moveDirection;
	public float moveSpeed;

	[Header("Debug")]
	private GameManager gameManager;
	private Rigidbody2D rb;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody2D>();
	}

	IEnumerator Start()
	{
		transform.SetParent(GameObject.Find("Bullets").transform);
		rb.velocity = moveDirection * moveSpeed;

		yield return new WaitForSeconds(10f);

		if (playerScript.photonView.IsMine)
			playerScript.photonView.RPC("DestroyBullet", RpcTarget.All, number);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (LayerMask.Equals(col.gameObject.layer, LayerMask.NameToLayer("Player")))
			if (playerScript.photonView.IsMine)
				col.gameObject.GetComponent<PlayerScript>().photonView.RPC("Damage", RpcTarget.All, damage, playerScript.photonView.ViewID);

		if (playerScript.photonView.IsMine)
			playerScript.photonView.RPC("DestroyBullet", RpcTarget.All, number);
	}

	public void SetColor(Color color)
	{
		GetComponent<Shapes2D.Shape>().settings.fillColor = this.color = color;
	}
}
