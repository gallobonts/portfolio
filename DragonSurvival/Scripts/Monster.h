/*
	base class for monsters
*/
#pragma once
#include <SFML\Graphics.hpp>
#include "SoundManager.h"


enum GameState {MENU,GAMEPLAY,PAUSE,LOSESCREEN};//game state used in the project

//works like unity's tags
enum CollisionID {e_Player,e_Fire,PowerUP,None};

//more effecient then calling math.pi 
const float PI= 3.1415926535897932384626433832795f;
struct UpdateReturn
{
	Sound::SoundEffect SFX;
};


//all ai states possible
namespace AI
{
enum AIState
{
	Wander,
	Dead,
	Attack,
	Flee
};
};


float RandomNumber(float Min, float Max);

class Monster
{
protected:	
	AI::AIState aiState;
	sf::Texture texture;
	sf::Sprite sprite;
	std::vector<sf::FloatRect> colliders;
	bool alive;
public:
	virtual void Update(sf::Time DeltaTime);
	virtual void Draw(sf::RenderWindow* window);
	Monster();
	virtual std::vector<sf::FloatRect> GetCollision();
	virtual float HandleCollision(int ColliderNum,Sound::SoundEffect & sfx);
	bool isAlive();
};

