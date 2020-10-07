using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviourPunCallbacks
{
	[Header("Control")]
	public int owner;
	public int team;

	[Header("Health")]
	public int damage;

	[Header("Movement")]
	public Vector2 moveDirection;
	public float moveSpeed;

	[Header("Appearance")]
	public Color color;

	[Header("Debug")]
	private GameManager gameManager;

	[Space(10)]

	private Rigidbody2D rb;

	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		transform.SetParent(GameObject.Find("Bullets").transform);
		Destroy(gameObject, 10f);

		rb.velocity = moveDirection * moveSpeed;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (LayerMask.Equals(col.gameObject.layer, LayerMask.NameToLayer("Player")))
		{
			PlayerScript playerScript = col.gameObject.GetComponent<PlayerScript>();
			PhotonView photonView = PhotonNetwork.GetPhotonView(owner);

			if (playerScript.team != team && photonView.IsMine)
				playerScript.photonView.RPC("Damage", RpcTarget.All, damage);
		}

		Destroy(gameObject);
	}

	public void SetTeam(int team)
	{
		GetComponent<Shapes2D.Shape>().settings.fillColor = gameManager.teams[this.team = team];
	}
}
