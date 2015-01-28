#pragma once
#include <list>
#include <map>
#include <SFML\Audio.hpp>
#include <iostream>
#include <memory>
#include <assert.h>

namespace Sound
{
	enum Music
	{
		MenuLoop,
		GameLoop,
		LoseLoop
	};
	enum SoundEffect
	{
		OgreDeath,
		PigDeath,
		CrabDeath,
		MageDeath,
		FairyDeath,
		DragonDeath,
		GlassBreak,
		Explosion,
		DragonFlame,
		DragonFireBall,
		EggCrack,
		Click,
		PowerUp,
		NONE
		
	};
}


class MySoundBuffer
{
private:
	std::map<Sound::SoundEffect,sf::SoundBuffer> mSoundMap;
public:
	void load(Sound::SoundEffect,const std::string & FileName);
	sf::SoundBuffer &  Get(Sound::SoundEffect);
	
};

class SoundManager
{
private:
	sf::Music music;
	std::map<Sound::Music,std::string> FileNames;
	float volume;

	std::list<sf::Sound> SFX;//sounds currently playing
	MySoundBuffer SoundBuffer;


public:
	SoundManager();
	void Play(Sound::Music background);//play background music

	void Play(Sound::SoundEffect effect);//play sound effect
	void RemoveStoppedSounds();//delete all the sounds from the list that aren't being played
	
	void SetPause(bool paused);
	void SetVolume(float newVolume);

};
