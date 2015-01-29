/*
	handles the player controls
*/
#pragma once
#include "PlayerClass.h"
#include <iostream>
#include <SFML\Graphics\Rect.hpp>
#include <math.h>


PlayerClass::PlayerClass()
{

}

PlayerClass::PlayerClass(sf::Vector2f newScreenSize,int PlayerClassNum)
{
	alive=true;
	PlayerClassNumber=PlayerClassNum %2 + 1;
	PlayerClassNumber=PlayerClassNum;

	texture.loadFromFile("Assets/Textures/SpriteSheet.png");
//	sprite.setTexture(texture); oddly enough it doesn't take

	CanAttack=true;
	AttackType= PowerUpType::eFireBall;
	MaxAttackDelay= 1.4;
	MinAttackDelay= 0.4;
	AttackDelay=MaxAttackDelay;
	AttackWait=0;

	ExplosiveFireBallCount=1;

	FlameThrowerLife=false;
	MaxFlameSize=2;
	MinFlameSize=.5;
	FlameSize=MinFlameSize;

	if(PlayerClassNum==1)
	{
	keyBinds[LEFTKEY]=sf::Keyboard::Left;
	keyBinds[RIGHTKEY]=sf::Keyboard::Right;
	keyBinds[UPKEY]=sf::Keyboard::Up;
	keyBinds[DOWNKEY]=sf::Keyboard::Down;
	keyBinds[ATTACKKEY]=sf::Keyboard::Space;
	keyBinds[SPECIALKEY]=sf::Keyboard::RControl;

	}
	else
	{
	keyBinds[LEFTKEY]=sf::Keyboard::A;
	keyBinds[RIGHTKEY]=sf::Keyboard::D;
	keyBinds[UPKEY]=sf::Keyboard::W;
	keyBinds[DOWNKEY]=sf::Keyboard::S;
	keyBinds[ATTACKKEY]=sf::Keyboard::LControl;
	keyBinds[SPECIALKEY]=sf::Keyboard::F;
	}



	//sprites
	if(PlayerClassNumber==1)
	{
	RedDragon[0]=sf::IntRect(300, 1131, 33, 76);
	RedDragon[1]=sf::IntRect(333, 1131, 33, 76 );
	RedDragon[2]=RedDragon[0];
	RedDragon[3]=sf::IntRect(366, 1131, 33, 76 );

	}
	else
	{
	RedDragon[0]=sf::IntRect(200, 1131, 33, 76);
	RedDragon[1]=sf::IntRect(233, 1131, 33, 76 );
	RedDragon[2]=RedDragon[0];
	RedDragon[3]=sf::IntRect(266, 1131, 33, 76 );
	}
	
	//animation
	AnimationNumber=0;
	AnimationTimer=.5;
	AnimationWait=0;
	screenSize=newScreenSize;//the boundaries for the PlayerClass

	//setting up the sprite
	sprite.setTextureRect(RedDragon[AnimationNumber]);//piece of the sprite sheet
	if(PlayerClassNumber==1)
	{sprite.setPosition(150,500);}
	else{sprite.setPosition(600,500);}
	//setting sprite to center
	halfSpriteSize= sf::Vector2f(sprite.getLocalBounds().width/2,sprite.getLocalBounds().height/2);

	sprite.setOrigin(halfSpriteSize);
	sprite.setScale(sf::Vector2f(1.5f, 1.5f));
}

