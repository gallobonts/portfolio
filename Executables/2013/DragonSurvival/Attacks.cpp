#include "Attacks.h"



Attack::Attack()
{
}
void Attack::Draw(sf::RenderWindow* window)
{
}
bool Attack::Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation)
{
	return true;
}

bool Attack::CheckCollisions(sf::FloatRect collision)
{
	return false;
}

bool Attack::HandleCollision()
{
	return true;
}


FireBall::FireBall(sf::Vector2f StartingPosition,float Orientation,sf::Vector2f newScreenSize)
{
	alive=true;

	texture.loadFromFile("Assets/Textures/SpriteSheet.png");
	sprite.setTexture(texture);


	screenSize=newScreenSize;
	sprite.setPosition(StartingPosition);
	sprite.setRotation(Orientation);
	Speed=150;

	//change angle in degrees to vector orientation
	Direction.x=std::sin(Orientation* 3.141592653589793 / 180.0);
	Direction.y=std::cos(Orientation* 3.141592653589793 / 180.0)*-1;
	sprite.move(60.0f*Direction);

	
	
	//sprites
	AttackAnim[0]=sf::IntRect( 300, 731, 100, 100 );
	AttackAnim[1]=sf::IntRect( 200, 628, 100, 100 );
	AttackAnim[2]=sf::IntRect( 300, 628, 100, 100 );
	AttackAnim[3]=sf::IntRect(400, 628, 100, 100 );
	AttackAnim[4]=sf::IntRect( 0, 731, 100, 100 );
	AttackAnim[5]=sf::IntRect(100, 731, 100, 100 );
	AttackAnim[6]=sf::IntRect(200, 731, 100, 100 );
	sprite.setTextureRect(AttackAnim[0]);//piece of the sprite sheet
	sprite.setScale(0.5f,0.5f);
	sf::Vector2f halfSpriteSize= sf::Vector2f(sprite.getLocalBounds().width/2,sprite.getLocalBounds().height/2);
	sprite.setOrigin(halfSpriteSize);

	//animation
	AttackAnimNum=0;
	AttackAnimTimer=.5;
	AttackAnimWait=0;
	screenSize=newScreenSize;//the boundaries for the player
}

bool FireBall::Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation)
{

	sprite.move(Speed*DeltaTime.asSeconds()*Direction);
	sf::Vector2f truePosition[2];
	sf::FloatRect GlobalBounds=sprite.getGlobalBounds();
	truePosition[0].x=GlobalBounds.left;
	truePosition[1].x=GlobalBounds.left+GlobalBounds.width;
	truePosition[0].y=GlobalBounds.top;
	truePosition[1].y=GlobalBounds.top+GlobalBounds.height;

	if(truePosition[0].x<0||truePosition[1].x>screenSize.x||truePosition[0].y<0||truePosition[1].y>screenSize.y)
	{
		return true;
	}
	UpdateAnimation(DeltaTime);
	return false;
}

void FireBall::UpdateAnimation(sf::Time DeltaTime)
{
	//animate dragon
	AttackAnimWait+=DeltaTime.asSeconds();
	if(AttackAnimWait>=AttackAnimTimer)
	{	
		AttackAnimWait=0;
		AttackAnimNum++;
		AttackAnimNum%= MaxAttackAnimNum;
		sprite.setTextureRect(AttackAnim[AttackAnimNum]);
	}
}

void FireBall::Draw(sf::RenderWindow* window)
{

	window->draw(sprite);
}
bool FireBall::CheckCollisions(sf::FloatRect collision)
{
	return sprite.getGlobalBounds().intersects(collision);
}
bool FireBall::HandleCollision()
{return true;}
void FireBall::Die()
{
	alive=false;
}


ExplosiveFireBall::ExplosiveFireBall(sf::Vector2f StartingPosition,float Orientation,sf::Vector2f newScreenSize)
{
	alive=true;
	exploded=false;
	explosionWait=0;
	explostionTimer=1.5f;

	texture.loadFromFile("Assets/Textures/SpriteSheet.png");
	sprite.setTexture(texture);


	screenSize=newScreenSize;
	sprite.setPosition(StartingPosition);
	sprite.setRotation(Orientation);
	Speed=150;

	//change angle in degrees to vector orientation
	Direction.x=std::sin(Orientation* 3.141592653589793 / 180.0);
	Direction.y=std::cos(Orientation* 3.141592653589793 / 180.0)*-1;
	sprite.move(60.0f*Direction);

	
	
	//sprites
	AttackAnim[0]=sf::IntRect( 300, 731, 100, 100 );
	AttackAnim[1]=sf::IntRect( 200, 628, 100, 100 );
	AttackAnim[2]=sf::IntRect( 300, 628, 100, 100 );
	AttackAnim[3]=sf::IntRect(400, 628, 100, 100 );
	AttackAnim[4]=sf::IntRect( 0, 731, 100, 100 );
	AttackAnim[5]=sf::IntRect(100, 731, 100, 100 );
	AttackAnim[6]=sf::IntRect(200, 731, 100, 100 );
	sprite.setTextureRect(AttackAnim[0]);//piece of the sprite sheet
	sprite.setScale(0.5f,0.5f);
	sf::Vector2f halfSpriteSize= sf::Vector2f(sprite.getLocalBounds().width/2,sprite.getLocalBounds().height/2);
	sprite.setOrigin(halfSpriteSize);

	//animation
	AttackAnimNum=0;
	AttackAnimTimer=.5;
	AttackAnimWait=0;
	screenSize=newScreenSize;//the boundaries for the player
}

