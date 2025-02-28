using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlaying : MonoBehaviour
{
    public AudioSource gameOverSFX;
	public AudioSource dieSFX;
	public AudioSource winSFX;
	public AudioSource toadSFX;
	public AudioSource oneUpSFX;
	public AudioSource bossWinSFX;
	public AudioSource coinSFX;
	public AudioSource fireBallSFX;
	public AudioSource flagSFX;
	public AudioSource jumpSFX;
	public AudioSource pipeSFX;
	public AudioSource powerSFX;
	public AudioSource stompSFX; 
	public AudioSource mainOST;

	public void GameOverSFX()
	{
		gameOverSFX.Play();
		mainOST.Stop();
	}
	public void DieSFX()
	{
		dieSFX.Play();
		mainOST.Stop();
	}
	public void WinSFX()
	{
		winSFX.Play();
		mainOST.Stop();
	}
	public void ToadSFX()
	{
		toadSFX.Play();
		mainOST.Stop();
	}
	public void OneUpSFX()
	{
		oneUpSFX.Play();
	}
	public void BossWinSFX()
	{
		bossWinSFX.Play();
		mainOST.Stop();
	}
	public void CoinSFX()
	{
		coinSFX.Play();
	}
	public void FireBallSFX()
	{
		fireBallSFX.Play();
	}
	public void FlagSFX()
	{
		flagSFX.Play();
	}
	public void JumpSFX()
	{
		jumpSFX.Play();
	}
	public void PipeSFX()
	{
		pipeSFX.Play();
	}
	public void PowerSFX()
	{
		powerSFX.Play();
	}
	public void StompSFX()
	{
		stompSFX.Play();
	}
}
