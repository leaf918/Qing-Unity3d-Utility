using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPlayerUI : MonoBehaviour {
	private static SoundPlayerUI	instance;
	public static string		soundPathPrefix = "sounds/";

	public enum SoundName {
		arrow_hit_board,
		arrow_shoot,
		changlumanman,
		drum,
		MultiplayerGameStart
	}
	private List<AudioSource> audioSources = new List<AudioSource>();
	public static SoundPlayerUI Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = new GameObject( "SoundPlayerUI" ).AddComponent<SoundPlayerUI>();
				DontDestroyOnLoad( instance );
			}
			return(instance);
		}
	}

	private void PlayIt( AudioClip p_sound, bool p_loop )
	{
		foreach ( AudioSource audio in audioSources )   /* Look for free ones */
		{
			if ( !audio.isPlaying )                 /* Check if it is free */
			{
				audio.loop	= p_loop;
				audio.clip	= p_sound;
				audio.Play();
				return;
			}
		}
		audioSources.Add( gameObject.AddComponent<AudioSource>() ); /* Make a new one of none found. */
		audioSources[audioSources.Count - 1].loop	= p_loop;
		audioSources[audioSources.Count - 1].clip	= p_sound;
		audioSources[audioSources.Count - 1].Play();

		print( "add new one audio source" + audioSources.Count );
	}

	//SoundPlayerUI.Instance.PlayUISound(SoundPlayerUI.SoundName.arrow_hit_board);

	public void PlayUISound( SoundName sn, bool repeat = false )
	{
		AudioClip ac = Resources.Load( soundPathPrefix + sn.ToString() ) as AudioClip;
		PlayIt( ac, repeat );
	}


	public void StopLooping(SoundName sn  )
	{
		string p_YourLoopingSound=sn.ToString();
		foreach ( AudioSource audio in audioSources )
		{
			if ( audio.clip.name == p_YourLoopingSound )
				audio.Stop();
		}
	}
}
