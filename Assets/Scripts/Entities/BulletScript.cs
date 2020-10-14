using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public PlayerScript owner;
	public int number;

	[Header("Health")]
	public int damage;

	[Header("Movement")]
	public Vector2 moveDirection;
	public float moveSpeed;

	[Header("Debug")]
	private GameManager gameManager;

	[Space(10)]

	private Rigidbody2D rb;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody2D>();
	}

	IEnumerator Start()
	{
		GetComponent<Shapes2D.Shape>().settings.fillColor = gameManager.teams[owner.team].color;
		transform.SetParent(GameObject.Find("Bullets").transform);

		rb.velocity = moveDirection * moveSpeed;

		yield return new WaitForSeconds(10f);

		Destroy();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (LayerMask.Equals(col.gameObject.layer, LayerMask.NameToLayer("Player")))
		{
			PlayerScript playerScript = col.gameObject.GetComponent<PlayerScript>();

			if (playerScript.team != owner.team && owner.photonView.IsMine)
				playerScript.photonView.RPC("Damage", RpcTarget.All, damage);
		}

		Destroy();
	}

	void Destroy()
	{
		if (owner != null)
		{
			if (owner.photonView.IsMine)
				owner.photonView.RPC("DestroyBullet", RpcTarget.All, owner.photonView.ViewID, number);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
