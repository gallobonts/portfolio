#pragma once
#include "Monster.h"
#include <iostream>


class Flask: public Monster
{
private:
	sf::Sprite sprite;

	int AnimationNumber;
	static const int MaxAnimationNumber=4;
	sf::IntRect OgreAnimation[MaxAnimationNumber];
	float AnimationTimer;
	float AnimationWait;

	sf::Texture texture;

	void UpdateAnimation(sf::Time DeltaTime);

public:
	Flask();
	virtual void Update(sf::Time DeltaTime);

	sf::Sprite GetSprite();
	void Draw(sf::RenderWindow* window);

	std::vector<sf::FloatRect> GetCollision();
	void HandleCollision();


};