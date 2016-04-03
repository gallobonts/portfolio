#include "CrabClass.h"


CrabClass::CrabClass()
{


}

CrabClass::CrabClass(sf::Vector2f position,sf::Vector2f newScreenSize)
{

	alive=true;
	aiState= AI::AIState::Wander;
	//sprites
	CrabAnimation[0]=sf::IntRect(400, 422, 100, 50 );
	CrabAnimation[1]=sf::IntRect(400, 1231, 100, 50 );
	CrabAnimation[2]=CrabAnimation[0];
	CrabAnimation[3]=sf::IntRect(0, 1303, 100, 50 );
	
	//animation
	AnimationNumber=0;
	AnimationTimer=.5;
	AnimationWait=0;
	screenSize=newScreenSize;//the boundaries for the player

	//ChangeDirection
	DirectionChangeTimerConst=2.0f;
	DirectionChangeTimer=DirectionChangeTimerConst;
	DirectionChangeWait=0;


	//setting up the sprite
	texture.loadFromFile("Assets/Textures/SpriteSheet.png");
	sprite.setTexture(texture);
	sprite.setTextureRect(CrabAnimation[AnimationNumber]);//piece of the sprite sheet
	
	//handle collisions
	colliders.resize(1);
	colliders[0]=sprite.getGlobalBounds();

	//set position
	halfSpriteSize= sf::Vector2f(sprite.getLocalBounds().width/2,sprite.getLocalBounds().height/2);
	sprite.setPosition(position);
	sprite.setOrigin(halfSpriteSize);
	sprite.setScale(sf::Vector2f(0.5f, 0.5f));


}

void CrabClass::Update(sf::Time DeltaTime)
{
	switch (aiState)
	{
	case AI::AIState::Wander:
		{
		UpdateAnimation(DeltaTime);
		Wander(DeltaTime);
		}
	}


}

void CrabClass::Wander(sf::Time DeltaTime)
{

	float speed=50.0f*DeltaTime.asSeconds();
	sf::Vector2f movement(0,0);
	sf::Vector2f tempMovement=movement;
	sf::Vector2f truePosition[2];
	sf::FloatRect GlobalBounds=sprite.getGlobalBounds();
	truePosition[0].x=GlobalBounds.left;
	truePosition[1].x=GlobalBounds.left+GlobalBounds.width;
	truePosition[0].y=GlobalBounds.top;
	truePosition[1].y=GlobalBounds.top+GlobalBounds.height;

	DirectionChangeWait+=DeltaTime.asSeconds();
	if(DirectionChangeWait>=DirectionChangeTimer)
	{
		DirectionChangeWait=0;
		ChangeDirection();
	}


	tempMovement= speed*Direction;


	if(truePosition[0].x+tempMovement.x<0 || truePosition[1].x+tempMovement.x>screenSize.x )
		{
			tempMovement.x*=-1;
			Direction.x*=-1;
		}
	if(truePosition[0].y+tempMovement.y<0 || truePosition[1].y+tempMovement.y>screenSize.y)
		{
			tempMovement.y*=-1;
			Direction.y*=-1;
		}

	movement=tempMovement;
	//use unit vector to determine direction
	sprite.setRotation( (std::atan2f(movement.x, -movement.y)* 180 / PI)-90);

	sprite.move(movement);

}
void CrabClass::ChangeDirection()
{
		//something is going wrong here	
		srand((unsigned)time(0));
		Direction.x= RandomNumber(-1.0f,1.0f);
		Direction.y= RandomNumber(-1.0f,1.0f);
		DirectionChangeTimer= RandomNumber(DirectionChangeTimerConst,DirectionChangeTimerConst*2);
}



void CrabClass::UpdateAnimation(sf::Time DeltaTime)
{
	AnimationWait+=DeltaTime.asSeconds();
	if(AnimationWait>=AnimationTimer)
	{
		AnimationWait=0;
		AnimationNumber++;
		AnimationNumber%=MaxAnimationNumber;
		sprite.setTextureRect(CrabAnimation[AnimationNumber]);//piece of the sprite sheet
	}
}
 
sf::Sprite CrabClass::GetSprite()
{
	return sprite;
}



void  CrabClass::Draw(sf::RenderWindow* window)
{

//	sprite.setTexture(texture);
	window->draw(sprite);
}

std::vector<sf::FloatRect> CrabClass::GetCollision()
{
	std::vector<sf::FloatRect> submission;
	submission.push_back(sprite.getGlobalBounds());
	return submission;
}


float CrabClass::HandleCollision(int ColliderNum,Sound::SoundEffect & sfx)
{
	sfx=Sound::NONE;
	if(ColliderNum==0)//if collided with self...else it collided with an object the monster owns (like the mages flask for example
	{
	alive=false;
	sfx= Sound::CrabDeath;
	return 100;
	}

}