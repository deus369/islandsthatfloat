using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{

	public Transform player;
	public Transform myPortalRelativeTransform, otherPortalRelativeTransform;

	private bool playerIsOverlapping = false;


	// Update is called once per frame
	void LateUpdate()
	{ 
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				otherPortalRelativeTransform.position = player.position;
				otherPortalRelativeTransform.rotation = player.rotation;
				//otherPortal matches to player position and rotation

				myPortalRelativeTransform.localPosition = otherPortalRelativeTransform.localPosition;
				myPortalRelativeTransform.localRotation = otherPortalRelativeTransform.localRotation;
				//then myPortal matches local position+rotation of otherPortal

				player.position = myPortalRelativeTransform.position;
				player.rotation = myPortalRelativeTransform.rotation;
				//then transforms position of player to myPortal empty
				print("Player is teleported");

				playerIsOverlapping = false;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}
}