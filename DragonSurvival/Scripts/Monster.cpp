/*
	base monster class
*/
#pragma once
#include "Monster.h"


//most monster things require random numbers, this is an attempt at making it more random
float RandomNumber(float Min, float Max)
{
    return (  (float) rand()   / (float)RAND_MAX ) * (Max - Min) + Min;
}


Monster::Monster()
{
}
void Monster::Update(sf::Time DeltaTime)
{

}

void Monster::Draw(sf::RenderWindow* window)
{

}

std::vector<sf::FloatRect> Monster::GetCollision()
{
	std::vector<sf::FloatRect> submission;
	submission.push_back(sprite.getGlobalBounds());
	return submission;
}

bool Monster::isAlive()
{
	return alive;
}

float Monster::HandleCollision(int ColliderNum,Sound::SoundEffect & sfx)
{return 0;}