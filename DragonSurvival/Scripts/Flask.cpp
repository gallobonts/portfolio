/*
flask thrown by mage
*/
#include "Flask.h"


Flask::Flask()
{

}

void Flask::UpdateAnimation(sf::Time DeltaTime)
{

}

void Flask::Update(sf::Time DeltaTime)
{
	switch (aiState)
	{
	case AI::AIState::Dead:
		{
		UpdateAnimation(DeltaTime);
		}
	}
}

void Flask::Draw(sf::RenderWindow* window)
{

}
 
std::vector<sf::FloatRect> Flask::GetCollision()
{
	std::vector<sf::FloatRect> test(1);
	test[0]=sprite.getGlobalBounds();
	return test;
}

void Flask::HandleCollision()
{
}