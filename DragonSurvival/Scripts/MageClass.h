/*
throws flasks as an atteck
*/

#pragma once
#include "Monster.h"
#include <iostream>

class MageClass:  public Monster
{
private:
	sf::Sprite sprite;

	//animation
	int AnimationNumber;
	static const int MaxAnimationNumber=5;
	sf::IntRect MageAnimation[MaxAnimationNumber];
	float AnimationTimer;
	float AnimationWait;
	
	sf::Vector2f AgroRadius;  
	float speed;
	sf::Vector2f Direction;
	float DirectionChangeTimer;
	float DirectionChangeWait;
	float DirectionChangeTimerConst;
	float Cooldown;

	sf::Vector2f screenSize;
	sf::Vector2f halfSpriteSize;//half because all the uses require hal

	sf::Texture texture;//also shooting to get rid of this when draw method is done correctly
	void Wander(sf::Time DeltaTime);
	void ThrowFlask(sf::Time DeltaTime);

	void UpdateAnimation(sf::Time DeltaTime);
	void ChangeDirection();

public:
	//initialization
	MageClass();
	MageClass(sf::Vector2f position,sf::Vector2f newScreenSize);

	//update
	void Update(sf::Time DeltaTime);

	//draw
	sf::Sprite GetSprite();//shooting for this to be our draw method
	void Draw(sf::RenderWindow* window);//temporary quick and dirty draw method

	//collisions
	std::vector<sf::FloatRect> GetCollision();
	float HandleCollision(int ColliderNum,Sound::SoundEffect & sfx);
	
};