void PlayerClass::PowerUp(PowerUpType type)
{
	FlameThrowerLife=false;
	switch(type)
	{
	case PowerUpType::eFireBall:
		if(AttackType==PowerUpType::eFlameThrower)
		{AttackType=type;}
		if(AttackDelay>=MinAttackDelay)
		{AttackDelay-=.05;}
		break;
	case PowerUpType::eFlameThrower:
		AttackType=type;
		if(FlameSize<=MaxFlameSize)
		FlameSize+=.05;
		break;
	case PowerUpType::eSpecial:
		AddExplosiveFireball();
		break;
	case PowerUpType::eTripleFireBall:
		AttackType=type;
		break;
	}
}
UpdateReturn PlayerClass::Update(sf::Time DeltaTime)
{

	UpdateReturn Ureturn;
	Ureturn.SFX=Sound::NONE;
	
	if(alive)
	{
	bool isMoving=false;

	float speed=75.0f*DeltaTime.asSeconds();
	sf::Vector2f movement(0,0);
	sf::Vector2f tempMovement=movement;
	sf::Vector2f truePosition[2];
	sf::FloatRect GlobalBounds=sprite.getGlobalBounds();
	truePosition[0].x=GlobalBounds.left;
	truePosition[1].x=GlobalBounds.left+GlobalBounds.width;
	truePosition[0].y=GlobalBounds.top;
	truePosition[1].y=GlobalBounds.top+GlobalBounds.height;

	if (sf::Keyboard::isKeyPressed(keyBinds[LEFTKEY]))
	{
		
		tempMovement.x-=speed;
		if(truePosition[0].x+tempMovement.x>0)
		{
			movement=tempMovement;
			isMoving=true;
		}
		else
		{
			tempMovement=movement;
		}
	}
	if (sf::Keyboard::isKeyPressed(keyBinds[RIGHTKEY]))
	{
		tempMovement.x+=speed;
	if(truePosition[1].x+tempMovement.x<screenSize.x)
		{
			movement=tempMovement;
			isMoving=true;
		}
		else
		{
			tempMovement=movement;
		}

	}
	if (sf::Keyboard::isKeyPressed(keyBinds[UPKEY]))
	{
		tempMovement.y-=speed;
		if(truePosition[0].y+tempMovement.y>0)
		{
			movement=tempMovement;
			isMoving=true;
		}
		else
		{
			tempMovement=movement;
		}
	}	
	
	if (sf::Keyboard::isKeyPressed(keyBinds[DOWNKEY]))
	{
		tempMovement.y+=speed;
		if(truePosition[1].y+tempMovement.y<screenSize.y)
		{
			movement=tempMovement;
			isMoving=true;
		}
		else
		{
			tempMovement=movement;
		}
	}

	if(sf::Keyboard::isKeyPressed(keyBinds[ATTACKKEY]))
	{
		switch(AttackType)
		{
		case PowerUpType::eFireBall:
			if(CanAttack)
			{
			AttackList.push_back(new FireBall(sprite.getPosition(),sprite.getRotation(),screenSize));
			Ureturn.SFX=Sound::DragonFireBall;
			CanAttack=false;
			AttackWait=0;
			}
			break;
		case PowerUpType::eTripleFireBall:
			if(CanAttack)
			{
			sf::Vector2f FireBallPosition=sprite.getPosition();
			float FireballRotation=sprite.getRotation();
			sf::Vector2f Direction[2];
			//change angle in degrees to vector orientation
			Direction[0].x=std::sin(FireballRotation-90.0f* 3.141592653589793 / 180.0f);
			Direction[0].y=std::cos(FireballRotation-90.0f* 3.141592653589793 / 180.0f)*-1;

			Direction[1].x=std::sin(FireballRotation+90* 3.141592653589793 / 180.0);
			Direction[1].y=std::cos(FireballRotation+90* 3.141592653589793 / 180.0)*-1;

			AttackList.push_back(new FireBall(FireBallPosition,FireballRotation,screenSize));

			FireBallPosition+= Direction[0]*30.f;
			FireballRotation+=45;
			AttackList.push_back(new FireBall(FireBallPosition,FireballRotation,screenSize));

			FireBallPosition+= Direction[1]*60.f;
			FireballRotation-=90;
			AttackList.push_back(new FireBall(FireBallPosition,FireballRotation,screenSize));

			Ureturn.SFX=Sound::DragonFireBall;
			CanAttack=false;
			}
			break;
		case PowerUpType::eFlameThrower:

			if(FlameThrowerLife==false)
			{
				AttackList.push_back(new FlameThrower(sprite.getPosition(),sprite.getRotation(),screenSize,FlameSize));
				FlameThrowerLife=true;
			}
		}
	}
	else
	{FlameThrowerLife=false;}

	if(sf::Keyboard::isKeyPressed(keyBinds[SPECIALKEY])&&CanAttack)
	{
		if(ExplosiveFireBallCount>0)
		{
			AttackList.push_back(new ExplosiveFireBall(sprite.getPosition(),sprite.getRotation(),screenSize));
		Ureturn.SFX=Sound::NONE;
		CanAttack=false;
		AttackWait=0;
		ExplosiveFireBallCount--;

		}
	}

	if(isMoving)
	{
	sprite.move(movement);

	//use unit vector to determine direction
	sprite.setRotation( std::atan2f(movement.x, -movement.y)* 180 / PI);

	//animate dragon
	AnimationWait+=DeltaTime.asSeconds();
	if(AnimationWait>=AnimationTimer)
	{
		AnimationWait=0;
		AnimationNumber++;
		AnimationNumber%=MaxAnimationNumber;
		sprite.setTextureRect(RedDragon[AnimationNumber]);//piece of the sprite sheet
	}

	}//end isMoving
	
	if(!CanAttack)
	{
		AttackWait+=DeltaTime.asSeconds();
		if(AttackWait>=AttackDelay)
		{
			AttackWait=0;
			CanAttack=true;
		}
	}


		for(std::list<Attack*>::const_iterator i=AttackList.begin(),end=AttackList.end(); i!=end;)
		{   
			if((*i)->Update(DeltaTime,FlameThrowerLife,sprite.getPosition(),sprite.getRotation()))
			{i=AttackList.erase(i);break;}
			else
			{i++;}
		}

		}
		return Ureturn;

}



