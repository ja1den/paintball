using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class BulletScript : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public int actor;
	public int number;

	[Space(10)]

	public PlayerScript playerScript;

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
		actor = playerScript.photonView.OwnerActorNr;

		GetComponent<Shapes2D.Shape>().settings.fillColor = gameManager.teams[playerScript.team].color;
		transform.SetParent(GameObject.Find("Bullets").transform);

		rb.velocity = moveDirection * moveSpeed;

		yield return new WaitForSeconds(10f);

		if (playerScript.photonView.IsMine)
			playerScript.photonView.RPC("DestroyBullet", RpcTarget.All, playerScript.photonView.ViewID, number);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (LayerMask.Equals(col.gameObject.layer, LayerMask.NameToLayer("Player")))
		{
			PlayerScript script = col.gameObject.GetComponent<PlayerScript>();

			if (script.team != playerScript.team && playerScript.photonView.IsMine)
				script.photonView.RPC("Damage", RpcTarget.All, damage);
		}

		if (playerScript.photonView.IsMine)
			playerScript.photonView.RPC("DestroyBullet", RpcTarget.All, playerScript.photonView.ViewID, number);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (actor == otherPlayer.ActorNumber) Destroy(gameObject);
	}
}
