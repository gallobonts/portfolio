/*
	handles all the powerups
*/
#pragma once
#include "Monster.h"

enum PowerUpType
{
	eTripleFireBall,
	eFlameThrower,
	eFireBall,
	eSpecial,
	eLives
};

class PowerUp
{
private:
	sf::Sprite sprite;
	sf::Texture tex;
	PowerUpType type;

	//animations
	int AnimationNumber;
	bool Animate;
	static const int  maxAninimationNumber=3;
	sf::IntRect Animation[maxAninimationNumber];
	float AnimationTimer;
	float AnimationWait;

	void UpdateAnimation(sf::Time deltaTime);
public:
	PowerUp();
	PowerUp(sf::Vector2f startingPosition);
	void Update(sf::Time deltaTime);
	sf::FloatRect GetCollision();
	//draw
	void Draw(sf::RenderWindow* window);

	float HandleCollision();
	PowerUpType GetType();
};