sf::Sprite PlayerClass::GetSprite()
{
	return sprite;
}

void PlayerClass::SetLife(bool isAlive)
{
	alive=isAlive;
}

void PlayerClass::ResetPosition()
{
	sprite.setRotation(0);
	if(PlayerClassNumber==1)
	{sprite.setPosition(150,500);}
	else{sprite.setPosition(600,500);} 
	AttackList.clear();
	ExplosiveFireBallCount=1;
	FlameSize=MinFlameSize;
	AttackDelay=MaxAttackDelay;
	AttackType=PowerUpType::eFireBall;
	
}
//temporary quick and dirt draw approach


void  PlayerClass::Draw(sf::RenderWindow* window)
{
	if(alive)
	{
	for(std::list<Attack*>::const_iterator i=AttackList.begin(),end=AttackList.end(); i!=end;i++)
	{(*i)->Draw(window);}
	sprite.setTexture(texture);
	window->draw(sprite);

	
	}
	
}

CollisionID PlayerClass::CheckCollisions(sf::FloatRect collision)
{
	CollisionID collider=CollisionID::None;
	if(collision.height>=500|collision.left<=0||collision.width<=10||collision.top<=0)
	{return collider;}

	if(sprite.getGlobalBounds().intersects(collision))
	{
		collider= CollisionID::e_Player;
//		std::cout<<"collision at height "<<collision.height<<" left "<<collision.left<<" top "<<collision.top<<" width "<< collision.width<<std::endl;
//		std::cout<<"globalbounds at height "<<sprite.getGlobalBounds().height<<" left "<<sprite.getGlobalBounds().left<<" top "<<sprite.getGlobalBounds().top<<" width "<< sprite.getGlobalBounds().width<<std::endl;
		return collider;
	
	}
	else
	{	

		for(std::list<Attack*>::const_iterator i=AttackList.begin(),end=AttackList.end(); i!=end;i++)
		{   
//			std::cout<<"collision at height "<<collision.height<<"left "<<collision.left<<"top "<<collision.top<<"width "<< collision.width<<std::endl;
	
			if((*i)->CheckCollisions(collision))
			{
				collider= CollisionID::e_Fire;
				if((*i)->HandleCollision())
				{AttackList.remove((*i));}
				return collider;
			}
		}

	}

	return collider;

}


void PlayerClass::AddExplosiveFireball()
{
	if(ExplosiveFireBallCount<MaxExplosiveFireBall)
	{
		ExplosiveFireBallCount++;
	}

}
int PlayerClass::GetExplosiveFireBallCount()
{
	return ExplosiveFireBallCount;
}