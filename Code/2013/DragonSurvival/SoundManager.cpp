/*
	holds all the sfx of the game, and prevents them from interfering with each other
*/
#include "SoundManager.h"

void MySoundBuffer::load(Sound::SoundEffect newSound,const std::string & FileName)
{
	sf::SoundBuffer loadSoundBuffer;
	loadSoundBuffer.loadFromFile(FileName);
	auto inserted= mSoundMap.insert(std::make_pair(newSound,loadSoundBuffer));
}

sf::SoundBuffer & MySoundBuffer::Get(Sound::SoundEffect ID) 
{

	return mSoundMap.find(ID)->second;

}

SoundManager::SoundManager() :music(),FileNames(),volume(100.f),SFX()
{
	FileNames[Sound::Music::MenuLoop]= "Assets/Music/MenuLoop.wav";
	FileNames[Sound::Music::GameLoop]= "Assets/Music/GameLoop.wav";
	FileNames[Sound::Music::LoseLoop]= "Assets/Music/LoseMusic.wav";

	MySoundBuffer* newSound;
	//ogre death sfx
	SoundBuffer.load(Sound::SoundEffect::OgreDeath,"Assets/SFX/Ogre_Death.wav");

	//pig death sfx
	SoundBuffer.load(Sound::SoundEffect::PigDeath,"Assets/SFX/pig_scream.wav");
	
	//crab death sfx
	SoundBuffer.load(Sound::SoundEffect::CrabDeath,"Assets/SFX/Crab_Death.wav");

	//deagon death sfx
	SoundBuffer.load(Sound::SoundEffect::DragonDeath,"Assets/SFX/Dragon_Death.wav");

	//mage death sfx
	SoundBuffer.load(Sound::SoundEffect::MageDeath,"Assets/SFX/human_scream_pain_male.wav");

	//fairy death sfx
	SoundBuffer.load(Sound::SoundEffect::FairyDeath, "Assets/SFX/Fairy_Death.wav");

	//egg crack sfx
	SoundBuffer.load(Sound::SoundEffect::EggCrack,"Assets/SFX/egg_crack.wav");

	//explosion sfx
	SoundBuffer.load(Sound::SoundEffect::Explosion,"Assets/SFX/explosion.wav");

	//glass break sfx
	SoundBuffer.load(Sound::SoundEffect::GlassBreak,"Assets/SFX/glass_break_2.wav");
	
	//fireball sfx
	SoundBuffer.load(Sound::SoundEffect::DragonFireBall,"Assets/SFX/dragon_fire_FireBall_SFX.wav");


	//flame sfx
	SoundBuffer.load(Sound::SoundEffect::DragonFlame,"Assets/SFX/DragonFlameThrower.wav");

	//click sfx
	SoundBuffer.load(Sound::SoundEffect::Click,"Assets/SFX/Click.wav");

	//Power up sfx
	SoundBuffer.load(Sound::SoundEffect::PowerUp,"Assets/SFX/Power_Up.wav");


	

}

void SoundManager::Play(Sound::Music background)
{
	if(!music.openFromFile(FileNames[background]))
	{
		std::cout<<"Music file "<<FileNames[background]<<" failed to load"<<std::endl;
	}
	music.setVolume(volume);
	music.setLoop(true);
	music.play();


}


void SoundManager::Play(Sound::SoundEffect effect)
{
	if(effect!=Sound::NONE)
	{
		SFX.push_back(sf::Sound(SoundBuffer.Get(effect)));//take the sound from our buffer and put it into the sound list
		SFX.back().play();

	}
}


void SoundManager::RemoveStoppedSounds()
{
	SFX.remove_if([] (const sf::Sound & sound)// i need to look this up...why does this work?
	{
		return sound.getStatus()==sf::Sound::Stopped;
	});

}

void SoundManager::SetPause(bool paused)
{
	if (paused)
	{
		music.pause();
	}
	else
	{
		music.play();
	}
}
void SoundManager::SetVolume(float newVolume)
{
	if(newVolume<0)
	{newVolume=0;}
	else if(newVolume>100)
	{newVolume=100;}

	volume=newVolume;
}
