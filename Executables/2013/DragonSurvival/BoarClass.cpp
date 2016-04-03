#include "BoarClass.h"


BoarClass::BoarClass()
{
	speed=2;
	ArgoRadius.x=600;
	ArgoRadius.y=600;

}

BoarClass::BoarClass(sf::Vector2f position,sf::Vector2f newScreenSize)
{

	alive=true;
	aiState= AI::AIState::Wander;
	//sprites
	BoarAnimation[0]=sf::IntRect(100, 0, 100, 264 );
	BoarAnimation[1]=sf::IntRect(0, 0, 100, 264  ); 
	BoarAnimation[2]=sf::IntRect(200, 0, 100, 264 );
	BoarAnimation[3]= BoarAnimation[1]; 
	
	//animation
	AnimationNumber=0;
	AnimationTimer=.5;
	AnimationWait=0;
	screenSize=newScreenSize;//the boundaries for the player

	//ChangeDirection
	DirectionChangeTimerConst=2.0f;
	DirectionChangeTimer=DirectionChangeTimerConst;
	DirectionChangeWait=0;


	speed=2;
	ArgoRadius.x=600;
	ArgoRadius.y=600;

	//setting up the sprite
	texture.loadFromFile("Assets/Textures/SpriteSheet.png");
	sprite.setTexture(texture);
	sprite.setTextureRect(BoarAnimation[AnimationNumber]);//piece of the sprite sheet
	
	//handle collisions
	colliders.resize(1);
	colliders[0]=sprite.getGlobalBounds();

	//set position
	halfSpriteSize= sf::Vector2f(sprite.getLocalBounds().width/2,sprite.getLocalBounds().height/2);
	sprite.setPosition(position);
	sprite.setOrigin(halfSpriteSize);
	sprite.setScale(sf::Vector2f(0.5f, 0.5f));


}

void BoarClass::Update(sf::Time DeltaTime)
{
	
	Acceleration -= (Acceleration);// /15
	sprite.move(Acceleration);


//	if (Players[] < AgroRadius)
//	{
//		AI::AIState::Attack;
//	}

	switch (aiState)
	{
	case AI::AIState::Wander:
		{
		UpdateAnimation(DeltaTime);
		Wander(DeltaTime);
		}
	case AI::AIState::Attack:
		{
		UpdateAnimation(DeltaTime);
		//Seek(sf::Vector2f);
		}
	}


}




void BoarClass::Wander(sf::Time DeltaTime)
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
	sprite.setRotation( (std::atan2f(movement.x, -movement.y)* 180 / PI)-180);

	sprite.move(movement);

}
void BoarClass::ChangeDirection()
{
		//something is going wrong here	
		srand((unsigned)time(0));
		Direction.x= RandomNumber(-1.0f,1.0f);
		Direction.y= RandomNumber(-1.0f,1.0f);
		DirectionChangeTimer= RandomNumber(DirectionChangeTimerConst,DirectionChangeTimerConst*2);
}



void BoarClass::UpdateAnimation(sf::Time DeltaTime)
{
	AnimationWait+=DeltaTime.asSeconds();
	if(AnimationWait>=AnimationTimer)
	{
		AnimationWait=0;
		AnimationNumber++;
		AnimationNumber%=MaxAnimationNumber;
		sprite.setTextureRect(BoarAnimation[AnimationNumber]);//piece of the sprite sheet
	}
}
 



void  BoarClass::Draw(sf::RenderWindow* window)
{


	window->draw(sprite);
}

std::vector<sf::FloatRect> BoarClass::GetCollision()
{
	std::vector<sf::FloatRect> submission;
	submission.push_back(sprite.getGlobalBounds());
	return submission;
}


float BoarClass::HandleCollision(int ColliderNum,Sound::SoundEffect & sfx)
{
	sfx=Sound::NONE;
	alive=false;
	sfx= Sound::PigDeath;
	return 100;

}


void BoarClass::Seek(sf::Vector2f s, sf::Time DeltaTime)
{
	sf::Vector2f T = s- sprite.getPosition();
	float l = sqrt(T.x * T.x + T.y * T.y);
	T.x = T.x/l * 50.0 * DeltaTime.asSeconds();
	T.y = T.y/l * 50.0 * DeltaTime.asSeconds();

	sf::Vector2f truePosition[2];
	sf::FloatRect GlobalBounds=sprite.getGlobalBounds();
	sf::Vector2f movement(0,0);
	sf::Vector2f tempMovement=movement;
	truePosition[0].x=GlobalBounds.left;
	truePosition[1].x=GlobalBounds.left+GlobalBounds.width;
	truePosition[0].x=GlobalBounds.top;
	truePosition[1].x=GlobalBounds.top+GlobalBounds.height;

	if(truePosition[0].x+tempMovement.x<0 || truePosition[1].x+tempMovement.x>screenSize.x )
		{
			tempMovement.x*=-1;
		}
	if(truePosition[0].y+tempMovement.y<0 || truePosition[1].y+tempMovement.y>screenSize.y)
		{
			tempMovement.y*=-1;
		}

	movement=tempMovement;
	sprite.move(movement);
}


