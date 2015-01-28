#pragma once
#include <SFML/System.hpp>
#include <SFML/Graphics.hpp>
#include "Monster.h"
#include "Attacks.h"
#include "PowerUp.h"
#include "SoundManager.h"
#include <list>





class PlayerClass
{
private:
	bool alive;
	//animation
	int AnimationNumber;
	static const int MaxAnimationNumber=4;
	sf::IntRect RedDragon[MaxAnimationNumber];
	float AnimationTimer;
	float AnimationWait;
	bool FlameThrowerLife;

	//attacks
	std::list<Attack*> AttackList;
	PowerUpType AttackType;
	bool CanAttack;
	float MaxAttackDelay;
	float MinAttackDelay;
	float AttackDelay;
	float AttackWait;

	float FlameThrowerTimer;
	float MaxFlameSize;
	float MinFlameSize;
	float FlameSize;

	float TripleShotTimer;

	int ExplosiveFireBallCount;
	static const int MaxExplosiveFireBall=3;


	//2PlayerClass
	int PlayerClassNumber;

	//controls
	sf::Keyboard::Key keyBinds[6];
	static int const LEFTKEY=0;
	static int const RIGHTKEY=1;
	static int const UPKEY=2;
	static int const DOWNKEY=3;
	static int const ATTACKKEY=4;
	static int const SPECIALKEY=5;
	
	sf::Sprite sprite;
	sf::Texture texture;//also shooting to get rid of this when draw method is done correctly

	sf::Vector2f screenSize;
	sf::Vector2f halfSpriteSize;//half because all the uses require hal

public:

	PlayerClass();
	PlayerClass(sf::Vector2f newScreenSize,int PlayerClassNum);
	
	UpdateReturn Update(sf::Time DeltaTime);

	sf::Sprite GetSprite();//shooting for this to be our draw method
	void Draw(sf::RenderWindow* window,sf::Texture tex);//temporary quick and dirty draw method
	void Draw(sf::RenderWindow* window);//temporary quick and dirty draw method

	CollisionID CheckCollisions(sf::FloatRect collision);
	void SetLife(bool isAlive);
	void ResetPosition();
	void AddExplosiveFireball();
	int GetExplosiveFireBallCount();
	void PowerUp(PowerUpType type);
};


