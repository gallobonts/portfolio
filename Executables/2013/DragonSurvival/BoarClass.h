#pragma once
#include "Monster.h"
#include <iostream>

class BoarClass:  public Monster
{
private:
	sf::Sprite sprite;

	//animation
	int AnimationNumber;
	static const int MaxAnimationNumber=4;
	sf::IntRect BoarAnimation[MaxAnimationNumber];
	float AnimationTimer;
	float AnimationWait;
	
	sf::Vector2f Direction;
	float DirectionChangeTimer;
	float DirectionChangeWait;
	float DirectionChangeTimerConst;

	sf::Vector2f screenSize;
	sf::Vector2f halfSpriteSize;//half because all the uses require hal

	sf::Texture texture;//also shooting to get rid of this when draw method is done correctly
	void Wander(sf::Time DeltaTime);
	void Seek(sf::Vector2f, sf::Time DeltaTime);

	void UpdateAnimation(sf::Time DeltaTime);
	void ChangeDirection();

	float speed;
	sf::Vector2f Acceleration;
	sf::Vector2f ArgoRadius;
public:
	//initialization
	BoarClass();
	BoarClass(sf::Vector2f position,sf::Vector2f newScreenSize);

	//update
	void Update(sf::Time DeltaTime);

	//draw
	void Draw(sf::RenderWindow* window);

	//collisions
	std::vector<sf::FloatRect> GetCollision();
	float HandleCollision(int ColliderNum,Sound::SoundEffect & sfx);
	
};




