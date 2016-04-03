/*
	handles the player's attacks
*/
#pragma once
#include <SFML\Graphics.hpp>
#include <iostream>
//#include <mutex>

/*
	base attack
*/
class Attack
{
protected:
	//determines how long to stay alive
	float AttackAnimTimer;
	float AttackAnimWait;
	int AttackAnimNum;
	//direction of the attack
	sf::Vector2f Direction;
	sf::Vector2f screenSize;
	//fire texture
	sf::Texture texture;

public:
	Attack();
	virtual void Draw(sf::RenderWindow* window);
	virtual bool Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation);
	virtual bool CheckCollisions(sf::FloatRect collision);
	virtual bool HandleCollision();
};

/*
base attack
*/
class FireBall: public Attack
{
private:
	bool alive;
	sf::Sprite sprite;

	static const int MaxAttackAnimNum =7;

	sf::IntRect AttackAnim[MaxAttackAnimNum];

	float Speed;

	//private functions
	void Die();
	void UpdateAnimation(sf::Time DeltaTime);
public:
	FireBall(sf::Vector2f StartingPosition,float Orientation,sf::Vector2f newScreenSize);
	bool Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation);
	void Draw(sf::RenderWindow* window);
	bool CheckCollisions(sf::FloatRect collision);
	bool CheckLife();
	bool HandleCollision();
};

/*
constanst flame, but doesn't have as much distance
*/
class FlameThrower:public Attack
{
	private:
	bool alive;
	sf::Sprite sprite;

	static const int MaxAttackAnimNum =5;

	sf::IntRect AttackAnim[MaxAttackAnimNum];
	float Speed;

	//private functions
	void Die();
	void UpdateAnimation(sf::Time DeltaTime);
public:
	FlameThrower(sf::Vector2f StartingPosition,float Orientation,sf::Vector2f newScreenSize,float FlameSize);
	bool Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation);
	void Draw(sf::RenderWindow* window);
	bool CheckCollisions(sf::FloatRect collision);
	bool CheckLife();
	bool HandleCollision();

};

/*
similiar to fireball, but effects a bigger area
*/
class ExplosiveFireBall:public Attack
{
	private:
	bool alive;
	float explostionTimer;
	float explosionWait;
	bool exploded;
	sf::Sprite sprite;

	static const int MaxAttackAnimNum =7;

	sf::IntRect AttackAnim[MaxAttackAnimNum];

	float Speed;

	//private functions
	void Die();
	void Explode();
	void UpdateAnimation(sf::Time DeltaTime);
public:
	ExplosiveFireBall(sf::Vector2f StartingPosition,float Orientation,sf::Vector2f newScreenSize);
	bool Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation);
	void Draw(sf::RenderWindow* window);
	bool CheckCollisions(sf::FloatRect collision);
	bool CheckLife();
	bool HandleCollision();
};