bool ExplosiveFireBall::Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation)
{
	UpdateAnimation(DeltaTime);
	explosionWait+=DeltaTime.asSeconds();
	if(!exploded)
	{
		sprite.move(Speed*DeltaTime.asSeconds()*Direction);
		sf::Vector2f truePosition[2];
		sf::FloatRect GlobalBounds=sprite.getGlobalBounds();
		truePosition[0].x=GlobalBounds.left;
		truePosition[1].x=GlobalBounds.left+GlobalBounds.width;
		truePosition[0].y=GlobalBounds.top;
		truePosition[1].y=GlobalBounds.top+GlobalBounds.height;

		if(explosionWait>explostionTimer)
		{Explode();}

		if(truePosition[0].x<0||truePosition[1].x>screenSize.x||truePosition[0].y<0||truePosition[1].y>screenSize.y)
		{
			return true;
		}
		else
		{return false;}
	}	
	else//exploded state
	{
		if(explosionWait>explostionTimer)
		{
			return true;
		}
		else
		{return false;}
	}

	
}

bool ExplosiveFireBall::HandleCollision()
{
	Explode();
	return false;
}
void ExplosiveFireBall::Explode()
{
	if(!exploded)
	{
	exploded=true;
	sprite.setScale(3.0f,3.0f);
	explosionWait=0;
	explostionTimer=3.0f;
	}
}

void ExplosiveFireBall::UpdateAnimation(sf::Time DeltaTime)
{
	//animate dragon
	AttackAnimWait+=DeltaTime.asSeconds();
	if(AttackAnimWait>=AttackAnimTimer)
	{	
		AttackAnimWait=0;
		AttackAnimNum++;
		AttackAnimNum%= MaxAttackAnimNum;
		sprite.setTextureRect(AttackAnim[AttackAnimNum]);
	}
}

void ExplosiveFireBall::Draw(sf::RenderWindow* window)
{

	window->draw(sprite);
}
bool ExplosiveFireBall::CheckCollisions(sf::FloatRect collision)
{
	bool collided=false;
	collided= sprite.getGlobalBounds().intersects(collision);
	if(collided)
	{Explode();return true;}
	else
	{return false;}
}
void ExplosiveFireBall::Die()
{
	alive=false;
}

FlameThrower::FlameThrower(sf::Vector2f StartingPosition,float Orientation,sf::Vector2f newScreenSize,float FlameSize)
{
	alive=true;

	texture.loadFromFile("Assets/Textures/SpriteSheet.png");
	sprite.setTexture(texture);


	screenSize=newScreenSize;
	sprite.setPosition(StartingPosition);
	sprite.setRotation(Orientation);
	Speed=150;

	//change angle in degrees to vector orientation
	Direction.x=std::sin(Orientation* 3.141592653589793 / 180.0);
	Direction.y=std::cos(Orientation* 3.141592653589793 / 180.0)*-1;
	sprite.move(60.0f*Direction);

	
	
	//sprites
	AttackAnim[0]=sf::IntRect(200, 264, 100, 225 );
	AttackAnim[1]=sf::IntRect(300, 0, 100, 225 );
	AttackAnim[2]=sf::IntRect(400, 0, 100, 225 );
	AttackAnim[3]=sf::IntRect(0, 264, 100, 225 );
	AttackAnim[4]=sf::IntRect(100, 264, 100, 225 );


	sprite.setTextureRect(AttackAnim[0]);//piece of the sprite sheet
	sprite.setScale(FlameSize,FlameSize);
	sf::Vector2f halfSpriteSize= sf::Vector2f(sprite.getLocalBounds().width/2,sprite.getLocalBounds().height/2);
	sprite.setOrigin(halfSpriteSize);

	//animation
	AttackAnimNum=0;
	AttackAnimTimer=.2;
	AttackAnimWait=0;
	screenSize=newScreenSize;//the boundaries for the player
}

bool FlameThrower::Update(sf::Time DeltaTime,bool AttackCheck,sf::Vector2f StartingPosition,float Orientation)
{
	sprite.setPosition(StartingPosition);
	sprite.setRotation(Orientation);

	//change angle in degrees to vector orientation
	Direction.x=std::sin(Orientation* 3.141592653589793 / 180.0);
	Direction.y=std::cos(Orientation* 3.141592653589793 / 180.0)*-1;
	sprite.move(110.0f*Direction);
	UpdateAnimation(DeltaTime);

	return !AttackCheck;
	
} 

void FlameThrower::UpdateAnimation(sf::Time DeltaTime)
{

		//animate dragon
		AttackAnimWait+=DeltaTime.asSeconds();
		if(AttackAnimWait>=AttackAnimTimer )
		{	
			AttackAnimWait=0;
			AttackAnimNum++;
			if(AttackAnimNum>=MaxAttackAnimNum)
			{AttackAnimNum=2;}
		}
	
	sprite.setTextureRect(AttackAnim[AttackAnimNum]);
}

void FlameThrower::Draw(sf::RenderWindow* window)
{

	window->draw(sprite);
}
bool FlameThrower::CheckCollisions(sf::FloatRect collision)
{
	return sprite.getGlobalBounds().intersects(collision);
}
bool FlameThrower::HandleCollision()
{return false;}
void FlameThrower::Die()
{
	alive=false;
}
