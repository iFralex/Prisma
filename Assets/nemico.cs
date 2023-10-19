using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class nemico : Photon.MonoBehaviour
{
	AIDestinationSetter ai;
	public int hp = 500;

    void Start()
    {
		ai = GetComponent<AIDestinationSetter>();
    }

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (col.gameObject.GetComponent<PhotonView>().isMine)
				col.gameObject.GetComponent<movimento>().Danno(50);
			ai.scontrato = true;
			ai.ai.canMove = false;
		}
	}

	private void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			ai.scontrato = false;
			ai.ai.canMove = true;
		}
	}

	public void Danno(int d)
    {
		photonView.RPC("DannoOnline", PhotonTargets.AllBufferedViaServer, d);
    }

	[PunRPC]
	void DannoOnline(int d)
    {
		hp -= d;
		if (hp < 0)
		{
			usaOggetti.cambTes.GetComponent<gameManager>().ai.Remove(GetComponent<AIDestinationSetter>());
			movimento.statistiche.teschi++;
			if (photonView.isMine)
				PhotonNetwork.Destroy(gameObject);
		}
    }
}
