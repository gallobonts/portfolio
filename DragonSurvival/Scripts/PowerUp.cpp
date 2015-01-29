/*
	handles all the powerups
*/

#include "PowerUp.h"

PowerUp::PowerUp()
{
}
PowerUp::PowerUp(sf::Vector2f StartingPosition)
{
	tex.loadFromFile("Assets/Textures/SpriteSheet.png");
	sprite.setPosition(StartingPosition);
	AnimationNumber=0;
	AnimationTimer=.5;
	AnimationWait=0;
	//randomly select a powerup
	int rand = RandomNumber(0,4);
	
	sprite.setScale(.5,.5);

	switch (rand)
	{
	case 0:
		type= PowerUpType::eFlameThrower;
		Animate=true;
		Animation[0]=sf::IntRect( 100, 1031, 100, 100);
		Animation[1]=sf::IntRect( 200, 1031, 100, 100);
		Animation[2]=sf::IntRect( 300, 1031, 100, 100);
		break;
	case 1:
		type= PowerUpType::eLives;
		Animate=false;
		Animation[0]=sf::IntRect(200, 1131, 33, 76);
		sprite.setScale(.75f,.75f);
		break;
	case 2:
		type= PowerUpType::eSpecial;
		Animate=false;
		Animation[0]=sf::IntRect( 200, 931, 100, 100);
		break;
	case 3:
		type= PowerUpType::eTripleFireBall;
		Animate=true;
		Animation[0]=sf::IntRect( 400, 1031, 100, 100);
		Animation[1]=sf::IntRect( 0, 1131, 100, 100);
		Animation[2]=sf::IntRect( 100, 1131, 100, 100);
		break;
	case 4:
		type= PowerUpType::eFireBall;
		Animate=true;
		Animation[0]=sf::IntRect( 300, 931, 100, 100);
		Animation[1]=sf::IntRect( 400, 931, 100, 100);
		Animation[2]=sf::IntRect( 0, 931, 100, 100);
		break;
	}
	sprite.setTexture(tex);
	sprite.setTextureRect(Animation[AnimationNumber]);

}

void PowerUp::Update(sf::Time DeltaTime)
{
	if(Animate)
	{UpdateAnimation(DeltaTime);}

}

sf::FloatRect PowerUp::GetCollision()
{
	return sprite.getGlobalBounds();
}

float PowerUp::HandleCollision()
{
	return 30;
}

void PowerUp::UpdateAnimation(sf::Time DeltaTime)
{
	//animate dragon
	AnimationWait+=DeltaTime.asSeconds();
	if(AnimationWait>=AnimationTimer)
	{	
		AnimationWait=0;
		AnimationNumber++;
		AnimationNumber%= maxAninimationNumber;
		sprite.setTextureRect(Animation[AnimationNumber]);
	}
}


void PowerUp::Draw(sf::RenderWindow* window)
{

	window->draw(sprite);
}

PowerUpType PowerUp::GetType()
{
	return type;
}